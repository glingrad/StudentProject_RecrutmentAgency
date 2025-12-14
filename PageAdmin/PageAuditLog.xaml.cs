using RecruitmentAgency.ApplicationData;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecruitmentAgency.PageAdmin
{
    public partial class PageAuditLog : Page
    {
        public PageAuditLog()
        {
            InitializeComponent();
            LoadAuditLogs();
        }


        private void LoadAuditLogs()
        {
            try
            {
                // Загружаем логи с данными пользователей и типами действий
                var auditLogs = AppConnect.modelOdb.AuditLogs
                    .Select(l => new {
                        l.LogId,
                        l.ActionDateTime,                  
                        UserName = $"{l.Users.LastName} {l.Users.FirstName} {l.Users.MiddleName}",
                        ActionName = l.AuditActions.ActionName,
                        l.Details,
                        l.IpAddress
                    })
                    .OrderByDescending(l => l.ActionDateTime)
                    .ToList();

                DgAuditLogs.ItemsSource = auditLogs;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке журнала аудита: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }
    }
}