using RecruitmentAgency.ApplicationData;
using RecruitmentAgency.PageMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecruitmentAgency.PageManager
{
    // Главная страница соискателя: функционал и выход
    public partial class PageManagerMain : Page
    {
        private Users currentUser;

        public PageManagerMain(Users User )
        {
            InitializeComponent();
            currentUser = User;
        }

        private void BtnCreateVacancy_Click(object sender, RoutedEventArgs e)
        {
           //
        }

        private void BtnManageClients_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageManager.PageManageClients(currentUser));
        }

        private void BtnTrackVacancies_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void BtnBilling_Click(object sender, RoutedEventArgs e)
        {
           //
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }
        private void BtnCandidates_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void BtnSearchAndMatch_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void BtnInterviews_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {
            //
        }
    }
}
