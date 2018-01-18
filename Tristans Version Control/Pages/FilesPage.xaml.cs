using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            LoadTestData();


            FilesListView.ItemsSource = filesCollection;
        }

        private void LoadTestData()
        {
            //
            //test data
            //
            TvcFile example1 = new TvcFile() { FilePath = "path1", FileName = "name1", TimerInterval = 1 };
            example1.SavePaths.Add("name1Save1");
            example1.SavePaths.Add("name1Save2");
            example1.SavePaths.Add("name1Save3");
            TvcFile example2 = new TvcFile() { FilePath = "path2", FileName = "name2", TimerInterval = 2 };
            example2.SavePaths.Add("name2Save1");
            example2.SavePaths.Add("name2Save2");
            example2.SavePaths.Add("name2Save3");
            TvcFile example3 = new TvcFile() { FilePath = "path3", FileName = "name3", TimerInterval = 3 };
            example3.SavePaths.Add("name3Save1");
            example3.SavePaths.Add("name3Save2");
            example3.SavePaths.Add("name3Save3");
            TvcFile example4 = new TvcFile() { FilePath = "path4", FileName = "name4", TimerInterval = 4 };
            example4.SavePaths.Add("name4Save1");
            example4.SavePaths.Add("name4Save2");
            example4.SavePaths.Add("name4Save3");
            TvcFile example5 = new TvcFile() { FilePath = "path5", FileName = "name5", TimerInterval = 5 };
            example5.SavePaths.Add("name5Save1");
            example5.SavePaths.Add("name5Save2");
            example5.SavePaths.Add("name5Save3");
            filesCollection.Add(example1);
            filesCollection.Add(example2);
            filesCollection.Add(example3);
            filesCollection.Add(example4);
            filesCollection.Add(example5);
            //
            //end test data
            //
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
                SelectedFileLabel.Content = selectedItem.FileName;
                SavePathsListView.ItemsSource = selectedItem.SavePaths;
                TimerIntervalTextBox.Text = selectedItem.TimerInterval.ToString();
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                TvcFile tvcFile = new TvcFile()
                {
                    FilePath = openFileDialog.FileName,
                    FileName = openFileDialog.SafeFileName,
                    TimerInterval = 5,
                    SavePaths = { System.IO.Path.ChangeExtension(openFileDialog.FileName, null) + " -- Version History"}
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
    }
}