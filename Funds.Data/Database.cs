using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Funds.Data
{
    public class Database
    {
        private string _connectionString;

        public Database(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Contributor> GetAllCon()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Contributors";
            connection.Open();

            List<Contributor> contributors = new List<Contributor>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                contributors.Add(new Contributor
                {
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    Cell = (int)reader["Cell"],
                    Date = (DateTime)reader["Date"],
                    AlwaysInclude = (bool)reader["AlwaysInclude"],
                    id = (int)reader["id"]

                });

            }
            return contributors;

        }
        public int GetSumDep(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT ISNULL(sum(amount), 0) from Deposits where PersonId=@id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            int sum = (int)cmd.ExecuteScalar();
            return sum;

        }
        public int ThoseWhoGave(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"select count (*) from contributions where Simchaid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            int Count = (int)cmd.ExecuteScalar();
            return Count;

        }
        public int TotalCon()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT COUNT(*) FROM Contributors";
            connection.Open();
            int Count = (int)cmd.ExecuteScalar();
            return Count;

        }
        public int GetSumCon(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT ISNULL(sum(amount), 0) from Contributions where ContributorId=@id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            int sum = (int)cmd.ExecuteScalar();
            return sum;

        }
        public string GetSim(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT ISNULL(name, 0) from Simchos where id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            string name = (string)cmd.ExecuteScalar();
            return name;

        }
        public int GetSimTotal(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT ISNULL(sum(amount), 0) from Contributions where SimchaId=@id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            int total = (int)cmd.ExecuteScalar();
            return total;

        }
        public List<History> GetDep(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * from Deposits where PersonId=@id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();

            List<History> histories = new List<History>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                histories.Add(new History
                {
                    amount = (int)reader["amount"],
                    Date = (DateTime)reader["Date"],
                    Name="Deposit"
                    //id = (int)reader["id"],
                    //personid = (int)reader["personId"]
                });

            }
            return histories;
        }
            public List<History> GetSimByid(int id)
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select * from Contributions c left join simchos s on c.SimchaId=s.Id where c.ContributorId=@id";
                cmd.Parameters.AddWithValue("@id", id);
                connection.Open();

                List<History> histories = new List<History>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    histories.Add(new History
                    {
                        Name = (string)reader["name"],
                        Date = (DateTime)reader["Date"],
                        //id = (int)reader["id"],
                        amount=(int)reader["amount"]
                        
                    });

                }
                return histories;

            }
        public int Add(Contributor contributor)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Contributors VALUES(@FirstName,@LastName,@Cell,@Date,@AlwaysInclude) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@FirstName", contributor.FirstName);
            cmd.Parameters.AddWithValue("@LastName", contributor.LastName);
            cmd.Parameters.AddWithValue("@Cell", contributor.Cell);
            cmd.Parameters.AddWithValue("@Date", contributor.Date);
            cmd.Parameters.AddWithValue("@AlwaysInclude", contributor.AlwaysInclude);
            conn.Open();
            int id = (int)(decimal)cmd.ExecuteScalar();
            return id;
        }

        public void AddDeposit(Deposit deposit)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Deposits VALUES(@PersonId,@Amount,@Date)";
            cmd.Parameters.AddWithValue("@PersonId", deposit.personid);
            cmd.Parameters.AddWithValue("@Amount", deposit.Amount);
            cmd.Parameters.AddWithValue("@Date", deposit.Date);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        public void AddSimcha(Simcha simcha)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Simchos VALUES(@Name,@Date)";
            cmd.Parameters.AddWithValue("@Name", simcha.Name);
            cmd.Parameters.AddWithValue("@Date", simcha.Date);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        public void AddContribution(Contributions contribution)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Contributions VALUES(@SimchaId,@ContributorId,@Amount)";
            cmd.Parameters.AddWithValue("@SimchaId", contribution.simchaId);
            cmd.Parameters.AddWithValue("@ContributorId", contribution.ContributorId);
            cmd.Parameters.AddWithValue("@Amount", contribution.amount);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }

        public List<Simcha> GetAllSim()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Simchos";
            connection.Open();

            List<Simcha> simchos = new List<Simcha>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                simchos.Add(new Simcha
                {
                    Name = (string)reader["Name"],
                    Date = (DateTime)reader["Date"],
                    id = (int)reader["id"]
                });

            }
            return simchos;

        }
        public Contributor GetConById(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Contributors where id=@id";
            cmd.Parameters.AddWithValue("@Id", id);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            Contributor con = new Contributor
            {
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                id=(int)reader["id"]
            };

            return con;
        }
            public void Edit(Contributor contributor)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"Update Contributors set FirstName=@FirstName, LastName=@LastName, Cell=@Cell, Date=@Date, AlwaysInclude=@AlwaysInclude WHERE id=@id";
            cmd.Parameters.AddWithValue("@FirstName", contributor.FirstName);
            cmd.Parameters.AddWithValue("@LastName", contributor.LastName);
            cmd.Parameters.AddWithValue("@Cell", contributor.Cell);
            cmd.Parameters.AddWithValue("@Date", contributor.Date);
            cmd.Parameters.AddWithValue("@AlwaysInclude", contributor.AlwaysInclude);
            cmd.Parameters.AddWithValue("@id", contributor.id);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
    }
        public class Contributor
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Cell { get; set; }
            public bool AlwaysInclude { get; set; }
            public DateTime Date { get; set; }
            public int id { get; set; }
            public int Deposit { get; set; }
        public int balance { get; set; }
        public int x { get; set; }
        }
        public class Deposit
        {
            public int id { get; set; }
        public int personid { get; set; }
        public int Amount { get; set; }
            public DateTime Date { get; set; }
        }
        public class Simcha
        {
            public DateTime Date { get; set; }
            public string Name { get; set; }
            public int id { get; set; }
        public int amount { get; set; }
        public int giver { get; set; }
        public int TotalGiven { get; set; }
        }
    public class History
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public int amount { get; set; }
    }
    public class Contributions
    {
        public int ContributorId { get; set; }
        public int amount { get; set; }
        public int simchaId { get; set; }
        public bool Include { get; set; }
    }
}
