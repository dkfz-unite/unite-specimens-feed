﻿using FluentValidation;

namespace Unite.Specimens.Feed.Web.Models.Drugs.Validators;

public class DrugScreeningModelValidator : AbstractValidator<DrugScreeningModel>
{
    public DrugScreeningModelValidator()
    {
        RuleFor(model => model.Drug)
            .NotEmpty()
            .WithMessage("Should not be empty");

        RuleFor(model => model.Drug)
            .MaximumLength(255)
            .WithMessage("Maximum length is 255");

        RuleFor(model => model.Gof)
            .Must(value => value >= 0 && value <= 1)
            .WithMessage("Should be in range [0, 1]");

        RuleFor(model => model.Dss)
            .Must(value => value >= 0 && value <= 1)
            .WithMessage("Should be in range [0, 1]");

        RuleFor(model => model.DssS)
            .Must(value => value >= -1 && value <= 1)
            .WithMessage("Should be in range [-1, 1]");

        RuleFor(model => model.MinDose)
            .Must(value => value > 0)
            .WithMessage("Should be greater than 0");

        RuleFor(model => model.MaxDose)
            .Must(value => value > 0)
            .WithMessage("Should be greater than 0");

        RuleFor(model => model.Dose25)
            .Must(value => value > 0)
            .WithMessage("Should be greater than 0");

        RuleFor(model => model.Dose50)
            .Must(value => value > 0)
            .WithMessage("Should be greater than 0");

        RuleFor(model => model.Dose75)
            .Must(value => value > 0)
            .WithMessage("Should be greater than 0");

        RuleForEach(model => model.Doses)
            .Must(value => value > 0)
            .WithMessage("Should be greater than 0");

        RuleForEach(model => model.Responses)
            .Must(value => value >= -1 && value <= 1)
            .WithMessage("Should be in range [-1, 1]");
    }
}
