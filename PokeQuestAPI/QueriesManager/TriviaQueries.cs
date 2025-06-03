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
    public class TriviaQueries
    {
        //private static readonly string connectionString = "Data Source=JHONNOISES\\JHONSMEMORIES;Initial Catalog=PokeQuest;Integrated Security=True";
        private static readonly string connectionString = "Host=localhost;Username=postgres;Password=postgres1234;Database=PokeQuest;Port=5432";

        /// <summary>
        /// Obtiene las trivias filtrando por pokemon
        /// </summary>
        public static async Task<List<Trivia>> GetTriviasByPokemon(string pokemon, int cant)
        {
            var filteredTrivias = new List<Trivia>();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand("select t.pokemon, t.pregunta, t.respuestacorrecta, t.respuestasincorrectas, p.sprite from trivias t inner join pokemones p on t.pokemon = p.nombre where t.pokemon ilike @pokemon", conn))
                {
                    try
                    {
                        conn.Open();
                        string sanitizedPokemon = $"{EscapeLikePattern(pokemon)}%";
                        command.Parameters.Add("@pokemon", NpgsqlDbType.Varchar, 30).Value = sanitizedPokemon;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                List<string> respuestas = new List<string>();
                                respuestas.Add(reader.GetString(2));
                                foreach(string respuesta in reader.GetString(3).Split("-"))
                                {
                                    respuestas.Add(respuesta);
                                }
                                Shuffle(respuestas);
                                filteredTrivias.Add(new Trivia
                                {
                                    Pokemon = reader.GetString(0),
                                    Pregunta = reader.GetString(1),
                                    RespuestaCorrecta = reader.GetString(2),
                                    Respuestas = respuestas,
                                    SpriteURL = reader.GetString(4)
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
            Shuffle(filteredTrivias);
            if (cant > filteredTrivias.Count) cant = filteredTrivias.Count;
            filteredTrivias = filteredTrivias.GetRange(0, cant);
            return filteredTrivias;
        }

        /// <summary>
        /// Obtiene las trivias filtrando por pokemon
        /// </summary>
        public static async Task<List<Trivia>> GetTriviasByType(string tipo, int cant)
        {
            var filteredTrivias = new List<Trivia>();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand("select t.pokemon, t.pregunta, t.respuestacorrecta, t.respuestasincorrectas, p.sprite from trivias t inner join pokemones p on t.pokemon = p.nombre where p.tipos ilike @tipo", conn))
                {
                    try
                    {
                        conn.Open();
                        if (tipo.ToLowerInvariant() == "todos") tipo = "";
                        string sanitizedType = $"%{EscapeLikePattern(tipo)}%";
                        command.Parameters.Add("@tipo", NpgsqlDbType.Varchar, 22).Value = sanitizedType;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                List<string> respuestas = new List<string>();
                                respuestas.Add(reader.GetString(2));
                                foreach (string respuesta in reader.GetString(3).Split("-"))
                                {
                                    respuestas.Add(respuesta);
                                }
                                Shuffle(respuestas);
                                filteredTrivias.Add(new Trivia
                                {
                                    Pokemon = reader.GetString(0),
                                    Pregunta = reader.GetString(1),
                                    RespuestaCorrecta = reader.GetString(2),
                                    Respuestas = respuestas,
                                    SpriteURL = reader.GetString(4)
                                }); ;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            Shuffle(filteredTrivias);
            if (cant > filteredTrivias.Count) cant = filteredTrivias.Count;
            filteredTrivias = filteredTrivias.GetRange(0, cant);
            return filteredTrivias;
        }

        private static void Shuffle(List<string> lista)
        {
            Random rng = new Random();
            int n = lista.Count;
            while(n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                string temp = lista[k];
                lista[k] = lista[n];
                lista[n] = temp;
            }
        }

        private static void Shuffle(List<Trivia> lista)
        {
            Random rng = new Random();
            int n = lista.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Trivia temp = lista[k];
                lista[k] = lista[n];
                lista[n] = temp;
            }
        }

        private static string EscapeLikePattern(string input)
        {
            return input.Replace("\\", "\\\\")
                       .Replace("%", "\\%")
                       .Replace("_", "\\_");
        }
    }
}
