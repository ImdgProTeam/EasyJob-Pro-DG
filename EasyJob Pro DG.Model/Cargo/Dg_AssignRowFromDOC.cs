namespace EasyJob_ProDG.Model.Cargo
{
    public partial class Dg
    {
        /// <summary>
        /// Method to assign row number for dg in DOC table
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public void AssignRowFromDOC()
        {
            if (string.IsNullOrEmpty(dgclass)) return;
            if(dgclass.Substring(0,1)=="1") DgRowInDOC = (byte)(dgclass == "1.4S" ? 1 : 0);
            else switch (dgclass)
            {
                case "2.1":
                    DgRowInDOC = 2;
                    break;
                case "2.2":
                    DgRowInDOC = 3;
                    break;
                case "2.3":
                    DgRowInDOC = (byte)(Flammable?4:5);
                    break;
                    case "3":
                    DgRowInDOC = (byte) (flashPoint < 23 ? 6 : 7);
                    break;
                case "4.1":
                    DgRowInDOC = 8;
                    break;
                case "4.2":
                    DgRowInDOC = 9;
                    break;
                case "4.3":
                    DgRowInDOC = (byte) (Liquid ? 10 : 11);
                    break;
                case "5.1":
                    DgRowInDOC = 12;
                    break;
                case "5.2":
                    DgRowInDOC = 13;
                    break;
                case "6.1":
                    if (Liquid)
                    {
                        if (Flammable) DgRowInDOC = (byte) (flashPoint < 23 ? 14 : 15);
                        else DgRowInDOC = 16;
                    }
                    else DgRowInDOC = 17;
                    break;
                case "8":
                    if (Liquid)
                    {
                        if (Flammable) DgRowInDOC = (byte) (flashPoint < 23 ? 18 : 19);
                        else DgRowInDOC = 20;
                    }
                    else DgRowInDOC = 21;
                    break;
                case "9":
                    DgRowInDOC = 22;
                    break;
                default:
                    DgRowInDOC = 22;
                    break;
            }
        }
    }
}
