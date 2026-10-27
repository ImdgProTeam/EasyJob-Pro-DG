using System.Collections;

namespace EasyJob_ProDG.UI.View.Sort
{
    public class LeftToRightComparer : IComparer
    {
        /// <summary>
        /// Sort byte values of rows from left to right ( 6 .. 4 .. 2 .. 0 .. 1 .. 3 .. 5)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            if (x is byte && y is byte)
            {
                if ((byte)x % 2 == 0)
                {
                    if ((byte)y % 2 == 0)
                    {
                        return (byte)y - (byte)x;
                    }
                    else return -1;
                }
                else
                if ((byte)y % 2 == 0)
                {
                    return 1;
                }
                else return (byte)x - (byte)y;
            }
            return 0;
        }
    }
}
