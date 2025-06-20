using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GEDAI.Authorization.Api.Services;
using GEDAI.Authorization.Api.Repositories;
using GEDAI.Authorization.Api.Models;
using System.Reflection.Metadata;

namespace GEDAI.Authorization.Api.Controllers;

[Route("v1/account")]
public class AuthController : ControllerBase
{
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
    {
        return AuthenticateUser(model);
    }

    private ActionResult<dynamic> AuthenticateUser(User model)
    {
        User user = null;
        var threadUser = new Thread(() =>
        {
            user = UserRepository.Get(model.Username, model.Password);
        });

        Console.WriteLine($"Iniciando a Thread para autenticação do usuário {model.Username} início em: {DateTime.Now:O}");
        threadUser.Start();
        threadUser.Join(); // Aguarda a thread terminar antes de acessar user

        if (user == null || user.Id == 0)
            return NotFound(new { message = "Usuário ou senha inválidos" });

        var token = "";
        var threadToken = new Thread(() =>
        {
            token = TokenService.GenerateToken(user);
        });

        threadToken.Start();
        threadToken.Join(); // Aguarda a thread terminar antes de retornar

        Console.WriteLine($"Finalizando a Thread para geração de token do usuário {user.Username} termino em: {DateTime.Now:O}");

        user.Password = "";
        return new
        {
            user = new User
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            },
            token = token
        };
    }
}
