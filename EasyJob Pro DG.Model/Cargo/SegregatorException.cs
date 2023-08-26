namespace EasyJob_ProDG.Model.Cargo
{
    /// <summary>
    /// Class to contain exception for Segregator (class which should be used for segregation instead of given class from DG list)
    /// </summary>
    public class SegregatorException
    {
        public string SegrClass;
        public byte SegrCase;

        public SegregatorException (string _class, byte segrCase)
        {
            SegrClass = _class;
            SegrCase = segrCase;
        }

        public SegregatorException(string _class, Segregation.SegregationCase segrCase)
        {
            SegrClass = _class;
            SegrCase = (byte)segrCase;
        }

        /// <summary>
        /// Public constructor without parameters
        /// Made to facilitate serialization
        /// </summary>
        public SegregatorException()
        {
                
        }
    }
}
