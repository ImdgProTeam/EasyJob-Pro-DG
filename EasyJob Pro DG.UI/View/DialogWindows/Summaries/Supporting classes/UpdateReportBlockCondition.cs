using EasyJob_ProDG.UI.Utility;
using System;
using System.Windows;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.DialogWindows.Summaries
{
    public class UpdateReportBlockCondition : Observable
    {
        private bool _showZeroValues;

        public int ContainersCount { get; protected set; }
        public int ReefersCount { get; protected set; }
        public int DgContainersCount { get; protected set; }

        public ICommand ShowContainers { get; private set; }
        public ICommand ShowReefers { get; private set; }
        public ICommand ShowDg { get; private set; }

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

        void LoadCommands()
        {
            ShowContainers = new DelegateCommand(OnShowContainers, CanShowContainers);
            ShowReefers = new DelegateCommand(OnShowReefers, CanShowReefers);
            ShowDg = new DelegateCommand(OnShowDg, CanShowDg);
        }

        #region Command methods

        private void OnShowContainers(object obj)
        {
            OnShowContainersExectued.Invoke(this, new EventArgs());
        }

        private void OnShowReefers(object obj)
        {
            throw new NotImplementedException();
        }
        private void OnShowDg(object obj)
        {
            throw new NotImplementedException();
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

        #endregion

        #region Constructor
        public UpdateReportBlockCondition(int containersCount, int reefersCount, int dgContainersCount)
        {
            ContainersCount = containersCount;
            ReefersCount = reefersCount;
            DgContainersCount = dgContainersCount;

            LoadCommands();
            OnPropertyChanged();
        } 
        #endregion
    }
}
