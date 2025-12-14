using Microsoft.Win32;
using RecruitmentAgency.ApplicationData;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace RecruitmentAgency.PageAdmin
{
    // Меню администратора: навигация и выход
    public partial class PageMenuAdmin : Page
    {
        private Users currentUser;
        public PageMenuAdmin(Users currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void BtnManageUsers_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageAdmin.PageEditUsers(currentUser));
        }

        private void BtnManageVacancies_Click(object sender, RoutedEventArgs e)
        {
            // Управление вакансиями 
        }

        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {
            // Отчёты 
        }

        private void BtnSettingInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAudit_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageAdmin.PageAuditLog());
        }


        private void BtnBackup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Открываем диалог для выбора пути сохранения
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Файлы резервных копий (*.bak)|*.bak";
                saveDialog.Title = "Сохранить резервную копию";
                saveDialog.FileName = $"RecruitmentAgency_Backup_{DateTime.Now}.bak";

                if (saveDialog.ShowDialog() == true)
                {
                    string backupPath = saveDialog.FileName;

                    // Получаем строку подключения из Entity Framework
                    string connectionString = AppConnect.modelOdb.Database.Connection.ConnectionString;

                    // SQL команда для резервного копирования
                    string backupCommand = $"BACKUP DATABASE [{AppConnect.modelOdb.Database.Connection.Database}] " +
                                           $"TO DISK = N'{backupPath}' " +
                                           $"WITH NOFORMAT, NOINIT, NAME = N'RecruitmentAgency-Full Database Backup', " +
                                           $"SKIP, NOREWIND, NOUNLOAD, STATS = 10";

                    // Выполняем команду
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(backupCommand, connection))
                        {
                            command.CommandTimeout = 300; // 5 минут на выполнение
                            command.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show($"Резервная копия успешно создана!\nПуть: {backupPath}",
                                   "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании резервной копии:\n{ex.Message}",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    
}
