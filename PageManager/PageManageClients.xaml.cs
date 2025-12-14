using RecruitmentAgency.ApplicationData;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecruitmentAgency.PageManager
{
    public partial class PageManageClients : Page
    {
        private Users currentUser;
        private Clients selectedClient;

        public PageManageClients(Users user)
        {
            InitializeComponent();
            currentUser = user;
            LoadClients();
        }

        private void LoadClients()
        {
            try
            {
                var clients = AppConnect.modelOdb.Clients
                    .Where(c => c.IsActive) // Показываем только активных клиентов
                    .Select(c => new {
                        c.ClientId,
                        c.CompanyName,
                        c.ClientContacts.FirstOrDefault(ct => ct.ContactTypeId == 1).ContactPerson,
                        Phone = c.ClientContacts.FirstOrDefault(ct => ct.ContactTypeId == 1).ContactValue,
                        Email = c.ClientContacts.FirstOrDefault(ct => ct.ContactTypeId == 2).ContactValue,
                        ManagerName = c.Users.LastName + " " + c.Users.FirstName.Substring(0, 1) + ".",
                        c.CreatedAt,
                        c.IsActive
                    })
                    .ToList();

                DgClients.ItemsSource = clients;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DgClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgClients.SelectedItem != null)
            {
                dynamic item = DgClients.SelectedItem;
                selectedClient = AppConnect.modelOdb.Clients.Find((int)item.ClientId);
                BtnEditClient.IsEnabled = true;
                BtnDeleteClient.IsEnabled = true;
            }
            else
            {
                BtnEditClient.IsEnabled = false;
                BtnDeleteClient.IsEnabled = false;
                selectedClient = null;
            }
        }

        private void BtnAddClient_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageManager.PageClientForm(currentUser));
        }

        private void BtnEditClient_Click(object sender, RoutedEventArgs e)
        {
            if (selectedClient != null)
            {
                AppFrame.frameMain.Navigate(new PageManager.PageClientForm(currentUser, selectedClient));
            }
        }

        private void BtnDeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (selectedClient == null) return;

            if (MessageBox.Show($"Удалить клиента '{selectedClient.CompanyName}'?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    selectedClient.IsActive = false;
                    AppConnect.modelOdb.SaveChanges();
                    MessageBox.Show("Клиент помечен на удаление", "Успешно",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadClients();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }
    }
}