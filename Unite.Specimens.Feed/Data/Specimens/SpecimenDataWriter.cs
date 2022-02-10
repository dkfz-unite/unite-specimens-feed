﻿using Unite.Data.Entities.Donors;
using Unite.Data.Entities.Specimens;
using Unite.Data.Services;
using Unite.Specimens.Feed.Data.Exceptions;
using Unite.Specimens.Feed.Data.Specimens.Models;
using Unite.Specimens.Feed.Data.Specimens.Models.Audit;
using Unite.Specimens.Feed.Data.Specimens.Repositories;

namespace Unite.Specimens.Feed.Data.Specimens
{
    public class SpecimenDataWriter : DataWriter<SpecimenModel, SpecimensUploadAudit>
    {
        private readonly DonorRepository _donorRepository;
        private readonly SpecimenRepository _specimenRepository;


        public SpecimenDataWriter(DomainDbContext dbContext) : base(dbContext)
        {
            _donorRepository = new DonorRepository(dbContext);
            _specimenRepository = new SpecimenRepository(dbContext);
        }


        protected override void ProcessModel(SpecimenModel model, ref SpecimensUploadAudit audit)
        {
            var donor = FindOrCreateDonor(model.Donor, ref audit);

            var parentSpecimen = FindSpecimen(donor.Id, null, model.Parent, true);

            var specimen = FindSpecimen(donor.Id, parentSpecimen?.Id, model);

            if (specimen == null)
            {
                specimen = CreateSpecimen(donor.Id, parentSpecimen?.Id, model, ref audit);

                audit.Specimens.Add(specimen.Id);
            }
            else
            {
                UpdateSpecimen(specimen, model, ref audit);

                audit.Specimens.Add(specimen.Id);
            }
        }


        private Donor FindOrCreateDonor(DonorModel model, ref SpecimensUploadAudit audit)
        {
            var entity = _donorRepository.Find(model.ReferenceId);

            if (entity == null)
            {
                entity = _donorRepository.Create(model);

                audit.DonorsCreated++;
            }

            return entity;
        }

        private Specimen FindSpecimen(int donorId, int? parentId, SpecimenModel model, bool throwNotFound = false)
        {
            if (model == null)
            {
                return null;
            }

            var entity = _specimenRepository.Find(donorId, parentId, model);

            if (entity == null && throwNotFound)
            {
                throw new NotFoundException($"Parent specimen with id '{model.ReferenceId}' was not found");
            }
            else
            {
                return entity;
            }
        }

        private Specimen CreateSpecimen(int donorId, int? parentId, SpecimenModel model, ref SpecimensUploadAudit audit)
        {
            var entity = _specimenRepository.Create(donorId, parentId, model);

            if (entity.Tissue != null)
            {
                audit.TissuesCreated++;
            }
            else if (entity.CellLine != null)
            {
                audit.CellLinesCreated++;
            }
            else if (entity.Organoid != null)
            {
                audit.OrganoidsCreated++;
            }
            else if (entity.Xenograft != null)
            {
                audit.XenograftsCreated++;
            }

            return entity;
        }

        private Specimen UpdateSpecimen(Specimen entity, SpecimenModel model, ref SpecimensUploadAudit audit)
        {
            _specimenRepository.Update(ref entity, model);

            if (entity.Tissue != null)
            {
                audit.TissuesUpdated++;
            }
            else if (entity.CellLine != null)
            {
                audit.CellLinesUpdated++;
            }
            else if (entity.Organoid != null)
            {
                audit.OrganoidsUpdate++;
            }
            else if (entity.Xenograft != null)
            {
                audit.XenograftsUpdated++;
            }

            return entity;
        }
    }
}
