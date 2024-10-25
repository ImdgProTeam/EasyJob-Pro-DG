using EasyJob_ProDG.UI.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class ModelWrapper<T> : NotifyDataErrorInfoBase
    {
        public ModelWrapper(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            Model = model;
        }

        public T Model { get; }

        protected virtual bool SetValue<TValue>(TValue newValue,
                [CallerMemberName] string propertyName = null)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            var currentValue = (TValue)propertyInfo?.GetValue(Model);
            if (Equals(newValue, currentValue)) return false;

            propertyInfo?.SetValue(Model, newValue);
            OnPropertyChanged(propertyName);
            ValidatePropertyInternal(propertyName);

            return true;
        }

        protected virtual TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            return (TValue)typeof(T).GetProperty(propertyName)?.GetValue(Model);
        }

        /// <summary>
        /// Method registers collections to syncronize the changes made to the wrapperCollection.
        /// </summary>
        /// <typeparam name="TWrapper"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="wrapperCollection"></param>
        /// <param name="modelCollection"></param>
        protected virtual void RegisterCollection<TWrapper, TModel>(ObservableCollection<TWrapper> wrapperCollection,
            List<TModel> modelCollection) where TWrapper : ModelWrapper<TModel>
        {
            wrapperCollection.CollectionChanged += (s, e) =>
            {
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems.Cast<TWrapper>())
                    {
                        modelCollection.Remove(item.Model);
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems.Cast<TWrapper>())
                    {
                        modelCollection.Add(item.Model);
                    }
                }
            };
        }

        protected void ValidatePropertyInternal(string propertyName)
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

