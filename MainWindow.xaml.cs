using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp6
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=ExpenseTracker;Integrated Security=True";

        public MainWindow()
        {
            InitializeComponent();
            RefreshExpenses();
        }

        private void RefreshExpenses()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT ExpenseId, Amount, Description FROM Expenses", connection);
                    SqlDataReader reader = command.ExecuteReader();

                    ExpensesListBox.Items.Clear();
                    while (reader.Read())
                    {
                        ExpensesListBox.Items.Add($"ID: {reader["ExpenseId"]}, Сумма: {reader["Amount"]}, Описание: {reader["Description"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseWindow addExpenseWindow = new AddExpenseWindow();
            if (addExpenseWindow.ShowDialog() == true)
            {
                RefreshExpenses();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExpensesListBox.SelectedItem != null)
            {
                string selectedItem = ExpensesListBox.SelectedItem.ToString();
                string[] parts = selectedItem.Split(',');
                int expenseId = Convert.ToInt32(parts[0].Split(':')[1].Trim());

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("DELETE FROM Expenses WHERE ExpenseId = @ExpenseId", connection);
                        command.Parameters.AddWithValue("@ExpenseId", expenseId);
                        command.ExecuteNonQuery();
                        RefreshExpenses();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
            else
            {


                MessageBox.Show("Выберите запись для удаления.");
            }

        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            // Ваш код для обновления списка расходов
            // Например, можно заново привязать данные к ListBox
            RefreshExpenses();
        }
      

    }
}
    
