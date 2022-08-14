using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace EasyJob_ProDG.UI.Utility
{
    /// <summary>
    /// Observable collection which can be accessed from any thread.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        private SynchronizationContext synchronizationContext = SynchronizationContext.Current;

        public AsyncObservableCollection()
        {

        }

        public AsyncObservableCollection(IEnumerable<T> list) : base(list)
        {

        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (SynchronizationContext.Current == synchronizationContext)
            {
                RaiseCollecitonChanged(e);
            }
            else
            {
                synchronizationContext.Send(RaiseCollecitonChanged, e);
            }
        }

        private void RaiseCollecitonChanged(object param)
        {
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == synchronizationContext)
            {
                RaisePropertyChanged(e);
            }
            else
            {
                synchronizationContext.Send(RaisePropertyChanged, e);
            }
        }

        private void RaisePropertyChanged(object param)
        {
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }
    }
}
