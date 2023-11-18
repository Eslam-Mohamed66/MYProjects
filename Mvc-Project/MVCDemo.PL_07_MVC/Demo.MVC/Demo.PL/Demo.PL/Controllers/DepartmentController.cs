using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Demo.PL.Controllers
{
   
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        // private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<DepartmentController> _logger;
       // private readonly IEmployeeRepository _employeeRepository;

        public DepartmentController(
            IMapper mapper,
         //   IDepartmentRepository departmentRepository,
         IUnitOfWork unitOfWork,
            ILogger<DepartmentController> logger)
          //  IEmployeeRepository employeeRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;


            //_departmentRepository = departmentRepository;
            _logger = logger;
          //  _employeeRepository = employeeRepository;

        }

        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
           
            var departmentViewModels = _mapper.Map<IEnumerable<DepartmentViewModel>>(departments);

            return View(departmentViewModels);
            //ViewData["Message"] = "Hello From View Data";

            //ViewBag.MessageBag = "Hello View Bag";

            //TempData.Keep("Message");

          //  return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View(new DepartmentViewModel());
        }
        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentViewModel)
        {
            if (ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(departmentViewModel);
                _unitOfWork.DepartmentRepository.Add(department);
                TempData["Message"] = "Department Created Successfully";
                return RedirectToAction("Index");

            }
            else
            {
                return View(departmentViewModel);
            }
        }

        public IActionResult Details(int? id)
        {
            try
            {
    

                var department = _unitOfWork.DepartmentRepository.GetById(id);

                if (department == null)
                    return NotFound();
                var departmentViewModel = _mapper.Map<DepartmentViewModel>(department);

                return View(departmentViewModel);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }


        }

        public IActionResult Update(int? id)
        {
            if (id is null)
                return NotFound();

            var department = _unitOfWork.DepartmentRepository.GetById(id);
            if (department == null)
                return NotFound();


            var departmentViewModel = _mapper.Map<DepartmentViewModel>(department);

            return View(departmentViewModel);
        }
        [HttpPost]
        public IActionResult Update(int? id, DepartmentViewModel departmentViewModel)
        {
            if (id != departmentViewModel.Id)
                return NotFound();

            try
            {
                if (ModelState.IsValid)
                {
                    var department = _mapper.Map<Department>(departmentViewModel);
                    _unitOfWork.DepartmentRepository.Update(department);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(departmentViewModel);
        }

        public IActionResult Delete(int? id)
        {
            if (id is null)
                return NotFound();
            var department = _unitOfWork.DepartmentRepository.GetById(id);

            if (department == null)
                return NotFound();
            _unitOfWork.DepartmentRepository.Delete(department);

            return RedirectToAction("Index");
        }
    }
}

    

