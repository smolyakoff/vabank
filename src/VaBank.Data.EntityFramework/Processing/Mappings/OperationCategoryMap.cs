using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Processing.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Processing.Mappings
{
    public class OperationCategoryMap: EntityTypeConfiguration<OperationCategory>
    {
        public OperationCategoryMap()
        {
            ToTable("OperationCategory", "Processing").HasKey(x => x.Code);
            Property(x => x.Name).HasMaxLength(Restrict.Length.ShortName).IsRequired();
            Property(x => x.Description).HasMaxLength(Restrict.Length.BigString);
        }
    }
}
