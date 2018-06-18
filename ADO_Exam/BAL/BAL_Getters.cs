using ADO_Exam.MODEL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ADO_Exam
{
    public static partial class BAL
    {
        //CATEGORY GETTERS
        public static int GetCategoryIdByUrl(string URL)
        {
            try
            {
                return DBInstance.Categories?.Where(c => c.URL.Equals(URL)).Select(c => c.CategoryId)?.ToList().FirstOrDefault() ?? 0;
            }
            catch (Exception) { return 0; };
        }

        public static int GetCategoryIdByName(string name) => (int)DBInstance.Categories?.Where(c => c.Name.Equals(name)).Select(c => c.CategoryId)?.ToList().First();
        public static Task<int> GetCategoriesCountAsync() => Task.Run(() => { try { return DBInstance?.Categories?.Count() ?? 0; } catch (Exception) { return 0; } });
        public static Task<List<string>> GetCategoryNamesAsync() => Task.Run(() => { return DBInstance.Categories.Select(c => c.Name).ToList(); });

        //VACANCY GETTERS
        public static Task<int> GetVacanciesCountAsync() => Task.Run(() => { try { return DBInstance?.Vacancies?.Count() ?? 0; } catch (Exception) { return 0; } });
        public static Task<List<Vacancy>> GetVacanciesByCategoryIdAsync(int categoryId) => Task.Run(() => { return DBInstance.Vacancies.Where(v => v.CategoryId == categoryId).ToList(); });
        public static Task<List<Vacancy>> GetVacanciesByPubDateAsync(DateTime pubDate) => Task.Run(() => { return DBInstance.Vacancies.ToList().Where(v => v.PublishedDate.Date == pubDate.Date).ToList(); });
        public static Task<List<Vacancy>> GetVacanciesByEmailAsync(string email) => Task.Run(() => { return DBInstance.Vacancies.Where(v => v.Email.Equals(email)).ToList(); });
        public static Task<List<Vacancy>> GetVacanciesByDescriptionContextAsync(string descriptionContext) => Task.Run(() => { return DBInstance.Vacancies.Where(v => v.Description.ToLower().Contains(descriptionContext.ToLower())).ToList(); });

        //DB OTHER
        public static string[] GetCurrentDBConfig()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(currentConnectionString);
            return new string[] { builder.DataSource, builder.InitialCatalog, builder.UserID, builder.Password, builder.ConnectTimeout.ToString() };
        }
    }
}
