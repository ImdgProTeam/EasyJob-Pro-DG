namespace EasyJob_ProDG.UI.Messages
{
    class RemoveRowMessage
    {
        public byte Row;
        public string Collection;

        public RemoveRowMessage(byte row)
        {
            Row = row;
        }
        public RemoveRowMessage(byte row, string collection)
        {
            Row = row;
            Collection = collection;
        }

    }
}
