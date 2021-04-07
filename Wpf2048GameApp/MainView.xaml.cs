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
using System.Windows.Shapes;

namespace Wpf2048GameApp
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            int size = 0;
            if (Easy.IsChecked.Value)
            {
                size = 4;
            }
            else if (Normal.IsChecked.Value)
            {
                size = 5;
            }
            else if (Hard.IsChecked.Value)
            {
                size = 6;
            }

            
            MainWindow mainWindow = new MainWindow(size);
            this.Hide();
            mainWindow.ShowDialog();
            this.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
