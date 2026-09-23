namespace EasyJob_ProDG.Model.UserDefinedConflicts
{
    public enum UserDefinedConditionType : byte
    {
        General = 0,
        
        ContainerNumberLoaded,
        ContainerNumberDischarged,
        
        CellPositionLoaded,
        CellPositionEmpty,

        CellPositionLoadedWithReefer,
        CellPositionLoadedWithDg,

        ContainerLocationChanged,
        ContainerPODchanged,
        
        ContainerPropertyLoaded,
        DgPropertyLoaded,
        ReeferPropertyLoaded,

        DgNameContainsLoaded,
        DgClassLoaded,
        DgUnnoLoaded,

        DgNameContainsLoadedInCellPosition,
        DgClassLoadedInCellPosition,
        DgUnnoLoadedInCellPosition,

        ReeferSetPointLoaded,
        ReeferCommodityContains,

        NoNetWeightUnitsOnboard,
        POLUnitsOnboard,
        PODUnitsOnboard,

        ContainerNumberNotInPosition,

        FreeText = 255

    }

    public enum UserDefinedConditionOption : byte
    {
        EqualTo = 0,
        NotEqualTo = 1,
        GreaterThan = 2,
        LessThan = 3,
        GreaterThanOrEqualTo = 4,
        LessThanOrEqualTo = 5
    }
}
