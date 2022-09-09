using System.Reflection;
using static EasyJob_ProDG.UI.View.DialogWindows.PortToPortReportViewModel;

namespace EasyJob_ProDG.UI.View.DialogWindows
{

    #region Private class to be used in ReportDisplayValue to store the values
    /// <summary>
    /// Class contains all reportable values for a single POL with single POD (one reporting cell)
    /// </summary>
    public class SinglePortReportValue
    {
        const string _FORMATDECIMAL = Settings.UserUISettings._FORMATDECIMAL;

        /// <summary>
        /// POD of the cell
        /// </summary>
        internal string Port;

        /// <summary>
        /// Value to be displayed in the report
        /// </summary>
        public string DisplayValue { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="portCode"></param>
        internal SinglePortReportValue(string portCode)
        {
            Port = portCode ?? string.Empty;
            DisplayValue = ContainersCount.ToString();
        }

        internal uint ContainersCount = 0;
        internal uint Containers20Count = 0;
        internal uint Containers40Count = 0;
        internal uint ContainersOnDeckCount = 0;
        internal uint ContainersUnderDeckCount = 0;
        internal uint ContainersLoadedCount = 0; //not implemented
        internal uint ContainersEmptyCount = 0;  //not implemented
        internal uint ReefersCount = 0;
        internal decimal ContainersWeight = 0;  //not implemented
        internal uint DgContainersCount = 0;
        internal decimal DgNetWeight = 0.0m;
        internal decimal MPNetWeight = 0.0m;

        /// <summary>
        /// Sets the <see cref="DisplayValue"/> in accordance with the selected displayProperty.
        /// </summary>
        /// <param name="displayProperty">Name of property to be set as DisplayValue.</param>
        internal void SetDisplayValue(string displayProperty)
        {
            FieldInfo propertyInfo = typeof(SinglePortReportValue).GetField(displayProperty, BindingFlags.Instance | BindingFlags.NonPublic);
            if (propertyInfo is null) return;
            var value = propertyInfo.GetValue(this) ?? 0;
            string resultValue;
            var type = propertyInfo.FieldType.Name;
            switch (type)
            {
                case "int":
                case "uint":
                case "UInt32":
                    {
                        resultValue = value.ToString();
                        break;
                    }
                case "Decimal":
                    {
                        resultValue = ((decimal)value).ToString(_FORMATDECIMAL);
                        break;
                    }

                default:
                    resultValue = "0";
                    break;
            }
            DisplayValue = resultValue;
        }

        /// <summary>
        /// Sets the <see cref="DisplayValue"/> in accordance with the selected displayProperty consisting of more than two regular properties.
        /// </summary>
        /// <param name="option">Option from ReportOptions enum to apply.</param>
        internal void SetComplexDisplayValue(ReportOptions option)
        {
            string result;
            switch (option)
            {
                case ReportOptions.ContainersTotal20And40:
                    result = $"{ContainersCount} ({Containers20Count} / {Containers40Count})";
                    break;
                case ReportOptions.Containers20And40:
                    result = $"{Containers20Count} / {Containers40Count}";
                    break;
                case ReportOptions.ContainersOnUnderDeck:
                    result = $"{ContainersOnDeckCount} / {ContainersUnderDeckCount}";
                    break;
                case ReportOptions.ContainersLoadedAndEmpty:
                    result = "Not implemented";
                    break;
                case ReportOptions.ContainersAndWeight:
                    result = $"Not implemented";
                    break;
                case ReportOptions.DgNetWeightAndMP:
                    result = $"{DgNetWeight.ToString(_FORMATDECIMAL),13} / {MPNetWeight.ToString(_FORMATDECIMAL),13}";
                    break;
                case ReportOptions.DgCountAndDgNetWeight:
                    result = $"{DgContainersCount,4} / {DgNetWeight.ToString(_FORMATDECIMAL),13}";
                    break;
                case ReportOptions.DgCountAndDgNetWeightAndMP:
                    result = $"{DgContainersCount,4} / {DgNetWeight.ToString(_FORMATDECIMAL),13} / {MPNetWeight.ToString(_FORMATDECIMAL),13}";
                    break;
                default:
                    result = "0";
                    break;
            }
            DisplayValue = result;
        }

        /// <summary>
        /// Overrides default ToString() method that will be used to display selected values in DataGrid. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DisplayValue;
        }
    }

    #endregion
}

