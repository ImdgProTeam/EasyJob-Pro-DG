namespace EasyJob_ProDG.Data
{
    public static class AdditionalFunctions
    {

        /// <summary>
        /// Function converts temperature in degrees from Celcium to Farenheit
        /// </summary>
        /// <param name="degreesC"></param>
        /// <returns></returns>
        public static decimal ToFarenheit(this decimal degreesC)
        {
            return degreesC * 1.8M + 32;
        }

        /// <summary>
        /// Function converts temperature in degrees from Farenheit to Celcium
        /// </summary>
        /// <param name="degreesF"></param>
        /// <returns></returns>
        public static decimal ToCelcium(this decimal degreesF)
        {
            return (degreesF - 32)/1.8M;
        }
    }
}
