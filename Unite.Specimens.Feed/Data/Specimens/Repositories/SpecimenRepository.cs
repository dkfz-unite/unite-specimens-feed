﻿using System;
using Unite.Data.Entities.Specimens;
using Unite.Data.Services;
using Unite.Specimens.Feed.Data.Specimens.Models;

namespace Unite.Specimens.Feed.Data.Specimens.Repositories
{
    internal class SpecimenRepository
    {
        private readonly SpecimenRepositoryBase<TissueModel> _tissueRepository;
        private readonly SpecimenRepositoryBase<CellLineModel> _cellLineRepository;
        private readonly SpecimenRepositoryBase<OrganoidModel> _organoidRepository;
        private readonly SpecimenRepositoryBase<XenograftModel> _xenograftRepository;


        public SpecimenRepository(DomainDbContext dbContext)
        {
            _tissueRepository = new TissueRepository(dbContext);
            _cellLineRepository = new CellLineRepository(dbContext);
            _organoidRepository = new OrganoidRepository(dbContext);
            _xenograftRepository = new XenograftRepository(dbContext);
        }


        public Specimen Find(int donorId, int? parentId, SpecimenModel model)
        {
            if (model is TissueModel tissueModel)
            {
                return _tissueRepository.Find(donorId, parentId, tissueModel);
            }
            else if (model is CellLineModel cellLineModel)
            {
                return _cellLineRepository.Find(donorId, parentId, cellLineModel);
            }
            else if (model is OrganoidModel organoidModel)
            {
                return _organoidRepository.Find(donorId, parentId, organoidModel);
            }
            else if (model is XenograftModel xenograftModel)
            {
                return _xenograftRepository.Find(donorId, parentId, xenograftModel);
            }
            else
            {
                throw new NotImplementedException("Specimen type is not yet supported");
            }
        }

        public Specimen Create(int donorId, int? parentId, SpecimenModel model)
        {
            if (model is TissueModel tissueModel)
            {
                return _tissueRepository.Create(donorId, parentId, tissueModel);
            }
            else if (model is CellLineModel cellLineModel)
            {
                return _cellLineRepository.Create(donorId, parentId, cellLineModel);
            }
            else if (model is OrganoidModel organoidModel)
            {
                return _organoidRepository.Create(donorId, parentId, organoidModel);
            }
            else if (model is XenograftModel xenograftModel)
            {
                return _xenograftRepository.Create(donorId, parentId, xenograftModel);
            }
            else
            {
                throw new NotSupportedException("Specimen type is not yet supported");
            }
        }

        public void Update(ref Specimen entity, in SpecimenModel model)
        {
            if (model is TissueModel tissueModel)
            {
                _tissueRepository.Update(ref entity, tissueModel);
            }
            else if (model is CellLineModel cellLineModel)
            {
                _cellLineRepository.Update(ref entity, cellLineModel);
            }
            else if (model is OrganoidModel organoidModel)
            {
                _organoidRepository.Update(ref entity, organoidModel);
            }
            else if (model is XenograftModel xenograftModel)
            {
                _xenograftRepository.Update(ref entity, xenograftModel);
            }
            else
            {
                throw new NotSupportedException("Specimen type is not yet supported");
            }
        }
    }
}
