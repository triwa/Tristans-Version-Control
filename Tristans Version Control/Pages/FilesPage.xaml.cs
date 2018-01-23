using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using Tristans_Version_Control.Properties;

namespace Tristans_Version_Control
{
    //
    // Interaction logic for FilesPage.xaml
    //
    public partial class FilesPage : Page
    {
        public ObservableCollection<TvcFile> filesCollection = new ObservableCollection<TvcFile>();

        public FilesPage()
        {
            InitializeComponent();
            SelectedFileGrid.Visibility = Visibility.Hidden;
            LoadSettings();
            FilesListView.ItemsSource = filesCollection;
        }

        //removes the TvcFile from the collection
        private void DeleteFileButton_Click(object sender, RoutedEventArgs e)
        {
            Button sendingButton = (Button)sender;
            TvcFile selectedTvcFile = (TvcFile)sendingButton.DataContext;

            filesCollection.Remove(selectedTvcFile);
        }

        //shows the save paths of the selected TvcFile
        private void FilesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TvcFile selectedItem = (TvcFile)FilesListView.SelectedItem;

            if (selectedItem == null)
            {
                SelectedFileGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                SelectedFileGrid.DataContext = selectedItem;
                SelectedFileGrid.Visibility = Visibility.Visible;
            }
        }

        //remove the save path from the selected TvcFiles's savepath collection
        private void DeleteSavePathButton_Click(object sender, RoutedEventArgs e)
        {
            Button sendingButton = (Button)sender;
            TvcFile selectedFile = (TvcFile)FilesListView.SelectedItem;

            selectedFile.SavePaths.Remove((string)sendingButton.DataContext);
        }

        //creates a new TvcFile and adds it to the collection
        private void AddNewFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false
            };
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                TvcFile tvcFile = new TvcFile()
                {
                    FilePath = openFileDialog.FileName,
                    FileNameFull = openFileDialog.SafeFileName,
                    TimerInterval = 10,
                    SavePaths = { System.IO.Path.ChangeExtension(openFileDialog.FileName, null) + " -- Version History" }
                };

                filesCollection.Add(tvcFile);
                FilesListView.SelectedItem = tvcFile;
            }
        }

        //creates a new save location and adds it to the TvcFiles's savepath collection
        private void AddNewSaveButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
            folderDialog.IsFolderPicker = true;
            CommonFileDialogResult result = folderDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                TvcFile selectedFile = (TvcFile)FilesListView.SelectedItem;

                selectedFile.SavePaths.Add(folderDialog.FileName);
            }
        }
        
        //parse files from application settings and fill collection
        private void LoadSettings() {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<TvcFile>));
            StringReader stringReader = new StringReader(Settings.Default.XMLSerializedTvcFileCollection);
            filesCollection = (ObservableCollection<TvcFile>)xmlSerializer.Deserialize(stringReader);
        }
    }
}