using System;
using System.Linq;
using System.Windows;
using Tristans_Version_Control.Properties;
using System.Windows.Input;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using IWshRuntimeLibrary;

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
        Timer tvcTimer;
        System.Windows.Forms.NotifyIcon trayIcon = new System.Windows.Forms.NotifyIcon();
        System.Windows.Forms.ContextMenu trayMenu = new System.Windows.Forms.ContextMenu();

        public MainWindow()
        {
            InitializeComponent();
            InitializeTray();
            SettingsPage.DataContext = Settings.Default;
            tvcTimer = new Timer(TvcTimer_Elapsed, null, 0, 60000);

            //start minimized depending on setting
            if (Settings.Default.StartMinimizedBool)
            {
                Hide();
            }

            //startup page
            FilesMenuButton_Click(null, null);
        }

        private void InitializeTray()
        {
            trayIcon.Text = "Tristan's Version Control";
            trayIcon.Icon = Properties.Resources.super_special_icon_made_by_grant;

            trayMenu.MenuItems.Add("Settings", TraySettings);
            trayMenu.MenuItems.Add("Exit", TrayExit);

            //match the tray menu to the tray icon
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }

        private void TrayExit(object sender, EventArgs e)
        {
            Close();
        }

        private void TraySettings(object sender, EventArgs e)
        {
            Show();
        }

        //navigate to files menu
        private void FilesMenuButton_Click(object sender, RoutedEventArgs e)
        {
            FilesMenuButton.Background.Opacity = .5;
            NotificationsMenuButton.Background.Opacity = 0;
            SettingsMenuButton.Background.Opacity = 0;
            ContentFrame.Navigate(FilesPage);
        }

        //navigate to notifications menu
        private void NotificationsMenuButton_Click(object sender, RoutedEventArgs e)
        {
            FilesMenuButton.Background.Opacity = 0;
            NotificationsMenuButton.Background.Opacity = .5;
            SettingsMenuButton.Background.Opacity = 0;
            ContentFrame.Navigate(NotificationsPage);
        }

        //navigate to settings menu
        private void SettingsMenuButton_Click(object sender, RoutedEventArgs e)
        {
            FilesMenuButton.Background.Opacity = 0;
            NotificationsMenuButton.Background.Opacity = 0;
            SettingsMenuButton.Background.Opacity = .5;
            ContentFrame.Navigate(SettingsPage);
        }

        //Save settings and Minimize App
        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();

            //serialize the tvc files collection to xml and save to application settings
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<TvcFile>));
            StringWriter stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, FilesPage.filesCollection);
            Settings.Default.XMLSerializedTvcFileCollection = stringWriter.GetStringBuilder().ToString();

            //serialize the tvc notifications collection to xml and save to application settings
            xmlSerializer = new XmlSerializer(typeof(ObservableCollection<TvcNotificationProgram>));
            stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, NotificationsPage.selectedNotificationsCollection);
            Settings.Default.XMLSerializedNotificationsCollection = stringWriter.GetStringBuilder().ToString();
            Settings.Default.Save();

            //add or remove tvc from the startup folder
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            if (Settings.Default.StartWithWindowsBool)
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(startupFolder + "\\Tristans Version Control.lnk");
                shortcut.Description = "Tristans Version Control";
                shortcut.TargetPath = Directory.GetCurrentDirectory() + "\\Tristans Version Control.exe";
                shortcut.Save();
            }
            else
            {
                System.IO.File.Delete(startupFolder + "\\Tristans Version Control.lnk");
            }

            TvcTimer_Elapsed(null);
        }

        //backs up selected files and shows notifications
        private void TvcTimer_Elapsed(object state)
        {
            //counts down the remaining times on tvc files and backs up when it hits 0
            string currentTime = DateTime.Now.ToString().Replace("/", "-").Replace(":", ".");

            foreach (TvcFile tvcFile in FilesPage.filesCollection)
            {
                if (tvcFile.timerRemaining == 0)
                {
                    foreach (string savePath in tvcFile.SavePaths)
                    {
                        if (!Directory.Exists(savePath))
                        {
                            Directory.CreateDirectory(savePath);
                        }

                        System.IO.File.Copy(tvcFile.FilePath, savePath + "\\" + tvcFile.FileNameShort + " -- " + currentTime + tvcFile.FileExtension);
                    }

                    tvcFile.timerRemaining = tvcFile.TimerInterval;
                }
                else
                {
                    tvcFile.timerRemaining--;
                }
            }

            //when a program is opened for the first time, show a notification to open TVC
            System.Media.SoundPlayer player;

            foreach (TvcNotificationProgram tvcnotify in NotificationsPage.selectedNotificationsCollection)
            {
                if (Process.GetProcesses().Select(i => i.ProcessName).Contains(tvcnotify.ProgramName))
                {
                    if (tvcnotify.IsRunning == false)
                    {
                        player = new System.Media.SoundPlayer(Properties.Resources.bigGuy);
                        player.Play();

                        tvcnotify.IsRunning = true;
                        Settings.Default.Save();

                        MessageBoxResult result = MessageBox.Show(
                            "Wew lad, I see " + tvcnotify.ProgramName + " is set to have notifications. You wanna start backing up your files?",
                            "WEW",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question,
                            MessageBoxResult.None,
                            MessageBoxOptions.ServiceNotification);

                        if (result == MessageBoxResult.Yes)
                        {
                            player = new System.Media.SoundPlayer(Properties.Resources.forYou);
                            player.Play();

                            Show();
                        }
                        else
                        {
                            player = new System.Media.SoundPlayer(Properties.Resources.hotHead);
                            player.Play();
                        }
                    }
                }
                else if (tvcnotify.IsRunning)
                {
                    tvcnotify.IsRunning = false;
                }
            }
        }
        
        //Confirm Close and wipes collections depending on selected options
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit? This will stop any backups currently running.", "Exit TVC?", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                if (Settings.Default.RememberFilesBool == false)
                {
                    FilesPage.filesCollection.Clear();
                }
                if (Settings.Default.RememberNotificationsBool == false)
                {
                    NotificationsPage.selectedNotificationsCollection.Clear();
                }
                FinishButton_Click(null, null);
            }

            trayIcon.Visible = false;
        }

        private void WindowCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WindowTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
