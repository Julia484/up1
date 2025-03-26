using System;
using System.Data.SqlClient;
using System.Windows;

namespace WpfApp6
{
    public partial class AddExpenseWindow : Window
    {
        // Объявление переменной для строки подключения
        private string connectionString = "Data Source=localhost;Initial Catalog=ExpenseTracker;Integrated Security=True";

        public AddExpenseWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            decimal amount;
            if (decimal.TryParse(AmountTextBox.Text, out amount))
            {
                string description = DescriptionTextBox.Text;

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("INSERT INTO Expenses (Amount, Description, UserId, CategoryId) VALUES (@Amount, @Description, 1, 1)", connection);
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@Description", description);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Расход добавлен.");
                    DialogResult = true; // Закрытие окна с результатом
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Введите правильную сумму.");
            }
        }
    }
}
