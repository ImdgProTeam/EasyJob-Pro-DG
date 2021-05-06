using System.Collections.Generic;

namespace EasyJob_ProDG.Model.Transport
{
    public class DOC
    {
        public byte[,] DOCtable { get; set; }
        private byte[] SingleHoldDOC;
        public string[] DOCadditional { get; set; }
        public byte NumberOfClasses => (byte)DOCtable.GetLength(1);
        public byte NumberOfRows => (byte)DOCtable.GetLength(0);
        const byte DefaultNumberOfClasses = 23;


        public DOC(int numberofholds)
        {
            //Table from DOC with permission to load certain class into certain CH
            //0 - NOT ALLOWED, 1 - PACKAGED GOODS ALLOWED, 2- ALLOWED WITH REMARK
            //First row [0] - Weather deck
            DOCtable = new byte[numberofholds+1,DefaultNumberOfClasses];
            for (byte i = 0; i <= numberofholds; i++)
            {
                SetNewDOCRow(i);
            }
            //{     
            //    { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1},     //Weather deck
            //    { 1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1},
            //    { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1},
            //    { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1},
            //    { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1},
            //    { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1},
            //    { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1},
            //    { 0,1,0,1,1,1,0,1,1,1,0,1,1,0,0,1,1,1,1,1,1,1,1},
            //    { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}        
            //};

            //THE BELOW CHECK AND DELETE IF NOT USED
            //
            //List of dg classes defined in DOC and titles to them
            //List<string> DOCclasses = new List<string>()
            //{
            //    "1.1-1.6\nExplosives",                                          //0
            //    "1.4(S)\nExplosives, Division 1.4 Compatibiliyu group 'S'",     //1
            //    "2.1\nFlammable gases",                                         //2
            //    "2.2\nNon-flammable, Non-toxic gases",                          //3
            //    "2.3\nToxic gases (flammable)",                                 //4
            //    "2.3\nToxic gases (non-flammable)",                             //5
            //    "3\nFlammable liquids - low and intermediate flashpoint, <23°C",
            //    "3\nFlammable liquids - high flashpoint, >=23°C but <=60°C",
            //    "4.1\nFlammable solids, self-reactive substances and solid desensitized explosives",
            //    "4.2\nSolids liable to spontaneous combustion",
            //    "4.3\nSubstances which, in contact with water, emit flammable gases (liquids)",
            //    "4.3\nSubstances which, in contact with water, emit flammable gases (solids)",
            //    "5.1\nOxidising substances (agents)",
            //    "5.2\nOrganic peroxides",
            //    "6.1\nToxic substances (liquids) - low and intermediate flashpoint, <23°C",
            //    "6.1\nToxic substances (liquids) - high flashpoint, >=23°C but <=60°C",
            //    "6.1\nToxic substances (liquids) - non flammable",
            //    "6.1\nToxic substances (solids)",
            //    "8\nCorrosives (liquids) - low and intermediate flashpoint, <23°C",
            //    "8\nCorrosives (liquids) - high flashpoint, >=23°C but <=60°C",
            //    "8\nCorrosives (liquids) - non flammable",
            //    "8\nCorrosives (solids)",
            //    "9\nMiscellaneous Dangerous Substances and Articles"
            //};
        }


        public void SetDOCTableRow(string line, byte holdNr)
        {
            SingleHoldDOC = new byte[NumberOfClasses];
            string[] lineSplit = line.Split(',');
            if (lineSplit.Length != NumberOfClasses) 
                Output.ThrowMessage("Error number of classes handed over to DOC!");
            byte i = 0;
            foreach (string figure in lineSplit)
            {
                SingleHoldDOC[i] = (byte.Parse(figure));
                i++;
            }
            SetDOCRow(holdNr);
        }
        //private void AddLineToDOCTable(string line)
        //{
        //    SingleHoldDOC = new List<byte>();
        //    string[] lineSplit = line.Split(',');
        //    foreach(string figure in lineSplit)
        //    {
        //        SingleHoldDOC.Add(byte.Parse(figure));
        //    }
        //    SetDOCRow(SingleHoldDOC);
        //}
        private void SetDOCRow(byte holdNr)
        {
            byte i = 0;
            foreach(byte value in SingleHoldDOC)
            {
                DOCtable[holdNr, i] = value;
                i++;
            }
        }

        private void SetNewDOCRow(byte holdNr)
        {
            for (byte i = 0; i < NumberOfClasses; i++)
            {
                DOCtable[holdNr, i] = 1;
            }
        }

        public byte[] SingleHoldRecord(byte holdNumber)
        {
            byte[] array = new byte[NumberOfClasses];
            for (byte i = 0; i < NumberOfClasses; i++)
            {
                array[i] = DOCtable[holdNumber,i];
            }
            return array;
        }

        public void CopyHoldRecords(byte copyToHoldNumber, DOC copyFromDOC, byte copyFromHoldNumber)
        {
            for (byte i = 0; i < NumberOfClasses; i++)
            {
                DOCtable[copyToHoldNumber,i] = copyFromDOC.DOCtable[copyFromHoldNumber,i];
            }
        }
    }
}
