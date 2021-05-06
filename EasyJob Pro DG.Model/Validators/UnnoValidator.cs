using System.Linq;

namespace EasyJob_ProDG.Model.Validators
{
    public static class UnnoValidator
    {
        public static bool Validate(int unno)
        {
            var entries = (from entry in ProgramFiles.DgDataBase.Descendants("DG")
                           where (int)entry.Attribute("unno") == unno
                           select entry);

            return entries.Any();
        }
    }
}
