namespace EasyJob_ProDG.Model.Cargo
{
    public interface IContainer
    {
        string ContainerNumber { get; set; }
        bool HasNoNumber { get; }
        
        string ContainerType { get; set; }
        bool ContainerTypeRecognized { get; set; }
        bool IsClosed { get; set; }
        bool IsRf { get; set; }
        
        string POD { get; set; }
        string POL { get; set; }
        string FinalDestination { get; set; }
        
        string Carrier { get; set; }
    }
}
