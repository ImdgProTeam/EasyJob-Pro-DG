using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EasyJob_ProDG.UI.Utility
{
    /// <summary>
    /// Implements Observable (INotifyPropertyChanged) and INotifyDataErrorInfo
    /// </summary>
    public class NotifyDataErrorInfoBase : Observable, INotifyDataErrorInfo
    {
        /// <summary>
        /// Dictionary contains list of errors for various properties
        /// </summary>
        private Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public bool HasErrors => _errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName)
                ? _errorsByPropertyName[propertyName]
                : null;
        }

        /// <summary>
        /// Invokes ErrorChanged event for the given property
        /// </summary>
        /// <param name="propertyName">given property name</param>
        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Adds the error to dictionary
        /// </summary>
        /// <param name="propertyName">property in dictionary to which error is added</param>
        /// <param name="error">error description</param>
        protected void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName[propertyName] = new List<string>();
            }

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        /// <summary>
        /// Clears all errors from dictionary for the given property
        /// </summary>
        /// <param name="propertyName">All errors will be cleared from dictionary for the property</param>
        protected void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}
