using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.ViewModel;
using EasyJob_ProDG.UI.Wrapper;
using System.Windows.Threading;

namespace EasyJob_ProDG.UI.Data
{
    public class ConflictsList : AsyncObservableCollection<ConflictPanelItemViewModel>
    {
        // ---------------- Public constructors -------------------------------------
        public ConflictsList()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
        }
        private Dispatcher dispatcher;


        // ---------------- Public methods ------------------------------------------

        /// <summary>
        /// Creates or updates existing conflicts.
        /// </summary>
        /// <param name="dgList">DgWrapperList to be checked for conflicts</param>
        public void CreateConflictList(DgWrapperList dgList)
        {
            dispatcher.Invoke(() =>
            { //When open new file
                if (dgList.IsCollectionNew)
                {
                    CreateConflictListForNewCollection(dgList);
                }
                //When modifying working dg list
                else
                {
                    UpdateConflictListForCurrentDgList(dgList);
                }
            });
        }

        /// <summary>
        /// Renews all unit stowage conflicts in ConflictList
        /// </summary>
        /// <param name="unit"></param>
        public void UpdateDgWrapperStowageConfilicts(DgWrapper unit)
        {
            UpdateUnitStowageConflicts(unit);
        }


        // ---------------- Private methods -----------------------------------------

        /// <summary>
        /// Checks all dg in dgList and adds stowage, segregation and SW conflicts to ConflictList
        /// </summary>
        /// <param name="dgList"></param>
        private void CreateAllConflicts(DgWrapperList dgList)
        {
            CreateStowageConflictList(dgList);
            CreateSegregationConflictList(dgList);
            CreateSwConflicts();
        }

        /// <summary>
        /// Updates existing ConflictsList with changes from DgList.
        /// </summary>
        /// <param name="dgList">Modified DgList.</param>
        /// <returns></returns>
        private void UpdateConflictListForCurrentDgList(DgWrapperList dgList)
        {
            //creating new conflict list to compare with the old one
            ConflictsList tempConflicts = new ConflictsList();

            //create temporary list of conflicts
            tempConflicts.CreateAllConflicts(dgList);

            //update conflict list and remove temp list
            UpdateConflictList(dgList, tempConflicts);
        }

        /// <summary>
        /// Creates new conflict list for new DgList.
        /// </summary>
        /// <param name="dgList">New DgList</param>
        /// <returns></returns>
        private void CreateConflictListForNewCollection(DgWrapperList dgList)
        {

            Clear();
            CreateAllConflicts(dgList);
            dgList.IsCollectionNew = false;
        }

        /// <summary>
        /// Updates only stowage conflicts for the selected DgWrapper.
        /// </summary>
        /// <param name="unit">DgWrapper which stowage conflicts shall be updated.</param>
        private void UpdateUnitStowageConflicts(DgWrapper unit)
        {
            ushort iterations = (ushort)this.Count;

            //Clear unit associated stowage conflicts
            for (ushort i = 0, n = 0; i < iterations; i++, n++)
            {
                var conflict = this[n];
                if (conflict.IsStowageConflict && conflict.DgID == unit.Model.ID)
                {
                    Remove(conflict);
                    n--;
                }
            }

            CreateStowageConflict(unit);
            CreateSwConflicts(unit);
        }

        /// <summary>
        /// Adds all stowage conflicts to Conflict list
        /// </summary>
        /// <param name="dgList">DgWrapperList</param>
        private void CreateStowageConflictList(DgWrapperList dgList)
        {
            foreach (DgWrapper dg in dgList)
                CreateStowageConflict(dg);
        }

        /// <summary>
        /// Adds stowage conflicts to ConflictList of a single dg
        /// </summary>
        /// <param name="dg"></param>
        private void CreateStowageConflict(DgWrapper dg)
        {
            if (dg.IsConflicted && dg.Conflicts.FailedStowage)
                foreach (string s in dg.Conflicts.StowageConflictsList)
                {
                    var newConflict = new ConflictPanelItemViewModel(dg, s);
                    AddNewConflict(newConflict);
                }
        }

        /// <summary>
        /// Adds all SW conflicts to Conflict list
        /// </summary>
        private void CreateSwConflicts()
        {
            var specialGroups = Stowage.SWgroups;
            foreach (var unit in specialGroups.ListSW19List)
            {
                var unitWrapper = new DgWrapper(unit);
                ConflictPanelItemViewModel conf = new ConflictPanelItemViewModel(unitWrapper, "SW19")
                {
                    GroupParam =
                        "SW19 For batteries transported in accordance with special provisions 376 or 377, category C, unless transported on a short international voyage. Please check cargo documents of the following units: "
                };
                AddNewConflict(conf);
            }
            foreach (var unit in specialGroups.ListSW22List)
            {
                var unitWrapper = new DgWrapper(unit);
                ConflictPanelItemViewModel conf = new ConflictPanelItemViewModel(unitWrapper, "SW22")
                {
                    GroupParam =
                        "SW22 For WASTE AEROSOLS and WASTE GAS CARTRIDGES: category C, clear of living quarters. Please check cargo documents of the unit "
                };
                AddNewConflict(conf);
            }
        }

