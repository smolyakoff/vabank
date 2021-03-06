﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Processing.Entities;

namespace VaBank.Data.EntityFramework.Processing.Mappings
{
    public class CardTransactionMap: EntityTypeConfiguration<CardTransaction>
    {
        public CardTransactionMap()
        {
            ToTable("CardTransaction", "Processing");
            HasRequired(x => x.Card).WithMany().Map(x => x.MapKey("CardID"));
        }
    }
}
