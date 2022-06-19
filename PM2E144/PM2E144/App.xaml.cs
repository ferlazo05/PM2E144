using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PM2E144.Controller;
using System.IO;

namespace PM2E144
{
    public partial class App : Application
    {
        static DataBase db;
        public static DataBase DBase
        {
            get
            {
                if (db == null)
                {
                    String FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Sites.db3");
                    db = new DataBase(FolderPath);
                }
                return db;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
