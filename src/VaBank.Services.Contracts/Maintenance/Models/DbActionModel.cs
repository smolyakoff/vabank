using System.Collections.Generic;

namespace VaBank.Services.Contracts.Maintenance.Models
{
    public class DbActionModel
    {
        public DbActionModel()
        {
            Changes = new List<DbChangeModel>();
        }

        public string TableName { get; set; }

        public List<DbChangeModel> Changes { get; set; }
    }
}
