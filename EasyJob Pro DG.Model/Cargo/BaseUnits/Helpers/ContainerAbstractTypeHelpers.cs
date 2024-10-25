using EasyJob_ProDG.Data.Info_data;

namespace EasyJob_ProDG.Model.Cargo
{
    internal static class ContainerAbstractTypeHelpers
    {
        /// <summary>
        /// Updates information derived from container types dictionary on a IContainer item
        /// </summary>
        internal static void UpdateContainerType(this IContainer container)
        {
            //reset to default
            //by default IsClosed = true, TypeRecognized = false;
            container.ContainerTypeRecognized = false;
            container.IsClosed = true;

            var type = CodesDictionary.ContainerType.GetContainerType(container.ContainerType);
            if (type != null)
            {
                container.IsClosed = type.IsClosed;
                container.ContainerTypeRecognized = true;
            }
        }
    }
}