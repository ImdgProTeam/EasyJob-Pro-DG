using System.Collections.Generic;
using System.Linq;

namespace EasyJob_ProDG.Model.Cargo
{
    public static class CargoPlanExtensionMethods
    {
        /// <summary>
        /// Checks if a CargoPlan unit exists in a collection (same number and location).
        /// </summary>
        /// <param name="container">CargoPlan unit to be found.</param>
        /// <param name="collection">Collection of the units to be searched through.</param>
        /// <returns>True if the unit is found.</returns>
        public static bool ContainsUnitInList<TM, T>(this ICollection<TM> collection, T container)
            where T : IContainer, ILocationOnBoard
            where TM : IContainer, ILocationOnBoard
        {
            if (collection == null) throw new System.ArgumentNullException(nameof(collection), "Argument expected to exist.");
            if (container == null) throw new System.ArgumentNullException(nameof(container), "Argument expected to exist.");

            return collection.Any(c =>
               string.Equals(c.ContainerNumber, container.ContainerNumber) && c.Location == container.Location);
        }

        /// <summary>
        /// Checks if a CargoPlan unit ContainerNumber exists in a collection
        /// </summary>
        /// <param name="container">CargoPlan unit whose Number to be found.</param>
        /// <param name="collection">Collection of the units to be searched through.</param>
        /// <returns>True if the unit is found.</returns>
        public static bool ContainsUnitWithSameContainerNumberInList<TM, T>(this ICollection<TM> collection, T container)
            where T : IContainer
            where TM : IContainer
        {
            if (collection == null) throw new System.ArgumentNullException(nameof(collection), "Argument expected to exist.");
            if (container == null) throw new System.ArgumentNullException(nameof(container), "Argument expected to exist.");

            return collection.Any(c =>
                string.Equals(c.ContainerNumber, container.ContainerNumber));
        }

        /// <summary>
        /// Finds an IContainer unit with requiered ContainerNumber in the collection.
        /// </summary>
        /// <typeparam name="T">Shall be not null and have ContainerNumber property.</typeparam>
        /// <param name="collection">Collection of IContainers to search through.</param>
        /// <param name="unit">CargoPlan unit which ContainerNumber will be searched.</param>
        /// <returns>The requested type T inhereted from IContainer from collection with matching ContainerNumber.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static T FindContainerByContainerNumber<T>(this ICollection<T> collection, IContainer unit)
            where T : IContainer
        {
            if (collection == null) throw new System.ArgumentNullException(nameof(collection), "Argument expected to exist.");
            if (unit == null) throw new System.ArgumentNullException(nameof(unit), "Argument expected to exist.");

            return collection.FirstOrDefault(c => string.Equals(c.ContainerNumber, unit.ContainerNumber));
        }

        /// <summary>
        /// Finds an IContainer unit with requiered ContainerNumber in the collection.
        /// </summary>
        /// <typeparam name="T">Shall be not null and have ContainerNumber property.</typeparam>
        /// <param name="collection">Collection of IContainers to search through.></param>
        /// <param name="containerNumber">ContainerNumber as a string.</param>
        /// <returns>The requested type T inhereted from IContainer from collection with matching ContainerNumber.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static T FindContainerByContainerNumber<T>(this ICollection<T> collection, string containerNumber)
            where T:IContainer
        {
            if (collection == null) throw new System.ArgumentNullException(nameof(collection), "Argument expected to exist.");
            if (string.IsNullOrEmpty(containerNumber)) throw new System.ArgumentNullException(nameof(containerNumber), "Argument expected to exist.");

            return collection.FirstOrDefault(c => string.Equals(c.ContainerNumber, containerNumber));
        }
    }
}
