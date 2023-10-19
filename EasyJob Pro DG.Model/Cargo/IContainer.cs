namespace EasyJob_ProDG.Model.Cargo
{
    public interface IContainer
    {
        bool IsClosed { get; set; }
        bool IsRf { get; set; }
        bool ContainerTypeRecognized { get; set; }
        string ContainerNumber { get; set; }
        bool HasNoNumber { get; }
        string POD { get; set; }
        string POL { get; set; }
        string FinalDestination { get; set; }
        string ContainerType { get; set; }
        string Carrier { get; set; }
    }
}
