using ADO_Exam.MODEL;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ADO_Exam.PAGES
{
    /// <summary>
    /// Interaction logic for SEARCH.xaml
    /// </summary>
    public partial class SEARCH : Page
    {
        private static List<CheckBox> checkBoxesList = null;
        public SEARCH()
        {
            InitializeComponent();
            LoadResources();
        }

        private async void LoadResources()
        {
            checkBoxesList = new List<CheckBox>() { CHKBX_Category, CHKBX_Date, CHKBX_Email, CHKBX_Phrase };
            CategorySearch.ItemsSource = await BAL.GetCategoryNamesAsync();
        }


        private async void SearchBTN_Click(object sender, RoutedEventArgs e)
        {
            if (await BAL.GetVacanciesCountAsync() == 0)
            {
                BAL.MessageDBIsEmpty();
                return;
            }


            CheckBox active = GetCheckedCheckBox();

            if (active == null)
            {
                MessageBox.Show("Вы не выбрали метод поиска!");
                return;
            }

            try
            {
                List<Vacancy> query = null;

                switch (active.Name)
                {
                    case "CHKBX_Category":
                        {
                            if (CategorySearch.SelectedValue == null)
                            {
                                MessageNoSearchOptionSelected();
                                return;
                            }

                            int categoryID = BAL.GetCategoryIdByName((string)CategorySearch.SelectedValue);
                            query = await BAL.GetVacanciesByCategoryIdAsync(categoryID);
                        }
                        break;
                    case "CHKBX_Date":
                        {
                            if (!DateSearch.SelectedDate.HasValue)
                            {
                                MessageNoSearchOptionSelected();
                                return;
                            }

                            query = await BAL.GetVacanciesByPubDateAsync(DateSearch.SelectedDate.Value.Date);
                        }
                        break;
                    case "CHKBX_Email":
                        {
                            if (String.IsNullOrWhiteSpace(EmailSearch.Text))
                            {
                                MessageNoSearchOptionSelected();
                                return;
                            }

                            query = await BAL.GetVacanciesByEmailAsync(EmailSearch.Text);
                        }
                        break;
                    case "CHKBX_Phrase":
                        {
                            if (String.IsNullOrWhiteSpace(PhraseSearch.Text))
                            {
                                MessageNoSearchOptionSelected();
                                return;
                            }

                            query = await BAL.GetVacanciesByDescriptionContextAsync(PhraseSearch.Text);
                        }
                        break;
                }

                if (query.Count == 0)
                    MessageBox.Show("Данных по запросу не найдено!");
                else
                    MainListView.ItemsSource = query;
            }
            catch (Exception)
            {
                BAL.MessageDBAccessProblems();
            }
        }

        private void MessageNoSearchOptionSelected()
        {
            MessageBox.Show("Вы не выбрали/ввели значение для поиска!");
        }


        private CheckBox GetCheckedCheckBox()
        {
            foreach (CheckBox checkBox in checkBoxesList)
            {
                if ((bool)checkBox.IsChecked)
                    return checkBox;
            }

            return null;
        }

        private void CHKBX_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            UncheckAllCheckboxesExceptGiven(checkBox);

            switch (checkBox.Name)
            {
                case "CHKBX_Category":
                    {
                        DisableAllSearchOptions();
                        CategorySearch.IsEnabled = true;
                    }
                    break;
                case "CHKBX_Date":
                    {
                        DisableAllSearchOptions();
                        DateSearch.IsEnabled = true; ;
                    }
                    break;
                case "CHKBX_Email":
                    {
                        DisableAllSearchOptions();
                        EmailSearch.IsEnabled = true;
                    }
                    break;
                case "CHKBX_Phrase":
                    {
                        DisableAllSearchOptions();
                        PhraseSearch.IsEnabled = true;
                    }
                    break;
            }

        }

        private void DisableAllSearchOptions()
        {
            CategorySearch.IsEnabled = DateSearch.IsEnabled = EmailSearch.IsEnabled = PhraseSearch.IsEnabled = false;
            CategorySearch.SelectedIndex = -1;
            DateSearch.SelectedDate = null;
            EmailSearch.Text = String.Empty;
            PhraseSearch.Text = String.Empty;
        }

        private void UncheckAllCheckboxesExceptGiven(CheckBox given)
        {
            foreach (CheckBox checkBox in checkBoxesList)
            {
                if (!checkBox.Equals(given))
                    checkBox.IsChecked = false;
            }
        }

        private void CHKBX_Unchecked(object sender, RoutedEventArgs e)
        {
            DisableAllSearchOptions();
        }
    }
}
