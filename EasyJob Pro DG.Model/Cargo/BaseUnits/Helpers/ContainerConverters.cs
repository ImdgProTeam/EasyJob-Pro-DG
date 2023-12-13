namespace EasyJob_ProDG.Model.Cargo
{
    internal static class ContainerConverters
    {
        /// <summary>
        /// Converts Dg back to plain Container
        /// </summary>
        /// <returns>Type of Container</returns>
        public static ContainerAbstract ConvertToContainer(this Dg dg)
        {
            return new Container()
            {
                ContainerNumber = dg.ContainerNumber,
                ContainerType = dg.ContainerType,
                Location = dg.Location,
                HoldNr = dg.HoldNr,
                IsClosed = dg.IsClosed,
                POD = dg.POD,
                POL = dg.POL,
                FinalDestination = dg.FinalDestination,
                Carrier = dg.Carrier,
                IsRf = dg.IsRf
            };
        }


        /// <summary>
        /// Method converting Container into a blank Dg
        /// </summary>
        /// <returns></returns>
        public static Dg ConvertToDg(this Container container)
        {
            Dg dg = new Dg();
            dg.CopyContainerAbstractInfo(container);
            if (container.IsRf) dg.DgClass = "Reefer";
            return dg;
        }
    }
}
