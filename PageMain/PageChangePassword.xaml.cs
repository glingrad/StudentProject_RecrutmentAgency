using RecruitmentAgency.ApplicationData;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;

namespace RecruitmentAgency.PageMain
{
    public partial class PageChangePassword : Page
    {
        private Users currentUser;

        private readonly HashSet<char> Digits;
        private readonly HashSet<char> SpecialChars;

        public PageChangePassword(Users user)
        {
            InitializeComponent();
            currentUser = user;
            Digits = new HashSet<char>();
            SpecialChars = new HashSet<char>();
            for (char b = '0'; b <= '9'; b++)
            {
                Digits.Add(b);
            }
            string specialSymbols = "!@#$%^&_+-=*/,";
            foreach (char c in specialSymbols)
            {
                SpecialChars.Add(c);
            }

        }

        private void PbNewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidatePasswords();
        }

        private void PbConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidatePasswords();
        }

        private void ValidatePasswords()
        {
            string newPassword = PbNewPassword.Password;
            string confirmPassword = PbConfirmPassword.Password;

            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                TbPasswordStatus.Visibility = Visibility.Collapsed;
                BtnChangePassword.IsEnabled = false;
                return;
            }

            if (newPassword.Length < 6)
            {
                TbPasswordStatus.Text = "Пароль не соответствует требованиям: минимум 6 символов, буквы, цифры и специальные символы";
                TbPasswordStatus.Visibility = Visibility.Visible;
                BtnChangePassword.IsEnabled = false;
                PbNewPassword.BorderBrush = Brushes.Red;
                return;
            }


            bool hasLetter = false;
            bool hasDigit = false;
            bool hasSpecialChar = false;

            foreach (char c in newPassword)
            {
                if (char.IsLetter(c))
                {
                    hasLetter = true;
                }
                else if (Digits.Contains(c))
                {
                    hasDigit = true;
                }
                else if (SpecialChars.Contains(c))
                {
                    hasSpecialChar = true;
                }
            }


            if (hasLetter == true || hasDigit == true || hasSpecialChar == true)
            {
                MessageBox.Show("Текущий пароль не содержит букв,цифр и спец.символов", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (newPassword != confirmPassword)
            {
                TbPasswordStatus.Text = "Пароли не совпадают";
                TbPasswordStatus.Visibility = Visibility.Visible;
                BtnChangePassword.IsEnabled = false;
                return;
            }


            TbPasswordStatus.Visibility = Visibility.Collapsed;
            BtnChangePassword.IsEnabled = true;
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string currentPassword = PbCurrentPassword.Password;

                // Проверка текущего пароля
                if (currentPassword != currentUser.User_Password)
                {
                    MessageBox.Show("Текущий пароль введен неверно", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string newPassword = PbNewPassword.Password;

                // Обновление пароля в базе данных
                currentUser.User_Password = newPassword;
                currentUser.IsFirstLogin = false; // Сбрасываем флаг первой авторизации
                AppConnect.modelOdb.SaveChanges();

                MessageBox.Show("Пароль успешно изменен!", "Успешно",
                MessageBoxButton.OK, MessageBoxImage.Information);

                // Возвращаем пользователя на основную страницу
                AppFrame.frameMain.Navigate(new MainPage(currentUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении пароля: {ex.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаем пользователя на основную страницу
            AppFrame.frameMain.Navigate(new MainPage(currentUser));
        }
    }
}