using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ADO_Exam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow MainWindowInstance = null;
        public MainWindow()
        {
            InitializeComponent();
            MainWindowInstance = this;

            BAL.CheckSqlConnection();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem active = (MenuItem)sender;

            switch (active.Name)
            {
                case "CategoryLoad":
                    {
                        MainFrame.Source = pages["category"];
                        MenuItemBackgroundsColorHandler(active);
                    }
                    break;
                case "SearchLoad":
                    {
                        if (BAL.IsDBConnectionOpened)
                        {
                            MainFrame.Source = pages["search"];
                            MenuItemBackgroundsColorHandler(active);
                        }
                        else
                        {
                            BAL.MessageNoDBConnected();
                        }
                    }
                    break;
                case "OptionsLoad":
                    {
                        MainFrame.Source = pages["options"];
                        MenuItemBackgroundsColorHandler(active);
                    }
                    break;
            }
        }

        private static Dictionary<string, Uri> pages = new Dictionary<string, Uri>()
        {
            { "category", new Uri("/PAGES/CATEGORY.xaml", UriKind.RelativeOrAbsolute) },
            { "search", new Uri("/PAGES/SEARCH.xaml", UriKind.RelativeOrAbsolute) },
            { "options", new Uri("/PAGES/OPTIONS.xaml", UriKind.RelativeOrAbsolute) }
        };

        private void MenuItemBackgroundsColorHandler(MenuItem active)
        {
            CategoryLoad.Background = SearchLoad.Background = OptionsLoad.Background = Brushes.LightGray;
            active.Background = Brushes.LightBlue;
        }

        public void StatusBarDbConnectedChange(bool connected)
        {
            if (connected)
            {
                DBConnectionStatusBarLabel.Content = "База данных подключена".ToUpper();
                DBConnectionStatusBarLabel.Background = Brushes.LightGreen;
            }
            else
            {
                DBConnectionStatusBarLabel.Content = "База данных не подключена".ToUpper();
                DBConnectionStatusBarLabel.Background = Brushes.LightCoral;
            }
        }

    }
}
