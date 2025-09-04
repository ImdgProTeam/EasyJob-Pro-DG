using EasyJob_ProDG.UI.View.DialogWindows;
using EasyJob_ProDG.UI.View.DialogWindows.ToolWindows;
using System;
using System.Windows;

namespace EasyJob_ProDG.UI.Services.DialogServices
{
    /// <summary>
    /// Service made to handle toolbox windows.
    /// It will ensure only single window created, if required
    /// </summary>
    internal class ToolWindowsOperator : IToolWindowsOperator
    {
        private IWindowDisplayService _displayService => ServicesHandler.GetServicesAccess().WindowDisplayServiceAccess;

        private MergePortNamesWindow _mergePortNamesWindow;
        private SelectToolWindow _selectToolWindow;
        private FilterToolWindow _filterToolWindow;
        private SortToolWindow _sortToolWindow;
        private SetToolWindow _setToolWindow;


        public void ShowMergePortNamesWindow()
        {
            if (_mergePortNamesWindow != null)
            {
                _mergePortNamesWindow.Focus();
                return;
            }
            _mergePortNamesWindow = new MergePortNamesWindow();
            _displayService.ShowNormal(_mergePortNamesWindow, new MergePortNamesViewModel());
            _mergePortNamesWindow.Closed += OnWindowClosed;
        }

        public void ShowSelectToolWindow()
        {
            if (_selectToolWindow != null)
            {
                _selectToolWindow.Focus();
                return;
            }
            _selectToolWindow = new SelectToolWindow();
            _displayService.ShowNormal(_selectToolWindow, new SelectToolViewModel());
            _selectToolWindow.Closed += OnWindowClosed;
        }

        public void ShowFilterToolWindow()
        {
            if (_filterToolWindow != null)
            {
                _filterToolWindow.Focus();
                return;
            }
            _filterToolWindow = new FilterToolWindow();
            _displayService.ShowNormal(_filterToolWindow, new FilterToolViewModel());
            _filterToolWindow.Closed += OnWindowClosed;
        }

        public void ShowSortToolWindow()
        {
            if (_sortToolWindow != null)
            {
                _sortToolWindow.Focus();
                return;
            }
            _sortToolWindow = new SortToolWindow();
            _displayService.ShowNormal(_sortToolWindow, new SortToolViewModel());
            _sortToolWindow.Closed += OnWindowClosed;
        }

        public void ShowSetToolWindow()
        {
            if (_setToolWindow != null)
            {
                _setToolWindow.Focus();
                return;
            }
            _setToolWindow = new SetToolWindow();
            _displayService.ShowNormal(_setToolWindow, new SetToolViewModel());
            _setToolWindow.Closed += OnWindowClosed;
        }


        private void OnWindowClosed(object sender, EventArgs e)
        {
            var _window = sender as Window;
            _window.Closed -= OnWindowClosed;
            if (_window == _mergePortNamesWindow)
            {
                _mergePortNamesWindow = null;
            }
            if (_window == _selectToolWindow)
            {
                _selectToolWindow = null;
            }
            if (_window == _filterToolWindow)
            {
                _filterToolWindow = null;
            }
            if (_window == _sortToolWindow)
            {
                _sortToolWindow = null;
            }
            if (_window == _setToolWindow)
            {
                _setToolWindow = null;
            }
        }


        #region Singleton

        static ToolWindowsOperator _instance;
        public static ToolWindowsOperator GetOperator()
        {
            if (_instance == null)
            {
                _instance = new ToolWindowsOperator();
            }
            return _instance;
        }

        private ToolWindowsOperator()
        {

        }

        #endregion

    }
}
