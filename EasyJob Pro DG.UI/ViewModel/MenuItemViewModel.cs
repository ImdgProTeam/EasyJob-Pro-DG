using EasyJob_ProDG.UI.Utility;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel
{
    /// <summary>
    /// Class for dynamic context menu
    /// </summary>
    public class MenuItemViewModel : Observable
    {
        public string Header { get; set; }
        public ICommand Command { get; set; }
    }
}
