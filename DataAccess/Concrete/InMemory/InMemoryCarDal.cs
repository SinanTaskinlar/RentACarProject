using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.InMemory
{
    public class InMemoryCarDal : ICarDal
    {
        List<Car> _cars;

        public InMemoryCarDal()
        {
            _cars = new List<Car>
            {
               new Car { Id=1, Name ="Honda", BrandId=1, ColorId =1, DailyPrice=50, Description="Benzin", ModelYear=2017},
               new Car { Id=2, Name ="Mercedes", BrandId=2, ColorId =2, DailyPrice=85, Description="Dizel", ModelYear=2015},
               new Car { Id=3, Name ="Hyundai", BrandId=3, ColorId =2, DailyPrice=60, Description="Gaz", ModelYear=2011},
               new Car { Id=4, Name ="BMW", BrandId=4, ColorId =3, DailyPrice=70, Description="Hybrid", ModelYear=2014},
               new Car { Id=5, Name ="Seat", BrandId=5, ColorId =3, DailyPrice=40, Description="Benzin-Gaz", ModelYear=2020}
            };
        }

        public void Add(Car car)
        {
            _cars.Add(car);
        }

        public void Delete(Car car)
        {
            Car carToDelete = _cars.SingleOrDefault(c => c.Id == car.Id);
            _cars.Remove(carToDelete);
        }

        public Car Get(Expression<Func<Car, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<Car> GetAll()
        {
            return _cars;
        }

        public List<Car> GetAll(Expression<Func<Car, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public List<Car> GetById(int carId)
        {
            return _cars.Where(c => c.Id == carId).ToList();
        }

        public List<CarDetailDto> GetCarDetails(Expression<Func<Car, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public void Update(Car car)
        {
            Car carToUpdate = _cars.SingleOrDefault(c => c.Id == car.Id);
            carToUpdate.Name = car.Name;
            carToUpdate.Id = car.Id;
            carToUpdate.BrandId = car.BrandId;
            carToUpdate.ColorId = car.ColorId;
            carToUpdate.DailyPrice = car.DailyPrice;
            carToUpdate.Description = car.Description;
            carToUpdate.ModelYear = car.ModelYear;
        }
    }
}
