using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ADO_Exam.PAGES
{
    /// <summary>
    /// Interaction logic for OPTIONS.xaml
    /// </summary>
    public partial class OPTIONS : Page
    {
        public OPTIONS()
        {
            InitializeComponent();
            LoadResources();
        }

        private void LoadResources()
        {
            if (BAL.IsDBConnectionOpened)
                LoadCounts();
            else
                LoadCounts(true);

            UpdateOptionsInfo();
        }

        private async void LoadCounts(bool defaults = false)
        {
            if (defaults)
            {
                VacanciesCount.Content = CategoriesCount.Content = "нет данных";
            }
            else
            {
                try
                {
                    VacanciesCount.Content = await BAL.GetVacanciesCountAsync();
                    CategoriesCount.Content = await BAL.GetCategoriesCountAsync();
                }
                catch (Exception) {  }
            }
        }

        private void UpdateOptionsInfo()
        {
            string[] currentConfig = BAL.GetCurrentDBConfig();
            StringBuilder sb = new StringBuilder();
            sb.Append("ТЕКУЩИЕ НАСТРОЙКИ:\n");
            sb.Append($"Сервер базы данных: {currentConfig[0]}\n");
            sb.Append($"Название базы данных: {currentConfig[1]}\n");
            sb.Append($"Имя пользователя: {currentConfig[2]}\n");
            sb.Append($"Пароль пользователя: {currentConfig[3]}\n");
            sb.Append($"Таймаут: {currentConfig[4]}");

            CurrentOptions.Text = sb.ToString();
        }

        private void CleanDBBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!BAL.IsDBConnectionOpened)
            {
                BAL.MessageNoDBConnected();
                return;
            }

            try
            {
                BAL.CleanAllDBSetsOnServer();
                MessageBox.Show("БД очищены!");
                LoadResources();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при очистке БД!");
            }

        }



        private void ChangeConfigBTN_Click(object sender, RoutedEventArgs e)
        {
            if (AreAllInputsCheckInputs())
            {
                BAL.ChangeAppConfig(DataSource.Text, InitialCatalog.Text, UserID.Text, Password.Text);                
                MessageBox.Show("Данные успешно обновлены!");
                BAL.CheckSqlConnection();
                LoadResources();
            }
            else
                MessageBox.Show("Поля сервер БД и название БД обязательны!");
        }

        private bool AreAllInputsCheckInputs()
        {
            return !String.IsNullOrWhiteSpace(DataSource.Text) && !String.IsNullOrWhiteSpace(InitialCatalog.Text);
        }
    }
}
