using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class CarValidator:AbstractValidator<Car>
    {
        public CarValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.DailyPrice).GreaterThan(0);
            RuleFor(c => c.ModelYear).GreaterThan(1990);
            RuleFor(c => c.DailyPrice).NotEmpty();
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.Description).MinimumLength(2);
        }
    }
}
