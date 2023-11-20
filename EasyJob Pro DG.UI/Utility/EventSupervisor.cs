using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.View.UI;
using EasyJob_ProDG.UI.View.User_Controls;
using EasyJob_ProDG.UI.Wrapper;
using EasyJob_ProDG.UI.Wrapper.Dummies;
using System;
using static EasyJob_ProDG.UI.Wrapper.DgWrapper;

namespace EasyJob_ProDG.UI.Utility
{
    /// <summary>
    /// Watches for events in project and sends respective messages to DataMessenger
    /// </summary>
    public class EventSupervisor
    {
        //Initiating static EventSupervisor
        public static EventSupervisor evS = new EventSupervisor();

        //Static constructor
        static EventSupervisor()
        {
            //Wrapper events
            DgWrapper.OnDgPackingGroupChangedEventHandler += new DgWrapper.DgPackingGroupChangedEventHandler(OnDgPackingGroupChanged);
            DgWrapper.OnConflictListToBeChangedEventHandler += new DgWrapper.ConflictListToBeChangedEventHandler(OnConflictListToBeChanged);
            DgWrapper.OnDgPropertyUpdatedEventHandler += new DgWrapper.DgPropertyUpdatedEventHandler(OnDgPropertyUpdated);

            //CargoPlanChange events
            CargoPlanUnitPropertyChanger.OnConflictListToBeChangedEventHandler += new CargoPlanUnitPropertyChanger.ConflictListToBeChangedEventHandler(OnConflictListToBeChanged);


            //Ship profile window events
            OuterRowWrapper.OnOuterRowChangedEventHandler += new OuterRowWrapper.OuterRowChangedEventHandler(OnOuterRowChanged);
            DummySuperstructure.OnDummySuperstructureChangedEventHandler += new DummySuperstructure.DummySuperstructureChangedEventHandler(OnDummyAccommodationChanged);
            TabLivingQuarters.OnRemoveRecordEventHandler += new TabLivingQuarters.RemoveRecord(OnRemoveRecordFromLivingQuarters);
            TabHeatedStructures.OnRemoveRecordEventHandler += new TabHeatedStructures.RemoveRecord(OnRemoveRecordFromHeatedStructures);
            TabLSA.OnRemoveRecordEventHandler += new TabLSA.RemoveRecord(OnRemoveRecordFromLSA);

            //User settings window event

            //Main window events  
            MainWindow.OnWindowClosingEventHandler += new MainWindow.WindowClosing(OnMainWindowClosing);

        }

        // ---------------- Methods ----------------------------------------

        //Main window event related methods
        private static void OnMainWindowClosing()
        {
            DataMessenger.Default.Send(new ApplicationClosingMessage(), "closing");
        }

        //Ship profile window event related methods
        private static void OnRemoveRecordFromLivingQuarters(byte row)
        {
            DataMessenger.Default.Send(new RemoveRowMessage(row, "living quarters"));
        }
        private static void OnRemoveRecordFromHeatedStructures(byte row)
        {
            DataMessenger.Default.Send(new RemoveRowMessage(row, "heated structures"));
        }
        private static void OnRemoveRecordFromLSA(byte row)
        {
            DataMessenger.Default.Send(new RemoveRowMessage(row, "LSA"));
        }
        private static void OnOuterRowChanged(object sender)
        {
            DataMessenger.Default.Send(new ShipProfileWrapperMessage(), "Outer row changed");
        }
        private static void OnDummyAccommodationChanged(object sender)
        {
            DataMessenger.Default.Send(new MessageFromDummy(), "Dummy Accommodation changed");
        }

        //CargoPlan event related methods
        private static void OnDgPackingGroupChanged(object sender)
        {
            DoNothing();
        }
        private static void OnConflictListToBeChanged(object sender)
        {
            DataMessenger.Default.Send(new ConflictListToBeUpdatedMessage());
        }

        private static void OnDgPropertyUpdated(object sender)
        {
            DataMessenger.Default.Send(new DgListSelectedItemUpdatedMessage(), "selectionpropertyupdated");
        }

        //Other methods
        /// <summary>
        /// Dummy blank void method
        /// </summary>
        static void DoNothing() { }

    }
}
