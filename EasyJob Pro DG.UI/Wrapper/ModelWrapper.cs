﻿using EasyJob_ProDG.UI.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class ModelWrapper<T> : NotifyDataErrorInfoBase, IRevertibleChangeTracking
    {
        private Dictionary<string, object> _originalValues;
        public ModelWrapper(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            Model = model;
            _originalValues = new Dictionary<string, object>();
        }

        public T Model { get; }

        public bool IsChanged => _originalValues.Count > 0;
        public void AcceptChanges()
        {
            _originalValues.Clear();
            OnPropertyChanged(null);
        }

        public void RejectChanges()
        {
            foreach(var originalValueEntry in _originalValues)
            {
                typeof(T).GetProperty(originalValueEntry.Key).SetValue(Model, originalValueEntry.Value);
            }
            _originalValues.Clear();
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

