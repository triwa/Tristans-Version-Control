using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Threading;

namespace Tristans_Version_Control
{
    //
    // This object holds all the information for files that are being backed up
    //
    public class TvcFile : INotifyPropertyChanged
    {
        private string filePath;
        private string fileNameFull;
        public string FileNameShort { get; set; }
        public string FileExtension { get; set; }
        public ObservableCollection<string> SavePaths { get; set; }
        private int timerInterval;
        public int timerRemaining { get; set; }

        public TvcFile()
        {
            SavePaths = new ObservableCollection<string>();
            timerRemaining = 0;
        }
        
        public string FileNameFull
        {
            get => fileNameFull;
            set
            {
                fileNameFull = value;
                FileNameShort = Path.GetFileNameWithoutExtension(value);
                FileExtension = Path.GetExtension(value);

                OnPropertyChanged("FileName");
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