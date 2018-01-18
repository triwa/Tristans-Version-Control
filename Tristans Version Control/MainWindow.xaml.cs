using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tristans_Version_Control.Properties;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Xml;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;

namespace Tristans_Version_Control
{
    //
    // This applications provides a way to incrementally back up files, and show a notification when selected programs are open.
    // File options, Notifications, and Settings are saved in the application settings.
    //
    public partial class MainWindow : Window
    {
        FilesPage FilesPage = new FilesPage();
        NotificationsPage NotificationsPage = new NotificationsPage();
        SettingsPage SettingsPage = new SettingsPage();
        
        public MainWindow()
        {
            InitializeComponent();
            FilesMenuButton_Click(null,null);
        }

        //navigate to files menu
        private void FilesMenuButton_Click(object sender, RoutedEventArgs e)
        {
            FilesMenuButton.Background.Opacity = .8;
            NotificationsMenuButton.Background.Opacity = 0;
            SettingsMenuButton.Background.Opacity = 0;
            ContentFrame.Navigate(FilesPage);
        }

        //navigate to notifications menu
        private void NotificationsMenuButton_Click(object sender, RoutedEventArgs e)
        {
            FilesMenuButton.Background.Opacity = 0;
            NotificationsMenuButton.Background.Opacity = .8;
            SettingsMenuButton.Background.Opacity = 0;
            ContentFrame.Navigate(NotificationsPage);
        }

        //navigate to settings menu
        private void SettingsMenuButton_Click(object sender, RoutedEventArgs e)
        {
            FilesMenuButton.Background.Opacity = 0;
            NotificationsMenuButton.Background.Opacity = 0;
            SettingsMenuButton.Background.Opacity = .8;
            ContentFrame.Navigate(SettingsPage);
        }

        //Save enabled settings and Minimize App
        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            //serialize the tvc files collection to xml and save to application settings
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<TvcFile>));
            StringWriter stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, FilesPage.filesCollection);
            Settings.Default.XMLSerializedTvcFileCollection = stringWriter.GetStringBuilder().ToString();

            //save the notifications collection to settings
            //
            //
            //
            //
            //
            //
            //
            //
            //

            Settings.Default.Save();
        }

        //Confirm Close
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit? This will stop any backups currently running.", "Exit TVC?", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
