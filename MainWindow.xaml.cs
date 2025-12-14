using RecruitmentAgency.ApplicationData;
using RecruitmentAgency.PageMain;
using System;
using System.Windows;

namespace RecruitmentAgency
{
    public partial class MainWindow : Window
    {
        // Инициализация главного окна: подключение БД и навигация на страницу входа
        public MainWindow()
        {
            InitializeComponent();
            // Создание объекта класса подключения к БД
            AppConnect.modelOdb = new ApplicationData.RecruitmentAgencyEntities();
            //Создаем фрейм
            AppFrame.frameMain = FrmMain;

            FrmMain.Navigate(new PageLogin());
        }
    }
}