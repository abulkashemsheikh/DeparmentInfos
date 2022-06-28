using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeparmentInfos.Models
{
    public class DepartmentInfo
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual IList<Employee>Employees{ get; set; }
    }
public class DeparmentViewModel
{
    public IEnumerable<DepartmentInfo> departmentInfoVM { get; set; }
    public DepartmentInfo DepartmentInfoEntity { get; set; }
}
        public class Employee
        {
            [Key]
            public int ID { get; set; }
            [Required]
            public string Name { get; set; }
            public string ImagePath { get; set; }
            [NotMapped]
            public IFormFile Image { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime EntityDate { get; set; }
            public bool IsExportable { get; set; }
            [ForeignKey("DepartmentInfo")]
            public int DepartmentInfoID { get; set; }
            public DepartmentInfo DeparmentInfo { get; set; }
        }
   
}
