using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMigrate.Models
{
    public class ModelBase
    {
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public bool Deleted { get; set; } = false;
    }
}
