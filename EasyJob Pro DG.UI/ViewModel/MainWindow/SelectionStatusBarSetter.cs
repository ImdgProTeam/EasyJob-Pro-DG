using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.ViewModel.MainWindow
{
    internal static class SelectionStatusBarSetter
    {
        internal static string SetSelectionStatusBarTextForDg(object obj)
        {
            return SetSelectionStatusBar(obj);
        }
        private static string SetSelectionStatusBar(object obj)
        {
            var selectedDgs = obj as IList<object>;
            if (selectedDgs is null)
                return "None";
            //selected only one Dg
            if (selectedDgs.Count == 1)
            {
                DgWrapper dg = (DgWrapper)selectedDgs[0];
                string conflicted = !dg.IsConflicted ? "None                   "
                    : (dg.Conflicts.FailedStowage ? ("Stowage" + (dg.Conflicts.FailedSegregation ? " and segregation" : "                "))
                    : dg.Conflicts.FailedSegregation ? "Segregation           " : "");

                return $"{dg.DisplayContainerNumber}" +
                    $"\tPosition: {dg.Location}" +
                    $"\tUNNO:{dg.Unno: 0000}" +
                    $"\tNet weight: " + string.Format("{0, 12 : # ##0.000}", dg.DgNetWeight) +
                    $"\tConflicts: {conflicted}" +
                    $"{(dg.IsLq ? "\tLQ" : "\t  ")}" +
                    $"{(dg.IsMp ? "\tMP" : "\t  ")}" +
                    $"{(dg.IsRf ? "\tReefer" : "\t      ")}" +
                    $"{(dg.IsOpen ? "\t\tOpen type" : "\t\t         ")}" +
                    $"{(dg.HasLocationChanged ? "\tRestow" : "\t      ")}";
            }
            //multiple selection
            else
            {
                byte containersCount = 0;
                byte dgCount = 0;
                byte reefersCount = 0;
                byte restowCount = 0;
                byte changedPODCount = 0;
                byte confilictedCount = 0;
                decimal dgNetWeight = 0.0m;
                string _cntrNumber = string.Empty;
                string _firstSelectedNumber = string.Empty;

                foreach (DgWrapper d in selectedDgs)
                {
                    if (d.ContainerNumber != _cntrNumber && d.ContainerNumber != _firstSelectedNumber)
                    {
                        if (string.IsNullOrEmpty(_firstSelectedNumber))
                            _firstSelectedNumber = d.ContainerNumber;
                        containersCount++;
                        _cntrNumber = d.ContainerNumber;
                        if (d.IsRf)
                            reefersCount++;
                        if (d.HasLocationChanged)
                            restowCount++;
                        if (d.HasPodChanged)
                            changedPODCount++;
                    }
                    dgCount++;
                    dgNetWeight += d.DgNetWeight;
                    if (d.IsConflicted)
                        confilictedCount++;
                }

                return $"DG: {dgCount,3} \tContainers: {containersCount,3} \tReefers: {reefersCount,3}" +
                    $"\t\tDG net weight: {dgNetWeight,14: # ### ##0.000}" +
                    $"\t\t\tConfilcted DG: {confilictedCount,3}" +
                    $"\tContainers restowed: {restowCount,3}" +
                    $"\tPOD changed: {changedPODCount,3}";
            }

        }
    }
}
