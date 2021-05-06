using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyJob_ProDG.UI.Utility
{
    /// <summary>
    /// Implements INotifyPropertyChanged interface
    /// </summary>
    public class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
