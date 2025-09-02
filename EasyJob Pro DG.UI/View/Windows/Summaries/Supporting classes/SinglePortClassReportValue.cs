using static EasyJob_ProDG.UI.View.DialogWindows.DgSummaryReportViewModel;

namespace EasyJob_ProDG.UI.View.DialogWindows.Summaries
{
    /// <summary>
    /// Class contains all reportable values for a single Class with single POD (one reporting cell)
    /// </summary>
    public class SinglePortClassReportValue
    {
        const string _FORMATDECIMAL = Settings.UserUISettings._FORMATDECIMAL;

        /// <summary>
        /// Dg Class of the cell
        /// </summary>
        internal string Class;

        /// <summary>
        /// Value to be displayed in the report
        /// </summary>
        public string DisplayValue { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="portCode"></param>
        internal SinglePortClassReportValue(string dgCalss)
        {
            Class = dgCalss ?? string.Empty;
            //DisplayValue = ContainersCount.ToString();
        }

        internal uint ContainersCount = 0;
        internal decimal DgNetWeight = 0.0m;

        /// <summary>
        /// Sets the <see cref="DisplayValue"/> in accordance with the selected property display option.
        /// </summary>
        /// <param name="option">Option from <see cref="PropertyReportOptions"/> enum to apply.</param>
        internal void SetDisplayValue(PropertyReportOptions option)
        {
            string result;
            switch (option)
            {
                case PropertyReportOptions.ContainersCount:
                    result = ContainersCount.ToString();
                    break;
                case PropertyReportOptions.DgNetWight:
                    result = DgNetWeight.ToString(_FORMATDECIMAL);
                    break;
                case PropertyReportOptions.ContainersCoundAndDgNetWeight:
                    result = $"{ContainersCount,3} / " + DgNetWeight.ToString(_FORMATDECIMAL);
                    break;
                default:
                    result = "0";
                    break;
            }
            DisplayValue = result;
        }

        public override string ToString()
        {
            return DisplayValue;
        }
    }

}



