using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.ViewModel.MainWindow
{
    internal static class SelectionStatusBarSetter
    {
        internal static string GetSelectionStatusBarTextForContainer(object obj)
        {
            return GenerateStatusBarTextForContainer(obj);
        }
        private static string GenerateStatusBarTextForContainer(object obj)
        {
            var selectedContainers = obj as IList<object>;
            if (selectedContainers is null)
                return "None";
            //selected only one Container
            if (selectedContainers.Count == 1)
            {
                ContainerWrapper container = (ContainerWrapper)selectedContainers[0];

                return $"{container.DisplayContainerNumber} | " +
                    $"Position: {container.Location} | " +
                    $"{(container.IsRf ? "Reefer" : "      ")} | " +
                    $"{(container.ContainsDgCargo ? $"Contains DG" : "           ")} | " +
                    $"{(container.IsOpen ? "Open type" : "         ")} | " +
                    $"{(container.HasLocationChanged ? "Restow" : "      ")} | " +
                    $"{(container.HasPodChanged ? "POD changed" : "")}";
            }
            //multiple selection
            else
            {
                int containersCount = 0;
                int dgContainersCount = 0;
                int dgCount = 0;
                int reefersCount = 0;
                int restowCount = 0;
                int changedPODCount = 0;

                foreach (ContainerWrapper c in selectedContainers)
                {
                    containersCount++;
                    if (c.IsRf)
                        reefersCount++;
                    if (c.HasLocationChanged)
                        restowCount++;
                    if (c.HasPodChanged)
                        changedPODCount++;
                    if (c.ContainsDgCargo)
                        dgContainersCount++;
                    dgCount += c.DgCountInContainer;
                }

                return $"Containers:{containersCount,3} | Reefers:{reefersCount,3} | DG Containers:{dgContainersCount,3} | " +
                    $"DG cargoes:{dgCount,3} | " +
                    $"Containers restowed:{restowCount,3} | " +
                    $"POD changed:{changedPODCount,3}";
            }
        }

        internal static string GetSelectionStatusBarTextForDg(object obj)
        {
            return GenerateSelectionStatusBarForDg(obj);
        }
        private static string GenerateSelectionStatusBarForDg(object obj)
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
                    : dg.Conflicts.FailedSegregation ? "Segregation            " : "");

                return $"{dg.DisplayContainerNumber} | " +
                    $"Position: {dg.Location} | " +
                    $"UNNO:{dg.Unno: 0000} | " +
                    $"Net weight: " + string.Format("{0, 12 : # ##0.000}", dg.DgNetWeight) + " | " +
                    $"Conflicts: {conflicted} | " +
                    $"{(dg.IsLq ? "LQ" : "  ")} | " +
                    $"{(dg.IsMp ? "MP" : "  ")} | " +
                    $"{(dg.IsRf ? "Reefer" : "      ")} | " +
                    $"{(dg.IsOpen ? "Open type" : "         ")} | " +
                    $"{(dg.HasLocationChanged ? "Restow" : "      ")}";
            }
            //multiple selection
            else
            {
                int containersCount = 0;
                int dgCount = 0;
                int reefersCount = 0;
                int restowCount = 0;
                int changedPODCount = 0;
                int confilictedCount = 0;
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

                return $"DG:{dgCount,3} | Containers:{containersCount,3} | Reefers:{reefersCount,3}" +
                    $" | DG net weight:{dgNetWeight,14: # ### ##0.000}" +
                    $" | Confilcted DG:{confilictedCount,3}" +
                    $" | Containers restowed:{restowCount,3}" +
                    $" | POD changed:{changedPODCount,3}";
            }

        }
    }
}
