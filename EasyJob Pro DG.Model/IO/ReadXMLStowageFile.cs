using EasyJob_ProDG.Data;
using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.Transport;
using System;
using System.Linq;
using System.Xml.Linq;

namespace EasyJob_ProDG.Model.IO
{
    internal class ReadXMLStowageFile
    {
        const string DISCHARGED_TAG_VALUE = "1";

        /// <summary>
        /// Creates <see cref="CargoPlan"/> from xml file.
        /// </summary>
        /// <param name="fileName">xml file full path.</param>
        /// <returns></returns>
        internal static CargoPlan ReadFile(string fileName)
        {
            CargoPlan cargoPlan = new CargoPlan();

            var xmlDocument = XDocument.Load(fileName);


            if (!xmlDocument.Root.HasElements) return cargoPlan;

            var bayPlanElement = xmlDocument.Element("BayPlan");
            if (bayPlanElement is null) return cargoPlan;

            ReadVoyageInfo(cargoPlan, bayPlanElement);

            var containerRecords = xmlDocument.Descendants("Container");
            if (containerRecords.Count() < 1) return cargoPlan;

            ReadCargo(cargoPlan, containerRecords);
            return cargoPlan;
        }

        private static void ReadCargo(CargoPlan cargoPlan, System.Collections.Generic.IEnumerable<XElement> containerRecords)
        {
            foreach (var record in containerRecords)
            {
                Container container = new Container();
                try
                {
                    if (FilterDischargedTag(record, cargoPlan.VoyageInfo.PortOfDeparture))
                        continue;
                    ReadContainer(record, container);
                    ReadReefer(record, container);
                    ReadDgs(cargoPlan, record, container);
                }
                catch (Exception ex)
                {
                    LogWriter.Write($"Reading unit {container.ContainerNumber} caused exception {ex.Message}.");
                }
                finally
                {
                    if (!string.IsNullOrEmpty(container.ContainerNumber) && !string.IsNullOrEmpty(container.Location))
                        cargoPlan.AddContainer(container);
                }
            }
        }

