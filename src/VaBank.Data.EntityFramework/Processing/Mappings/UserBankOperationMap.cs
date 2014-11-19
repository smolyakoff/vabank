using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Processing.Entities;

namespace VaBank.Data.EntityFramework.Processing.Mappings
{
    public class UserBankOperationMap: EntityTypeConfiguration<UserBankOperation>
    {
        public UserBankOperationMap()
        {
            ToTable("User_Account", "Accounting");
        }
    }
}
