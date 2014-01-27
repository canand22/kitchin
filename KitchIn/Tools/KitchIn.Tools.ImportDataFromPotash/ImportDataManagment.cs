using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using KitchIn.Tools.ImportDataFromPotash.Entities;

namespace KitchIn.Tools.ImportDataFromPotash
{
    using System.Text;

    using KitchIn.Core.Enums;

    public class ImportDataManagment
    {
        private IDictionary<string, int> CatgoriesDictionary { get; set; }

        private const string ConnectionString = "Data Source=(local);Initial Catalog=KitchIn;Integrated Security=true";

        private const string QueryStringTemplate = "INSERT INTO [KitchIn].[dbo].[Products] (UpcCode, ShortName, Name, CategoryId, StoreId, IngredientName, TypeAdd, ModificationDate) VALUES ";

        public ImportDataManagment()
        {
            CatgoriesDictionary = new Dictionary<string, int>();
            ReadCategories();
        }

        private void ReadCategories()
        {
            string queryString = "SELECT * from dbo.Categories;";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader[1].ToString();
                    int id = Int32.Parse(reader[0].ToString());

                    CatgoriesDictionary.Add(name, id);
                }
            }
        }

        public void MigrationGrocery(IList<GroceryItem> groceryItems)
        {
            Console.WriteLine("Start writing products in DB!");
            const int PotashId = 1;
            var countRow = 0;
            var lengthBlock = 100;
            var queryString = QueryStringTemplate;
            foreach (var item in groceryItems)
            {
                Int32 category;
                try
                {
                    category = CatgoriesDictionary[item.Category];
                }
                catch (Exception ex)
                {
                    category = CatgoriesDictionary["OTHER"];
                }

                if (String.IsNullOrEmpty(item.PosDescription))
                {
                    Console.WriteLine("POS DESCRIPTION in row {0} empted", countRow);
                    countRow++;
                    continue;
                }
                var upc = item.UpcCode.Replace("'", "''");
                var pos = item.PosDescription.Replace("'", "''");
                var description = item.LongDescription.Replace("'", "''");
                var ingredientName = item.ApiMainMatch.Replace("'", "''");
                var typeAdd = TypeAdd.Exported.ToString();
                var modificationDate = DateTime.Now;
                var product = String.Format("('{0}', '{1}', '{2}', {3}, {4}, '{5}', '{6}', '{7}')", 
                    upc, pos, description, category, PotashId, ingredientName, typeAdd, modificationDate);
                queryString += product;
                countRow++;
                if (countRow % lengthBlock == 0)
                {
                    queryString += ";";
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(queryString, connection);
                        try
                        {
                            var i = command.ExecuteNonQuery();
                            Console.WriteLine("Write data block from {0} to {1}", countRow - lengthBlock, countRow);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Could not write data block from {0} to {1}", countRow - lengthBlock, countRow);                            
                        }
                        queryString = QueryStringTemplate;
                    }

                }else
                {
                    queryString += ",";
                }
            }

            if (queryString[queryString.Length - 1] == ',')
            {
                StringBuilder lastQueryString = new StringBuilder();
                lastQueryString.Append(queryString);
                lastQueryString[queryString.Length - 1] = ';';
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(lastQueryString.ToString(), connection);
                    try
                    {
                        var i = command.ExecuteNonQuery();
                        Console.WriteLine("Write last data block...");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Could not write last data block!");
                    }
                }
            }

            Console.WriteLine("Finish writing products in DB!");
        }

        public void MigrationNonFood(IList<NonFoodItem> nonFoodItems)
        {
            Console.WriteLine("Start writing non-food products in DB!");
            const int PotashId = 1;
            const int Category = 19;
            var countRow = 0;
            var lengthBlock = 100;
            var queryString = QueryStringTemplate;
            foreach (var item in nonFoodItems)
            {
                if (String.IsNullOrEmpty(item.PosDescription))
                {
                    Console.WriteLine("POS DESCRIPTION in row {0} empted", countRow);
                    countRow++;
                    continue;
                }
                var upc = item.UpcCode.Replace("'", "''");
                var pos = item.PosDescription.Replace("'", "''");
                var description = item.LongDescription.Replace("'", "''");
                var ingredientName = string.Empty;
                var typeAdd = TypeAdd.Exported.ToString();
                var modificationDate = DateTime.Now;

                var product = String.Format("('{0}', '{1}', '{2}', {3}, {4}, '{5}', '{6}', '{7}')", 
                    upc, pos, description, Category, PotashId, ingredientName, typeAdd, modificationDate);
                queryString += product;
                countRow++;
                if (countRow % lengthBlock == 0)
                {
                    queryString += ";";
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(queryString, connection);
                        try
                        {
                            var i = command.ExecuteNonQuery();
                            Console.WriteLine("Write data block from {0} to {1}", countRow - lengthBlock, countRow);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Could not write data block from {0} to {1}", countRow - lengthBlock, countRow);
                        }
                        queryString = QueryStringTemplate;
                    }

                }
                else
                {
                    queryString += ",";
                }
            }

            if (queryString[queryString.Length - 1] == ',')
            {
                StringBuilder lastQueryString = new StringBuilder();
                lastQueryString.Append(queryString);
                lastQueryString[queryString.Length - 1] = ';';
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(lastQueryString.ToString(), connection);
                    try
                    {
                        var i = command.ExecuteNonQuery();
                        Console.WriteLine("Write last data block...");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Could not write last data block!");
                    }
                }
            }

            Console.WriteLine("Finish writing products in DB!");
        }
    }
}
