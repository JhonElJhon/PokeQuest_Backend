using PokeQuestAPI.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PokeQuestAPI.QueriesManager
{
    public static class AbilityQueries
    {
        private static readonly string connectionString = "Data Source=JHONNOISES\\JHONSMEMORIES;Initial Catalog=PokeQuest;Integrated Security=True";

        /// <summary>
        /// Obtener todos las habilidades
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Ability>> GetAllAbilities()
        {
            var abilities = new List<Ability>();
            using (var conn = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"SELECT * FROM Habilidades", conn))
                {
                    try
                    {
                        conn.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {

                                abilities.Add(new Ability
                                {
                                    ID = reader.GetInt32(0),
                                    NombreEN = reader.GetString(1),
                                    NombreES = reader.GetString(2)
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
            return abilities;
        }

        /// <summary>
        /// Filtrar solo por nombre
        /// </summary>
        public static async Task<List<Ability>> FilterAbilitiesByName(string searchTerm)
        {
            var filteredAbilities = new List<Ability>();
            using (var conn = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"SELECT * FROM Habilidades WHERE NombreES like '{searchTerm}%'", conn))
                {
                    try
                    {
                        conn.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                filteredAbilities.Add(new Ability
                                {
                                    ID = reader.GetInt32(0),
                                    NombreEN = reader.GetString(1),
                                    NombreES = reader.GetString(2)
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

            return filteredAbilities;
        }

    }
}
