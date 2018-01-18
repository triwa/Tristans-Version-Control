using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Threading;

namespace Tristans_Version_Control
{
    //
    // This object holds all the information for files that are being backed up
    //
    public class TvcFile : INotifyPropertyChanged
    {
        private string filePath;
        private string fileName;
        private ObservableCollection<string> savePaths = new ObservableCollection<string>();
        private int timerInterval;
        
        public TvcFile()
        {
        }
        
        public string FileName
        {
            get => fileName;
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }
        public ObservableCollection<string> SavePaths
        {
            get => savePaths;
            set
            {
                savePaths = value;
                OnPropertyChanged("SavePaths");
            }
        }

        public int TimerInterval
        {
            get => timerInterval;
            set
            {
                timerInterval = value;
                OnPropertyChanged("TimerInterval");
            }
        }

        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string changedProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(changedProperty));
        }
    }
}