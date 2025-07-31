using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo.Presentation.Controllers;
//[Authorize]
public class EmployeesController(IEmployeeService EmployeeService,
    IWebHostEnvironment environment,
    ILogger<EmployeesController> logger)
    : Controller
{
    private readonly IEmployeeService _EmployeeService = EmployeeService;
    private readonly IWebHostEnvironment _environment = environment;
    private readonly ILogger<EmployeesController> _logger = logger;

    #region Index => Home Page  
    // All Employees 
    [HttpGet]
    public async Task<IActionResult> Index(string? SearchValue)
    {
        // 1. 
        var Employees = await _EmployeeService.GetAllAsync(SearchValue);
        // 2. Send Model To View 
        return View(Employees);
    }
    #endregion

    #region Create 
    [HttpGet]
    public async Task<IActionResult> Create([FromServices] IDepartmentService departmentService)
    {
        var departments = await departmentService.GetAllAsync();

        var items = new SelectList(departments,
            nameof(DepartmentResponse.Id),
            nameof(DepartmentResponse.Name));

        ViewBag.Departments = items;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeRequest request)
    {
        //1. Server Side Validation 
        //Invalid Model State =>  
        if (!ModelState.IsValid) return View(request);
        // Valid Model State 

        try
        {
            var result = await _EmployeeService.AddAsync(request);
            // If Employee is Created => RedirectToAction Index 
            if (result > 0) return RedirectToAction(nameof(Index));
            // else 

            ModelState.AddModelError(string.Empty, "Can't Create Employee Now");
            return View(request);

        }
        catch (Exception ex)
        {

            // Prod 
            // Log Error 
            // Return F M 
            if (_environment.IsProduction())
            {
                _logger.LogError(ex.Message);

                ModelState.AddModelError(string.Empty, "Can't Create Employee Now");
                return View(request);
            }

            // Dev 
            // Return Ex Details 
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(request);
        }

    }

    #endregion

    #region Details 
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (!id.HasValue) return BadRequest(); //400

        var Employee = await _EmployeeService.GetByIdAsync(id.Value);

        if (Employee is null) return NotFound(); // 404

        return View(Employee);
    }
    #endregion

    #region Edit
    [HttpGet]
    public async Task<IActionResult> Edit(int? id, [FromServices] IDepartmentService departmentService)
    {
        if (!id.HasValue) return BadRequest(); //400

        var Employee = await _EmployeeService.GetByIdAsync(id.Value);
        if (Employee is null) return NotFound(); // 404

        var employeeRequest = new EmployeeUpdateRequest
        {
            Address = Employee.Address,
            Age = Employee.Age,
            Email = Employee.Email,
            EmployeeType = Enum.Parse<EmployeeType>(Employee.EmployeeType), // string => Enum (Employee Type)
            Gender = Enum.Parse<Gender>(Employee.Gender),
            HiringDate = Employee.HiringDate,
            Id = Employee.Id,
            IsActive = Employee.IsActive,
            Name = Employee.Name,
            PhoneNumber = Employee.PhoneNumber,
            Salary = Employee.Salary
        };
        var departments = await departmentService.GetAllAsync();

        var items = new SelectList(departments,
            nameof(DepartmentResponse.Id),
            nameof(DepartmentResponse.Name));

        ViewBag.Departments = items;
        return View(employeeRequest);
    }
    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] int id, EmployeeUpdateRequest request)
    {

        if (id != request.Id) return BadRequest();
        //Invalid Model State =>  
        if (!ModelState.IsValid) return View(request);
        // Valid Model State 

        try
        {
            var result = await _EmployeeService.UpdateAsync(request);
            // If Employee is Created => RedirectToAction Index 
            if (result > 0) return RedirectToAction(nameof(Index));
            // else 

            ModelState.AddModelError(string.Empty, "Can't Create Employee Now");
            return View(request);

        }
        catch (Exception ex)
        {

            // Prod 
            // Log Error 
            // Return F M 
            if (_environment.IsProduction())
            {
                _logger.LogError(ex.Message);

                ModelState.AddModelError(string.Empty, "Can't Create Employee Now");
                return View(request);
            }

            // Dev 
            // Return Ex Details 
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(request);
        }
    }
    #endregion

    #region Delete 
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (!id.HasValue) return BadRequest(); //400

        var Employee = await _EmployeeService.GetByIdAsync(id.Value);
        if (Employee is null) return NotFound(); // 404
        return View(Employee); //EmployeeDetailsResponse
    }
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> ConfirmDelete(int? id)
    {
        if (!id.HasValue) return BadRequest(); //400


        try
        {
            var result = await _EmployeeService.DeleteAsync(id.Value);
            // If Employee is Created => RedirectToAction Index 
            if (result) return RedirectToAction(nameof(Index));
            // else 

            //ModelState.AddModelError(string.Empty, "Can't Create Employee Now");
            //return View(request);

            /// TODO 
            /// Send Data TO Index Action TO Return it to Index View 
            return RedirectToAction(nameof(Index));


        }
        catch (Exception ex)
        {

            // Prod 
            // Log Error 
            // Return F M 
            if (_environment.IsProduction())
            {
                _logger.LogError(ex.Message);

                ModelState.AddModelError(string.Empty, "Can't Create Employee Now");

            }

            // Dev 
            // Return Ex Details 
            //ModelState.AddModelError(string.Empty, ex.Message);
            //return View(request);


            /// TODO 
            /// Send Data TO Index Action TO Return it to Index View 
            return RedirectToAction(nameof(Index));


        }


    }
    #endregion
}
