using RecruitmentAgency.ApplicationData;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecruitmentAgency.PageMain
{
    public partial class PageLogin : Page
    {
        // Инициализация страницы входа
        public PageLogin()
        {
            InitializeComponent();
        }

        // Авторизация пользователя по email и паролю с переходом по ролям
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Поиск пользователя по логину и паролю
                var userObj = AppConnect.modelOdb.Users.FirstOrDefault(x =>
                    x.Email == TbLogin.Text &&
                    x.User_Password == PbPassword.Password);

                if (userObj == null)
                {
                    MessageBox.Show("Пользователь не найден",
                        "Notification",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                if (userObj.IsBlocked)
                {
                    MessageBox.Show("Вы заблокированы. Обратитесь к администратору", "Доступ запрещен",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (userObj.User_Password != PbPassword.Password)
                {
                    // Увеличиваем счетчик неудачных попыток
                    userObj.FailedLoginAttempts++;

                    // Блокируем пользователя при 3 неудачных попытках
                    if (userObj.FailedLoginAttempts >= 3)
                    {
                        userObj.IsBlocked = true;
                        AppConnect.modelOdb.SaveChanges();
                        MessageBox.Show("Вы заблокированы. Обратитесь к администратору", "Доступ запрещен",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    AppConnect.modelOdb.SaveChanges();
                    MessageBox.Show($"Неверный пароль",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Сбрасываем счетчик неудачных попыток при успешной авторизации
                userObj.FailedLoginAttempts = 0;
                AppConnect.modelOdb.SaveChanges();

                MessageBox.Show($"Добро пожаловать, {userObj.FirstName}!", "Успешно",
                MessageBoxButton.OK, MessageBoxImage.Information);

                AuditService.LogLogin(userObj.UserId, userObj.Email);

                // Проверяем, первая ли это авторизация
                if (userObj.IsFirstLogin)
                {
                    // Перенаправляем на страницу смены пароля
                    AppFrame.frameMain.Navigate(new PageChangePassword(userObj));
                }
                else
                {
                    // Перенаправляем в главное меню
                    AppFrame.frameMain.Navigate(new MainPage(userObj));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка работы с базой данных: {ex.Message }", "Критическая ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Завершение приложения
        private void BtnOut_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}