using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokeQuestAPI.QueriesManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokeQuestAPI.Classes;
using PokeQuestAPI.Enums;
using PokeQuestAPI.Services;

namespace PokeQuestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly JwtService _jwtService;

        public UserController(ILogger<UserController> logger, JwtService jwtService)
        {
            _logger = logger;
            _jwtService = jwtService;
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
                _logger.LogError(ex, "Error during register");
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
                //Si la lista está vacía, entonces no se introdujo una contraseña correcta, de lo contrario, el usuario existe.
                // La lista siempre tiene 0 o 1 elemento
                return result.Count > 0 ? Ok(new
                {
                    Token = _jwtService.GenerateToken(result[0]),
                    UserId = result[0].ID,
                    Username = result[0].Nombre,
                    Email = result[0].Email
                }) : BadRequest("Contraseña incorrecta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [Route("get/{username}")]
        [HttpGet]
        public async Task<IActionResult> GetUser(string username)
        {
            try
            {
                if (!await UserQueries.UserExists(username)) return BadRequest("Este nombre de usuario no existe");
                List<User> result = new List<User>();
                result = await UserQueries.GetUser(username);
                //Si la lista está vacía, entonces no se introdujo una contraseña correcta, de lo contrario, el usuario existe.
                // La lista siempre tiene 0 o 1 elemento
                return result.Count > 0 ? Ok(result[0]) : BadRequest("No se encontró el usuario");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during get method");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [Route("getAllUsers")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                List<User> result = new List<User>();
                result = await UserQueries.GetAllUsers();
                //Si la lista está vacía, entonces no se introdujo una contraseña correcta, de lo contrario, el usuario existe.
                // La lista siempre tiene 0 o 1 elemento
                return result.Count > 0 ? Ok(result) : BadRequest("No se encontró ningún usuario");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during get method");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [Route("update")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateModel model)
        {
            try
            {
                if (model.NombreNuevo != model.NombreViejo)
                {
                    if (!await UserQueries.UserExists(model.NombreViejo)) return BadRequest("Este nombre de usuario no existe");

                    if (await UserQueries.UserExists(model.NombreNuevo)) return BadRequest("Este nombre de usuario ya existe");
                }
                if(model.EmailNuevo != model.EmailViejo)
                {
                    if (await UserQueries.EmailExists(model.EmailNuevo)) return BadRequest("Este correo ya está en uso");
                }
                
                RegisterUserResultEnum result = new RegisterUserResultEnum();
                result = await UserQueries.UpdateUser(model);
                // La lista siempre tiene 0 o 1 elemento
                return result == RegisterUserResultEnum.Created ? Ok() : BadRequest("Hay un error en los datos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [Route("addPoints")]
        [HttpPut]
        public async Task<IActionResult> AddPoints([FromBody] AddPointsModel model)
        {
            try
            {

                if (!await UserQueries.UserExists(model.Usuario)) return BadRequest("Este nombre de usuario no existe");

                RegisterUserResultEnum result = new RegisterUserResultEnum();
                result = await UserQueries.AddPoints(model);
                // La lista siempre tiene 0 o 1 elemento
                return result == RegisterUserResultEnum.Created ? Ok() : BadRequest("Hay un error en los datos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during adition");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}

