using System;
using System.Collections.Generic;


namespace EasyJob_Pro_DG
{
    public class Container
    {
        public string cntrNr;   //EQD+CN+...+
        public string cntrType;  //EQD+CN+ABCD1234567+...+
        public bool typeRecognized;
        public string cntrLocation; //LOC+147+BBBRRTT::5'
        public string cnPOL;      //LOC+9+...(:139:6)'
        public string cnPOD;      //LOC+11+...'
        public string finalDestination; //LOC+83+...'
        public string carrier;  //NAD+CA+YMLU:172:20;
        public byte dgInContainer;
        public List<Dg> dg;
        public bool RF;
        public double RFtmp;
        public bool underdeck;
        public byte holdNr;
        public byte bay;
        public byte row;
        public byte tier;
        public byte size; //20' or 40'
        public bool closed; //closed cargo transport unit
        //public reefer RF;     //TMP+2+...:CEL'
        //public size DIM;


        /// <summary>
        /// Set: will record location and update fields row, bay, underdeck etc.
        /// </summary>
        public string Location
        {
            get { return cntrLocation; }
            set
            {
                cntrLocation = value;
                DefineContainerLocation();
            }
        }

        public Container() 
            {
                cntrNr = null;
                dgInContainer =0;
                RF = false;
                underdeck = false;
                closed = true;
                typeRecognized = false;
            }

        public void DefineContainerLocation()
        {
            bay= byte.Parse(cntrLocation.Remove(cntrLocation.Length-4));
            row = byte.Parse(cntrLocation.Remove(0, cntrLocation.Length-4).Remove(2));
            tier = byte.Parse(cntrLocation.Remove(0,cntrLocation.Length-2));
            underdeck = tier < 78;
            size = (byte)(bay % 2 == 0 ? 40 : 20);
        }

        public Dg ConvertToDg()
        {
            Dg dg = new Dg();
            dg.CopyContainerInfo(this);
            if (this.RF) dg.Dgclass = "Reefer";
            return dg;
        }

    }
}
