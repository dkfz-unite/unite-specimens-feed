﻿using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Context.Repositories;
using Unite.Data.Context.Services.Tasks;
using Unite.Data.Entities.Donors;
using Unite.Data.Entities.Specimens;

using CNV = Unite.Data.Entities.Genome.Variants.CNV;
using SSM = Unite.Data.Entities.Genome.Variants.SSM;
using SV = Unite.Data.Entities.Genome.Variants.SV;

namespace Unite.Specimens.Feed.Web.Services;

public class SpecimenIndexingTasksService : IndexingTaskService<Donor, int>
{
    protected override int BucketSize => 1000;
    private readonly SpecimensRepository _specimensRepository;
    private readonly DonorsRepository _donorsRepository;


    public SpecimenIndexingTasksService(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
        _specimensRepository = new SpecimensRepository(dbContextFactory);
        _donorsRepository = new DonorsRepository(dbContextFactory);
    }


    public override void CreateTasks()
    {
        IterateEntities<Specimen, int>(specimen => true, specimen => specimen.Id, specimens =>
        {
            CreateSpecimenIndexingTasks(specimens);
        });
    }

    public override void CreateTasks(IEnumerable<int> specimenIds)
    {
        IterateEntities<Specimen, int>(specimen => specimenIds.Contains(specimen.Id), specimen => specimen.Id, specimens =>
        {
            CreateSpecimenIndexingTasks(specimens);
        });
    }

    public override void PopulateTasks(IEnumerable<int> specimenIds)
    {
        IterateEntities<Specimen, int>(specimen => specimenIds.Contains(specimen.Id), specimen => specimen.Id, specimens =>
        {
            CreateDonorIndexingTasks(specimens);
            CreateImageIndexingTasks(specimens);
            CreateSpecimenIndexingTasks(specimens);
            CreateVariantIndexingTasks(specimens);
            CreateGeneIndexingTasks(specimens);
        });
    }


    protected override IEnumerable<int> LoadRelatedDonors(IEnumerable<int> keys)
    {
        return _specimensRepository.GetRelatedDonors(keys).Result;
    }

    protected override IEnumerable<int> LoadRelatedImages(IEnumerable<int> keys)
    {
        return _specimensRepository.GetRelatedImages(keys).Result;
    }

    protected override IEnumerable<int> LoadRelatedSpecimens(IEnumerable<int> keys)
    {
        var donorIds = _specimensRepository.GetRelatedDonors(keys).Result;

        return _donorsRepository.GetRelatedSpecimens(donorIds).Result;
    }

    protected override IEnumerable<int> LoadRelatedGenes(IEnumerable<int> keys)
    {
        return _specimensRepository.GetRelatedGenes(keys).Result;
    }

    protected override IEnumerable<long> LoadRelatedSsms(IEnumerable<int> keys)
    {
        return _specimensRepository.GetRelatedVariants<SSM.Variant>(keys).Result;
    }

    protected override IEnumerable<long> LoadRelatedCnvs(IEnumerable<int> keys)
    {
        return _specimensRepository.GetRelatedVariants<CNV.Variant>(keys).Result;
    }

    protected override IEnumerable<long> LoadRelatedSvs(IEnumerable<int> keys)
    {
        return _specimensRepository.GetRelatedVariants<SV.Variant>(keys).Result;
    }
}
