using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramDataControllerBehavior.Data
{
    class ViewModel
    {
        ObservableCollection<ClassData> items;
        ObservableCollection<ConnectionData> connections;
        public ObservableCollection<ClassData> Elements
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Elements));
            }
        }
        public ObservableCollection<ConnectionData> Connections
        {
            get
            {
                return connections;
            }
            set
            {
                connections = value;
                OnPropertyChanged(nameof(Connections));
            }
        }
        public ViewModel()
        {
            Elements = new ObservableCollection<ClassData>();
            Connections = new ObservableCollection<ConnectionData>();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    } 
    public class Office
    {
        public ObservableCollection<ClassData> Elements { get; set; }
        public ObservableCollection<ConnectionData> Connections { get; set; }
    }
}
