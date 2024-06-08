using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Final
{
    internal class ConexionBD
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al abrir la conexión: " + ex.Message);
            }

            return connection;
        }

        public static void CloseConnection(SqlConnection connection)
        {
            try
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cerrar la conexión: " + ex.Message);
            }
        }
    }
}
