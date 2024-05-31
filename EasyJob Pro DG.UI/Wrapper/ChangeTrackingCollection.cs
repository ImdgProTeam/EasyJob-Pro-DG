using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class ChangeTrackingCollection<T> : ObservableCollection<T>, IRevertibleChangeTracking
        where T : class, IRevertibleChangeTracking, INotifyPropertyChanged
    {
        private IList<T> _originalCollection;

        private ObservableCollection<T> _addedItems;
        private ObservableCollection<T> _removedItems;
        private ObservableCollection<T> _modifiedItems;

        public ReadOnlyObservableCollection<T> AddedItems { get; }
        public ReadOnlyObservableCollection<T> RemovedItems { get; }
        public ReadOnlyObservableCollection<T> ModifiedItems { get; }

        public ChangeTrackingCollection(IEnumerable<T> items) : base(items)
        {
            _originalCollection = this.ToList();

            AttachItemPropertyChangedHandler(_originalCollection);

            _addedItems = new ObservableCollection<T>();
            _removedItems = new ObservableCollection<T>();
            _modifiedItems = new ObservableCollection<T>();

            AddedItems = new ReadOnlyObservableCollection<T>(_addedItems);
            RemovedItems = new ReadOnlyObservableCollection<T>(_removedItems);
            ModifiedItems = new ReadOnlyObservableCollection<T>(_modifiedItems);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var added = this.Where(current => _originalCollection.All(orig => orig != current));
            var removed = _originalCollection.Where(orig => this.All(current => current != orig));
            var modified = this.Except(removed).Except(added).Where(item => item.IsChanged).ToList();

            AttachItemPropertyChangedHandler(added);
            DetachItemPropertyChangedHandler(removed);

            UpdateObservableCollection(_addedItems, added);
            UpdateObservableCollection(_removedItems, removed);
            UpdateObservableCollection(_modifiedItems, modified);

            base.OnCollectionChanged(e);
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsChanged)));
        }

        private void UpdateObservableCollection(ObservableCollection<T> collection, IEnumerable<T> items)
        {
            collection.Clear();
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        private void DetachItemPropertyChangedHandler(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                item.PropertyChanged -= ItemPropertyChanged;
            }
        }

        private void AttachItemPropertyChangedHandler(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                item.PropertyChanged += ItemPropertyChanged;
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = sender as T;
            if (_addedItems.Contains(item))
            {
                return;
            }

            if (item.IsChanged)
            {
                if (!_modifiedItems.Contains(item))
                {
                    _modifiedItems.Add(item);
                }
            }
            else
            {
                if (_modifiedItems.Contains(item))
                {
                    _modifiedItems.Remove(item);
                }
            }
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsChanged)));
        }

        public bool IsChanged => AddedItems.Count > 0 || RemovedItems.Count > 0 || ModifiedItems.Count > 0;

        public void AcceptChanges()
        {
            _addedItems.Clear();
            _removedItems.Clear();
            _modifiedItems.Clear();

            foreach (var item in this)
            {
                item.AcceptChanges();
            }

            _originalCollection = this.ToList();
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsChanged)));
        }

        public void RejectChanges()
        {
            foreach (var addedItem in _addedItems.ToList())
            {
                Remove(addedItem);
            }
            foreach (var removedItem in _removedItems.ToList())
            {
                Add(removedItem);
            }
            foreach (var modifiedItem in _modifiedItems.ToList())
            {
                modifiedItem.RejectChanges();
            }
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsChanged)));
        }
    }
}
