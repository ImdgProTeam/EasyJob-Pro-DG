namespace EasyJob_ProDG.Model.Cargo
{
    public interface ILocationOnBoard
    {
        // ---------- public properties -----------------------
        bool IsUnderdeck { get; set; }
        byte Bay { get; set; }
        byte HoldNr { get; set; }
        byte Row { get; set; }
        byte Size { get; set; }
        byte Tier { get; set; }
        string Location { get; set; }
    }
}
