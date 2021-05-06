using System.Collections.Generic;
using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.Transport;

namespace EasyJob_ProDG.Model.IO.EasyJobCondition
{
    /// <summary>
    /// Intermediate class to record EasyJobCondition to text file.
    /// </summary>
    internal class CargoPlanAssociated : List<ConditionUnit>
    {
        public Voyage VoyageInfo { get; set; }

        protected internal CargoPlanAssociated()
        {

        }

        /// <summary>
        /// Creates checked CargoPlanAssociated from CargoPlan.
        /// </summary>
        /// <param name="cargoPlan">CargoPlan to be recorded.</param>
        internal CargoPlanAssociated(CargoPlan cargoPlan)
        {
            VoyageInfo = cargoPlan.VoyageInfo;

            CreateCargoPlanAssociatedFromCargoPlan(cargoPlan);

            //check for missing dg
            CheckForMissingRecordsInNewCargoPlanAssociated(cargoPlan);
        }

        /// <summary>
        /// Creates CargoPlanAssociated from CargoPlan.
        /// </summary>
        /// <param name="cargoPlan">CargoPlan to be converted to AssociatedPlan.</param>
        private void CreateCargoPlanAssociatedFromCargoPlan(CargoPlan cargoPlan)
        {
            foreach (var container in cargoPlan.Containers)
            {
                var conditionUnit = new ConditionUnit().ToConditionUnit(container);

                //if (conditionUnit.DgCountInContainer > 0) conditionUnit.DgCargo.Clear();
                conditionUnit.DgCargoInContainer ??= new List<Dg>();
                conditionUnit.DgCountInContainer = 0;

                //Updating reefer info
                bool found = false;
                foreach (var reefer in cargoPlan.Reefers)
                {
                    if (reefer.ContainerNumber == conditionUnit.ContainerNumber)
                    {
                        conditionUnit.SetTemperature = reefer.SetTemperature;
                        conditionUnit.IsRf = true;
                        found = true;
                        break;
                    }
                }
                if(!found) conditionUnit.IsRf = false;


                //Updating Dg info
                foreach (var dg in cargoPlan.DgList)
                {
                    if (dg.ContainerNumber == conditionUnit.ContainerNumber)
                    {
                        conditionUnit.DgCargoInContainer.Add(dg);
                        conditionUnit.DgCountInContainer++;
                    }
                }

                this.Add(conditionUnit);
            }
        }

        //TODO: Method CheckForMissingRecordsInNewCargoPlanAssociated in CargoPlanAssociated.cs to be revised
        //and removed if not required

        /// <summary>
        /// Checks if there are still Dg remaining which have not been added to CargoPlanAssociated.
        /// </summary>
        /// <param name="cargoPlan">Original CargoPlan.</param>
        private void CheckForMissingRecordsInNewCargoPlanAssociated(CargoPlan cargoPlan)
        {
            List<ConditionUnit> newConditionUnitsList = new List<ConditionUnit>();

            foreach (var dg in cargoPlan.DgList)
            {
                bool containerFound = false;
                foreach (var container in cargoPlan.Containers)
                {
                    if (container.ContainerNumber == dg.ContainerNumber)
                    {
                        containerFound = true;
                        break;
                    }
                }

                if (containerFound) continue;

                //if not found
                //Check to be substituted with DgDataGrid check with ContainerDataGrid
                {
                    //Check if same unit number has been previously added 

                    //if newList is empty
                    if (newConditionUnitsList.Count == 0)
                    {
                        ConditionUnit conditionUnit = new ConditionUnit().ToConditionUnit(dg);
                        conditionUnit.DgCountInContainer++;

                        this.Add(conditionUnit);
                        newConditionUnitsList.Add(conditionUnit);

                        continue;
                    }

                    foreach (var newUnit in newConditionUnitsList)
                    {
                        //if found
                        if (newUnit.ContainerNumber == dg.ContainerNumber)
                        {
                            //Add dg to existing condition unit
                            foreach (var conditionUnit in this)
                            {
                                if (conditionUnit.ContainerNumber == dg.ContainerNumber)
                                {
                                    conditionUnit.DgCountInContainer++;
                                    containerFound = true;
                                    break;
                                }
                            }
                        }
                    }

                    //if not found
                    if (!containerFound)
                    {
                        ConditionUnit conditionUnit = new ConditionUnit().ToConditionUnit(dg);
                        conditionUnit.DgCountInContainer++;

                        this.Add(conditionUnit);
                        newConditionUnitsList.Add(conditionUnit);
                    }
                }
            }
        }
    }
}
