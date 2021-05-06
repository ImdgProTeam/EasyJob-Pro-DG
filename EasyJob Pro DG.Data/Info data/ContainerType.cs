using System;

namespace EasyJob_ProDG.Data.Info_data
{
    public partial class CodesDictionary
    {
        public class ContainerType
        {
            // --------- public properties ---------------------

            public string Code { get; }
            public string Description { get; }
            public int Length { get; }
            public double Height { get; }
            public string Group { get; }
            public bool Closed { get; }


            // -------- public constructors --------------------

            /// <summary>
            /// Creates a new instance of ContainerType class
            /// </summary>
            /// <param name="code">Container type code</param>
            /// <param name="descr">Type description</param>
            /// <param name="length">Container length</param>
            /// <param name="height">Container height</param>
            /// <param name="group">Container type group</param>
            /// <param name="closed">True if it is closed type container</param>
            public ContainerType(string code, string descr, int length, double height, string group, bool closed)
            {
                Code = code;
                Description = descr;
                Length = length;
                Height = height;
                Group = group;
                Closed = closed;
            }

            /// <summary>
            /// Parses line from dictionary and creates an instance
            /// </summary>
            /// <param name="line">Line from CodesDictionary</param>
            public ContainerType(string line)
            {
                string[] temp = line.Split(',');
                Code = temp[0];
                Description = temp[1];
                Length = Convert.ToInt16(temp[2]);

                Height = Double.Parse(temp[3].Replace('.', ','));
                Group = temp[4];
                Closed = Convert.ToBoolean(temp[5]);
            }

        }
    }
}