using System;
using Microsoft.Data.SqlClient;

namespace CSharpDBDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Connecting to the database
            
            using (var connection = new SqlConnection("Server=.\\SQLEXPRESS; Database=SoftUni; Integrated Security=true; TrustServerCertificate=True"))
            {
                connection.Open();
                //Making a query.
                var query = @"SELECT * FROM Employees";
                var sqlCommand = new SqlCommand(query,connection);

                var sqlReader = sqlCommand.ExecuteReader();

                sqlReader.Read();
                Console.WriteLine(sqlReader[1]);
                sqlReader.Read();
                Console.WriteLine(sqlReader[1]);
                sqlReader.Read();
                Console.WriteLine(sqlReader[1]);
                sqlReader.Read();
                Console.WriteLine(sqlReader[1]);
                sqlReader.Read();
                Console.WriteLine(sqlReader[1]);


            }
           
        }
    }
}
