﻿using FluentValidation;

namespace Unite.Specimens.Feed.Web.Models.Base.Validators;

public class LineInfoModelValidator : AbstractValidator<LineInfoModel>
{
    public LineInfoModelValidator()
    {
        RuleFor(model => model)
            .Must(HaveAnythingSet)
            .WithMessage("At least one field has to be set");
    }


    private bool HaveAnythingSet(LineInfoModel model)
    {
        return !string.IsNullOrWhiteSpace(model.Name)
            || !string.IsNullOrWhiteSpace(model.DepositorName)
            || !string.IsNullOrWhiteSpace(model.DepositorEstablishment)
            || model.EstablishmentDate != null
            || !string.IsNullOrWhiteSpace(model.PubMedLink)
            || !string.IsNullOrWhiteSpace(model.AtccLink)
            || !string.IsNullOrWhiteSpace(model.ExPasyLink);
    }
}
