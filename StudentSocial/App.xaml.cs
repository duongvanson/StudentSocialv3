﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using StudentSocial.Common;
using StudentSocial.GUI;

namespace StudentSocial
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            if(!Paths.checkToken())
            {
                Paths.createdPaths();
                WLogin wLogin = new WLogin();
                wLogin.Show();
            }
            else
            {
                WHome wHome = new WHome();
                wHome.Show();
            }
        } 
    }
}
