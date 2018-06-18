using ADO_Exam.MODEL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace ADO_Exam.PAGES
{
    /// <summary>
    /// Interaction logic for CATEGORY.xaml
    /// </summary>
    public partial class CATEGORY : Page
    {

        private static IEnumerable<Vacancy> vacancies = null;
        public CATEGORY()
        {
            InitializeComponent();
        }

        private async void SaveBTN_Click(object sender, RoutedEventArgs e)
        {

            if (!BAL.IsDBConnectionOpened)
            {
                BAL.MessageNoDBConnected();
                return;
            }

            if (!CheckInputsNotEmpty())
            {
                MessageBox.Show("Введите оба поля!");
                return;
            }

            try
            {
                XDocument.Load(InputRSS.Text);
                int categoryID = BAL.GetCategoryIdByUrl(InputRSS.Text);

                if (categoryID == 0)
                {
                    await BAL.AddCategoryToDB(new Category() { Name = InputName.Text, URL = InputRSS.Text });
                    categoryID = BAL.GetCategoryIdByUrl(InputRSS.Text);
                }

                await LoadDataFromResourceAsync(InputRSS.Text, categoryID);
                await BAL.AddRangeOfVacanciesToDB(vacancies);
            }
            catch (FileNotFoundException) {
                MessageBox.Show("Неверно указан RSS канал!");
            }

        }

        private bool CheckInputsNotEmpty()
        {
            return !String.IsNullOrWhiteSpace(InputName.Text) && !String.IsNullOrWhiteSpace(InputRSS.Text);
        }

        private Task LoadDataFromResourceAsync(string rssLink, int categoryID)
        {

            Task worker = new Task(() =>
            {
                XDocument resource = XDocument.Load(rssLink);

                vacancies = resource.Root.Element("channel").Elements("item").Select(item =>
                           new Vacancy
                           {
                               Name = item.Element("title").Value,
                               CategoryId = categoryID,
                               Link = item.Element("link").Value,
                               Description = item.Element("description").Value,
                               PublishedDate = DateTime.Parse(item.Element("pubDate").Value),
                               Email = item.Element("author").Value
                           });
            });

            worker.Start();

            return worker;
        }
    }
}
