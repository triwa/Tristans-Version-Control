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


namespace Tristans_Version_Control
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FilesMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("FilesPage.xaml",UriKind.Relative));
        }

        private void NotificationsMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("NotificationsPage.xaml", UriKind.Relative));
        }
    }
}
