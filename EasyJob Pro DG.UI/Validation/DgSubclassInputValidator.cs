namespace EasyJob_ProDG.UI.Validation
{
    internal static class DgSubclassInputValidator
    {
        internal static bool Validate(string inputValue, out string outputValue)
        {
            var setvalue = inputValue.Replace(",", " ").Replace("  ", " ").Trim();
            outputValue = setvalue;

            string[] array = setvalue.Split(' ');
            for (int i = 0; i < array.Length; i++)
            {
                //max 2 subclasses
                if (i > 1) array[i] = null;

                if (!DgClassInputValidator.Validate(array[i], out array[i]))
                    return false;
            }
            setvalue = string.Join(" ", array).Trim();

            outputValue = setvalue;
            return true;
        }
    }
}
