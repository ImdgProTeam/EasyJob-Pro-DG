﻿using EasyJob_ProDG.UI.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class ModelWrapper<T> : NotifyDataErrorInfoBase, IRevertibleChangeTracking
    {
        private Dictionary<string, object> _originalValues;
        private List<IRevertibleChangeTracking> _trackingObjects;

        public ModelWrapper(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            Model = model;
            _originalValues = new Dictionary<string, object>();
            _trackingObjects = new List<IRevertibleChangeTracking>();
        }

        public T Model { get; }

        public bool IsChanged => _originalValues.Count > 0 || _trackingObjects.Any(o => o.IsChanged);
        public void AcceptChanges()
        {
            _originalValues.Clear();
            foreach (var trackingObject in _trackingObjects)
            {
                trackingObject.AcceptChanges();
            }
            OnPropertyChanged(null);
        }

        public void RejectChanges()
        {
            foreach (var originalValueEntry in _originalValues)
            {
                typeof(T).GetProperty(originalValueEntry.Key).SetValue(Model, originalValueEntry.Value);
            }
            _originalValues.Clear();
            foreach (var trackingObject in _trackingObjects)
            {
                trackingObject.RejectChanges();
            }
            OnPropertyChanged("");
        }

        protected virtual bool SetValue<TValue>(TValue newValue,
                [CallerMemberName] string propertyName = null)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            var currentValue = (TValue)propertyInfo?.GetValue(Model);
            if (Equals(newValue, currentValue)) return false;

            UpdateOriginalValues(currentValue, newValue, propertyName);
            propertyInfo?.SetValue(Model, newValue);
            OnPropertyChanged(propertyName);
            OnPropertyChanged(propertyName + "IsChanged");
            ValidatePropertyInternal(propertyName);

            return true;
        }

        private void UpdateOriginalValues<TValue>(TValue currentValue, object newValue, string propertyName)
        {
            if (!_originalValues.ContainsKey(propertyName))
            {
                _originalValues.Add(propertyName, currentValue);
                OnPropertyChanged(nameof(IsChanged));
            }
            else
            {
                if (Equals(newValue, _originalValues[propertyName]))
                {
                    _originalValues.Remove(propertyName);
                    OnPropertyChanged(nameof(IsChanged));
                }
            }
        }

        protected virtual TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            return (TValue)typeof(T).GetProperty(propertyName)?.GetValue(Model);
        }

        protected virtual TValue GetOriginalValue<TValue>(string propertyName)
        {
            return _originalValues.ContainsKey(propertyName)
                ? (TValue)_originalValues[propertyName]
                : GetValue<TValue>(propertyName);
        }

        /// <summary>
        /// Method registers collections to syncronize the changes made to the wrapperCollection.
        /// </summary>
        /// <typeparam name="TWrapper"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="wrapperCollection"></param>
        /// <param name="modelCollection"></param>
        protected void RegisterCollection<TWrapper, TModel>(ChangeTrackingCollection<TWrapper> wrapperCollection,
            List<TModel> modelCollection) where TWrapper : ModelWrapper<TModel>
        {
            wrapperCollection.CollectionChanged += (s, e) =>
            {
                modelCollection.Clear();
                modelCollection.AddRange(wrapperCollection.Select(w => w.Model));
            };
            RegisterTrackingObject(wrapperCollection);
        }

        /// <summary>
        /// Registers complex properties (wrappers) to track the changes.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="wrapper"></param>
        protected void RegisterComplexProperty<TModel>(ModelWrapper<TModel> wrapper)
        {
            RegisterTrackingObject(wrapper);
        }

        private void RegisterTrackingObject<TTrackingObject>(TTrackingObject trackingObject)
            where TTrackingObject : IRevertibleChangeTracking, INotifyPropertyChanged
        {
            if (!_trackingObjects.Contains(trackingObject))
            {
                _trackingObjects.Add(trackingObject);
                trackingObject.PropertyChanged += TrackingObjectPropertyChanged;
            }
        }

        private void TrackingObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(IsChanged))
            {
                OnPropertyChanged(nameof(IsChanged));
            }
        }

        protected bool GetIsChanged(string propertyName)
        {
            return _originalValues.ContainsKey(propertyName);
        }

        private void ValidatePropertyInternal(string propertyName)
        {
            ClearErrors(propertyName);
            var errors = ValidateProperty(propertyName);
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    AddError(propertyName, error);
                }
            }
        }

        protected virtual IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }

    }
}

