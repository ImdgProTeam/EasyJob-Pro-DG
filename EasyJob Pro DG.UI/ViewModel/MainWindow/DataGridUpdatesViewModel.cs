using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Utility.Messages;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DataGridUpdatesViewModel : DataGridViewModelBase
    {
        public Visibility Visible { get; set; }
        IEnumerable<ContainerWrapper> UpdatedContainers { get; set; }

        #region Constructor
        // ---------- Constructor ---------------
        public DataGridUpdatesViewModel() : base()
        {
            UpdatedContainers = new List<ContainerWrapper>();
            Visible = Visibility.Collapsed;
        }

        #endregion

        #region Startup logic

        /// <summary>
        /// Sets data source to View property
        /// </summary>
        protected override void SetDataView()
        {
            SetPlanViewSource(UpdatedContainers);
        }

        /// <summary>
        /// Assigns handler methods for commands
        /// </summary>
        protected override void LoadCommands()
        {

        }

        /// <summary>
        /// Registers for messages in DataMessenger
        /// </summary>
        protected override void RegisterInDataMessenger()
        {
            DataMessenger.Default.Unregister(this);
            DataMessenger.Default.Register<ShowUpdatesMessage>(this, OnShowUpdatesMessageReceived);
        }

        #endregion

        /// <summary>
        /// Sets UpdatedContainers with respective list of containers
        /// </summary>
        /// <param name="message"></param>
        private void OnShowUpdatesMessageReceived(ShowUpdatesMessage message)
        {
            if (message is null) return;

            switch (message.Units)
            {
                case View.Units.Containers:
                    UpdatedContainers = message.ContainersToShow.Select(c => new ContainerWrapper(c));
                    break;
                case View.Units.Reefers:
                    UpdatedContainers = message.ContainersToShow
                        .Where(c => c.IsRf)
                        .Select(c => new ContainerWrapper(c));
                    break;
                case View.Units.DgContainers:
                    UpdatedContainers = message.ContainersToShow
                        .Where(c => c.ContainsDgCargo)
                        .Select(c => new ContainerWrapper(c));
                    break;
                default:
                    break;
            }
            SetDataView();
            OnPropertyChanged(nameof(UnitsPlanView));

            Visible = Visibility.Visible;
            OnPropertyChanged(nameof(Visible));
        }


        #region Implementation of abstract class methods

        protected override void OnAddNewUnit(object obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSelectionChanged(object obj)
        {
            SetSelectionStatusBar(obj);
        }
        #endregion

        #region Commands


        #endregion
    }
}
