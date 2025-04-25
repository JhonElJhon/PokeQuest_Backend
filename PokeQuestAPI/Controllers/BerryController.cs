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
    public class BerryController : ControllerBase
    {
        private readonly ILogger<BerryController> _logger;

        public BerryController(ILogger<BerryController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Obtener los nombres de las frutas filtrando por letras que haya escrito el usuario y el tipo seleccionado
        /// </summary>
        /// <param name="term"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [Route("getBerriesByFilter/{term}/{tipo}")]
        [HttpGet]
        public async Task<IActionResult> GetBerriesByFilter(string term, string tipo)
        {
            try
            {
                List<Berry> result = new();

                if (string.IsNullOrWhiteSpace(term) && string.IsNullOrWhiteSpace(tipo))
                {
                    result = await BerryQueries.GetAllBerries();
                }

                if (!string.IsNullOrWhiteSpace(term))
                {
                    if (!string.IsNullOrWhiteSpace(tipo))
                    {
                        result = await BerryQueries.FilterBerriesByNameAndType(
                            term.ToLowerInvariant(), tipo);
                    }
                    else
                    {
                        result = await BerryQueries.FilterBerriesByName(
                            term.ToLowerInvariant());
                    }
                }
                if (string.IsNullOrWhiteSpace(term) && !string.IsNullOrWhiteSpace(tipo))
                {
                    result = await BerryQueries.FilterBerriesByType(tipo);
                }

                return result.Count > 0 ? Ok(result) : StatusCode(500, "Ha habido un error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving berries");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}

