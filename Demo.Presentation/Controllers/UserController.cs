namespace CMS.PL.Controllers;
public class UserController(UserManager<ApplicationUser> userManager,
    IWebHostEnvironment environment,
    ILogger<UserController> logger)
    : Controller
{
    #region Index
    [HttpGet]
    public IActionResult Index(string? searchInput)
    {
        IEnumerable<UserDto> users;
        if (string.IsNullOrEmpty(searchInput))
        {
            users = userManager.Users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                UserName = u.UserName,
                Email = u.Email,
                Roles = userManager.GetRolesAsync(u).Result,
            });
        }
        else
        {
            users = userManager.Users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                UserName = u.UserName,
                Email = u.Email,
                Roles = userManager.GetRolesAsync(u).Result,
            }).Where(u => u.FullName.ToLower().Contains(searchInput.ToLower()));
        }
        return View(users);
    } 
    #endregion

    #region Details
    [HttpGet]
    public IActionResult Details(string? id)
    {
        if (id is null) return BadRequest();

        var user = userManager.FindByIdAsync(id).Result;
        if (user is null) return NotFound("User is not found");

        var userDto = new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            UserName = user.UserName,
            Email = user.Email,
            Roles = userManager.GetRolesAsync(user).Result
        };

        return View(userDto);
    } 
    #endregion

    #region Edit
    [HttpGet]
    public IActionResult Edit(string? id)
    {
        if (id is null) return BadRequest();

        var user = userManager.FindByIdAsync(id).Result;
        if (user is null) return NotFound("User is not found");

        var userDto = new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            UserName = user.UserName,
            Email = user.Email,
            Roles = userManager.GetRolesAsync(user).Result
        };

        return View(userDto);
    }
    [HttpPost]
    public IActionResult Edit([FromRoute] string? id, UserDto userDto)
    {
        if (!ModelState.IsValid) return BadRequest();
        if (id is null || id != userDto.Id) return BadRequest("Invalid Operation !!");

        try
        {
            var user = userManager.FindByIdAsync(id).Result;

            if (user is null) return NotFound("User not found !!");

            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            user.FullName = userDto.FullName;
            user.Id = userDto.Id;
            userManager.GetRolesAsync(user);

            var result = userManager.UpdateAsync(user).Result;
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "User is not updated successfully !!");
            return View(user);
        }
        catch (Exception ex)
        {
            if (environment.IsDevelopment())
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(userDto);
            }
            logger.LogError(ex.Message);
            return View("ErrorView");
        }

    }
    #endregion

    #region Delete
    [HttpGet]
    public IActionResult Delete(string? id)
    {
        if (id is null) return BadRequest();

        var user = userManager.FindByIdAsync(id).Result;
        if (user is null) return NotFound("User is not found");

        var userDto = new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            UserName = user.UserName,
            Email = user.Email,
            Roles = userManager.GetRolesAsync(user).Result
        };

        return View(userDto);
    }
    [HttpPost]
    public IActionResult Delete([FromRoute] string? id, UserDto userDto)
    {
        if (!ModelState.IsValid) return BadRequest();
        if (id is null || id != userDto.Id) return BadRequest("Invalid Operation !!");

        try
        {
            var user = userManager.FindByIdAsync(id).Result;

            if (user is null) return NotFound("User not found !!");

            var result = userManager.DeleteAsync(user).Result;
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "User is not deleted successfully !!");
            return View(user);
        }
        catch (Exception ex)
        {
            if (environment.IsDevelopment())
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                if (ex.HResult == -2146233088)
                {
                    return View("DeleteUserError", userDto.UserName);
                }
                //return View(userDto); -2146233088
            }
            logger.LogError(ex.Message);
            return View("ErrorView");
        }

    } 
    #endregion

}
