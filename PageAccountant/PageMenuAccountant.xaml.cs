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
using RecruitmentAgency.ApplicationData;
using RecruitmentAgency.PageMain;

namespace RecruitmentAgency.PageAccountant
{
    // Меню бухгалтера (Заглушки)
    public partial class PageMenuAccountant : Page
    {

        public PageMenuAccountant() 
        {
            InitializeComponent();
        }

        private void BtnManageReports_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnSendReportsToTaxService_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            // Вернуться в главное меню
            AppFrame.frameMain.GoBack();
        }

        private void BtnCalculateCommissions_Click(object sender, RoutedEventArgs e) 
        {

        }
        private void BtnCreateInvoices_Click(object sender, RoutedEventArgs e) 
        { 

        }
    }
}
