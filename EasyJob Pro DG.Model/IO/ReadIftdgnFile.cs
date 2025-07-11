﻿using EasyJob_ProDG.Data;
using EasyJob_ProDG.Data.Info_data;
using EasyJob_ProDG.Model.Cargo;
using System;
using System.Linq;

namespace EasyJob_ProDG.Model.IO
{
    internal class ReadIftdgnFile
    {
        private static CargoPlan _cargoPlan;
        static string tPOL;
        static string tPOD;

        /// <summary>
        /// Converts EdiSegmentArray (of IFTDGN file) into CargoPlan.
        /// </summary>
        /// <param name="segmentArray"></param>
        /// <returns></returns>
        internal static CargoPlan ReadSegments(EdiSegmentArray segmentArray)
        {
            _cargoPlan = new CargoPlan();

            DefineSegments(segmentArray);

            return _cargoPlan;
        }

        /// <summary>
        /// Converts segments into CargoPlan units.
        /// </summary>
        /// <param name="segmentArray"></param>
        private static void DefineSegments(EdiSegmentArray segmentArray)
        {
            bool isIftdn = false;

            Dg dgUnit = null;
            Container container = null;

            for (int i = 0; i < segmentArray.Count; i++)
            {
                string segment = segmentArray.Array[i];
                var segmentDef = segment.Length > 3 ? segment.Remove(3) : segmentArray.Array[i];

                switch (segmentDef)
                {
                    case "UNH":
                        if (segment.Contains("IFTDGN"))
                            isIftdn = true;
                        break;

                    case "BGM":
                        continue;

                    case "EQD":
                        DefineEQD(segment);
                        break;

                    case "LOC":
                        DefineLOCsegment(container, segment);
                        break;

                    //Goods items details
                    //'GID+1+40:4G:::FIBREBOARD BOX
                    case "GID":
                        DefineGDI(ref dgUnit, segment);
                        break;

                    case "DGS":
                        ReadBaplieFile.ReadDgSegment(segment, dgUnit);
                        break;

                    case "FTX":
                        //FTX+AAC++P
                        //FTX+AAC++SEG+IMDG CODE SEGREGATION GROUP -:ALKALIS
                        //FTX+AAD++TLQ+CORROSIVE LIQUID, N.O.S., PACKING GROUP III.:(POTASSIUM HYDROXIDE)

                        //marine pollutant
                        if (segment == "FTX+AAC++P")
                        {
                            dgUnit.IsMp = true;
                            dgUnit.mpDetermined = true;
                        }
                        else if (segment == "FTX+AAC++N")
                        {
                            dgUnit.IsMp = false;
                            dgUnit.mpDetermined = true;
                        }

                        //segregation group
                        else if (segment.StartsWith("FTX+AAC++SEG"))
                        {
                            DefineSegregationGroup(dgUnit, segment);
                        }

                        //AAD
                        else if (segment.StartsWith("FTX+AAD"))
                        {
                            //limited quantity
                            DefineLq(dgUnit, segment);

                            //proper shipping name
                            DefineProperShippingName(dgUnit, segment);

                            //technical name
                            DefineTechnicalName(dgUnit, segment);

                            //packing group
                            DefinePackingGroup(dgUnit, segment);
                        }
                        break;

                    //Measurements
                    case "MEA":
                        DefineNetWeight(dgUnit, segment);
                        break;

                    //Split goods placement
                    case "SGP":
                        DefineSGP(ref container, dgUnit, segment);
                        break;

                    default:
                        break;
                }
            }
        }

        #region Methods to define Container properties

        /// <summary>
        /// Adds containers to CargoPlan from EQD segments list.
        /// </summary>
        /// <param name="segment"></param>
        private static void DefineEQD(string segment)
        {
            if (segment.Substring(4, 2) == "CN")
            {
                segment = segment.Replace(" ", "");

                Container a = new Container();
                a.ContainerNumber = segment.Substring(7, segment.IndexOf("+", 7, StringComparison.Ordinal) - 7);
                a.ContainerType = segment.Substring(segment.IndexOf("+", 7, StringComparison.Ordinal) + 1, 4);

                _cargoPlan.Containers.Add(a);
            }
        }

