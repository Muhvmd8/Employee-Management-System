using Demo.Presentation.Models.Roles;

namespace CMS.PL.Controllers;
[Authorize(Roles = "Admin")]
public class RoleController(RoleManager<IdentityRole> roleManager,
    IWebHostEnvironment environment,
    ILogger<RoleController> logger,
    UserManager<ApplicationUser> userManager)
    : Controller
{
    #region Index
    [HttpGet]
    public IActionResult Index(string? searchInput)
    {
        IEnumerable<RoleDto> users;
        if (string.IsNullOrEmpty(searchInput))
        {
            users = roleManager.Roles.Select(u => new RoleDto
            {
                Id = u.Id,
                Name = u.Name,
            });
        }
        else
        {
            users = roleManager.Roles.Select(u => new RoleDto
            {
                Id = u.Id,
                Name = u.Name,

            }).Where(r => r.Name.ToLower().Contains(searchInput.ToLower()));
        }
        return View(users);
    } 
    #endregion

    #region Details
    [HttpGet]
    public IActionResult Details(string? id)
    {
        if (id is null) return BadRequest();

        var role = roleManager.FindByIdAsync(id).Result;
        if (role is null) return NotFound("Role is not found");

        var roleDto = new RoleDto
        {
            Id = role.Id,
            Name = role.Name
        };

        return View(roleDto);
    }
    #endregion

    #region Create
    [HttpGet]
    public IActionResult Create() => View();
    [HttpPost]
    public IActionResult Create(RoleDto roleDto)
    {
        if (ModelState.IsValid)
        {
            var role = roleManager.FindByNameAsync(roleDto.Name).Result;
            if (role is null)
            {
                role = new IdentityRole
                {
                    Name = roleDto.Name,
                };

                var result = roleManager.CreateAsync(role).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
        }
        return View(roleDto);
    }
    #endregion

    #region Edit
    [HttpGet]
    public IActionResult Edit(string? id)
    {
        if (id is null) return BadRequest();

        var role = roleManager.FindByIdAsync(id).Result;
        if (role is null) return NotFound("Role is not found");

        var roleDto = new RoleDto
        {
            Id = role.Id,
            Name = role.Name
        }; 
        
        return View(roleDto);
    }
    [HttpPost]
    public IActionResult Edit([FromRoute] string? id, RoleDto roleDto)
    {
        if (!ModelState.IsValid) return BadRequest();
        if (id is null || id != roleDto.Id) return BadRequest("Invalid Operation !!");

        try
        {
            var role = roleManager.FindByIdAsync(id).Result;

            if (role is null) return NotFound("Role not found !!");

            role = roleManager.FindByNameAsync(roleDto.Name).Result;
            if (role is not null)
            {
                role.Name = roleDto.Name;

                var result = roleManager.UpdateAsync(role).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            ModelState.AddModelError(string.Empty, "Role is not updated successfully !!");
            return View(role);
        }
        catch (Exception ex)
        {
            if (environment.IsDevelopment())
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(roleDto);
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

        var role = roleManager.FindByIdAsync(id).Result;
        if (role is null) return NotFound("Role is not found");

        var roleDto = new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
        };

        return View(roleDto);
    }
    [HttpPost]
    public IActionResult Delete([FromRoute] string? id, RoleDto roleDto)
    {
        if (!ModelState.IsValid) return BadRequest();
        if (id is null || id != roleDto.Id) return BadRequest("Invalid Operation !!");

        try
        {
            var role = roleManager.FindByIdAsync(id).Result;

            if (role is null) return NotFound("Role not found !!");

            var result = roleManager.DeleteAsync(role).Result;
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Role is not deleted successfully !!");
            return View(role);
        }
        catch (Exception ex)
        {
            if (environment.IsDevelopment())
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                if (ex.HResult == -2146233088)
                {
                    return View("DeleteUserError", roleDto.Name);
                }
                //return View(userDto); -2146233088
            }
            logger.LogError(ex.Message);
            return View("ErrorView");
        }

    }
    #endregion

    #region Add Or Remove User
    [HttpGet]
    public IActionResult AddOrRemoveUser(string id)
    {
        var role = roleManager.FindByIdAsync(id).Result;

        if (role is null) return NotFound();

        var usersInRole = new List<UserInRoleViewModel>();
        var users = userManager.Users.ToList();
        foreach ( var user in users)
        {
            var userInRole = new UserInRoleViewModel
            {
                Id = user.Id,
                UserName = user.UserName
            };

            if (userManager.IsInRoleAsync(user, role.Name).Result)
            {
                userInRole.IsSelected = true;
            }
            else
            {
                userInRole.IsSelected = false;
            }
            usersInRole.Add(userInRole);
        }
        ViewData["RoleId"] = role.Id;
        return View(usersInRole); 
    }

    [HttpPost]
    public async Task<IActionResult> AddOrRemoveUser(string roleId, List<UserInRoleViewModel> users)
    {
        var role = await roleManager.FindByIdAsync(roleId);
        if (role is null) return NotFound();

        if (ModelState.IsValid)
        {
            foreach (var user in users)
            {
                var appUser = await userManager.FindByIdAsync(user.Id);
                if (appUser is not null)
                {
                    var isInRole = await userManager.IsInRoleAsync(appUser, role.Name);

                    if (user.IsSelected && !isInRole)
                    {
                        await userManager.AddToRoleAsync(appUser, role.Name);
                    }
                    else if (!user.IsSelected && isInRole)
                    {
                        await userManager.RemoveFromRoleAsync(appUser, role.Name);
                    }
                }
            }

            return RedirectToAction(nameof(Edit), new { id = roleId });
        }

        return View(users);
    }

    #endregion
}
