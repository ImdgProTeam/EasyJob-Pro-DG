using System.Windows;

namespace EasyJob_ProDG.UI.Services.DialogServices
{
    public interface IDialogWindow
    {
        object DataContext { get; set; }
        bool? DialogResult { get; set; }
        Window Owner { get; set; }
        void Close();
        bool? ShowDialog();

    }
}
