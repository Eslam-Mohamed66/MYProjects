using AutoMapper;
using Demo.DAL.Entities;
using Demo.PL.Models;

namespace Demo.PL.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModel,Employee>().ReverseMap();
            //CreateMap<Employee ,EmployeeViewModel>();
            CreateMap<DepartmentViewModel,Department>().ReverseMap();
        }
    }
}
