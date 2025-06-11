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
    public static class BerryQueries
    {
        //private static readonly string connectionString = "Data Source=JHONNOISES\\JHONSMEMORIES;Initial Catalog=PokeQuest;Integrated Security=True";
        //private static readonly string connectionString = "Host=localhost;Username=postgres;Password=postgres1234;Database=PokeQuest;Port=5432";
        private static readonly string connectionString = "Host=ep-cool-block-a810mzkj-pooler.eastus2.azure.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_mpzPHSv1XE7c;SSL Mode=Require;Trust Server Certificate=true";

        /// <summary>
        /// Obtener todas las frutas
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Berry>> GetAllBerries()
        {
            var berries = new List<Berry>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand("SELECT Frutas.ID, Frutas.Nombre, Tipos.NombreES as Tipo, Frutas.Sprite from Frutas inner join Tipos on Frutas.Tipo = Tipos.ID;", conn))
                {
                    try
                    {
                        conn.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {

                                berries.Add(new Berry
                                {
                                    ID = reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Tipo = reader.GetString(2),
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
            return berries;
        }

        /// <summary>
        /// Obtiene las frutas filtrando por nombre y tipo
        /// </summary>
        public static async Task<List<Berry>> FilterBerriesByNameAndType(string searchTerm, string searchType)
        {
            var filteredBerries = new List<Berry>();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand("SELECT Frutas.ID, Frutas.Nombre, Tipos.NombreES as Tipo, Frutas.Sprite from Frutas inner join Tipos on Frutas.Tipo = Tipos.ID where Frutas.Nombre ilike @searchTerm and Tipos.NombreES ilike @searchType", conn))
                {
                    try
                    {
                        conn.Open();
                        string sanitizedSearchTerm = $"{EscapeLikePattern(searchTerm)}%";
                        command.Parameters.Add("@searchTerm", NpgsqlDbType.Varchar, 11).Value = sanitizedSearchTerm;
                        command.Parameters.Add("@searchType", NpgsqlDbType.Varchar, 22).Value = searchType;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                
                                filteredBerries.Add(new Berry
                                {
                                    ID = reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Tipo = reader.GetString(2),
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

            return filteredBerries;
        }

        /// <summary>
        /// Filtrar solo por nombre
        /// </summary>
        public static async Task<List<Berry>> FilterBerriesByName(string searchTerm)
        {
            var filteredBerries = new List<Berry>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand($"SELECT Frutas.ID, Frutas.Nombre, Tipos.NombreES as Tipo, Frutas.Sprite from Frutas inner join Tipos on Frutas.Tipo = Tipos.ID where Frutas.Nombre ilike @searchTerm", conn))
                {
                    try
                    {
                        conn.Open();
                        string sanitizedSearchTerm = $"{EscapeLikePattern(searchTerm)}%";
                        command.Parameters.Add("@searchTerm", NpgsqlDbType.Varchar, 11).Value = sanitizedSearchTerm;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                filteredBerries.Add(new Berry
                                {
                                    ID = reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Tipo = reader.GetString(2),
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

            return filteredBerries;
        }

        /// <summary>
        /// Filtrar solo por tipo
        /// </summary>
        public static async Task<List<Berry>> FilterBerriesByType(string searchType)
        {
            var filteredBerries = new List<Berry>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand($"SELECT Frutas.ID, Frutas.Nombre, Tipos.NombreES as Tipo, Frutas.Sprite from Frutas inner join Tipos on Frutas.Tipo = Tipos.ID where Tipos.NombreES ilike @searchType", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@searchType", NpgsqlDbType.Varchar, 22).Value = searchType;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                filteredBerries.Add(new Berry
                                {
                                    ID = reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Tipo = reader.GetString(2),
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

            return filteredBerries;
        }

        private static string EscapeLikePattern(string input)
        {
            return input.Replace("\\", "\\\\")
                       .Replace("%", "\\%")
                       .Replace("_", "\\_");
        }
    }
}
