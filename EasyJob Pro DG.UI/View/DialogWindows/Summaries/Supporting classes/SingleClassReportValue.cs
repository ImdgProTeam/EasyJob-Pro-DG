using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Utility;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.View.DialogWindows.Summaries
{

    /// <summary>
    /// Class contains all reportable values for a single Dg class
    /// </summary>
    public class SingleClassReportValue : Observable
    {
        const string _TOTAL = DgSummaryReportViewModel._TOTAL;
        const string _MP = DgSummaryReportViewModel._MP;


        /// <summary>
        /// POD of the cell
        /// </summary>
        internal string Class;

        internal Dictionary<string, SinglePortClassReportValue> ClassCountValues;

        private List<string> containerNumbers;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="portCode"></param>
        internal SingleClassReportValue(string portCode)
        {
            Class = portCode ?? string.Empty;
            ClassCountValues = new Dictionary<string, SinglePortClassReportValue>();
            ClassCountValues.Add(_TOTAL, new SinglePortClassReportValue(_TOTAL));
            ClassCountValues.Add(_MP, new SinglePortClassReportValue(_MP));
            containerNumbers = new();
        }

        /// <summary>
        /// Adds Dg values to the class counts.
        /// </summary>
        /// <param name="dg"></param>
        internal void AddDg(Dg dg)
        {
            bool containerAlreadyExistsInThisClass = containerNumbers.Contains(dg.ContainerNumber);

            if (!ClassCountValues.ContainsKey(dg.POD))
            {
                ClassCountValues.Add(dg.POD, new SinglePortClassReportValue(dg.POD) { DgNetWeight = dg.DgNetWeight, ContainersCount = 1 });
            }
            else
            {
                if (!containerAlreadyExistsInThisClass)
                    ClassCountValues[dg.POD].ContainersCount++;
                ClassCountValues[dg.POD].DgNetWeight += dg.DgNetWeight;
            }

            if (dg.IsMp)
            {
                ClassCountValues[_MP].DgNetWeight += dg.DgNetWeight;
                if (!containerAlreadyExistsInThisClass) ClassCountValues[_MP].ContainersCount++;
            }
            ClassCountValues[_TOTAL].DgNetWeight += dg.DgNetWeight;
            if (!containerAlreadyExistsInThisClass)
            {
                ClassCountValues[_TOTAL].ContainersCount++;
                containerNumbers.Add(dg.ContainerNumber);
            }
        }

        /// <summary>
        /// Switches display property in ClassCountValues in accordance with selected property.
        /// </summary>
        /// <param name="reportOption">Selected <see cref="PropertyReportOptions"/> property.</param>
        internal void SetDisplayValues(DgSummaryReportViewModel.PropertyReportOptions reportOption)
        {
            foreach (var port in ClassCountValues)
            {
                port.Value.SetDisplayValue(reportOption);
            }
        }
    }

}



