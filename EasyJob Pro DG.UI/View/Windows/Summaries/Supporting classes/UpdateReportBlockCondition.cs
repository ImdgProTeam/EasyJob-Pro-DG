using EasyJob_ProDG.UI.Utility;
using System;
using System.Windows;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.DialogWindows.Summaries
{
    public class UpdateReportBlockCondition : Observable
    {
        private bool _showZeroValues;
        ShowContainersEventArgs _args;

        public int ContainersCount { get; protected set; }
        public int ReefersCount { get; protected set; }
        public int DgContainersCount { get; protected set; }

        public bool ShowZeroValues
        {
            get => _showZeroValues;
            set
            {
                _showZeroValues = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowVisible));
            }
        }
        public Visibility ShowVisible => HasValues || _showZeroValues ? 
            Visibility.Visible : Visibility.Collapsed;

        public bool HasValues => ContainersCount > 0 || ReefersCount > 0 || DgContainersCount > 0;

        #region Highlight selection
        public bool HighlightContainers { get; private set; } = false;
        public bool HighlightReefers { get; private set; } = false;
        public bool HighlightDgContainers { get; private set; } = false;
        public bool HighlightTitle => HighlightContainers || HighlightReefers || HighlightDgContainers;

        /// <summary>
        /// Sets all 'Highlight' property to false.
        /// </summary>
        public void ClearHighlights()
        {
            HighlightContainers = false;
            HighlightReefers = false;
            HighlightDgContainers = false;
            OnPropertyChanged(null);
        }

        private void SetHighlightContainers(bool value)
        {
            HighlightContainers = value;
            OnPropertyChanged(nameof(HighlightContainers));
            OnPropertyChanged(nameof(HighlightTitle));
        }

        private void SetHighlightReefers(bool value)
        {
            HighlightReefers = value;
            OnPropertyChanged(nameof(HighlightReefers));
            OnPropertyChanged(nameof(HighlightTitle));
        }

        private void SetHighlightDgContainers(bool value)
        {
            HighlightDgContainers = value;
            OnPropertyChanged(nameof(HighlightDgContainers));
            OnPropertyChanged(nameof(HighlightTitle));
        }
        #endregion


        #region Commands

        public ICommand ShowContainers { get; private set; }
        public ICommand ShowReefers { get; private set; }
        public ICommand ShowDg { get; private set; }

        void LoadCommands()
        {
            ShowContainers = new DelegateCommand(OnShowContainers, CanShowContainers);
            ShowReefers = new DelegateCommand(OnShowReefers, CanShowReefers);
            ShowDg = new DelegateCommand(OnShowDg, CanShowDg);
        }

        #endregion


        #region Command methods

        private void OnShowContainers(object obj)
        {
            _args.UnitsToShow = Units.Containers;
            OnShowContainersExectued.Invoke(this, _args);
            SetHighlightContainers(true);
        }

        private void OnShowReefers(object obj)
        {
            _args.UnitsToShow = Units.Reefers;
            OnShowContainersExectued.Invoke(this, _args);
            SetHighlightReefers(true);
        }
        private void OnShowDg(object obj)
        {
            _args.UnitsToShow = Units.DgContainers;
            OnShowContainersExectued.Invoke(this, _args);
            SetHighlightDgContainers(true);
        }

        private bool CanShowContainers(object obj)
        {
            if (ContainersCount > 0) return true;
            return false;
        }

        private bool CanShowReefers(object obj)
        {
            if (ReefersCount > 0) return true;
            return false;
        }

        private bool CanShowDg(object obj)
        {
            if (DgContainersCount > 0) return true;
            return false;
        }

        #endregion

        #region Events

        internal event EventHandler OnShowContainersExectued;
        internal class ShowContainersEventArgs : EventArgs
        {
            internal Units UnitsToShow { get; set; }
        }

        #endregion

        #region Constructor
        public UpdateReportBlockCondition(int containersCount, int reefersCount, int dgContainersCount)
        {
            ContainersCount = containersCount;
            ReefersCount = reefersCount;
            DgContainersCount = dgContainersCount;

            LoadCommands();
            OnPropertyChanged();

            _args = new ShowContainersEventArgs();
        }
        #endregion

    }
}
