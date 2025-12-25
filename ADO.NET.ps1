-- Microsoft sql and configuration and json packages needes first our app.ADD
-- CREATE PROCEDURE AddWallet
-- @Holder varchar(50),
-- @Balance decimal(18,2)
-- AS
-- BEGIN
--     INSERT INTO Wallets (Holder, Balance)
--     VALUES (@Holder, @Balance);
-- END
-- GO

-- CREATE PROCEDURE GetWallet
-- AS
-- BEGIN
--     SELECT * FROM Wallets;
-- END
-- GO

-- C# Code to call the stored procedure form object wallet.
Wallet walletToInsert = new Wallet
{
    Holder = "John Doe",
    Balance = 1000.00M
};

SqlConnection conn = new SqlConnection(config.GetSection("ConnectionStrings:DefaultConnection").Value);

var sql = "INSERT INTO Wallets (Holder, Balance) VALUES (@Holder, @Balance);";

SqlParameter holderParam = new SqlParameter
{
    ParameterName = "@holder",
    SqlDbType = SqlDbType.VarChar,
    Direction = ParameterDirection.Input,
    value = walletToInsert.Holder
};


SqlParameter balanceParameter = new SqlParameter
{
    ParameterName = "@balance",
    SqlDbType = SqlDbType.Decimal,
    Direction = ParameterDirection.Input,
    value = walletToInsert.Balance
};

SqlCommand cmd = new SqlCommand(sql, conn);

cmd.Parameters.Add(holderParam);
cmd.Parameters.Add(balanceParameter);
cmd.CommandType = CommandType.text;

conn.Open();

if (cmd.ExecuteNonQuery()) {
    Console.WriteLine("Wallet inserted successfully.");
}else {
    Console.WriteLine("Error inserting wallet.");
}

conn.Close();

--C# Code to call the UpdateWallet
SqlConnection conn = new SqlConnection(config.GetSection("ConnectionStrings:DefaultConnection").Value);

var sql = "UPDATE Wallets SET Holder = @Holder, Balance = @Balance WHERE Id = @Id;";

SqlParameter idParam = new SqlParameter
{
    ParameterName = "@Id",
    SqlDbType = SqlDbType.Int,
    Direction = ParameterDirection.Input,
    value = 1
};

SqlParameter holderParam = new SqlParameter
{
    ParameterName = "@holder",
    SqlDbType = SqlDbType.VarChar,
    Direction = ParameterDirection.Input,
    value = "Jane Doe"
};

SqlParameter balanceParameter = new SqlParameter
{
    ParameterName = "@balance",
    SqlDbType = SqlDbType.Decimal,
    Direction = ParameterDirection.Input,
    value = 2000.20
};

SqlCommand cmd = new SqlCommand(sql, conn);
cmd.Parameters.Add(idParam);
cmd.Parameters.Add(holderParam);
cmd.Parameters.Add(balanceParameter);
cmd.CommandType = CommandType.Text;

conn.Open();
if (cmd.ExecuteNonQuery() > 0) {
    Console.WriteLine("Wallet updated successfully.");
}else {
    Console.WriteLine("Error updating wallet.");
}

conn.Close();

--- C# Code to call the GetWallet stored procedure and read data.
SqlConnection conn = new SqlConnection(config.GetSection("ConnectionStrings:DefaultConnection").Value);

SqlCommand cmd = new SqlCommand("GetWallet", conn);
cmd.CommandType = CommandType.StoredProcedure;

conn.Open();
SqlDataReader reader = cmd.ExecuteReader();
while (reader.Read())
{
    var wallet = new Wallet
    {
        Id = Convert.ToInt32(reader["Id"]),
        Holder = reader["Holder"].ToString(),
        Balance = Convert.ToDecimal(reader["Balance"])
    };

    Console.WriteLine(wallet);
}
conn.Close();

--data Adapter approach

SqlConnection conn = new SqlConnection(config.GetSection("ConnectionStrings:DefaultConnection").Value);

var sql = "SELECT * FROM Wallets";

conn.Open();

SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

DataTable walletTable = new DataTable();

adapter.Fill(walletTable);

conn.Close();

foreach (DataRow row in walletTable.Rows)
{
    var wallet = new Wallet
    {
        Id = Convert.ToInt32(row["Id"]),
        Holder = row["Holder"].ToString(),
        Balance = Convert.ToDecimal(row["Balance"])
    };

    Console.WriteLine(wallet);
}

Console.ReadKey();

-- C# Code to delete a wallet by id.
SqlConnection conn = new SqlConnection(config.GetSection("ConnectionStrings:DefaultConnection").Value);

var sql = "DELETE FROM Wallets WHERE Id = @Id;";

SqlParameter idParam = new SqlParameter
{
    ParameterName = "@Id",
    SqlDbType = SqlDbType.Int,
    Direction = ParameterDirection.Input,
    value = 1
};

