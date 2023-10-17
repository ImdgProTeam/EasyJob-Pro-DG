using EasyJob_ProDG.Data.Info_data;
using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.Transport;
using System;
using System.Diagnostics;
using System.Linq;

namespace EasyJob_ProDG.Model.IO
{
    internal class ReadIftdgnFile
    {
        private static CargoPlan _cargoPlan;
        private static ShipProfile _ship;

        /// <summary>
        /// Converts EdiSegmentArray into CargoPlan.
        /// </summary>
        /// <param name="segmentArray"></param>
        /// <param name="ship">Current ShipProfile.</param>
        /// <returns></returns>
        internal static CargoPlan ReadSegments(EdiSegmentArray segmentArray, ShipProfile ship)
        {
            _cargoPlan = new CargoPlan();
            _ship = ship;

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
            bool started = false;
            bool isContainerListCompleted = false;

            string pol = null;
            string pod = null;

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
                        if (segment.StartsWith("BGM"))
                            started = true;
                        continue;

                    case "EQD":
                        if (segment.Substring(4, 2) == "CN")
                        {
                            segment = segment.Replace(" ", "");

                            Container a = new Container();
                            a.ContainerNumber = segment.Substring(7, segment.IndexOf("+", 7, StringComparison.Ordinal) - 7);
                            a.ContainerType = segment.Substring(segment.IndexOf("+", 7, StringComparison.Ordinal) + 1, 4);

                            _cargoPlan.Containers.Add(a);
                        }
                        break;

                    #region LOC
                    case "LOC":
                        if (segment.StartsWith("LOC+9+"))
                        {
                            //POL
                            pol = segment.Length > 10 ? segment.Substring(6, 5) : segment.Substring(6, segment.Length - 6);
                            break;
                        }
                        else if (segment.StartsWith("LOC+11"))
                        {
                            //POD
                            pod = segment.Length > 11 ? segment.Substring(7, 5) : segment.Substring(7);
                            break;
                        }
                        else if (segment.StartsWith("LOC+147"))
                        {
                            //Container location
                            if (container != null)
                            {
                                container.Location = segment.Substring(8, 7); ;
                                container.HoldNr = _ship.DefineCargoHoldNumber(container.Bay);

                                dgUnit?.CopyContainerInfo(container);
                                _cargoPlan.DgList.Add(dgUnit);
                            }
                            break;
                        }
                        break;
                    #endregion

                    //Goods items details
                    //'GID+1+40:4G:::FIBREBOARD BOX
                    case "GID":
                        dgUnit = new Dg();

                        var fields = segment.Split('+');

                        if (fields[2].Length == 0) break;
                        if (!fields[2].Contains(':')) break;

                        var packages = fields[2].Split(':');
                        ushort.TryParse(packages[0], out dgUnit.numberOfPackages);
                        dgUnit.typeOfPackages = packages[1];
                        dgUnit.typeOfPackagesDescription = packages[packages.Length - 1];

                        dgUnit.MergePackagesInfo();
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

                            string[] segregationGroupsFromSegment = segment.Substring(segment.IndexOf(':') + 1).ToLower().Replace(" ", "").Split(':');
                            foreach (var segregationGroupFromSegment in segregationGroupsFromSegment)
                            {
                                //if contains alien symbols - clear them
                                string segregationGroupFromSegmentFinal =
                                    (segregationGroupFromSegment.Contains("("))
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
                                    Debug.WriteLine($">>>>>>>IFTDGN segregation group record thrown an exception: {segregationGroupFromSegmentFinal}");
                                }
                            }
                        }

                        //AAD
                        else if (segment.StartsWith("FTX+AAD"))
                        {
                            //limited quantity
                            if (segment.Contains("LQ") || segment.ToLower().Contains("limited quant") ||
                                     segment.ToLower().Replace(" ", "").Contains("ltdqty"))
                            {
                                dgUnit.IsLq = true;
                            }

                            //proper shipping name

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


                            //technical name
                            if (segment.Contains(':'))
                                dgUnit.TechnicalName = segment.Substring(segment.IndexOf(':') + 1);

                            if (!string.IsNullOrEmpty(dgUnit.PackingGroup)) break;

                            //packing group
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
                        break;

                    //Measurements
                    case "MEA":
                        //MEA+AAE+AAL+KGM:1000.000
                        if (segment.StartsWith("MEA+AAE+AAL+KGM"))
                            dgUnit.DgNetWeight = decimal.Parse(segment.Substring(segment.IndexOf(':') + 1));
                        break;

                    //Split goods placement
                    case "SGP":
                        //SGP+PONU1903721+6
                        string number = segment.Substring(4, 11);
                        if (number.Contains('+')) number = number.Remove(number.IndexOf('+'));

                        var cont = _cargoPlan.Containers.FirstOrDefault(c => c.ContainerNumber == number);
                        cont.POL = pol;
                        cont.POD = pod;
                        cont.DgCountInContainer++;

                        dgUnit.CopyContainerInfo(cont);
                        _cargoPlan.DgList.Add(dgUnit);
                        break;

                    default:
                        break;
                }
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
    }
}
