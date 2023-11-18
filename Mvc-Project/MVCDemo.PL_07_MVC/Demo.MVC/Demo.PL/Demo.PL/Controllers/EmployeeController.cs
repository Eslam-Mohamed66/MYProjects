using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IActionResult Index(string SearchValue = "", int searchId = 0)
        {
            IEnumerable<Employee> employees;
            IEnumerable<EmployeeViewModel> mappedEmployees;
            if (string.IsNullOrEmpty(SearchValue) && searchId == 0)
            {
                employees = _unitOfWork.EmployeeRepository.GetAll();
                mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            }
            else if (searchId != 0)
            {
                // If only search term is provided, search by name
                employees = _unitOfWork.EmployeeRepository.SearchByDepartmentId(searchId);
              mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

        }
            else 
            {
                employees = _unitOfWork.EmployeeRepository.Search(SearchValue);
                mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            }
            return View(mappedEmployees);

        }




        public IActionResult Create()
        {
            ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
            return View(new EmployeeViewModel());
        }

        [HttpPost]

        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            //employee.Department = _unitOfWork.DepartmentRepository.GetById(employee.DepartmentId);
            // ModelState["Department"].ValidationState = ModelValidationState.Valid;
            if (ModelState.IsValid)
            {
                try
                {
                    //Employee employee = new Employee
                    //{
                    //    Name = employeeVM.Name,
                    //    Address = employeeVM.Address,
                    //    Email = employeeVM.Email,
                    //    Salary = employeeVM.Salary,
                    //    DepartmentId = employeeVM.DepartmentId,
                    //    HireDate = employeeVM.HireDate,
                    //    IsActive = employeeVM.IsActive,
                    //};

                    var employee = _mapper.Map<Employee>(employeeVM);
                    employee.ImageUrl = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                    _unitOfWork.EmployeeRepository.Add(employee);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
            ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();

            return View(employeeVM);
        }

        public IActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var employee = _unitOfWork.EmployeeRepository.GetById(id);
                if (employee == null)
                    return NotFound();

                var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

                return View(employeeViewModel);
            }
            catch (Exception ex)
            {
                
                return RedirectToAction("Error", "Home");
            }
        }
        public IActionResult Update(int? id)
        {

            if (id == null)
                return NotFound();
            ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
            var employee = _unitOfWork.EmployeeRepository.GetById(id);
            if (employee == null)
                return NotFound();

            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

            return View(employeeViewModel);
        }

        [HttpPost]
        public IActionResult Update(int? id, EmployeeViewModel employeeViewModel)
        {
            if (id != employeeViewModel.Id)
                return NotFound();

            try
            {
                if (ModelState.IsValid)
                {
                    employeeViewModel.ImageUrl = DocumentSettings.UploadFile(employeeViewModel.Image, "Images");
                    var employee = _mapper.Map<Employee>(employeeViewModel);
                    _unitOfWork.EmployeeRepository.Update(employee);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(employeeViewModel);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = _unitOfWork.EmployeeRepository.GetById(id);
            if (employee == null)
                return NotFound();
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", "Images");
            var ImagePath = Path.Combine(folderPath, employee.ImageUrl);
            System.IO.File.Delete(ImagePath);
            _unitOfWork.EmployeeRepository.Delete(employee);

            return RedirectToAction("Index");
        }

    }

}
