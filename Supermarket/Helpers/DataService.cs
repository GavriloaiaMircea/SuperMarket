using System;
using System.Data.SqlClient;
using System.Windows;
using Supermarket.Models;
using Supermarket.ViewModel;

namespace Supermarket.Helpers
{
    public static class DataService
    {
        private static readonly string connectionString = "Server=DESKTOP-MSEGQK7;Database=SupermarketDB;Integrated Security=True;";

        public static User GetUserByUsernameAndPassword(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username and password are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT NumeUtilizator, TipUtilizator, isDeleted 
                FROM Utilizatori 
                WHERE NumeUtilizator = @NumeUtilizator AND Parola = @Parola";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@NumeUtilizator", username));
                        command.Parameters.Add(new SqlParameter("@Parola", password));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read() && !reader.GetBoolean(reader.GetOrdinal("isDeleted")))
                            {
                                return new User
                                {
                                    Username = reader.GetString(reader.GetOrdinal("NumeUtilizator")),
                                    UserType = reader.GetString(reader.GetOrdinal("TipUtilizator")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("isDeleted"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to login: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null; // Return null if user not found or is deleted or exception occurs
        }

        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT UtilizatorID, NumeUtilizator, Parola, TipUtilizator, isDeleted FROM Utilizatori";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.GetBoolean(reader.GetOrdinal("isDeleted")))
                            {
                                users.Add(new User
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("UtilizatorID")),
                                    Username = reader.GetString(reader.GetOrdinal("NumeUtilizator")),
                                    Password = reader.GetString(reader.GetOrdinal("Parola")),
                                    UserType = reader.GetString(reader.GetOrdinal("TipUtilizator")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("isDeleted"))
                                });
                            }
                        }
                    }
                }
            }
            return users;
        }

        public static void AddUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Utilizatori (NumeUtilizator, Parola, TipUtilizator, isDeleted) VALUES (@NumeUtilizator, @Parola, @TipUtilizator, 0)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NumeUtilizator", user.Username);
                    command.Parameters.AddWithValue("@Parola", user.Password); // Consider using parameterized queries for security
                    command.Parameters.AddWithValue("@TipUtilizator", user.UserType);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteUser(int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Utilizatori SET isDeleted = 1 WHERE UtilizatorID = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Utilizatori SET NumeUtilizator = @NumeUtilizator, Parola = @Parola, TipUtilizator = @TipUtilizator WHERE UtilizatorID = @UtilizatorID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NumeUtilizator", user.Username);
                    command.Parameters.AddWithValue("@Parola", user.Password);
                    command.Parameters.AddWithValue("@TipUtilizator", user.UserType);
                    command.Parameters.AddWithValue("@UtilizatorID", user.ID);
                    int affectedRows = command.ExecuteNonQuery();

                    // Asigură-te că o linie a fost actualizată
                    if (affectedRows == 0)
                    {
                        MessageBox.Show("No user was updated, please check the user ID.", "Update Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        public static List<Stoc> GetAllStocks()
        {
            List<Stoc> stocks = new List<Stoc>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT s.*, p.NumeProdus, p.CodDeBare
            FROM Stocuri s
            JOIN Produse p ON s.ProductID = p.ProdusID
            WHERE s.Cantitate > 0 AND (s.DataExpirarii IS NULL OR s.DataExpirarii > GETDATE())";
                SqlCommand command = new SqlCommand(query, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stocks.Add(new Stoc
                        {
                            ID = (int)reader["ID"],
                            ProductID = (int)reader["ProductID"],
                            ProductName = reader["NumeProdus"].ToString(),
                            CodDeBare = reader["CodDeBare"].ToString(),
                            Cantitate = (decimal)reader["Cantitate"],
                            UnitateDeMasura = reader["UnitateDeMasura"].ToString(),
                            DataAprovizionarii = reader.IsDBNull(reader.GetOrdinal("DataAprovizionarii")) ? null : (DateTime?)reader["DataAprovizionarii"],
                            DataExpirarii = reader.IsDBNull(reader.GetOrdinal("DataExpirarii")) ? null : (DateTime?)reader["DataExpirarii"],
                            PretAchizitie = (decimal)reader["PretAchizitie"],
                            PretVanzare = (decimal)reader["PretVanzare"]
                        });
                    }
                }
            }
            return stocks;
        }

        public static void AddStock(Stoc stock)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
            INSERT INTO Stocuri (ProductID, Cantitate, UnitateDeMasura, DataAprovizionarii, DataExpirarii, PretAchizitie)
            VALUES (@ProductID, @Cantitate, @UnitateDeMasura, @DataAprovizionarii, @DataExpirarii, @PretAchizitie)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", stock.ProductID);
                        command.Parameters.AddWithValue("@Cantitate", stock.Cantitate);
                        command.Parameters.AddWithValue("@UnitateDeMasura", stock.UnitateDeMasura ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@DataAprovizionarii", DateTime.Now);
                        command.Parameters.AddWithValue("@DataExpirarii", stock.DataExpirarii ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@PretAchizitie", stock.PretAchizitie);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static List<Produs> GetAllProducts()
        {
            List<Produs> products = new List<Produs>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ProdusID, NumeProdus FROM Produse WHERE IsDeleted = 0";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Produs
                            {
                                ProdusID = reader.GetInt32(0),
                                NumeProdus = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return products;
        }

        public static List<ProdusExtended> GetAllExtendedProducts()
        {
            List<ProdusExtended> products = new List<ProdusExtended>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT p.ProdusID, p.NumeProdus, p.CodDeBare, c.NumeCategorie, pr.NumeProducator
            FROM Produse p
            JOIN Categorii c ON p.CategorieID = c.CategorieID
            JOIN Producatori pr ON p.ProducatorID = pr.ProducatorID
            WHERE p.IsDeleted = 0";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new ProdusExtended
                            {
                                ProdusID = reader.GetInt32(0),
                                NumeProdus = reader.GetString(1),
                                CodeDeBare = reader.GetString(2),
                                NumeCategorie = reader.GetString(3),
                                NumeProducator = reader.GetString(4)
                            });
                        }
                    }
                }
            }
            return products;
        }

        public static List<Categorie> GetAllCategories()
        {
            List<Categorie> categories = new List<Categorie>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT CategorieID, NumeCategorie FROM Categorii WHERE IsDeleted = 0";
                SqlCommand command = new SqlCommand(query, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Categorie
                        {
                            CategorieID = reader.GetInt32(0),
                            NumeCategorie = reader.GetString(1)
                        });
                    }
                }
            }
            return categories;
        }

        public static List<Producator> GetAllProducers()
        {
            List<Producator> producers = new List<Producator>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ProducatorID, NumeProducator, TaraOrigine FROM Producatori WHERE IsDeleted = 0";
                SqlCommand command = new SqlCommand(query, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        producers.Add(new Producator
                        {
                            ProducatorID = reader.GetInt32(0),
                            NumeProducator = reader.GetString(1),
                            TaraOrigine = reader.GetString(2)
                        });
                    }
                }
            }
            return producers;
        }

        public static void AddProduct(Produs produs)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            INSERT INTO Produse (NumeProdus, CodDeBare, CategorieID, ProducatorID)
            VALUES (@NumeProdus, @CodDeBare, @CategorieID, @ProducatorID)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NumeProdus", produs.NumeProdus);
                command.Parameters.AddWithValue("@CodDeBare", produs.CodeDeBare);
                command.Parameters.AddWithValue("@CategorieID", produs.CategorieID);
                command.Parameters.AddWithValue("@ProducatorID", produs.ProducatorID);
                command.ExecuteNonQuery();
            }
        }

        public static void DeleteProduct(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Produse SET isDeleted = 1 WHERE ProdusID = @ProdusID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProdusID", productId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateProduct(Produs produs)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            UPDATE Produse
            SET NumeProdus = @NumeProdus, CodDeBare = @CodDeBare
            WHERE ProdusID = @ProdusID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NumeProdus", produs.NumeProdus);
                    command.Parameters.AddWithValue("@CodDeBare", produs.CodeDeBare);
                    command.Parameters.AddWithValue("@ProdusID", produs.ProdusID);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void AddCategory(Categorie category)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Categorii (NumeCategorie, IsDeleted) VALUES (@NumeCategorie, 0)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NumeCategorie", category.NumeCategorie);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateCategory(Categorie category)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Categorii SET NumeCategorie = @NumeCategorie WHERE CategorieID = @CategorieID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NumeCategorie", category.NumeCategorie);
                    command.Parameters.AddWithValue("@CategorieID", category.CategorieID);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static decimal CalculateTotalValueByCategory(int categoryId)
        {
            decimal totalValue = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT SUM(s.Cantitate * s.PretVanzare) AS TotalValue
                    FROM Stocuri s
                    JOIN Produse p ON s.ProductID = p.ProdusID
                    WHERE p.CategorieID = @CategorieID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategorieID", categoryId);
                var result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    totalValue = (decimal)result;
                }
            }
            return totalValue;
        }

        public static void AddManufacturer(Producator producer)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Producatori (NumeProducator, TaraOrigine, IsDeleted) VALUES (@NumeProducator, @TaraOrigine, 0)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NumeProducator", producer.NumeProducator);
                    command.Parameters.AddWithValue("@TaraOrigine", producer.TaraOrigine);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateManufacturer(Producator producer)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Producatori SET NumeProducator = @NumeProducator, TaraOrigine = @TaraOrigine WHERE ProducatorID = @ProducatorID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NumeProducator", producer.NumeProducator);
                    command.Parameters.AddWithValue("@TaraOrigine", producer.TaraOrigine);
                    command.Parameters.AddWithValue("@ProducatorID", producer.ProducatorID);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Produs> GetProductsByManufacturer(int manufacturerId)
        {
            List<Produs> products = new List<Produs>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT NumeProdus
            FROM Produse
            WHERE ProducatorID = @ProducatorID AND IsDeleted = 0";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProducatorID", manufacturerId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Produs
                            {
                                NumeProdus = reader.GetString(0)
                            });
                        }
                    }
                }
            }
            return products;
        }

        public static List<Bon> GetBonuri()
        {
            List<Bon> bonuri = new List<Bon>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT BonID, DataEliberarii, Casier, NumeProdus, Cantitate, Subtotal, Total FROM BonuriDetalii";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int bonID = reader.GetInt32(0);
                            Bon bon = bonuri.FirstOrDefault(b => b.BonID == bonID);

                            if (bon == null)
                            {
                                bon = new Bon
                                {
                                    BonID = bonID,
                                    DataEliberarii = reader.GetDateTime(1),
                                    Casier = reader.GetString(2),
                                    Total = reader.GetDecimal(6)
                                };
                                bonuri.Add(bon);
                            }

                            BonProdus produs = new BonProdus
                            {
                                NumeProdus = reader.GetString(3),
                                Cantitate = reader.GetDecimal(4),
                                Subtotal = reader.GetDecimal(5)
                            };

                            bon.Produse.Add(produs);
                        }
                    }
                }
            }

            return bonuri;
        }

        public static Bon GetBiggestBonOfDay(DateTime date)
        {
            Bon biggestBon = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT TOP 1 b.BonID, b.DataEliberarii, u.NumeUtilizator AS Casier, SUM(t.Cantitate * s.PretVanzare) AS Total
            FROM Bonuri b
            JOIN Utilizatori u ON b.CasierID = u.UtilizatorID
            JOIN Tranzactii t ON b.BonID = t.BonID
            JOIN Produse p ON t.ProdusID = p.ProdusID
            JOIN Stocuri s ON p.ProdusID = s.ProductID
            WHERE CAST(b.DataEliberarii AS DATE) = @SelectedDate
            GROUP BY b.BonID, b.DataEliberarii, u.NumeUtilizator
            ORDER BY Total DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SelectedDate", date.Date);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            biggestBon = new Bon
                            {
                                BonID = reader.GetInt32(0),
                                DataEliberarii = reader.GetDateTime(1),
                                Casier = reader.GetString(2),
                                Total = reader.GetDecimal(3)
                            };
                        }
                    }
                }
            }

            return biggestBon;
        }

        public static List<UserIncome> GetUserIncomesByMonth(int userId, int year, int month)
        {
            List<UserIncome> incomes = new List<UserIncome>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT 
                CAST(b.DataEliberarii AS DATE) AS [Date],
                SUM(t.Cantitate * s.PretVanzare) AS [Income]
            FROM 
                Bonuri b
                JOIN Tranzactii t ON b.BonID = t.BonID
                JOIN Stocuri s ON t.ProdusID = s.ProductID
            WHERE 
                b.CasierID = @UserId AND
                YEAR(b.DataEliberarii) = @Year AND
                MONTH(b.DataEliberarii) = @Month
            GROUP BY 
                CAST(b.DataEliberarii AS DATE)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@Month", month);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            incomes.Add(new UserIncome
                            {
                                Date = reader.GetDateTime(0),
                                Income = reader.GetDecimal(1)
                            });
                        }
                    }
                }
            }
            return incomes;
        }

        public static List<Stoc> SearchStocksByName(string name)
        {
            List<Stoc> stocks = new List<Stoc>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT s.*, p.NumeProdus, p.CodDeBare
            FROM Stocuri s
            JOIN Produse p ON s.ProductID = p.ProdusID
            WHERE p.NumeProdus LIKE '%' + @Name + '%' AND s.Cantitate > 0 AND (s.DataExpirarii IS NULL OR s.DataExpirarii > GETDATE())";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", name);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stocks.Add(new Stoc
                        {
                            ID = (int)reader["ID"],
                            ProductID = (int)reader["ProductID"],
                            ProductName = reader["NumeProdus"].ToString(),
                            CodDeBare = reader["CodDeBare"].ToString(),
                            Cantitate = (decimal)reader["Cantitate"],
                            UnitateDeMasura = reader["UnitateDeMasura"].ToString(),
                            DataAprovizionarii = reader.IsDBNull(reader.GetOrdinal("DataAprovizionarii")) ? null : (DateTime?)reader["DataAprovizionarii"],
                            DataExpirarii = reader.IsDBNull(reader.GetOrdinal("DataExpirarii")) ? null : (DateTime?)reader["DataExpirarii"],
                            PretAchizitie = (decimal)reader["PretAchizitie"],
                            PretVanzare = (decimal)reader["PretVanzare"]
                        });
                    }
                }
            }
            return stocks;
        }

        public static List<Stoc> SearchStocksByBarcode(string barcode)
        {
            List<Stoc> stocks = new List<Stoc>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT s.*, p.NumeProdus, p.CodDeBare
            FROM Stocuri s
            JOIN Produse p ON s.ProductID = p.ProdusID
            WHERE p.CodDeBare LIKE '%' + @Barcode + '%' AND s.Cantitate > 0 AND (s.DataExpirarii IS NULL OR s.DataExpirarii > GETDATE())";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Barcode", barcode);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stocks.Add(new Stoc
                        {
                            ID = (int)reader["ID"],
                            ProductID = (int)reader["ProductID"],
                            ProductName = reader["NumeProdus"].ToString(),
                            CodDeBare = reader["CodDeBare"].ToString(),
                            Cantitate = (decimal)reader["Cantitate"],
                            UnitateDeMasura = reader["UnitateDeMasura"].ToString(),
                            DataAprovizionarii = reader.IsDBNull(reader.GetOrdinal("DataAprovizionarii")) ? null : (DateTime?)reader["DataAprovizionarii"],
                            DataExpirarii = reader.IsDBNull(reader.GetOrdinal("DataExpirarii")) ? null : (DateTime?)reader["DataExpirarii"],
                            PretAchizitie = (decimal)reader["PretAchizitie"],
                            PretVanzare = (decimal)reader["PretVanzare"]
                        });
                    }
                }
            }
            return stocks;
        }


        public static void EmitBon(Bon bon)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Get the ID of the cashier based on their username
                    string getUserIdQuery = "SELECT UtilizatorID FROM Utilizatori WHERE NumeUtilizator = @NumeUtilizator";
                    SqlCommand getUserIdCommand = new SqlCommand(getUserIdQuery, connection, transaction);
                    getUserIdCommand.Parameters.AddWithValue("@NumeUtilizator", bon.Casier);
                    int casierID = (int)getUserIdCommand.ExecuteScalar();

                    string insertBonQuery = @"
                INSERT INTO Bonuri (DataEliberarii, CasierID)
                VALUES (@DataEliberarii, @CasierID);
                SELECT SCOPE_IDENTITY();";

                    SqlCommand bonCommand = new SqlCommand(insertBonQuery, connection, transaction);
                    bonCommand.Parameters.AddWithValue("@DataEliberarii", bon.DataEliberarii);
                    bonCommand.Parameters.AddWithValue("@CasierID", casierID);
                    int bonID = Convert.ToInt32(bonCommand.ExecuteScalar());

                    foreach (var bonProdus in bon.Produse)
                    {
                        string insertTranzactieQuery = @"
                    INSERT INTO Tranzactii (BonID, ProdusID, Cantitate)
                    VALUES (@BonID, (SELECT ProdusID FROM Produse WHERE NumeProdus = @NumeProdus), @Cantitate)";

                        SqlCommand tranzactieCommand = new SqlCommand(insertTranzactieQuery, connection, transaction);
                        tranzactieCommand.Parameters.AddWithValue("@BonID", bonID);
                        tranzactieCommand.Parameters.AddWithValue("@NumeProdus", bonProdus.NumeProdus);
                        tranzactieCommand.Parameters.AddWithValue("@Cantitate", bonProdus.Cantitate);
                        tranzactieCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public static void UpdateStockQuantity(int productId, decimal newQuantity)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "UPDATE Stocuri SET Cantitate = @Cantitate WHERE ProductID = @ProductID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Cantitate", newQuantity);
                    command.Parameters.AddWithValue("@ProductID", productId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


    }
}
