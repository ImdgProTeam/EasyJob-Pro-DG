using EasyJob_ProDG.Model.Cargo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyJob_ProDG.UI.View.DialogWindows
{

    /// <summary>
    /// Class will store and switch between all the display values for one port of loading (one row)
    /// </summary>
    public class ReportDisplayPort : IComparable<ReportDisplayPort>
    {
        public string Port { get; set; }
        public List<SinglePortReportValue> DisplayValues => displayValues;

        private List<SinglePortReportValue> displayValues;
        private SinglePortReportValue totals;


        #region Constructors
        public ReportDisplayPort(string portCode)
        {
            Port = portCode;
            displayValues = new List<SinglePortReportValue>();
            totals = new SinglePortReportValue("Total");
            displayValues.Add(totals);
        }

        #endregion

        #region Methods used in creation of ReportValues

        /// <summary>
        /// Adds dg values to the respective port record.
        /// Shall always follow respective AddContainer method.
        /// If <see cref="SinglePortReportValue"/> is not found for the dg port of discharging -> exception will be thrown.
        /// </summary>
        /// <param name="dg">Dg with the same POL as <see cref="ReportDisplayPort.Port"/></param>
        internal void AddDg(Dg dg)
        {
            if (dg == null) return;

            SinglePortReportValue portRecord = displayValues.SingleOrDefault(x => string.Equals(x.Port, dg.POD));
            if (portRecord == null) throw new ArgumentNullException("Port record for Dg cannot be created for a port which does not exist in container list.");
            portRecord.DgNetWeight += dg.DgNetWeight;
            totals.DgNetWeight += dg.DgNetWeight;
            if (dg.IsMp)
            {
                portRecord.MPNetWeight += dg.DgNetWeight;
                totals.MPNetWeight += dg.DgNetWeight;
            }
        }

        /// <summary>
        /// Increments and updates respective <see cref="SinglePortReportValue"/> for container with the same POD.
        /// If <see cref="SinglePortReportValue"/> for the container.POD does not exist -> it will be created.
        /// Also updates <see cref="totals"/> values.
        /// </summary>
        /// <param name="container">Container with the same POL as <see cref="ReportDisplayPort.Port"/></param>
        internal void AddContainer(Container container)
        {
            if (container == null) return;
            SinglePortReportValue portRecord = displayValues.SingleOrDefault(x => string.Equals(x.Port, container.POD));
            if (portRecord == null)
            {
                portRecord = new SinglePortReportValue(container.POD);
                displayValues.Add(portRecord);
            }

            portRecord.ContainersCount++;
            totals.ContainersCount++;
            if (container.IsUnderdeck)
            {
                portRecord.ContainersUnderDeckCount++;
                totals.ContainersUnderDeckCount++;
            }
            else
            {
                portRecord.ContainersOnDeckCount++;
                totals.ContainersOnDeckCount++;
            }
            if (container.Bay % 2 == 0)
            {
                portRecord.Containers40Count++;
                totals.Containers40Count++;
            }
            else
            {
                portRecord.Containers20Count++;
                totals.Containers20Count++;
            }
            if (container.IsRf)
            {
                portRecord.ReefersCount++;
                totals.ReefersCount++;
            }
            if (container.DgCountInContainer > 0)
            {
                portRecord.DgContainersCount++;
                totals.DgContainersCount++;
            }

        }

        /// <summary>
        /// Adds new <see cref="SinglePortReportValue"/> new port of discharging to the Port.
        /// </summary>
        /// <param name="newPort"><see cref="SinglePortReportValue"/> to be added</param>
        internal void AddPort(SinglePortReportValue newPort)
        {
            displayValues.Add(newPort);
        }

        /// <summary>
        /// Switches display value in DisplayValues in accordance with selected property.
        /// Works only for single value display options.
        /// </summary>
        /// <param name="reportOption">Selected DisplayValue property.</param>
        internal void SetDisplayValues(string displayPorperty)
        {
            foreach (var port in DisplayValues)
            {
                port.SetDisplayValue(displayPorperty);
            }
        }

        /// <summary>
        /// Switches to complex display value in DisplayValues in accordance with selected option.
        /// Works for complex display options.
        /// </summary>
        /// <param name="reportOption">Selected DisplayValue option.</param>
        internal void SetDisplayValues(PortToPortReportViewModel.ReportOptions option)
        {
            foreach (var port in DisplayValues)
            {
                port.SetComplexDisplayValue(option);
            }
        }


        /// <summary>
        /// Implements IComparer interface.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ReportDisplayPort? other)
        {
            if (other == null)
                return 0;
            return this.Port.CompareTo(other.Port);
        }

        #endregion

    }
}

