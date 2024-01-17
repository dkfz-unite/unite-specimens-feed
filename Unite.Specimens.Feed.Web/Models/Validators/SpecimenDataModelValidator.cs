﻿using FluentValidation;
using Unite.Specimens.Feed.Web.Models.Base;
using Unite.Specimens.Feed.Web.Models.Base.Validators;

namespace Unite.Specimens.Feed.Web.Models.Validators;

public class SpecimenDataModelValidator : AbstractValidator<SpecimenDataModel>
{
    private readonly IValidator<MaterialModel> _materialModelValidator;
    private readonly IValidator<LineModel> _lineModelValidator;
    private readonly IValidator<OrganoidModel> _organoidModelValidator;
    private readonly IValidator<XenograftModel> _xenograftModelValidator;
    private readonly IValidator<MolecularDataModel> _molecularDataModelValidator;
    private readonly IValidator<InterventionModel> _interventionModelValidator;
    private readonly IValidator<DrugScreeningModel> _drugScreeningModelValidator;


    public SpecimenDataModelValidator()
    {
        _materialModelValidator = new MaterialModelValidator();
        _lineModelValidator = new LineModelValidator();
        _organoidModelValidator = new OrganoidModelValidator();
        _xenograftModelValidator = new XenograftModelValidator();
        _molecularDataModelValidator = new MolecularDataModelValidator();
        _interventionModelValidator = new InterventionModelValidator();
        _drugScreeningModelValidator = new DrugScreeningModelValidator();


        RuleFor(model => model.Id)
            .NotEmpty()
            .WithMessage("Should not be empty");

        RuleFor(model => model.Id)
            .MaximumLength(255)
            .WithMessage("Maximum length is 255");


        RuleFor(model => model.ParentId)
            .MaximumLength(255)
            .WithMessage("Maximum length is 255");

        RuleFor(model => model.ParentType)
            .Empty()
            .When(model => string.IsNullOrEmpty(model.ParentId))
            .WithMessage("Should be empty if parent specimen Id is not set");

        RuleFor(model => model.ParentType)
            .NotEmpty()
            .When(model => !string.IsNullOrEmpty(model.ParentId))
            .WithMessage("Should not be empty if parent specimen Id is set");


        RuleFor(model => model.DonorId)
            .NotEmpty()
            .WithMessage("Should not be empty");

        RuleFor(model => model.DonorId)
            .MaximumLength(255)
            .WithMessage("Maximum length is 255");


        RuleFor(model => model.CreationDate)
            .Empty()
            .When(model => model.CreationDay.HasValue)
            .WithMessage("Either 'CreationDate' or 'CreationDay' can be set, not both");


        RuleFor(model => model.CreationDay)
            .Empty()
            .When(model => model.CreationDate.HasValue)
            .WithMessage("Either 'CreationDate' or 'CreationDay' can be set, not both");


        RuleFor(model => model)
            .Must(HaveModelSet)
            .WithMessage("Specific specimen data (Tissue, CellLine or Xenograft) has to be set");


        RuleFor(model => model.Material)
            .SetValidator(_materialModelValidator);

        RuleFor(model => model.Line)
            .SetValidator(_lineModelValidator);

        RuleFor(model => model.Organoid)
            .SetValidator(_organoidModelValidator);

        RuleFor(model => model.Xenograft)
            .SetValidator(_xenograftModelValidator);

        RuleFor(model => model.MolecularData)
            .SetValidator(_molecularDataModelValidator);

        RuleForEach(model => model.Interventions)
            .SetValidator(_interventionModelValidator);

        RuleForEach(model => model.DrugScreenings)
            .SetValidator(_drugScreeningModelValidator);
    }


    private bool HaveModelSet(SpecimenDataModel model)
    {
        return model.Material != null
            || model.Line != null
            || model.Organoid != null
            || model.Xenograft != null;
    }
}


public class SpecimenModelsValidator : AbstractValidator<SpecimenDataModel[]>
{
    private readonly IValidator<SpecimenDataModel> _specimenModelValidator;


    public SpecimenModelsValidator()
    {
        _specimenModelValidator = new SpecimenDataModelValidator();


        RuleForEach(model => model)
            .SetValidator(_specimenModelValidator);
    }
}
