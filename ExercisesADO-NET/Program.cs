using System;
using Microsoft.Data.SqlClient;

namespace ExercisesADO_NET
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Server =.\\SQLEXPRESS; Database = MinionsDB; Integrated Security = true; TrustServerCertificate = True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Inital setup of the Database 
                var crateDatabase = "CREATE DATABASE MinionsDB";
                using (var command = new SqlCommand(crateDatabase, connection))
                {
                    command.ExecuteNonQuery();
                }

                //Create tables for MinionsDB

                var tables = GetCreateTablesFunction();
                foreach (var item in tables)
                {
                    var command = new SqlCommand(item, connection);
                   command.ExecuteNonQuery();
                }

                //Insert atles 5 rows in every table

                var tablesData = InsertInformationsInTables();
                foreach (var item in tablesData)
                {
                    var command = new SqlCommand(item, connection);
                    command.ExecuteNonQuery();
                }


                
            }
        }


        private static string[] GetCreateTablesFunction()
        {
            var tables = new String[]
            {
              "CREATE TABLE Countries(Id INT PRIMARY KEY,Name VARCHAR(MAX))",
              "CREATE TABLE Towns (Id INT PRIMARY KEY,Name VARCHAR(MAX),ContryCode INT REFERENCES Countries(Id))",
              "CREATE TABLE Minions(Id INT PRIMARY KEY,Name VARCHAR(MAX),Age INT,TownId INT REFERENCES Towns(Id))",
              "CREATE TABLE EvilnessFactors (Id INT PRIMARY KEY,Name VARCHAR(MAX))",
              "CREATE TABLE Villains (Id INT PRIMARY KEY,Name VARCHAR(MAX), EvilnessFactorId INT REFERENCES EvilnessFactors(Id))",
              "CREATE TABLE MinionsVillains (MinionId INT REFERENCES Minions(Id),VillainId INT REFERENCES Villains(Id),CONSTRAINT PK_Person PRIMARY KEY (MinionId,VillainId))"
            };

            return tables;
        }


        private static string[] InsertInformationsInTables()
        {
            var inserData = new string[]
            {
                "INSERT INTO Countries(Id,Name) VALUES (1,'BULGARIA'),(2,'GREECE'),(3,'UK'),(4,'NORWAY'),(5,'Macedonia')",
                "INSERT INTO Towns(Id,Name,ContryCode) VALUES (1,'SOFIA',1),(2,'LONDON',3),(3,'Athens',2),(4,'Oslo',4),(5,'Skopje',5)",
                "INSERT INTO Minions(Id,Name,Age,TownId) VALUES (1,'Pesho',20,1),(2,'Gosho',21,4),(3,'Simo',26,2),(4,'Rosi',20,3),(5,'Radi',24,5)",
                "INSERT INTO EvilnessFactors (Id,Name) VALUES (1,'super good'),(2,'good'),(3,'bad'),(4,'evil'),(5,'super evil')",
                "INSERT INTO Villains (Id,Name,EvilnessFactorId) VALUES (1,'Batman',2),(2,'SuperMan',1),(3,'Joker',5),(4,'Thanos',3),(5,'Rosi',4)",
                "INSERT INTO MinionsVillains (MinionId,VillainId) VALUES (1,5),(1,4),(2,4),(3,3),(2,3),(4,1),(5,2)"
            };

            return inserData;

        }
    }
    
}
