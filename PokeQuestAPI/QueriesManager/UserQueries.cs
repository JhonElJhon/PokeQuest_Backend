using PokeQuestAPI.Classes;
using PokeQuestAPI.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PokeQuestAPI.QueriesManager
{
    public static class UserQueries
    {
        private static readonly string connectionString = "Data Source=JHONNOISES\\JHONSMEMORIES;Initial Catalog=PokeQuest;Integrated Security=True";

        /// <summary>
        /// Verifica si algún usuario ya usa el mismo nombre
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> UserExists(string username)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"select Nombre from Usuarios (nolock) where Nombre = '{username}'", conn))
                {
                    try
                    {
                        conn.Open();
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
            using (var conn = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"select Email from Usuarios (nolock) where Email = '{email}'", conn))
                {
                    try
                    {
                        conn.Open();
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
            using (var conn = new SqlConnection(connectionString))
            {
                int avatar = register.Avatar;
                string nombre = register.Username;
                string email = register.Email;
                var result = PasswordHasher.CreateHash(register.Password);
                byte[] contraseña = result.hash;
                byte[] sal = result.salt;
                using (var command = new SqlCommand("INSERT INTO Usuarios(Avatar, Nombre, Email, Contraseña, Sal) VALUES(@avatar, @nombre, @email, @contraseña, @sal)", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@avatar", SqlDbType.Int).Value = avatar;
                        command.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = nombre;
                        command.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = email;
                        command.Parameters.Add("@contraseña", SqlDbType.VarBinary, 256).Value = contraseña;
                        command.Parameters.Add("@sal", SqlDbType.VarBinary, 128).Value = sal;

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
            using (var conn = new SqlConnection(connectionString))
            {
                string nombre = login.Username;
                using (var command = new SqlCommand("SELECT * From Usuarios WHERE Nombre = @nombre", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = nombre;
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
                                    Contraseña = (byte[])reader.GetSqlBinary(4),
                                    Sal = (byte[])reader.GetSqlBinary(5),
                                    FechaInicio = reader.GetDateTime(6),
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

    }
}
