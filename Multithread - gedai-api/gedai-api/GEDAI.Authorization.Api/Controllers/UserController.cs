using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GEDAI.Authorization.Api.Models;

namespace GEDAI.Authorization.Api.Controllers;

[Route("v1/account")]
[ApiController]
public class UserController : ControllerBase
{
    //For admin Only
    [HttpGet]
    [Route("Admins")]
    [Authorize(Roles = "employee,manager")]
    public IActionResult AdminEndPoint()
    {
        var currentUser = GetCurrentUser();
        var authHeader = Request.Headers["Authorization"].ToString();
        Console.WriteLine($"Authorization Header: {authHeader}");
        // Restante do código
        return Ok(currentUser);
    }
    private User GetCurrentUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var userClaims = identity.Claims;
            return new User
            {
                Username = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value,
                Id = int.Parse(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value)
            };
        }
        return null;
    }

    [HttpGet]
    [Route("anonymous")]
    [AllowAnonymous]
    public string Anonymous() => "Anônimo";

    [HttpGet]
    [Route("authenticated")]
    [Authorize]
    public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

    [HttpGet]
    [Route("employee")]
    [Authorize(Roles = "employee,manager")]
    public string Employee() => "Funcionário";

    [HttpGet]
    [Route("manager")]
    [Authorize(Roles = "manager")]
    public string Manager() => "Gerente";
}