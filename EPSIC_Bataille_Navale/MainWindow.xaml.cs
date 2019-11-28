﻿using EPSIC_Bataille_Navale.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EPSIC_Bataille_Navale
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Page currentPage;
        private static bool FullScreen;

        public MainWindow()
        {
            InitializeComponent();
            Home home = new Home();
            Content = home;
            currentPage = home;
            LoadPage(home);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.Enter) || Keyboard.IsKeyDown(Key.F11))
            {
                if (FullScreen)
                {
                    ResizeMode = ResizeMode.CanResize;
                    WindowState = WindowState.Normal;
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    FullScreen = false;
                }
                else
                {
                    WindowState = WindowState.Normal; //La fenêtre ne doit pas être maximizé lorsqu'on l'a met en plein écran, sinon la bar des tâches s'affiche quand même
                    ResizeMode = ResizeMode.NoResize;
                    WindowState = WindowState.Maximized;
                    WindowStyle = WindowStyle.None;
                    FullScreen = true;
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        public static void LoadPage(Page page)
        {
            GetWindow(currentPage).Content = page;
            currentPage = page;
        }
    }
}
