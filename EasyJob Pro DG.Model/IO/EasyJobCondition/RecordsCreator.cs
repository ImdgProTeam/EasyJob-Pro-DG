using System.Text;

namespace EasyJob_ProDG.Model.IO.EasyJobCondition
{
    /// <summary>
    /// Creator of records for EasyJobCondition.
    /// </summary>
    internal static class RecordsCreator
    {
        private static StringBuilder stringBuilder = new StringBuilder();

        /// <summary>
        /// Clears the record and creates a new string record for new container.
        /// </summary>
        internal static void AddNewRecord()
        {
            stringBuilder.Clear();
            stringBuilder.Append("C:");
        }

        /// <summary>
        /// Clears the record and creates a new string with specified opening type 
        /// </summary>
        internal static void AddNewSpecifiedRecord(string type)
        {
            stringBuilder.Clear();
            stringBuilder.Append(type + ":");
        }

        /// <summary>
        /// Adds value to the record and appends with the separator
        /// </summary>
        /// <param name="value"></param>
        internal static void AppendRecord(string value)
        {
            stringBuilder.Append(value);
            stringBuilder.Append('|');
        }

        /// <summary>
        /// Adds type separator specified as a parameter
        /// </summary>
        /// <param name="type"></param>
        internal static void AppendWitNewType(string type)
        {
            stringBuilder.Append(type + ":");
        }

        /// <summary>
        /// Gets current result
        /// </summary>
        /// <returns>String current result</returns>
        internal static string GetEntry()
        {
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Finalizes the entry
        /// </summary>
        /// <returns>String final result</returns>
        internal static string ReturnEntry()
        {
            stringBuilder.AppendLine("");
            return stringBuilder.ToString();
        }
    }
}
