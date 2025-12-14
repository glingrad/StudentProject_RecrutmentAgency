using RecruitmentAgency.ApplicationData;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RecruitmentAgency.PageAdmin
{
    public partial class PageCreateAcc : Page
    {

        private Users currentUser;
        public PageCreateAcc(Users currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        // Проверка совпадения паролей при изменении подтверждения
        private void PbConfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckPasswordMatch();
        }

        // Проверка совпадения паролей при изменении основного пароля
        private void TbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckPasswordMatch();
        }

        private void CheckPasswordMatch()
        {
            // Визуальная индикация совпадения паролей и активация кнопки создания
            if (TbPassword.Text == PbConfirm.Password)
            {
                PbConfirm.BorderBrush = Brushes.Green;
                BtnCreate.IsEnabled = true;
            }
            else
            {
                PbConfirm.BorderBrush = Brushes.Red;
                BtnCreate.IsEnabled = false;    
            }
        }

        // Создание нового пользователя с валидацией email и категорией
        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка на существование пользователя с таким email
                var existingUser = AppConnect.modelOdb.Users.FirstOrDefault(u => u.Email == TbEmail.Text);
                if (existingUser != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже есть",
                        "Notification",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }
                string UserCategory;
                // Логика определения категории пользователя по выбору ComboBox
                if (CbUserType.SelectedIndex == 0)
                {
                    UserCategory = "Менеджер";
                }
                else if (CbUserType.SelectedIndex == 1)
                {
                    UserCategory = "Бухгалтер";
                }
                else
                {
                    UserCategory = "Админ";
                }
                // Создание нового пользователя
                Users newUser = new Users()
                {
                    LastName = TbLastName.Text,
                    FirstName = TbFirstName.Text,
                    MiddleName = TbMiddleName.Text,
                    Email = TbEmail.Text,
                    Phone = TbPhone.Text,
                    User_Password = TbPassword.Text,
                    UserCategory = UserCategory,
                    CreatedAt = DateTime.Now,
                    IsFirstLogin = true,
                    IsBlocked = false,
                    FailedLoginAttempts = 0
                };
                // Добавление пользователя и сохранение
                AppConnect.modelOdb.Users.Add(newUser);
                AppConnect.modelOdb.SaveChanges();

                AuditService.LogUserCreation(currentUser.UserId, TbEmail.Text, UserCategory);

                // Вывод сообщения об успехе
                MessageBox.Show("Пользователь успешно зарегистрирован!",
                    "Notification",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                // Обработка ошибок базы данных
                string errorMessage;
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    errorMessage = ex.InnerException.Message;
                }
                else
                {
                    errorMessage = ex.Message;
                }
                string fullMessage = $"Ошибка при регистрации: {errorMessage}\n\n";
                MessageBox.Show(fullMessage,
                    "Критическая ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


    }
}