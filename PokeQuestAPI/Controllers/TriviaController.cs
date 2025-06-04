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
    public class TriviaController : ControllerBase
    {
        private readonly ILogger<TriviaController> _logger;

        public TriviaController(ILogger<TriviaController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Obtener las trivias filtrando por el nombre del pokemon, tipo y cantidad seleccionada
        /// </summary>
        /// <param name="pokemon"></param>
        /// <param name="tipo"></param>
        /// <param name="cant"></param>
        /// <returns></returns>
        [Route("getTriviasByFilter/{pokemon}/{tipo}/{cant}")]
        [HttpGet]
        public async Task<IActionResult> GetPokemonsByFilter(string pokemon, string tipo, string cant)
        {
            try
            {
                List<Trivia> result = new();
                int cantidad = int.Parse(cant);

                if (!string.IsNullOrWhiteSpace(pokemon))
                {
                    result = await TriviaQueries.GetTriviasByPokemon(pokemon, cantidad);
                }

                else
                {
                    result = await TriviaQueries.GetTriviasByType(tipo, cantidad);
                }

                return result.Count > 0 ? Ok(result) : BadRequest("No hay trivias de este tipo o pokemon");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving trivias");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}

