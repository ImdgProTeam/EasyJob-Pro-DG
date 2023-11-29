using EasyJob_ProDG.Data.Info_data;

namespace EasyJob_ProDG.Model.Transport
{
    public class DOC
    {
        byte DefaultNumberOfClasses = (byte)IMDGCode.DOCClassesDictionary.Count;

        #region Public properties

        //Table from DOC with permission to load certain class into certain CH
        //0 - NOT ALLOWED, 1 - PACKAGED GOODS ALLOWED, 2- ALLOWED WITH REMARK
        //First row [0] - Weather deck
        public byte[,] DOCtable { get; set; }

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

        /// <summary>
        /// Represents number of class cases (matches with <see cref="DefaultNumberOfClasses"/>
        /// </summary>
        public byte NumberOfClasses => (byte)DOCtable.GetLength(1);

        /// <summary>
        /// Represents number of cargo holds + weather deck
        /// </summary>
        public byte NumberOfRows => (byte)DOCtable.GetLength(0); 

        #endregion


        #region Public methods

        /// <summary>
        /// Updates selected DOCRow with parsed values from <see cref="string"/> line
        /// </summary>
        /// <param name="line">String containing the values (array of '0's and '1's)</param>
        /// <param name="holdNr">Hold number to assign the values ('0' is for weather deck).</param>
        public void SetDOCTableRow(string line, byte holdNr)
        {
            var singleHoldDOC = new byte[NumberOfClasses];
            string[] lineSplit = line.Split(',');
            if (lineSplit.Length != NumberOfClasses)
                Output.ThrowMessage("Error number of classes handed over to DOC!");
            byte i = 0;
            foreach (string figure in lineSplit)
            {
                singleHoldDOC[i] = (byte.Parse(figure));
                i++;
            }
            SetDOCRow(singleHoldDOC, holdNr);
        } 

        #endregion

        #region Private methods

        /// <summary>
        /// Fills selected DOCRow with provided values
        /// </summary>
        /// <param name="values"></param>
        /// <param name="holdNr"></param>
        private void SetDOCRow(byte[] values, byte holdNr)
        {
            byte i = 0;
            foreach (byte value in values)
            {
                DOCtable[holdNr, i] = value;
                i++;
            }
        }

        /// <summary>
        /// Fills all the values of the selected row with '1' (true)
        /// </summary>
        /// <param name="holdNr">Selected row</param>
        private void SetNewDefaultDOCRow(byte holdNr)
        {
            for (byte i = 0; i < NumberOfClasses; i++)
            {
                DOCtable[holdNr, i] = 1;
            }
        } 

        #endregion


        #region Constructor

        public DOC(int numberofholds)
        {
            DOCtable = new byte[numberofholds + 1, DefaultNumberOfClasses];
            for (byte i = 0; i <= numberofholds; i++)
            {
                SetNewDefaultDOCRow(i);
            }
        } 

        #endregion
    }
}
