namespace EasyJob_Pro_DG
{
    public class AdditionalFunctions
    {

        /// <summary>
        /// Function converts temperature in degrees from Celcium to Farenheit
        /// </summary>
        /// <param name="degreesC"></param>
        /// <returns></returns>
        public static double ToFarenheit(double degreesC)
        {
            return degreesC * 1.8 + 32;
        }

        /// <summary>
        /// Function converts temperature in degrees from Farenheit to Celcium
        /// </summary>
        /// <param name="degreesF"></param>
        /// <returns></returns>
        public static double ToCelcium(double degreesF)
        {
            return (degreesF - 32)/1.8;
        }
    }
}
