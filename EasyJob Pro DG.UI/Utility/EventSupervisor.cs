using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.View.UI;
using EasyJob_ProDG.UI.Wrapper;

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

            //Main window events  
            MainWindow.OnWindowClosingEventHandler += new MainWindow.WindowClosing(OnMainWindowClosing);

        }

        // ---------------- Methods ----------------------------------------

        //Main window event related methods
        private static void OnMainWindowClosing()
        {
            DataMessenger.Default.Send(new ApplicationClosingMessage(), "closing");
        }

        //WorkingCargoPlan event related methods
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