        /// <summary>
        /// In MSC .xml containers with tag <Discharged>value</Discharged> value == 1 are the containers 
        /// discharged in the current port - therefore they shall not be added in the <see cref="CargoPlan"/>.
        /// If <Discharged>1</Discharged> && POD = VoyageInfo.PortOfDeparture => not to be added.
        /// </summary>
        /// <param name="record"></param>
        /// <param name="container"></param>
        /// <returns>True if the container not to be added to <see cref="CargoPlan"/></returns>
        private static bool FilterDischargedTag(XElement record, string portOfDeparture)
        {
            var value = record.Element("Discharged").Value;
            var containerPOD = record.Element("DischargingPort").Value;

            if (string.Equals(value, DISCHARGED_TAG_VALUE)
                && string.Equals(containerPOD, portOfDeparture, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        private static void ReadDgs(CargoPlan cargoPlan, XElement record, Container container)
        {
            //Hazardous cargo
            var dgRecords = record.Descendants("HazardousCargo");
            if (dgRecords.Count() > 0)
                foreach (var dgRecord in dgRecords)
                {
                    var dg = new Dg();
                    dg.CopyContainerInfo(container);

                    try
                    {
                        if (ConvertToCorrectDgClass(dgRecord.Element("Class").Value, out string dgClass))
                            dg.DgClass = dgClass;
                        if (ushort.TryParse(dgRecord.Element("UNNumber").Value, out ushort unno))
                            dg.Unno = unno;
                        else
                        {
                            LogWriter.Write($"Cannot read unno for dg in {container.ContainerNumber}, value: {dgRecord.Element("UNNumber")}.");
                            throw new Exception();
                        }
                        dg.PackingGroup = dgRecord.Element("PackingGroup").Value;

                        if (decimal.TryParse(dgRecord.Element("FlashPoint").Value, out decimal flashPoint))
                            if (string.Equals(dgRecord.Element("FlashPointUnit").Value, "F"))
                            {
                                dg.FlashPointAsDecimal = flashPoint.ToCelcium();
                            }
                            else
                                dg.FlashPointAsDecimal = flashPoint;

                        if (decimal.TryParse(dgRecord.Element("NetWeight").Value, out decimal dgNetWeight))
                            dg.DgNetWeight = dgNetWeight;
                        if (string.Equals(dgRecord.Element("LimitedQuantity").Value, "Y") ||
                            string.Equals(dgRecord.Element("LimitedQuantity").Value.ToLower(), "true"))
                            dg.IsLq = true;
                        if (string.Equals(dgRecord.Element("MarinePollutant").Value, "Y") ||
                            string.Equals(dgRecord.Element("MarinePollutant").Value.ToLower(), "true"))
                            dg.IsMp = true;
                        if (!string.IsNullOrWhiteSpace(dgRecord.Element("MarinePollutant").Value))
                            dg.mpDetermined = true;

                        dg.Name = dgRecord.Element("Substance").Value;
                        if (dg.Unno == 1950 && (dg.Name.Contains("MAXIMUM CAPACITY OF 1 LITRE") || dg.Name.ToLower().Replace(" ", "").Contains("max1l")))
                            dg.IsMax1L = true;

                        string technicalName = dgRecord.Element("PSNameEnglish").Value;
                        if (!string.IsNullOrWhiteSpace(technicalName) && !string.Equals(technicalName.ToLower(), "na")
                                                                    && !string.Equals(technicalName.ToLower(), "n/a"))
                            dg.TechnicalName = technicalName;

                    }
                    catch (Exception ex)
                    {
                        LogWriter.Write($"Reading dg unno: {dg.Unno} for {container.ContainerNumber} caused exception {ex.Message}.");
                    }
                    finally
                    {
                        if (dg.Unno > 0)
                            cargoPlan.DgList.Add(dg);
                        container.DgCountInContainer++;
                    }
                }
        }

        private static void ReadContainer(XElement record, Container container)
        {
            container.ContainerNumber = record.Element("ContainerNumber").Value;

            container.Location = record.Element("StowPosition").Value;
            container.HoldNr = ShipProfile.DefineCargoHoldNumber(container.Bay);

            container.Carrier = record.Element("OperatorCode").Value;
            container.POL = record.Element("LoadingPort").Value;
            container.POD = record.Element("DischargingPort").Value;
            container.FinalDestination = record.Element("FinalDischargePort").Value;
            container.ContainerType = record.Element("CtrIsoCode").Value;
        }

        private static void ReadReefer(XElement record, Container container)
        {
            var reeferRecord = record.Element("OperatingReefer");
            if (reeferRecord == null) return;

            container.IsRf = true;

            if (decimal.TryParse(reeferRecord.Element("Temperature").Value, out decimal setTemp))
            {
                if (string.Equals(reeferRecord.Element("TemperatureUnit")?.Value, "F"))
                {
                    container.SetTemperature = setTemp.ToCelcium();
                }
                else
                    container.SetTemperature = setTemp;
            }
        }

        private static void ReadVoyageInfo(CargoPlan cargoPlan, XElement bayPlanElement)
        {
            cargoPlan.VoyageInfo.PortOfDeparture = bayPlanElement.Element("LoadingPort").Value;
            cargoPlan.VoyageInfo.VoyageNumber = bayPlanElement.Element("Voyage").Value;
            cargoPlan.VoyageInfo.PortOfDestination = string.Empty;
        }

        /// <summary>
        /// Corrects value as read from xml element <Class> to become a valid dg class.
        /// Used only with xml files from MSC.
        /// </summary>
        /// <param name="inputDgClass"></param>
        /// <param name="correctDgClass"></param>
        /// <returns>True if succeeded to correct the inputDgClass to become a valid dg class.</returns>
        private static bool ConvertToCorrectDgClass(string inputDgClass, out string correctDgClass)
        {
            correctDgClass = string.Empty;

            if (inputDgClass.Length == 1 && char.IsDigit(inputDgClass[0]))
            {
                correctDgClass = inputDgClass;
            }
            if (inputDgClass.Length == 2 && char.IsDigit(inputDgClass[0]) && char.IsDigit(inputDgClass[1]))
            {
                correctDgClass = $"{inputDgClass[0]}.{inputDgClass[1]}";
            }
            if (inputDgClass.Length == 3 && char.IsLetter(inputDgClass[2])
                && char.IsDigit(inputDgClass[0]) && char.IsDigit(inputDgClass[1]))
            {
                correctDgClass = inputDgClass.Insert(1, ".");
            }
            if (inputDgClass.Length == 4 && IMDGCodeValidator.IsValidDgClass(inputDgClass))
            {
                correctDgClass = inputDgClass;
            }
            if (IMDGCodeValidator.IsValidDgClass(correctDgClass)) return true;


            return false;
        }
    }
}