        /// <summary>
        /// Reads LOC segment and defines respective property.
        /// </summary>
        /// <param name="dgUnit"></param>
        /// <param name="container"></param>
        /// <param name="pol">reference to pol local variable</param>
        /// <param name="pod">reference to pod local variable</param>
        /// <param name="segment"></param>
        private static void DefineLOCsegment(Container container, string segment)
        {
            var location = ReadBaplieFile.ParseLOCsegment(segment);

            //POL
            if (segment.StartsWith("LOC+9+"))
            {
                tPOL = location;
            }
            //POD
            else if (segment.StartsWith("LOC+11"))
            {
                tPOD = location;
            }
            //Container location
            else if (segment.StartsWith("LOC+147"))
            {
                if (container == null) return;
                container.Location = location;
                container.HoldNr = Transport.ShipProfile.DefineCargoHoldNumber(container.Bay);

                foreach (var unit in _cargoPlan.DgList
                    .Where(d => d.ContainerNumber == container.ContainerNumber))
                {
                    unit.Location = location;
                    unit.HoldNr = container.HoldNr;
                }
            }
        }

        /// <summary>
        /// Initializes dgUnit.
        /// Reads packagings info and assigns it to the dgUnit.
        /// </summary>
        /// <param name="dgUnit"></param>
        /// <param name="segment"></param>
        private static void DefineGDI(ref Dg dgUnit, string segment)
        {
            dgUnit = new Dg();

            var fields = segment.Split('+');

            if (fields[2].Length == 0) return;
            if (!fields[2].Contains(':')) return;

            var packages = fields[2].Split(':');
            ushort.TryParse(packages[0], out dgUnit.numberOfPackages);
            dgUnit.typeOfPackages = packages[1];
            dgUnit.typeOfPackagesDescription = packages[packages.Length - 1];

            dgUnit.MergePackagesInfo();
        }

        /// <summary>
        /// Reads Split goods placement information and updates Cargo Plan with the dgUnit.
        /// </summary>
        /// <param name="pol"></param>
        /// <param name="pod"></param>
        /// <param name="dgUnit"></param>
        /// <param name="segment"></param>
        private static void DefineSGP(ref Container container, Dg dgUnit, string segment)
        {
            //SGP+PONU1903721+6
            string number = segment.Split('+')[1].Replace(" ", "");

            if (container == null || !string.Equals(container.ContainerNumber, number))
            {
                container = _cargoPlan.Containers.FirstOrDefault(c => c.ContainerNumber == number);
                if (container is null)
                {
                    string errorMessage = $"Error while reading IFTDGN file. Dg with container number {number} is not declared in EQD segments. The unit will not be created and added in Cargo plan.";
                    LogWriter.Write(errorMessage);
                    return;
                }

                container.POD = tPOD;
                container.POL = tPOL;
            }

            container.DgCountInContainer++;

            dgUnit.CopyContainerAbstractInfo(container);
            _cargoPlan.DgList.Add(dgUnit);
        }

        #endregion


        #region Methods to define DG properties

        /// <summary>
        /// Defines Segregation group from the segment and assigns it to the dgUnit
        /// </summary>
        /// <param name="dgUnit"></param>
        /// <param name="segment"></param>
        private static void DefineSegregationGroup(Dg dgUnit, string segment)
        {
            string[] segregationGroupsFromSegment = segment.Substring(segment.IndexOf(':') + 1).ToLower().Replace(" ", "").Split(':');
            foreach (var segregationGroupFromSegment in segregationGroupsFromSegment)
            {
                //if contains alien symbols - clear them
                string segregationGroupFromSegmentFinal =
                    segregationGroupFromSegment.Contains("(")
                        ? segregationGroupFromSegment.Remove(segregationGroupFromSegment.IndexOf("("))
                        : segregationGroupFromSegment;

                //check for possible digits included in the beginning
                for (int c = 0; c < segregationGroupFromSegmentFinal.Length; c++)
                {
                    if (char.IsLetter(segregationGroupFromSegmentFinal[0]))
                        break;
                    segregationGroupFromSegmentFinal = segregationGroupFromSegmentFinal.Remove(0, 1);
                }

                //find matching group from dictionary and assign
                try
                {
                    string segregationGroup = IMDGCode.SegregationGroupMatch[segregationGroupFromSegmentFinal];
                    dgUnit.SegregationGroup = segregationGroup;
                }
                catch (Exception)
                {
                    LogWriter.Write($"IFTDGN segregation group record thrown an exception: {segregationGroupFromSegmentFinal}");
                }
            }
        }

        /// <summary>
        /// Reads Net weight from the segment and assigns it to the dgUnit.
        /// </summary>
        /// <param name="dgUnit"></param>
        /// <param name="segment"></param>
        private static void DefineNetWeight(Dg dgUnit, string segment)
        {
            //MEA+AAE+AAL+KGM:1000.000
            if (segment.StartsWith("MEA+AAE+AAL+KGM"))
                dgUnit.DgNetWeight = decimal.Parse(segment.Substring(segment.IndexOf(':') + 1));
        }

