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
    public class PokemonController : ControllerBase
    {
		private readonly ILogger<PokemonController> _logger;

		public PokemonController(ILogger<PokemonController> logger)
		{
			_logger = logger;
		}
		/// <summary>
		/// Obtener los nombres de los pokemones filtrando por letras que haya escrito el usuario y el tipo seleccionado
		/// </summary>
		/// <param name="term"></param>
		/// <param name="tipo"></param>
		/// <returns></returns>
		[Route("getPokemonsByFilter/{term}/{tipo}")]
		[HttpGet]
		public async Task<IActionResult> GetPokemonsByFilter(string term, string tipo)
		{
            try
            {
                if (string.IsNullOrWhiteSpace(term) && string.IsNullOrWhiteSpace(tipo))
                {
                    return BadRequest("Debe suministrarse al menos un término del nombre o el tipo de pokemon");
                }

                List<Pokemon> result = new();

                if (!string.IsNullOrWhiteSpace(term))
                {
                    if (!string.IsNullOrWhiteSpace(tipo))
                    {
                        result = await Queries.FilterPokemonsByNameAndType(
                            term.ToLowerInvariant(),
                            tipo.ToLowerInvariant());
                    }
                    else
                    {
                        result = await Queries.FilterPokemonsByName(
                            term.ToLowerInvariant());
                    }
                }

                return result.Count > 0 ? Ok(result) : StatusCode(500, "Mano, revise esa query");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pokemons");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}

