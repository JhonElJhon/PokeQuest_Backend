using PokeQuestAPI.Classes;
using PokeQuestAPI.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace PokeQuestAPI.QueriesManager
{
    public static class UserQueries
    {
        //private static readonly string connectionString = "Data Source=JHONNOISES\\JHONSMEMORIES;Initial Catalog=PokeQuest;Integrated Security=True";
        //private static readonly string connectionString = "Host=localhost;Username=postgres;Password=postgres1234;Database=PokeQuest;Port=5432";
        private static readonly string connectionString = "Host=ep-cool-block-a810mzkj-pooler.eastus2.azure.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_mpzPHSv1XE7c;SSL Mode=Require;Trust Server Certificate=true";

        /// <summary>
        /// Verifica si algún usuario ya usa el mismo nombre
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> UserExists(string username)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand($"select Nombre from Usuarios where Nombre = @nombre", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@nombre", NpgsqlDbType.Varchar, 100).Value = username;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read()) return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            return false;
        }

        /// <summary>
        /// Verifica si algún usuario ya usa el mismo correo
        /// </summary>
        public static async Task<bool> EmailExists(string email)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand($"select Email from Usuarios where Email = @email", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@email", NpgsqlDbType.Varchar, 100).Value = email;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read()) return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            return false;
        }

        /// <summary>
        /// Añade un nuevo usuario
        /// </summary>
        public static async Task<RegisterUserResultEnum> RegisterNewUser(Register register)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                int avatar = register.Avatar;
                string nombre = register.Username;
                string email = register.Email;
                var result = PasswordHasher.CreateHash(register.Password);
                byte[] contraseña = result.hash;
                byte[] sal = result.salt;
                int puntaje = 0;
                int trivias = 0;
                using (var command = new NpgsqlCommand("INSERT INTO Usuarios(Avatar, Nombre, Email, Contraseña, Sal, Puntaje, Trivias) VALUES(@avatar, @nombre, @email, @contraseña, @sal, @puntaje, @trivias)", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@avatar", NpgsqlDbType.Integer).Value = avatar;
                        command.Parameters.Add("@nombre", NpgsqlDbType.Varchar, 100).Value = nombre;
                        command.Parameters.Add("@email", NpgsqlDbType.Varchar, 100).Value = email;
                        command.Parameters.Add("@contraseña", NpgsqlDbType.Bytea, 256).Value = contraseña;
                        command.Parameters.Add("@sal", NpgsqlDbType.Bytea, 128).Value = sal;
                        command.Parameters.Add("@puntaje", NpgsqlDbType.Integer).Value = puntaje;
                        command.Parameters.Add("@trivias", NpgsqlDbType.Integer).Value = trivias;

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return RegisterUserResultEnum.Error;
                    }
                }
                    
            }

            return RegisterUserResultEnum.Created;
        }
        public static async Task<List<User>> LoginUser(Login login)
        {
            List<User> usuario = new List<User>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                string nombre = login.Username;
                using (var command = new NpgsqlCommand("SELECT * From Usuarios WHERE Nombre = @nombre", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@nombre", NpgsqlDbType.Varchar, 100).Value = nombre;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {

                                usuario.Add(new User
                                {
                                    ID = reader.GetInt32(0),
                                    Avatar = reader.GetInt32(1),
                                    Nombre = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    Contraseña = (byte[])reader[4],
                                    Sal = (byte[])reader[5],
                                    Puntaje = reader.GetInt32(6),
                                    Trivias = reader.GetInt32(7),
                                    FechaInicio = reader.GetDateTime(8)
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
            if (!PasswordHasher.VerifyHash(login.Password, usuario[0].Contraseña, usuario[0].Sal)) return new List<User>();
            return usuario;
        }

        public static async Task<List<User>> GetAllUsers()
        {
            List<User> usuarios = new List<User>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand($"SELECT * From Usuarios", conn))
                {
                    try
                    {
                        conn.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                usuarios.Add(new User
                                {
                                    ID = reader.GetInt32(0),
                                    Avatar = reader.GetInt32(1),
                                    Nombre = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    Contraseña = (byte[])reader[4],
                                    Sal = (byte[])reader[5],
                                    Puntaje = reader.GetInt32(6),
                                    Trivias = reader.GetInt32(7),
                                    FechaInicio = reader.GetDateTime(8)
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

            return usuarios;
        }
        public static async Task<List<User>> GetUser(string username)
        {
            List<User> usuario = new List<User>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                string nombre = username;
                using (var command = new NpgsqlCommand($"SELECT * From Usuarios WHERE Nombre = @nombre", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@nombre", NpgsqlDbType.Varchar, 100).Value = nombre;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                usuario.Add(new User
                                {
                                    ID = reader.GetInt32(0),
                                    Avatar = reader.GetInt32(1),
                                    Nombre = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    Contraseña = (byte[])reader[4],
                                    Sal = (byte[])reader[5],
                                    Puntaje = reader.GetInt32(6),
                                    Trivias = reader.GetInt32(7),
                                    FechaInicio = reader.GetDateTime(8)
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

            return usuario;
        }

        public static async Task<RegisterUserResultEnum> UpdateUser(UpdateModel model)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                string nombreViejo = model.NombreViejo;
                string nombre = model.NombreNuevo;
                int avatar = model.Avatar;
                string email = model.EmailNuevo;
                using (var command = new NpgsqlCommand($"UPDATE Usuarios SET Avatar = @avatar, Nombre = @nombre,Email = @email WHERE Nombre = @nombreViejo", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@nombreViejo", NpgsqlDbType.Varchar, 100).Value = nombreViejo;
                        command.Parameters.Add("@nombre", NpgsqlDbType.Varchar, 100).Value = nombre;
                        command.Parameters.Add("@avatar", NpgsqlDbType.Integer).Value = avatar;
                        command.Parameters.Add("@email", NpgsqlDbType.Varchar, 100).Value = email;

                        command.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return RegisterUserResultEnum.Created;
        }

        public static async Task<RegisterUserResultEnum> AddPoints(AddPointsModel model)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                string nombre = model.Usuario;
                int puntos = model.Puntos;
                using (var command = new NpgsqlCommand($"UPDATE Usuarios SET Puntaje = Puntaje + @puntos, Trivias = Trivias + 1 WHERE Nombre = @nombre", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@nombre", NpgsqlDbType.Varchar, 100).Value = nombre;
                        command.Parameters.Add("@puntos", NpgsqlDbType.Integer).Value = puntos;

                        command.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return RegisterUserResultEnum.Created;
        }

    }
}
