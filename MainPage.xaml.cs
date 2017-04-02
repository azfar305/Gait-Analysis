﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Trace
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Camera_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CameraPage());
        }

        private void BackgroundRemoval_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BackgroundRemovalPage());
        }

        private void Angle_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AnglePage());
        }

        private void Gestures_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GesturesPage());
        }

        private void Face_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FacePage());
        }

        private void JointSelection_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new JointSelectionPage());
        }

        private void Features_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FeaturesPage());
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
