using ADO_Exam.MODEL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;

namespace ADO_Exam
{
    public static partial class BAL
    {
        private static DB DBInstance = new DB();
        public static bool IsDBConnectionOpened = false;
        private static SqlConnection connection = null;

        public static void CheckSqlConnection()
        {
            if (connection?.State == ConnectionState.Open)
                connection.Close();

            UpdateCurrentConfigAndConnectionStrings();
            connection = new SqlConnection(currentConnectionString);



            Category catTest = new Category
            {
                Name = "Test",
                URL = "Test"
            };

            Vacancy vacTest = new Vacancy
            {
                Name = "Test",
                CategoryId = 1,
                Link = "Test",
                Description = "Test",
                PublishedDate = DateTime.Now,
                Email = "Test"
            };

            try
            {
                connection.Open();
                IsDBConnectionOpened = true;
                MainWindow.MainWindowInstance.StatusBarDbConnectedChange(IsDBConnectionOpened);
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 4060)
            {
                try
                {
                    if (!DBInstance.Database.Exists())
                    {
                        DBInstance.Categories.Add(catTest);
                        DBInstance.Vacancies.Add(vacTest);
                        DBInstance.SaveChanges();
                        CleanAllDBSetsOnServer();

                        connection.Open();
                        IsDBConnectionOpened = true;
                        MainWindow.MainWindowInstance.StatusBarDbConnectedChange(IsDBConnectionOpened);
                    }
                }
                catch (Exception exs)
                {
                    IsDBConnectionOpened = false;
                    MainWindow.MainWindowInstance.StatusBarDbConnectedChange(IsDBConnectionOpened);
                }

            }
            catch (SqlException sqlEx) when (sqlEx.Number == 53)
            {
                IsDBConnectionOpened = false;
                MainWindow.MainWindowInstance.StatusBarDbConnectedChange(IsDBConnectionOpened);
            }

        }


        public async static Task AddCategoryToDB(Category category)
        {
            DBInstance.Categories.Add(category);
            await DBInstance.SaveChangesAsync();
        }

        public async static Task AddRangeOfVacanciesToDB(IEnumerable<Vacancy> vacancies)
        {
            DBInstance.Vacancies.AddRange(vacancies);
            await DBInstance.SaveChangesAsync();
        }

        public static void CleanAllDBSetsOnServer()
        {
            DBInstance.Database.ExecuteSqlCommand("DELETE Categories;");
        }




    }
}
