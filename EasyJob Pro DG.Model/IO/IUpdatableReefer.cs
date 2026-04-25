namespace EasyJob_ProDG.Model.IO
{
    public interface IUpdatableReefer
    {
        bool HasChangedLiveReeferMode { get; set; }
        bool HasSetPointChanged { get; set; }
        decimal OldSetTemperature { get; set; }
    }
}
