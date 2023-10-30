using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace EasyJob_ProDG.UI.Settings
{
    public class DgTableColumnSettings
    {

        //public System.Windows.Visibility Visibility { get; set; }
        //public int DisplayIndex { get; set; }
        //public string Width { get; set; }
        public DataGridLength DefaultWidth { get; set; }

        public DgTableColumnSettings(int displayIndex)
        {
            DisplayIndex = displayIndex;
            Visibility = System.Windows.Visibility.Visible;
            Width = "Auto";
        }
        public DgTableColumnSettings() { }

        public event PropertyChangedEventHandler PropertyChanged;
        protected internal void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Width
        {
            get { return m_width; }
            set { m_width = value; OnPropertyChanged("Width"); }
        }
        string m_width;
        public Visibility Visibility
        {
            get { return m_visibility; }
            set { m_visibility = value; OnPropertyChanged("Visibility"); }
        }
        Visibility m_visibility;
        public int DisplayIndex
        {
            get { return m_displayIndex; }
            set { m_displayIndex = value; OnPropertyChanged("DisplayIndex"); }
        }
        int m_displayIndex;
    }
}
