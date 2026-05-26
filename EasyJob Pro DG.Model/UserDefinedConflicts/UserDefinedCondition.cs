namespace EasyJob_ProDG.Model.UserDefinedConflicts
{
    public class UserDefinedCondition
    {
        public UserDefinedConditionType ConditionType { get; set; }
        public string ConditionDescription { get; set; }


        public string GetDescription(string containerNumber)
        {
            return "";
        }
    }


    public enum UserDefinedConditionType
    {
        General = 0,
        
        ContainerNumberLoaded = 1,
        ContainerNumberDischarged = 2,
        
        CellPositionLoaded = 3,
        CellPositionEmpty = 4,
        
        ContainerLocationChanged =5,
        ContainerPODchanged = 6,
        
        ContainerPropertyLoaded = 7,
        DgPropertyLoaded = 8,
        ReeferPropertyLoaded = 9,

        DgNameContainsLoaded = 10,
        DgClassLoaded = 11,
        DgUnnoLoaded = 12,

        ReeferSetPointLoaded = 13,
        ReeferCommodityContains = 14,

        NoNetWeightUnitsOnboard = 15,
        POLUnitsOnboard = 16,
        PODUnitsOnboard = 17,

    }
}
