﻿using LogistHelper.Models;
using LogistHelper.Services;
using LogistHelper.ViewModels.Windows;
using System.Windows;

namespace LogistHelper
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ContainerService.Services.GetService(typeof(MainWindowViewModel));

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.ContentControl = content;
            NavigationService.Navigate(PageType.Enter);
        }
    }
}