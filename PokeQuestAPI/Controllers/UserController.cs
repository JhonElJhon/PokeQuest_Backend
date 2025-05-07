using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokeQuestAPI.QueriesManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokeQuestAPI.Classes;
using PokeQuestAPI.Enums;

namespace PokeQuestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Crear un nuevo usuario o retornar un error si el nombre de usuario o correo ya existe
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterNewUser([FromBody] Register register)
        {
            try
            {
                if (await UserQueries.UserExists(register.Username)) return BadRequest("Este nombre de usuario ya existe");

                if (await UserQueries.EmailExists(register.Email)) return BadRequest("Este correo ya está en uso");

                RegisterUserResultEnum result = new RegisterUserResultEnum();
                result = await UserQueries.RegisterNewUser(register);
                return result == RegisterUserResultEnum.Created ? Ok(result) : StatusCode(500, "Ha habido un error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pokemons");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] Login login)
        {
            try
            {
                if (!await UserQueries.UserExists(login.Username)) return BadRequest("Este nombre de usuario no existe");
                List<User> result = new List<User>();
                result = await UserQueries.LoginUser(login);
                return result.Count > 0 ? Ok(result) : BadRequest("Contraseña incorrecta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pokemons");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}

