using PokeQuestAPI.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PokeQuestAPI.QueriesManager
{
    public static class BerryQueries
    {
        private static readonly string connectionString = "Data Source=JHONNOISES\\JHONSMEMORIES;Initial Catalog=PokeQuest;Integrated Security=True";

        /// <summary>
        /// Obtener todas las frutas
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Berry>> GetAllBerries()
        {
            var berries = new List<Berry>();
            using (var conn = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"SELECT Frutas.ID, Frutas.Nombre, Tipos.NombreES as Tipo, Frutas.Sprite from Frutas inner join Tipos on Frutas.Tipo = Tipos.ID;", conn))
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

            using (var conn = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"SELECT Frutas.ID, Frutas.Nombre, Tipos.NombreES as Tipo, Frutas.Sprite from Frutas inner join Tipos on Frutas.Tipo = Tipos.ID where Frutas.Nombre like @searchTerm% and Tipos.NombreES = @searchType", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@searchTerm", SqlDbType.VarChar, 11).Value = searchTerm;
                        command.Parameters.Add("@searchType", SqlDbType.VarChar, 22).Value = searchType;
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
            using (var conn = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"SELECT Frutas.ID, Frutas.Nombre, Tipos.NombreES as Tipo, Frutas.Sprite from Frutas inner join Tipos on Frutas.Tipo = Tipos.ID where Frutas.Nombre like @searchTerm%", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@searchTerm", SqlDbType.VarChar, 11).Value = searchTerm;
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
            using (var conn = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"SELECT Frutas.ID, Frutas.Nombre, Tipos.NombreES as Tipo, Frutas.Sprite from Frutas inner join Tipos on Frutas.Tipo = Tipos.ID where Tipos.NombreES = @searchType", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@searchType", SqlDbType.VarChar, 22).Value = searchType;
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
    }
}
