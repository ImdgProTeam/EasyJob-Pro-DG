namespace EasyJob_ProDG.Model.Transport
{
    public static class CellPositionExtensions
    {
        public static CellPosition Clone(this CellPosition cellPosition)
        {
            return new CellPosition()
            {
                Position = cellPosition.Position
            };
        }
    }
}
