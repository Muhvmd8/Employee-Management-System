namespace Demo.Presentation.Controllers;
public class DepartmentsController(IDepartmentService departmentService,
    IWebHostEnvironment environment,
    ILogger<DepartmentsController> logger)
    : Controller
{
    private readonly IDepartmentService _departmentService = departmentService;
    private readonly IWebHostEnvironment _environment = environment;
    private readonly ILogger<DepartmentsController> _logger = logger;

    #region Index => Home Page  
    // All Departments 
    [HttpGet]


    // ViewData     => ViewDataDictionary<string ,object> 
    // ViewBag      => ViewDataDictionary<string ,object> dynamic 
    // View Data & View Bag References the same Dictionary Object 
    // ViewData["message"] == ViewBag.Message = true 
    // 1. Send Data From Action To View 
    // 2. Send Data From View To PartialView 
    // 3. Send Data From View To LayOut 


    // TempData
    // Send Data from Action to Action 
    // send Data from Action to the view of the next request 
    public async Task<IActionResult> Index()
    {
        // 1. 

        var departments = await _departmentService.GetAllAsync();
        // 2. Send Model To View 
        ViewBag.Message = "Hello From ViewBag";
        return View(departments);
    }
    #endregion

    #region Create 
    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken] // =>  Action Filter
    public async Task<IActionResult> Create(DepartmentRequest request)
    {
        //1. Server Side Validation 
        //Invalid Model State =>  
        if (!ModelState.IsValid) return View(request);
        // Valid Model State 
        string message;
        try
        {
            var result = await _departmentService.AddAsync(request);
            // If Department is Created => RedirectToAction Index 
            if (result > 0) message = $"Department {request.Name} Created ";
            else message = $"can't Create Department {request.Name}  ";
            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
            // else 


        }
        catch (Exception ex)
        {

            // Prod 
            // Log Error 
            // Return F M 
            if (_environment.IsProduction())
            {
                _logger.LogError(ex.Message);

                ModelState.AddModelError(string.Empty, "Can't Create Department Now");
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

        var department = await _departmentService.GetByIdAsync(id.Value);

        if (department is null) return NotFound(); // 404

        return View(department);
    }
    #endregion

    #region Edit
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (!id.HasValue) return BadRequest(); //400

        var department = await _departmentService.GetByIdAsync(id.Value);
        if (department is null) return NotFound(); // 404
        return View(department.ToUpdateRequest()); //DepartmentDetailsResponse
    }
    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] int id, DepartmentUpdateRequest request)
    {

        if (id != request.Id) return BadRequest();
        //Invalid Model State =>  
        if (!ModelState.IsValid) return View(request);
        // Valid Model State 

        try
        {
            var result = await _departmentService.UpdateAsync(request);
            // If Department is Created => RedirectToAction Index 
            if (result > 0) return RedirectToAction(nameof(Index));
            // else 

            ModelState.AddModelError(string.Empty, "Can't Create Department Now");
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

                ModelState.AddModelError(string.Empty, "Can't Create Department Now");
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
    //[HttpGet]
    //public IActionResult Delete(int? id)
    //{
    //    if (!id.HasValue) return BadRequest(); //400

    //    var department = _departmentService.GetById(id.Value);
    //    if (department is null) return NotFound(); // 404
    //    return View(department); //DepartmentDetailsResponse
    //}
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> ConfirmDelete(int? id)
    {
        if (!id.HasValue) return BadRequest(); //400


        try
        {
            var result = await _departmentService.DeleteAsync(id.Value);
            // If Department is Created => RedirectToAction Index 
            if (result) return RedirectToAction(nameof(Index));
            // else 

            //ModelState.AddModelError(string.Empty, "Can't Create Department Now");
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

                ModelState.AddModelError(string.Empty, "Can't Create Department Now");

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