SqlCommand cmd = new SqlCommand(sql, conn);
cmd.Parameters.Add(idParam);
cmd.CommandType = CommandType.Text;
conn.Open();
if (cmd.ExecuteNonQuery() > 0) {
    Console.WriteLine("Wallet deleted successfully.");
}else {
    Console.WriteLine("Error deleting wallet.");
}
conn.Close();

--- C# trasaction example
SqlConnection conn = new SqlConnection(config.GetSection("ConnectionStrings:DefaultConnection").Value);

SqlCommand command = conn.CreateCommand();

command.CommandType = CommandType.Text;

conn.Open();

SqlTransaction transaction = conn.BeginTransaction();
command.Transaction = transaction;

try
{
    command.CommandText = "UPDATE Wallets SET Balance = Balance - 100.00 WHERE Id = 1;";
    command.ExecuteNonQuery();

    command.CommandText = "UPDATE Wallets SET Balance = Balance + 100.00 WHERE Id = 2;";
    command.ExecuteNonQuery();

    transaction.Commit();
    Console.WriteLine("Both wallets updated successfully.");
}
catch (Exception ex)
{
    transaction.Rollback();

    Console.WriteLine("Error occurred. Rolling back transaction. " + ex.Message);
    transaction.Rollback();
}
finally
{
    conn.Close();
}


-- dapper example
using Dapper;

IDbConnection db = new SqlConnection(config.GetSection("ConnectionStrings:DefaultConnection").Value);

var sql = "SELECT * FROM Wallets";

var wallets = db.Query<Wallet>(sql);

foreach (var wallet in wallets)
    Console.WriteLine(wallet);

Console.ReadKey();

-- Dapper insert example
using Dapper;

IDbConnection db = new SqlConnection(config.GetSection("ConnectionStrings:DefaultConnection").Value);

var sql = "INSERT INTO Wallets (Holder, Balance) VALUES (@Holder, @Balance);";

var walletToInsert = new Wallet
{
    Holder = "Dapper User",
    Balance = 500.00M
};

var rowsAffected = db.Execute(sql, walletToInsert);

Console.ReadKey();

-- Dapper stored procedure example
using Dapper;

IDbConnection db = new SqlConnection(config.GetSection("ConnectionStrings:DefaultConnection").Value);

var sql = "INSERT INTO Wallets (Holder, Balance) VALUES (@Holder, @Balance);
SELECT CAST(SCOPE_IDENTITY() as int);";

var walletToInsert = new Wallet
{
    Holder = "Dapper User",
    Balance = 500.00M
};
var newId = db.Query<int>(sql, walletToInsert).Single();

Console.ReadKey();

-- Dapper update example
using Dapper;
IDbConnection db = new SqlConnection(config.GetSection("ConnectionStrings:DefaultConnection").Value);

var walletToInsert = new Wallet { Id = 9, Holder = "Updated User", Balance = 750.00M };

var sql = "UPDATE Wallets SET Holder = @Holder, Balance = @Balance WHERE Id = @Id;";

var parametes = new {
        walletToInsert.Holder,
        walletToInsert.Balance,
        walletToInsert.Id
    };

var rowsAffected = db.Execute(sql, parametes);
Console.ReadKey();

-- Dapper delete example
using Dapper;
IDbConnection db = new SqlConnection(config.GetSection("ConnectionStrings:DefaultConnection").Value);

var sql = "DELETE FROM Wallets WHERE Id = @Id;";
var parameters = new { Id = 9 };
db.Execute(sql, parameters);
Console.ReadKey();

-- using Dapper QueryMultiple example
using Dapper;

IDbConnection db = new SqlConnection(config.GetSection("ConnectionStrings:DefaultConnection").Value);

var sql = "SELECT MIN(Balance) FROM Wallets;" +
          "SELECT MAX(Balance) FROM Wallets;";

var results = db.QueryMultiple(sql);
# two ways to read the results

# Console.WriteLine($"Min = {results.ReadSingle<decimal>()}" +
#                   $"\nMax = {results.ReadSingle<decimal>()}");

Console.WriteLine($"Min = {results.Read<decimal>().Single()}" +
                  $"\nMax = {results.Read<decimal>().Single()}");

-- Dapper transaction example
using Dapper;

IDbConnection db = new SqlConnection(config.GetSection("ConnectionStrings:DefaultConnection").Value);

decimal amountTotransfer = 2200m;

using (var transactionScope = new TransactionScope())
{
    // Freddie
    var WalletFrom = db.QuerySingle<Wallet>("SELECT * FROM Wallets WHERE Id = @Id", new { Id = 8 });

    // George
    var WalletTo = db.QuerySingle<Wallet>("SELECT * FROM Wallets WHERE Id =@Id", new { Id = 7 });

    db.Execute("UPDATE Wallets SET Balance = @Balance WHERE Id = @Id",
    new {
        Id = WalletFrom.Id,
        Balance = WalletFrom.Balance - amountTotransfer
    });

    db.Execute("UPDATE Wallets SET Balance = @Balance WHERE Id = @Id",
    new {
        Id = WalletTo.Id,
        Balance = WalletTo.Balance + amountTotransfer
    });

    transactionScope.Complete();
}
--------------------------------------------------------------------
