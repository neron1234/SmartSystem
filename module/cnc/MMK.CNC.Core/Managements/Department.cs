using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MMK.CNC.Core.Managements
{
    [Table("management_department")]

    public class Department : Entity<int>, IHasCreationTime
    {
        public string Name { get; set; }

        public string Level { get; set; }
        public string Code { get; set; }

        public string Icon { get; set; }

        public int Sort { get; set; }
        public int ParentId { get; set; }
        public DateTime CreationTime { get; set; }
        public Department()
        {
            CreationTime = DateTime.Now;
        }

    }
}
