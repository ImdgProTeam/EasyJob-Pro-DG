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
        
        DgNameContainsLoaded,
        DgTechnicalNameContains,
        DgClassLoaded,
        DgUnnoLoaded,
        DgFPLoaded,
        ReeferContainingDgLoaded,

        ReeferSetPointLoaded,
        ReeferCommodityContains,

        NoNetWeightUnitsOnboard,
        POLUnitsOnboard,
        PODUnitsOnboard,

        ContainerNumberNotInPosition,

        FreeText = 255

    }
}
