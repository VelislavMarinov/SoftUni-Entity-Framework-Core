using System;
using Microsoft.Data.SqlClient;

namespace Exercise_VillainNames
{
    class Program
    {
        static void Main(string[] args)
        {

            var connectionString = "Server =.\\SQLEXPRESS; Database = MinionsDB; Integrated Security = true; TrustServerCertificate = True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var findVillians = 
                    @"SELECT V.Name + ' - ' + CAST(COUNT(*) AS VARCHAR(10)) AS [Output]
                            FROM Villains v
	                        JOIN MinionsVillains m ON v.Id = m.VillainId
	                        GROUP BY V.Name
	                        HAVING COUNT(*) > 3
	                        ORDER BY COUNT(*) DESC
                    ";

                var command = new SqlCommand(findVillians, connection);

                using (SqlDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        Console.WriteLine(sqlReader[0]);
                    }
                    
                }
                

                




            }
        }
    }
}