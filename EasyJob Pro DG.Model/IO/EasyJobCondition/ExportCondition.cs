using EasyJob_ProDG.Model.Cargo;

namespace EasyJob_ProDG.Model.IO.EasyJobCondition
{
    public static class ExportCondition
    {
        /// <summary>
        /// Saves cargoPlan to EasyJobCondition file with the given name
        /// </summary>
        /// <param name="obj">CargoPlan object</param>
        /// <param name="filePath">Full path of file to be saved</param>
        public static void SaveFile(object obj, string filePath)
        {
            //Skip if CargoPlan does not exist or empty
            if ((CargoPlan)obj == null || ((CargoPlan)obj).IsEmpty) return;

            //Saves condition
            EasyJobCondition.SaveCondition(filePath, (CargoPlan)obj);

            //Changes FileName property
            OpenFile.SetFileName(OpenFile.GetFileNameWithExtension(filePath));
        }

    }
}
