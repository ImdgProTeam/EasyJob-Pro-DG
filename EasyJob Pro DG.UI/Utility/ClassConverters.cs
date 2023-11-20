using EasyJob_ProDG.UI.Wrapper.Dummies;
using EasyJob_ProDG.Model.Transport;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Utility
{
    public static class ClassConverters
    {
        /// <summary>
        /// Downgrades the collection of T to list of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static List<T> DowngradeCollectionToList<T>(ObservableCollection<T> collection)
        {
            //T classtype;
            List<T> list = new List<T>();
            foreach (T item in collection)
            {
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// Converts all items from DummyCargoHold to CargoHold and then downgrades the collection to list
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static List<CargoHold> DowngradeCollectionToList(ObservableCollection<CargoHoldWrapper> collection)
        {

            List<CargoHold> list = new List<CargoHold>();
            foreach (var item in collection)
            {
                list.Add(item.ToCargoHold());
            }
            return list;
        }

        /// <summary>
        /// Upgrades list of T to ObservableCollection of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ObservableCollection<T> UpgradeListToCollection<T>(List<T> list)
        {
            return new ObservableCollection<T>(list);
        }

    }
}
