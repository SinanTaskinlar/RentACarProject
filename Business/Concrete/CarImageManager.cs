using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Helpers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Business.Concrete
{
    public class CarImageManager : ICarImageService
    {
        ICarImageDal _carImageDal;

        public CarImageManager(ICarImageDal carImageDal)
        {
            _carImageDal = carImageDal;
        }

        [ValidationAspect(typeof(CarImageValidator))]
        public IResult Add(IFormFile file, CarImage carImage)
        {
            var result = BusinessRules.Run(CheckCarImageMaxLimit(carImage));
            if (result!=null)
            {
                return result;
            }

            carImage.ImagePath = FileHelper.AddAsync(file);
            carImage.Date = DateTime.Now;
            _carImageDal.Add(carImage);

            return new SuccessResult();
        }

        public IResult Delete(CarImage carImage)
        {
            var oldpath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\wwwroot"))
                + _carImageDal.Get(ci => ci.Id == carImage.Id).ImagePath;
            var result = BusinessRules.Run(FileHelper.DeleteAsync(oldpath));
            if (result!=null)
            {
                return result;
            }
            _carImageDal.Delete(carImage);
            return new SuccessResult();
        }

        public IDataResult<List<CarImage>> GetAll(Expression<Func<CarImage, bool>> filter = null)
        {
            return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll(filter));
        }

        public IDataResult<CarImage> GetById(int id)
        {
            return new SuccessDataResult<CarImage>(_carImageDal.Get(c => c.Id == id));
        }

        public IDataResult<List<CarImage>> GetImagesByCarId(int carId)
        {
            return new SuccessDataResult<List<CarImage>>(CheckIfCarHaveNoImage(carId));

        }

        [ValidationAspect(typeof(CarImageValidator))]
        public IResult Update(IFormFile file, CarImage carImage)
        {
            var oldpath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
                "..\\..\\..\\wwwroot")) + _carImageDal.Get(g => g.Id == carImage.Id).ImagePath;
            carImage.ImagePath = FileHelper.UpdateAsync(oldpath, file);
            carImage.Date = DateTime.Now;
            _carImageDal.Update(carImage);
            return new SuccessResult();
        }

        private IResult CheckCarImageMaxLimit(CarImage carImage)
        {
            if (_carImageDal.GetAll(ci=>ci.CarId==carImage.CarId).Count>=5)
            {
                return new ErrorResult(Messages.FailedCarImageLimit);
            }
            return new SuccessResult();
        }

        private IResult CheckIsAnyCarImage(int id)
        {
            if (_carImageDal.GetAll(ci => ci.CarId == id).Where(z => z.ImagePath == null).Count() > 0)
            {
                return new ErrorResult(Messages.FailedCarImageLimit);
            }
            return new SuccessResult();
        }


        private List<CarImage> CheckIfCarHaveNoImage(int id)
        {
            string path = Directory.GetCurrentDirectory() + @"\wwwroot\Images\default.jpg";
            var result = _carImageDal.GetAll(c => c.CarId == id);
            if (!result.Any())
            {
                return new List<CarImage> { new CarImage { CarId = id, ImagePath = path } };
            }
            return result;
        }
    }
}
