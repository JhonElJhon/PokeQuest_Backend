using PokeQuestAPI.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PokeQuestAPI.QueriesManager
{
    public static class PokemonQueries
    {
        private static readonly string connectionString = "Data Source=JHONNOISES\\JHONSMEMORIES;Initial Catalog=PokeQuest;Integrated Security=True";

        /// <summary>
        /// Obtener todos los pokemones
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Pokemon>> GetAllPokemons()
        {
            var pokemons = new List<Pokemon>();
            using (var conn = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"select * from Pokemones", conn))
                {
                    try
                    {
                        conn.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {

                                pokemons.Add(new Pokemon
                                {
                                    ID = reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Tipos = reader.GetString(2).Split(" "),
                                    SpriteURL = reader.GetString(3)
                                });
                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            return pokemons;
        }

        /// <summary>
        /// Obtiene los pokemones filtrando por nombre y tipo
        /// </summary>
        public static async Task<List<Pokemon>> FilterPokemonsByNameAndType(string searchTerm, string searchType)
        {
            var filteredPokemons = new List<Pokemon>();

            using (var conn = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"select * from Pokemones (nolock) where Nombre like '{searchTerm}%'", conn))
                {
                    try
                    {
                        conn.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                bool match = false;
                                string[] types = reader.GetString(2).Split(" ");

                                foreach(string type in types)
                                {
                                    if(type.ToLowerInvariant() == searchType)
                                    {
                                        match = true;
                                        break;
                                    }
                                }
                                if(match)
                                {
                                    filteredPokemons.Add(new Pokemon
                                    {
                                        ID = reader.GetInt32(0),
                                        Nombre = reader.GetString(1),
                                        Tipos = types,
                                        SpriteURL = reader.GetString(3)
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }

            return filteredPokemons;
        }

        /// <summary>
        /// Filtrar solo por nombre
        /// </summary>
        public static async Task<List<Pokemon>> FilterPokemonsByName(string searchTerm)
        {
            var filteredPokemons = new List<Pokemon>();
            using (var conn = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"select * from Pokemones (nolock) where Nombre like '{searchTerm}%'", conn))
                {
                    try
                    {
                        conn.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                filteredPokemons.Add(new Pokemon
                                {
                                    ID = reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Tipos = reader.GetString(2).Split(" "),
                                    SpriteURL = reader.GetString(3)
                                });
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return filteredPokemons;
        }

        /// <summary>
        /// Filtrar solo por tipo
        /// </summary>
        public static async Task<List<Pokemon>> FilterPokemonsByType(string searchType)
        {
            var filteredPokemons = new List<Pokemon>();
            using (var conn = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"select * from Pokemones (nolock) where Tipos like '%{searchType}%'", conn))
                {
                    try
                    {
                        conn.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                filteredPokemons.Add(new Pokemon
                                {
                                    ID = reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Tipos = reader.GetString(2).Split(" "),
                                    SpriteURL = reader.GetString(3)
                                });
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return filteredPokemons;
        }
    }
}
