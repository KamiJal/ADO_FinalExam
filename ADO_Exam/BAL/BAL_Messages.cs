using System.Windows;

namespace ADO_Exam
{
    public static partial class BAL
    {
        public static void MessageNoDBConnected()
        {
            MessageBox.Show("База данных не подключена!");
        }

        public static void MessageDBAccessProblems()
        {
            MessageBox.Show("Проблемы с доступом БД!");
        }

        public static void MessageDBIsEmpty()
        {
            MessageBox.Show("БД пуста!");
        }
    }
}
