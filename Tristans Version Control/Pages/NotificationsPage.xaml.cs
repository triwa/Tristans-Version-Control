using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using Tristans_Version_Control.Properties;

namespace Tristans_Version_Control
{
    /// <summary>
    /// Interaction logic for NotificationsPage.xaml
    /// </summary>
    public partial class NotificationsPage : Page
    {
        public ObservableCollection<TvcNotificationProgram> selectedNotificationsCollection = new ObservableCollection<TvcNotificationProgram>();
        private List<string> currentProgramsList = new List<string>();

        public NotificationsPage()
        {
            InitializeComponent();
            LoadSettings();
            RefreshCurrentPrograms();
            SelectedNotificationsListView.ItemsSource = selectedNotificationsCollection;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshCurrentPrograms();
        }

        //shows the currently running programs
        private void RefreshCurrentPrograms()
        {
            //skip inaccessible processes
            currentProgramsList = Process.GetProcesses()
                .Select(i=>i.ProcessName)
                .Distinct()
                .ToList();

            CurrentProcessesListView.ItemsSource = currentProgramsList;

            if (FilterTextBox.Text != "")
            {
                FilterTextBox_TextChanged(null, null);
            }
        }

        //filters the current programs listview by the value entered in the filter text box
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FilterTextBox.Text == "")
            {
                RefreshCurrentPrograms();
            }
            else
            {
                List<string> filteredList = currentProgramsList.Where(i => i.ToLower().Contains(FilterTextBox.Text)).ToList();
                CurrentProcessesListView.ItemsSource = filteredList;
            }
        }

        //adds selected program to the selected notifications collection
        private void EnableForSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProcessesListView.SelectedItems.Count != 0)
            {
                foreach (string item in CurrentProcessesListView.SelectedItems)
                {
                    if (selectedNotificationsCollection.Select(i => i.ProgramName).Contains(item))
                    {
                        MessageBox.Show("Notifications are already enabled for: " + item + ".", "wew lad", MessageBoxButton.OK);
                    }
                    else
                    {
                        selectedNotificationsCollection.Add(new TvcNotificationProgram() { ProgramName = item });
                    }
                }
            }
        }

        //removes program from selected notifications
        private void RemoveSelectedNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedNotificationsListView.SelectedItems.Count != 0)
            {
                foreach (TvcNotificationProgram item in SelectedNotificationsListView.SelectedItems.Cast<TvcNotificationProgram>().ToList())
                {
                    selectedNotificationsCollection.Remove(item);
                }
            }
        }

        //parse notification programs from application settings and fill collection
        private void LoadSettings()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<TvcNotificationProgram>));
            StringReader stringReader = new StringReader(Settings.Default.XMLSerializedNotificationsCollection);
            selectedNotificationsCollection = (ObservableCollection<TvcNotificationProgram>)xmlSerializer.Deserialize(stringReader);
        }
    }
}
