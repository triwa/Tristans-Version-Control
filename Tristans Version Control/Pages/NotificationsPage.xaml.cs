using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Interaction logic for NotificationsPage.xaml
    /// </summary>
    public partial class NotificationsPage : Page
    {
        public ObservableCollection<string> selectedNotificationsCollection = new ObservableCollection<string>();

        public NotificationsPage()
        {
            InitializeComponent();
            RefreshCurrentPrograms();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshCurrentPrograms();
        }

        //shows the currently running programs
        private void RefreshCurrentPrograms()
        {
            CurrentProgramsListView.ItemsSource = Process.GetProcesses().Select(i => i.ProcessName);
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
    }
}
