﻿using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.ViewModel.MainWindow;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DataGridReefersViewModel : DataGridContainerViewModelBase
    {
        #region Constructor

        // ---------- Constructor ---------------
        public DataGridReefersViewModel() : base()
        {
        }

        #endregion

        #region StartUp logic

        /// <summary>
        /// Sets data source to View property
        /// </summary>
        protected override void SetDataView()
        {
            SetPlanViewSource(CargoPlan.Reefers);
        }

        /// <summary>
        /// Assigns handler methods for commands
        /// </summary>
        protected override void LoadCommands()
        {

        }

        /// <summary>
        /// Subscribe for messages in DataMessenger
        /// </summary>
        protected override void RegisterInDataMessenger()
        {
            DataMessenger.Default.Register<CargoDataUpdated>(this, OnReeferInfoUpdated, "reeferinfoupdated");
        }

        #endregion

        #region Filter Logic
        // ----------- Filter logic ----------------

        #endregion

        #region AddReefer Logic

        protected override void OnAddNewUnit(object obj)
        {
            //Action
            CargoPlan.AddNewReefer(new Model.Cargo.Container()
            {
                ContainerNumber = UnitToAddNumber,
                Location = UnitToAddLocation.CorrectFormatContainerLocation()
            });

            //Scroll into the new Container
            SelectedUnit = CargoPlan.Reefers[CargoPlan.Reefers.Count - 1];
            OnPropertyChanged(nameof(SelectedUnit));
        }

        #endregion

        #region Methods
        // ----- Methods -----

        protected override void RemoveUnit(List<string> containerNumbers)
        {
            foreach (var number in containerNumbers)
            {
                CargoPlan.RemoveReefer(number, toUpdateInCargoPlan: true);
            }
        }

        /// <summary>
        /// Updates reeferView update after import manifest info
        /// </summary>
        /// <param name="obj"></param>
        private void OnReeferInfoUpdated(CargoDataUpdated obj)
        {
            dispatcher.Invoke(() =>
            {
                SetDataView();
                OnPropertyChanged(nameof(UnitsPlanView));
            });
        }

        #endregion
    }
}
