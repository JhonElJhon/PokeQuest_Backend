using PokeQuestAPI.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace PokeQuestAPI.QueriesManager
{
    public static class AbilityQueries
    {
        //private static readonly string connectionString = "Data Source=JHONNOISES\\JHONSMEMORIES;Initial Catalog=PokeQuest;Integrated Security=True";
        private static readonly string connectionString = "Host=localhost;Username=postgres;Password=postgres1234;Database=PokeQuest;Port=5432";

        /// <summary>
        /// Obtener todos las habilidades
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Ability>> GetAllAbilities()
        {
            var abilities = new List<Ability>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand("SELECT * FROM Habilidades", conn))
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
            using (var conn = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand("SELECT * FROM Habilidades WHERE NombreES ilike @searchTerm", conn))
                {
                    try
                    {
                        conn.Open();
                        string sanitizedSearchTerm = $"{EscapeLikePattern(searchTerm)}%";
                        command.Parameters.Add("@searchTerm", NpgsqlDbType.Varchar, 30).Value = sanitizedSearchTerm;
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
        private static string EscapeLikePattern(string input)
        {
            return input.Replace("\\", "\\\\")
                       .Replace("%", "\\%")
                       .Replace("_", "\\_");
        }
    }
}
