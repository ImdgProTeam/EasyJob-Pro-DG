using EasyJob_ProDG.Model.UserDefinedConflicts;
using EasyJob_ProDG.UI.Data.UserDefinedConflicts;
using System;
using System.Text;


// UDC file schema:

//UDC
//10
// 0 | value | 0 | 0 | 99 | 0 | 0 | 0 ||
// 0 | value | 0 | 0 | 99 | 0 | 0 | 0 ||
//EOUDC

//Descritption

//UDC = User defined conditions 
//10 - version (1.0)

//CONDITIONS:
// Type (UserDefinedConditionType) - byte | value : string |
// Cell position: hold | bay | row | tier | on / under deck |
// Condition option (UserDefinedConditionOption) : byte

// EOUDC = End of User Defined Conditions


namespace EasyJob_ProDG.Model.IO.UserDefinedConditions
{
    internal static class UDC_Records
    {
        #region Defaults and constants

        private const string FIRST_SEGMENT = "UDC";
        private const string LAST_SEGMENT = "EOUDC";

        /// <summary>
        /// Sequence of symbols used to mark end of condition
        /// </summary>
        private const string END_OF_LINE_MARKER = "||";
        private const uint CURRENT_VERSION = 10;

        #endregion

        #region Methods

        // ----- Read

        /// <summary>
        /// Checks mandatory UDC file attributes from the lines and returns its version.
        /// </summary>
        /// <param name="linesArray"></param>
        /// <returns>Null if UDC file lines are corrupt.</returns>
        internal static bool CheckUDCFileVersion(string[] linesArray)
        {
            uint version;

            if (string.Equals(linesArray[0], FIRST_SEGMENT))
                if (uint.TryParse(linesArray[1], out version))
                    if (version == CURRENT_VERSION)
                        return true;
            {
                Data.LogWriter.Write($"UDC file corrupt and cannot be read.");
                return false;
            }
        }

        /// <summary>
        /// Creates <see cref="UserDefinedConditionRepository"/> from the linesArray.
        /// </summary>
        /// <param name="linesArray"></param>
        /// <returns></returns>
        internal static UserDefinedConditionRepository CreateRepositoryFromLines(string[] linesArray)
        {
            var repository = new UserDefinedConditionRepository();

            foreach (var line in linesArray)
            {
                if (!line.EndsWith(END_OF_LINE_MARKER))
                {
                    if (string.Equals(line, LAST_SEGMENT))
                        break;
                    else
                        continue;
                }

                var condition = ParseUDCLine(line);
                repository.AddCondition(condition, out bool noValue);
            }

            return repository;
        }


        // ----- Record

        /// <summary>
        /// Creates text to be used in UDC file.
        /// </summary>
        /// <param name="repository"></param>
        internal static string CreateUDCFileFullText(UserDefinedConditionRepository repository)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(FIRST_SEGMENT);
            stringBuilder.AppendLine(CURRENT_VERSION.ToString());

            foreach (var condition in repository?.GetConditions)
            {
                stringBuilder.AppendLine(CreateConditionRecord(condition));
            }

            stringBuilder.AppendLine(LAST_SEGMENT);

            return stringBuilder.ToString();
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Tries to Parse a line into UserDefinedCondition.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>Null if failed to parse.</returns>
        private static UserDefinedCondition ParseUDCLine(string line)
        {
            try
            {
                string[] segment = line.Split('|');

                var condition = new UserDefinedCondition()
                {
                    ConditionType = (UserDefinedConditionType)(byte.Parse(segment[0])),
                    ConditionValue = segment[1],
                    CellPosition = new Transport.CellPosition()
                    {
                        HoldNr = byte.Parse(segment[2]),
                        Bay = byte.Parse(segment[3]),
                        Row = byte.Parse(segment[4]),
                        Tier = byte.Parse(segment[5]),
                        Underdeck = byte.Parse(segment[6])
                    },
                    ConditionOption = (UserDefinedConditionOption)(byte.Parse(segment[7]))
                };

                return condition;
            }
            catch (Exception ex)
            {
                Data.LogWriter.WriteError($"Reading UDC line {line} caused and exception: {ex.Message}");
            }

            return null;
        }


        /// <summary>
        /// Creates one line UserDefindedCondition record
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private static string CreateConditionRecord(UserDefinedCondition condition)
        {
            string result = $"{condition.ConditionType}|" +
                $"{condition.ConditionValue}" +
                $"{condition.CellPosition.HoldNr}|{condition.CellPosition.Bay}|{condition.CellPosition.Row}|" +
                $"{condition.CellPosition.Tier}|{condition.CellPosition.Underdeck}|" +
                $"{condition.ConditionOption}||";

            return result;
        } 

        #endregion
    }
}