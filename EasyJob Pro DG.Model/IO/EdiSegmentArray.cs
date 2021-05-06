namespace EasyJob_ProDG.Model.IO
{
    /// <summary>
    /// Supporting class to facilitate defining of segments
    /// </summary>
    internal class EdiSegmentArray
    {
        public string[] Array;
        public int ContainerCount;
        public int Count { get; }

        public EdiSegmentArray(string text)
        {
            Array = text.Split('\'');
            Count = Array.Length;
            for (int i = 0; i < Count; i++)
            {
                if (Array[i].StartsWith("LOC+147"))
                {
                    ContainerCount++;
                }
            }

        }
    }
}