        /// <summary>
        /// Reads Packing group info in the segment and assigns it to the dgUnit, if not assigned previously.
        /// </summary>
        /// <param name="dgUnit"></param>
        /// <param name="segment"></param>
        private static void DefinePackingGroup(Dg dgUnit, string segment)
        {
            if (!string.IsNullOrEmpty(dgUnit.PackingGroup)) return;

            //1.define way its recorded
            bool packingGroupSkipped = false;
            string packingGroupText = FindPackingGroupText(segment, ref packingGroupSkipped);

            //2. retrieve group
            if (!packingGroupSkipped)
            {
                string shortedSegment = segment.Replace(" ", "").Replace(",", "").Replace(".", "");
                string packingGroup = shortedSegment.Contains(':')
                    ? shortedSegment.Remove(shortedSegment.LastIndexOf(':')).
                        Substring(shortedSegment.IndexOf
                            (packingGroupText.Replace(" ", "").Replace(",", "").Replace(".", "")
                            , StringComparison.InvariantCulture) + 12)
                    : shortedSegment.Substring(shortedSegment.IndexOf(packingGroupText.Replace(" ", "").Replace(",", "").Replace(".", "")
                        , StringComparison.InvariantCulture) + 12);
                dgUnit.PackingGroup = packingGroup;
            }
        }

        /// <summary>
        /// Reads Technical Name from the segment and assigns it to the dgUnit.
        /// </summary>
        /// <param name="dgUnit"></param>
        /// <param name="segment"></param>
        private static void DefineTechnicalName(Dg dgUnit, string segment)
        {
            if (segment.Contains(':'))
                dgUnit.TechnicalName = segment.Substring(segment.IndexOf(':') + 1);
        }

        /// <summary>
        /// Reads Proper Shipping Name from the segment and assigns it to the dgUnit.
        /// </summary>
        /// <param name="dgUnit"></param>
        /// <param name="segment"></param>
        private static void DefineProperShippingName(Dg dgUnit, string segment)
        {
            //find index of start position for proper shipping name
            int startIndex = segment.Contains("+++")
                ? 10
                : segment.Contains("LQ+")
                    ? segment.IndexOf("LQ+") + 3
                    : segment.Contains(':')
                        ? segment.Substring(0, segment.IndexOf(':')).LastIndexOf('+') + 1
                        : segment.LastIndexOf('+') + 1;

            //cut the proper shipping name only
            dgUnit.Name = segment.Contains(':')
                ? segment.Remove(segment.IndexOf(':')).Substring(startIndex)
                : segment.Substring(startIndex);

            //determining any additional information from name
            string shortedName = dgUnit.Name
                .Replace(" ", "").Replace(",", "").Replace(".", "")
                .ToLower();
            if (shortedName.Contains("waste"))
            {
                dgUnit.IsWaste = true;
            }
            else if (shortedName.Contains("max1l"))
            {
                dgUnit.IsMax1L = true;
            }
        }

        /// <summary>
        /// Reads LQ from the segment and assigns it to the dgUnit.
        /// </summary>
        /// <param name="dgUnit"></param>
        /// <param name="segment"></param>
        private static void DefineLq(Dg dgUnit, string segment)
        {
            if (segment.Contains("LQ") || segment.ToLower().Contains("limited quant") ||
                                                 segment.ToLower().Replace(" ", "").Contains("ltdqty"))
            {
                dgUnit.IsLq = true;
            }
        }

        /// <summary>
        /// Determines weather segment contains information of packing group and returns how it's described
        /// </summary>
        /// <param name="segment">Segment of .edi FTX.</param>
        /// <param name="packingGroupSkipped">True - if no packing group description found.</param>
        /// <returns>Text how packing group is described in the segment.</returns>
        private static string FindPackingGroupText(string segment, ref bool packingGroupSkipped)
        {
            string packingGroupText = null;
            if (segment.Contains(", PACKING GROUP"))
                packingGroupText = ", PACKING GROUP";
            else if (segment.Contains(",PACKING GROUP"))
                packingGroupText = ",PACKING GROUP";
            else if (segment.Contains("PACKING GROUP"))
                packingGroupText = "PACKING GROUP";
            else if (segment.Contains(", PKG"))
                packingGroupText = ", PKG";
            else if (segment.Contains(",PKG"))
                packingGroupText = ",PKG";
            else if (segment.Contains("PKG"))
                packingGroupText = "PKG";
            else packingGroupSkipped = true;
            return packingGroupText;
        }

        #endregion

    }
}
