using System;
using Microsoft.Data.SqlClient;
using System.Linq;
namespace Add_Minion
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Server =.\\SQLEXPRESS; Database = MinionsDB; Integrated Security = true; TrustServerCertificate = True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var minionInfo = Console.ReadLine().Split(' ');
                var villian = Console.ReadLine().Split(" ")[1];
                var comandGetTowns = new SqlCommand(@"SELECT Name FROM Towns", connection);
                var townsId = 0;
                var idVillian = 0;

                using(SqlDataReader reader = comandGetTowns.ExecuteReader())
                {
                    bool hasTown = false;
                    while (reader.Read())
                    {
                        townsId++;
                        var town = ((string)reader[0]).ToLower();
                        var newTown = minionInfo[3].ToLower();
                        if(newTown == town)
                        {
                            hasTown = true;
                            
                        }   
                    }
                    reader.Close();
                    if (hasTown == false)
                    {
                        var townName = minionInfo[3];
                        townsId++;
                        var cmdCreateTown = new SqlCommand(@"INSERT INTO Towns(Name) VALUES (@town)", connection);
                        cmdCreateTown.Parameters.AddWithValue("@town", townName);
                        cmdCreateTown.ExecuteNonQuery();
                        Console.WriteLine($"Town {minionInfo[3]} was added to the database.");
                    }
                    
                }

                var comandGetVillians = new SqlCommand("SELECT Name FROM Villains", connection);
                using (SqlDataReader reader = comandGetVillians.ExecuteReader())
                {
                    bool hasVillian = false;
                    while (reader.Read())
                    {
                        idVillian++;
                        var curVillian = ((string)reader[0]).ToLower();
                        var newVillian = villian;
                        if (newVillian == curVillian)
                        {
                            hasVillian = true;
                            break;
                        }
                    }
                    reader.Close();
                    if (hasVillian == false)
                    {
                        idVillian++;
                        var cmdCreateVillian = new SqlCommand(@"INSERT INTO Villains(Name,EvilnessFactorId) VALUES (@villian,4)", connection);
                        cmdCreateVillian.Parameters.AddWithValue("@villian", villian);
                        cmdCreateVillian.ExecuteNonQuery();
                        Console.WriteLine($"Villain {villian} was added to the database.");
                    }
                    
                }
                var cmdMinionid = new SqlCommand(@"SELECT Id FROM Minions WHERE Name = @Name", connection);
                cmdMinionid.Parameters.AddWithValue("@name", minionInfo[1]);
                var minionId = cmdMinionid.ExecuteScalar();

                var comandCreateMinion = new SqlCommand(@"INSERT INTO Minions(Name,Age,TownId) VALUES (@Name,@Age,@townId)", connection);
                comandCreateMinion.Parameters.AddWithValue("@Name",minionInfo[1]);
                comandCreateMinion.Parameters.AddWithValue("@Age",minionInfo[2]);
                comandCreateMinion.Parameters.AddWithValue("townId",townsId);
                comandCreateMinion.ExecuteNonQuery();
                var connectMinionToVillian = new SqlCommand(@"INSERT INTO MinionsVillains (MinionId,VillainId) VALUES (@MinionId,@VillainId)", connection);
                connectMinionToVillian.Parameters.AddWithValue("@MinionId", (int)minionId);
                connectMinionToVillian.Parameters.AddWithValue("@VillainId",idVillian);
                Console.WriteLine($"Successfully added {minionInfo[1]} to be minion of {villian}.");
            }
        }
    }
}
