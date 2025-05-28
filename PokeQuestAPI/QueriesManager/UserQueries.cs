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
                using (var command = new SqlCommand($"select Nombre from Usuarios (nolock) where Nombre = @nombre", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = username;
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
                using (var command = new SqlCommand($"select Email from Usuarios (nolock) where Email = @email", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = email;
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
                int puntaje = 0;
                int trivias = 0;
                using (var command = new SqlCommand("INSERT INTO Usuarios(Avatar, Nombre, Email, Contraseña, Sal, Puntaje, Trivias) VALUES(@avatar, @nombre, @email, @contraseña, @sal, @puntaje, @trivias)", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@avatar", SqlDbType.Int).Value = avatar;
                        command.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = nombre;
                        command.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = email;
                        command.Parameters.Add("@contraseña", SqlDbType.VarBinary, 256).Value = contraseña;
                        command.Parameters.Add("@sal", SqlDbType.VarBinary, 128).Value = sal;
                        command.Parameters.Add("@puntaje", SqlDbType.Int).Value = puntaje;
                        command.Parameters.Add("@trivias", SqlDbType.Int).Value = trivias;

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

        public static async Task<List<User>> GetUser(string username)
        {
            List<User> usuario = new List<User>();
            using (var conn = new SqlConnection(connectionString))
            {
                string nombre = username;
                using (var command = new SqlCommand($"SELECT * From Usuarios WHERE Nombre = @nombre", conn))
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
            using (var conn = new SqlConnection(connectionString))
            {
                string nombreViejo = model.NombreViejo;
                string nombre = model.NombreNuevo;
                int avatar = model.Avatar;
                string email = model.EmailNuevo;
                using (var command = new SqlCommand($"UPDATE Usuarios SET Avatar = @avatar, Nombre = @nombre,Email = @email WHERE Nombre = @nombreViejo", conn))
                {
                    try
                    {
                        conn.Open();
                        command.Parameters.Add("@nombreViejo", SqlDbType.VarChar, 100).Value = nombreViejo;
                        command.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = nombre;
                        command.Parameters.Add("@avatar", SqlDbType.Int).Value = avatar;
                        command.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = email;

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
