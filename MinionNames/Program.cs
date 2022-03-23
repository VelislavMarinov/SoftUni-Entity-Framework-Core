using System;
using Microsoft.Data.SqlClient;

namespace MinionNames
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Server =.\\SQLEXPRESS; Database = MinionsDB; Integrated Security = true; TrustServerCertificate = True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var villianCount = "SELECT COUNT(*) FROM Villains";

                var commandGetCount = new SqlCommand(villianCount, connection);
                var count = (int)commandGetCount.ExecuteScalar();
                var id = int.Parse(Console.ReadLine());
                var findVillianMinions =
                    @"SELECT V.Name, mi.Name, mi.Age
                                      FROM Villains v
	                                  JOIN MinionsVillains m ON v.Id = m.VillainId
	                                  JOIN Minions mi ON m.MinionId = mi.Id
	                                  WHERE V.Id = @id
	                                  ORDER BY MI.Name
                    ";

                if (id <= count)
                {
                    var cmd = new SqlCommand(findVillianMinions, connection);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var villain = string.Empty;
                        bool villainIsNotEmpty = true;
                        var minionsCount = 1;
                        while (reader.Read())
                        {
                            if (villainIsNotEmpty)
                            {
                                villain = (string)reader[0];
                                Console.WriteLine($"Villain: {villain}");
                                villainIsNotEmpty = false;
                            }

                            Console.WriteLine($"{minionsCount}. {(string)reader[1]} - {(int)reader[2]}");
                            minionsCount++;
                        }

                    }
                }
                else
                {
                    Console.WriteLine("No villain with ID 10 exists in the database.");
                }
               

            }
        }
    }
}
