﻿using Unite.Data.Entities.Specimens.Enums;

namespace Unite.Specimens.Feed.Web.Services.Specimens
{
    public class MolecularDataModel
    {
        public MgmtStatus? MgmtStatus { get; set; }
        public IdhStatus? IdhStatus { get; set; }
        public IdhMutation? IdhMutation { get; set; }
        public GeneExpressionSubtype? GeneExpressionSubtype { get; set; }
        public MethylationSubtype? MethylationSubtype { get; set; }
        public bool? GcimpMethylation { get; set; }

        public void Sanitise()
        {

        }
    }
}
