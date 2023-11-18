namespace Demo.PL.Models
{
    using System.ComponentModel.DataAnnotations;
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code Is Required")]

        public string Code { get; set; }

        [Required(ErrorMessage = "Name Is Required")]

        [MaxLength(50)]

        public string Name { get; set; }

        public DateTime DateOfCreation { get; set; } = DateTime.Now;
    }
}
