namespace EasyJob_ProDG.Model.IO
{
    /// <summary>
    /// Supporting class to facilitate defining of segments
    /// </summary>
    internal class EdiSegmentArray
    {
        /// <summary>
        /// Array of EDI segments
        /// </summary>
        public readonly string[] Array;
        public readonly int ContainerCount;

        /// <summary>
        /// Count of EDI segments
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Creates an array of EDI segments from text
        /// </summary>
        /// <param name="text">Text read from edi</param>
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
