using EasyJob_ProDG.Data;
using EasyJob_ProDG.UI.View.DialogWindows;
using EasyJob_ProDG.UI.View.DialogWindows.ToolWindows;
using EasyJob_ProDG.UI.View.WindowBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyJob_ProDG.UI.Services.DialogServices
{
    /// <summary>
    /// Service made to handle toolbox windows.
    /// It will ensure only single window created, if required
    /// </summary>
    internal class ToolWindowsOperator : IToolWindowsOperator
    {
        private IWindowDisplayService _displayService => ServicesHandler.GetServicesAccess().WindowDisplayServiceAccess;

        private readonly Dictionary<Type, AnimatedToolWindow> _openWindows;


        public void ShowMergePortNamesWindow()
        {
            ShowWindow<MergePortNamesWindow, MergePortNamesViewModel>();
        }

        public void ShowSelectToolWindow()
        {
            ShowWindow<SelectToolWindow, SelectToolViewModel>();
        }

        public void ShowFilterToolWindow()
        {
            ShowWindow<FilterToolWindow, FilterToolViewModel>();
        }

        public void ShowSortToolWindow()
        {
            ShowWindow<SortToolWindow, SortToolViewModel>();
        }

        public void ShowSetToolWindow()
        {
            ShowWindow<SetToolWindow, SetToolViewModel>();
        }

        public void ShowUDCToolWindow()
        {
            ShowWindow<UserDefinedConditionsToolWindow, UserDefinedConditionToolViewModel>();
        }



        /// <summary>
        /// Method focuses on existing window, otherwise creates new window bound to new viewModel.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VM"></param>
        /// <returns>True if the window succesfully created. False if the window already exists.</returns>
        private bool ShowWindow<T, VM>()
            where T : AnimatedToolWindow
            where VM : class, new()
        {
            var type = typeof(T);

            //focus on existing window
            if (GetWindow<T>() is { } existing)
            {
                existing.Focus();
                return false;
            }

            //create constructors for window and view model
            var ctorWindow = typeof(T).GetConstructor(Type.EmptyTypes);
            var ctorVM = typeof(VM).GetConstructor(Type.EmptyTypes);

            if (ctorWindow == null || ctorVM == null)
            {
                LogWriter.WriteError($"Could not create constructor for type {typeof(T)} or {typeof(VM)} while creating tool window.");
            }

            //create instances of window and view model
            VM vm;
            T window = (T)ctorWindow.Invoke(null);
            vm = (VM)ctorVM.Invoke(null);

            //display window
            _displayService.ShowNormal(window, vm);
            window.Closed += OnWindowClosed;

            //add to openWindows collection
            _openWindows[typeof(T)] = window;

            return true;
        }

        public void CloseAllWindows()
        {
            foreach (AnimatedToolWindow window in _openWindows.Values.ToList())
            {
                window.Close();
            }
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            if (sender is not AnimatedToolWindow window)
                return;

            window.Closed -= OnWindowClosed;
            if (window.DataContext is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _openWindows.Remove(window.GetType());
        }

        /// <summary>
        /// Returns window from <see cref="_openWindows"/>, if exists, by its type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T? GetWindow<T>() where T : AnimatedToolWindow
                    => _openWindows.TryGetValue(typeof(T), out var w) ? (T)w : null;


        #region Singleton

        static ToolWindowsOperator _instance;
        public static ToolWindowsOperator GetOperator()
        {
            _instance ??= new ToolWindowsOperator();
            return _instance;
        }

        private ToolWindowsOperator()
        {
            _openWindows = new();
        }

        #endregion

    }
}
