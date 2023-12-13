namespace EasyJob_ProDG.Model.Cargo
{
    public interface ILocationOnBoard
    {
        // ---------- public properties -----------------------
        byte Bay { get; }
        byte Row { get; }
        byte Tier { get; }
        byte Size { get; }
        bool IsUnderdeck { get;  }
        byte HoldNr { get; }
        string Location { get; set; }
    }
}
