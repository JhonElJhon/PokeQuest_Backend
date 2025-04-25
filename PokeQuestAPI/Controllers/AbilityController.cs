using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokeQuestAPI.QueriesManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokeQuestAPI.Classes;

namespace PokeQuestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AbilityController : ControllerBase
    {
        private readonly ILogger<AbilityController> _logger;

        public AbilityController(ILogger<AbilityController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Obtener los nombres de las habilidades filtrando por letras que haya escrito el usuario
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        [Route("getAbilitiesByFilter/{term}")]
        [HttpGet]
        public async Task<IActionResult> GetAbilitiesByFilter(string term)
        {
            try
            {
                List<Ability> result = new();

                if (string.IsNullOrWhiteSpace(term))
                {
                    result = await AbilityQueries.GetAllAbilities();
                }

                else
                {
                    result = await AbilityQueries.FilterAbilitiesByName(
                            term.ToLowerInvariant());
                }

                return result.Count > 0 ? Ok(result) : StatusCode(500, "Ha habido un error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving abilities");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}

