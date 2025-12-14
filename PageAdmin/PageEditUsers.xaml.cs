using RecruitmentAgency.ApplicationData;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecruitmentAgency.PageAdmin
{
    /// <summary>
    /// Логика взаимодействия для PageEditUsers.xaml
    /// </summary>
    public partial class PageEditUsers : Page
    {
        private Users selectedUser;
        private Users currentUser;

        public PageEditUsers(Users user)
        {
            InitializeComponent(); 
            LoadUsers();
            this.currentUser = user;
        }

        string GetUserStatus(bool isBlocked)
        {
            if (isBlocked)
                return "Заблокирован";
            else
                return "Активен";
        }

        private void LoadUsers()
        {
            try
            {
                // Загружаем всех пользователей
                var usersList = AppConnect.modelOdb.Users.ToList();

                // Создаем представление для DataGrid с дополнительными полями
                var usersView = usersList.Select(u => new
                {
                    u.UserId,
                    FullName = $"{u.LastName} {u.FirstName} {u.MiddleName}",
                    u.Email,
                    u.UserCategory,
                    Status = GetUserStatus(u.IsBlocked)
                }).ToList();

                DgUsers.ItemsSource = usersView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке пользователей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUnlockUser_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null)
            {
                try
                {
                    selectedUser.IsBlocked = false;
                    selectedUser.FailedLoginAttempts = 0;
                    AppConnect.modelOdb.SaveChanges();

                    AuditService.LogUserUnlock(selectedUser.UserId, selectedUser.Email);

                    MessageBox.Show($"Пользователь {selectedUser.Email} успешно разблокирован", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadUsers(); // Обновляем список
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при разблокировке пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageCreateAcc(currentUser));
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            // Возврат к предыдущей странице
            AppFrame.frameMain.GoBack();
        }

        private void DgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = DgUsers.SelectedItem as Users;
        }

        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {
            //
        }
    }
}
