using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Department : BaseEntity
    {
        [Required(ErrorMessage = "Code Is Required")]

        public string Code { get; set; }

		[Required(ErrorMessage ="Name Is Required")]

        [MaxLength(50)]

        public string Name { get; set; }

        public DateTime DateOfCreation { get; set; } = DateTime.Now;
    }
}
