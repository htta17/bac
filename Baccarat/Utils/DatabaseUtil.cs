using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midas.Utils
{
    public static class DatabaseUtil
    {        
        public static string ConnectionString { get; set; }
        public static SqlDataReader SelectQuery(string commandString)
        {
            var connection = new SqlConnection(ConnectionString);
            try
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = commandString;
                SqlDataReader reader = command.ExecuteReader();
                connection.Close();
                return reader;
            }
            catch
            {
                return null;
            }
        }

        public static bool TestConnect()
        {
            var connection = new SqlConnection(ConnectionString);
            try
            {
                connection.Open();
                connection.Close();
                return true;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }
        public static int ExecuteNonQuery(string commandString, params SqlParameter[] parameters)
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = commandString;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            var result = command.ExecuteNonQuery();
            connection.Close();
            connection.Dispose();
            return result;
        }

        public static object ExecuteScalar(string commandString, params SqlParameter[] parameters)
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = commandString;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            var result = command.ExecuteScalar();
            connection.Close();
            connection.Dispose();
            return result;
        }

        public static DataTable SelectQueryCommand(string commandString, params SqlParameter[] parameters)
        {
            var finalResult = new DataTable();
            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = commandString;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            var adapter = new SqlDataAdapter(command);
            adapter.Fill(finalResult);

            connection.Close();
            return finalResult;
        }
    }
}
