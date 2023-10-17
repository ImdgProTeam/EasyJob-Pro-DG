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
            public bool IsClosed { get; }


            // ----- Public methods -----
            public static ContainerType GetContainerType(string code)
            {
                if(ContainerTypes.TryGetValue(code, out ContainerType containerType)) 
                    return containerType;
                return null;
            }

            
            // -------- private constructors --------------------

            #region Methods to create ContainerTypes
            /// <summary>
            /// Creates a new instance of ContainerType class
            /// </summary>
            /// <param name="code">Container type code</param>
            /// <param name="descr">Type description</param>
            /// <param name="length">Container length</param>
            /// <param name="height">Container height</param>
            /// <param name="group">Container type group</param>
            /// <param name="closed">True if it is closed type container</param>
            internal ContainerType(string code, string descr, int length, double height, string group, bool closed)
            {
                Code = code;
                Description = descr;
                Length = length;
                Height = height;
                Group = group;
                IsClosed = closed;
            }

            /// <summary>
            /// Parses line from dictionary and creates an instance
            /// </summary>
            /// <param name="line">Line from CodesDictionary</param>
            internal ContainerType(string line)
            {
                string[] temp = line.Split(',');
                Code = temp[0];
                Description = temp[1];
                Length = Convert.ToInt16(temp[2]);

                Height = Double.Parse(temp[3].Replace('.', ','));
                Group = temp[4];
                IsClosed = Convert.ToBoolean(temp[5]);
            }
            #endregion
        }
    }
}