using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexium2.ViewModel
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private Dictionary<string, string> _wordObj = new Dictionary<string, string>();
        public Dictionary<string, string> WordObj
        {
            get => _wordObj;
            set
            {
                _wordObj = value;
                //UpdateWordList();
                UpdateList();
                //OnPropertyChanged(nameof(WordObj));
            }
        }
        private ObservableCollection<string> _wordList = new ObservableCollection<string>();
        public ObservableCollection<string> WordList
        {
            get => _wordList;
            private set
            {
                _wordList = value;
                OnPropertyChanged(nameof(WordList));
            }
        }
        public MainWindowVM()
        {
            WordObj = new Dictionary<string, string>
            {
                { "Faiz", "Nama Writer" },
                { "Ahmad Faiz Bin Ghazali", "Nama Penuh Writer" }
            };

        }

        private void UpdateList()
        {
            WordList.Clear();
            foreach (var key in WordObj.Keys)
            {
                WordList.Add(key);
            }
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
