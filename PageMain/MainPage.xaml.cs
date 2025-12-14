using System.Windows;
using System.Windows.Controls;
using RecruitmentAgency.ApplicationData;
using RecruitmentAgency.PageAccountant;
using RecruitmentAgency.PageAdmin;
using RecruitmentAgency.PageManager;

namespace RecruitmentAgency.PageMain
{
    public partial class MainPage : Page
    {
        private Users currentUser; // Текущий вошедший пользователь

        public MainPage(Users user)
        {
            InitializeComponent();
            this.currentUser = user;           // Сохраняем информацию о пользователе
            SetupAccessRights();          // Настраиваем видимость кнопок в зависимости от роли
        }

        private void SetupAccessRights()
        {
            // Скрываем кнопки в зависимости от роли пользователя
            switch (currentUser.UserCategory)
            {
                case "Админ":
                    // Администратор видит все кнопки — ничего не скрываем
                    break;

                case "Бухгалтер":
                    // Бухгалтер не должен видеть управление и администрирование
                    BtnManagement.Visibility = Visibility.Collapsed;
                    BtnAdministration.Visibility = Visibility.Collapsed;
                    break;

                case "Менеджер":
                    // Менеджер не должен видеть бухгалтерию и администрирование
                    BtnAccounting.Visibility = Visibility.Collapsed;
                    BtnAdministration.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void BtnManagement_Click(object sender, RoutedEventArgs e)
        {
            // Доступ только для администратора и менеджера
            if (currentUser.UserCategory == "Админ" || currentUser.UserCategory == "Менеджер")
            {
                AppFrame.frameMain.Navigate(new PageManagerMain(currentUser)); // Переход в раздел управления
            }
            else
            {
                ShowAccessDenied();
            }
        }

        private void BtnAdministration_Click(object sender, RoutedEventArgs e)
        {
            // Доступ только администратору
            if (currentUser.UserCategory == "Админ")
            {
                AppFrame.frameMain.Navigate(new PageMenuAdmin(currentUser)); // Переход в админку
            }
            else
            {
                ShowAccessDenied();
            }
        }

        private void BtnAccounting_Click (object sender, RoutedEventArgs e)
        {
            // 
            if (currentUser.UserCategory == "Бухгалтер" || currentUser.UserCategory == "Админ")
            {
                AppFrame.frameMain.Navigate(new PageMenuAccountant()); // Переход в бухгалтерию
            }
            else
            {
                ShowAccessDenied();
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageLogin()); // Переход на страницу авторизации
        }

        private void ShowAccessDenied()
        {
            MessageBox.Show("У вас нет прав для доступа к этому разделу",
                "Доступ запрещён",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

    }
}