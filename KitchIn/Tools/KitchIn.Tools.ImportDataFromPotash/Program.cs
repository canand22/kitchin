using System;
using System.Collections.Generic;
using KitchIn.Tools.ImportDataFromPotash.Entities;
using KitchIn.Tools.ImportDataFromPotash.Converter;
using System.Data;

namespace KitchIn.Tools.ImportDataFromPotash
{
    class Program
    {
        private const String SheetNameGrocery = "GROCERY DATABASE (MASTER)";

        private const String SheetNameNonFood = "Non-Food Items";

        private const String FileName = "d:\\brv\\Projects\\KitchInApp\\kia_fork\\KitchIn\\Tools\\KitchIn.Tools.ImportDataFromPotash\\inputData\\Potash_StoreDatabase_SHARED_MASTERv4.xlsx";

        static void Main(string[] args)
        {
            Console.WriteLine("Migration start!");
            Console.WriteLine("Start reading exel file!");
            var dateGrocery = new LoadExcelWorker().ReadSheetInFile(FileName, SheetNameGrocery);
            var groceryDataFromExcel = new List<GroceryItem>();

            var dateNonFood = new LoadExcelWorker().ReadSheetInFile(FileName, SheetNameNonFood);
            var nonFoodDataFromExcel = new List<NonFoodItem>();

            Console.WriteLine("Start reading raws in sheet Grocery...");
            foreach (DataRow item in dateGrocery.Rows)
            {
                groceryDataFromExcel.Add(new GroceryItem(item));
            }
            Console.WriteLine("End reading raws in sheet Grocery...");

            Console.WriteLine("Start reading raws in sheet Non-Food...");
            foreach (DataRow item in dateNonFood.Rows)
            {
                nonFoodDataFromExcel.Add(new NonFoodItem(item));
            }
            Console.WriteLine("End reading raws in sheet Non-Food...");

            Console.WriteLine("End reading exel file!");
            var potashDataManagment = new ImportDataManagment();
            potashDataManagment.MigrationGrocery(groceryDataFromExcel);
            potashDataManagment.MigrationNonFood(nonFoodDataFromExcel);

            Console.WriteLine("Migration done!");
            Console.ReadKey();
        }
    }
}
