using RecruitmentAgency.ApplicationData;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecruitmentAgency.PageManager
{
    public partial class PageClientForm : Page
    {
        private Users currentUser;
        private Clients editingClient;

        public PageClientForm(Users user, Clients client = null)
        {
            InitializeComponent();
            currentUser = user;
            editingClient = client;

            if (client != null)
            {
                TitleBlock.Text = "Редактирование клиента";
                LoadClientData();
            }
        }

        private void LoadClientData()
        {
            TbCompany.Text = editingClient.CompanyName;

            // Получаем контактные данные
            var phoneContact = editingClient.ClientContacts.FirstOrDefault(c => c.ContactTypeId == 1);
            var emailContact = editingClient.ClientContacts.FirstOrDefault(c => c.ContactTypeId == 2);

            if (phoneContact != null)
            {
                TbContactPerson.Text = phoneContact.ContactPerson;
                TbPhone.Text = phoneContact.ContactValue;
            }

            if (emailContact != null)
            {
                TbEmail.Text = emailContact.ContactValue;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbCompany.Text))
            {
                MessageBox.Show("Введите название компании", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (editingClient == null)
                {
                    // Создаем нового клиента
                    editingClient = new Clients
                    {
                        CompanyName = TbCompany.Text,
                        ManagerId = currentUser.UserId,
                        CreatedAt = DateTime.Now,
                        IsActive = true
                    };

                    AppConnect.modelOdb.Clients.Add(editingClient);
                    AppConnect.modelOdb.SaveChanges(); // Сохраняем, чтобы получить ClientId

                    // Добавляем контакты
                    CreateContacts();

                    MessageBox.Show("Клиент добавлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Обновляем существующего клиента
                    editingClient.CompanyName = TbCompany.Text;

                    // Удаляем старые контакты
                    var oldContacts = AppConnect.modelOdb.ClientContacts
                        .Where(c => c.ClientId == editingClient.ClientId).ToList();

                    foreach (var contact in oldContacts)
                    {
                        AppConnect.modelOdb.ClientContacts.Remove(contact);
                    }

                    // Добавляем новые контакты
                    CreateContacts();

                    MessageBox.Show("Данные обновлены", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                AppConnect.modelOdb.SaveChanges();
                AppFrame.frameMain.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateContacts()
        {
            if (!string.IsNullOrWhiteSpace(TbContactPerson.Text) || !string.IsNullOrWhiteSpace(TbPhone.Text))
            {
                AppConnect.modelOdb.ClientContacts.Add(new ClientContacts
                {
                    ClientId = editingClient.ClientId,
                    ContactTypeId = 1, // Телефон
                    ContactPerson = TbContactPerson.Text,
                    ContactValue = TbPhone.Text
                });
            }

            if (!string.IsNullOrWhiteSpace(TbEmail.Text))
            {
                AppConnect.modelOdb.ClientContacts.Add(new ClientContacts
                {
                    ClientId = editingClient.ClientId,
                    ContactTypeId = 2, // Email
                    ContactValue = TbEmail.Text
                });
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }
    }
}