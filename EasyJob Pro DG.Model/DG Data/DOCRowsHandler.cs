namespace EasyJob_ProDG.Model.Cargo
{
    public static class DOCRowsHandler
    {
        /// <summary>
        /// Method to assign row number for dg in DOC table
        /// </summary>
        public static void AssignRowFromDOC(this Dg dg)
        {
            if (string.IsNullOrEmpty(dg.DgClass)) return;
            if(dg.DgClass.Substring(0,1)=="1") dg.DgRowInDOC = (byte)(dg.DgClass == "1.4S" ? 1 : 0);
            else switch (dg.DgClass)
            {
                case "2.1":
                    dg.DgRowInDOC = 2;
                    break;
                case "2.2":
                    dg.DgRowInDOC = 3;
                    break;
                case "2.3":
                    dg.DgRowInDOC = (byte)(dg.Flammable?4:5);
                    break;
                    case "3":
                    dg.DgRowInDOC = (byte) (dg.FlashPointDouble < 23 ? 6 : 7);
                    break;
                case "4.1":
                    dg.DgRowInDOC = 8;
                    break;
                case "4.2":
                    dg.DgRowInDOC = 9;
                    break;
                case "4.3":
                    dg.DgRowInDOC = (byte) (dg.Liquid ? 10 : 11);
                    break;
                case "5.1":
                    dg.DgRowInDOC = 12;
                    break;
                case "5.2":
                    dg.DgRowInDOC = 13;
                    break;
                case "6.1":
                    if (dg.Liquid)
                    {
                        if (dg.Flammable) dg.DgRowInDOC = (byte) (dg.FlashPointDouble < 23 ? 14 : 15);
                        else dg.DgRowInDOC = 16;
                    }
                    else dg.DgRowInDOC = 17;
                    break;
                case "8":
                    if (dg.Liquid)
                    {
                        if (dg.Flammable) dg.DgRowInDOC = (byte) (dg.FlashPointDouble < 23 ? 18 : 19);
                        else dg.DgRowInDOC = 20;
                    }
                    else dg.DgRowInDOC = 21;
                    break;
                case "9":
                    dg.DgRowInDOC = 22;
                    break;
                default:
                    dg.DgRowInDOC = 22;
                    break;
            }
        }
    }
}