        /// <summary>
        /// Adds SW conflicts of a selected unit to Conflict list
        /// </summary>
        private void CreateSwConflicts(DgWrapper dg)
        {
            var specialGroups = Stowage.SWgroups;
            foreach (var unit in specialGroups.ListSW19List)
            {
                if (unit.ID != dg.Model.ID) continue;
                var unitWrapper = new DgWrapper(unit);
                ConflictPanelItemViewModel conf = new ConflictPanelItemViewModel(unitWrapper, "SW19")
                {
                    GroupParam =
                        "SW19 For batteries transported in accordance with special provisions 376 or 377, category C, unless transported on a short international voyage. Please check cargo documents of the following units: "
                };
                AddNewConflict(conf);
            }
            foreach (var unit in specialGroups.ListSW22List)
            {
                if (unit.ID != dg.Model.ID) continue;
                var unitWrapper = new DgWrapper(unit);
                ConflictPanelItemViewModel conf = new ConflictPanelItemViewModel(unitWrapper, "SW22")
                {
                    GroupParam =
                        "SW22 For WASTE AEROSOLS and WASTE GAS CARTRIDGES: category C, clear of living quarters. Please check cargo documents of the unit "
                };
                AddNewConflict(conf);
            }
        }

        /// <summary>
        /// Adds all segregation conflicts to Conflict list
        /// </summary>
        /// <param name="dgList">DgWrapperList</param>
        private void CreateSegregationConflictList(DgWrapperList dgList)
        {
            foreach (DgWrapper dg in dgList)
                if (dg.IsConflicted && dg.Conflicts.FailedSegregation)
                {
                    foreach (Conflicts.SegregationConflict c in dg.Conflicts.SegregationConflictsList)
                    {
                        var newConflict =
                            new ConflictPanelItemViewModel(dg, c.Code, true, new DgWrapper(c.DgInConflict));
                        AddNewConflict(newConflict);
                    }
                }
        }

        /// <summary>
        /// Removes conflicts which do not exist in ConflictList to compare, adds new conflicts from the list.
        /// </summary>
        /// <param name="dgList">DgWrapperList</param>
        /// <param name="tempConflicts">New conflict list to be compared with</param>
        private void UpdateConflictList(DgWrapperList dgList, ConflictsList tempConflicts)
        {
            //initializing axillary fields
            int c = 0;
            int count = Count;

            //Remove redundant conflicts
            for (int i = 0; i < count; i++)
            {
                var con = this[i - c];

                //Check if reference unit removed from the list
                if (dgList.Contains(con.ContainerNumber))
                {
                    //Stowage
                    if (con.IsStowageConflict)
                        if (tempConflicts.Contains(con))
                            continue;

                    //Segregation
                    if (con.IsSegregationConflict)
                        //check if reference unit is still in list or is a reefer
                        if (dgList.Contains(con.ConflictingDgNumber) || con.Code == "SGC4")
                            if (tempConflicts.Contains(con))
                                continue;
                }

                //If not found - remove the conflict
                RemoveAt(i - c);
                c++;
            }

            //Add new conflicts
            foreach (var conf in tempConflicts)
            {
                AddNewConflict(conf);
            }
        }

        /// <summary>
        /// Method add a new ConflictPanelItem to ConflictList, if it does not already exist
        /// </summary>
        /// <param name="conf"></param>
        private void AddNewConflict(ConflictPanelItemViewModel conf)
        {
            if (Contains(conf)) return;
            Add(conf);
        }

        // ---------------- Private override methods ----------------------------------------

        /// <summary>
        /// Sets the rule to check weather a conflict exists in the list.
        /// </summary>
        /// <param name="conflict">ConflictPanelItem to be checked</param>
        /// <returns></returns>
        private new bool Contains(ConflictPanelItemViewModel conflict)
        {
            foreach (var con in this)
            {
                if (con.Location == conflict.Location
                    && con.ContainerNumber == conflict.ContainerNumber
                    && con.Code == conflict.Code
                    && con.Unno == conflict.Unno
                    && con.IsStowageConflict == conflict.IsStowageConflict)
                {
                    if (con.IsSegregationConflict)
                    {
                        if (conflict.IsSegregationConflict
                            && con.ConflictingDgNumber == conflict.ConflictingDgNumber
                            && con.ConflictingDgLocation == conflict.ConflictingDgLocation
                            && con.ConflictingDgUnno == conflict.ConflictingDgUnno)
                            return true;
                        continue;
                    }
                    return true;
                }
            }
            return false;
        }

    }
}
