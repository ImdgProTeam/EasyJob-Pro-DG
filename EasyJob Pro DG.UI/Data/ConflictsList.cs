using System.Collections.ObjectModel;
using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Wrapper;
using EasyJob_ProDG.UI.ViewModel;

namespace EasyJob_ProDG.UI.Data
{
    public class ConflictsList : ObservableCollection<ConflictPanelItemViewModel>
    {
        // ---------------- Private fields ------------------------------------------
        private static bool _isTempConflictsCreating;

        // ---------------- Public constructors -------------------------------------
        public ConflictsList() { }


        // ---------------- Public methods ------------------------------------------

        /// <summary>
        /// Creates or updates existing conflicts.
        /// </summary>
        /// <param name="dgList">DgWrapperList to be checked for conflicts</param>
        public void CreateConflictList(DgWrapperList dgList)
        {
            //When open new file
            if (dgList.IsCollectionNew)
            {
                if (this.Count > 0) UnregisterInMessenger();
                Clear();
                CreateAllConflicts(dgList);
                dgList.IsCollectionNew = false;
            }
            //When modifying working dg list
            else
            {
                //creating new conflict list to compare with the old one
                ConflictsList tempConflicts = new ConflictsList();

                //create temporary list of conflicts
                _isTempConflictsCreating = true;
                tempConflicts.CreateAllConflicts(dgList);
                _isTempConflictsCreating = false;

                //update conflict list and remove temp list
                UpdateConflictList(dgList, tempConflicts);
            }
        }

        /// <summary>
        /// Renews all unit stowage conflicts in ConflictList
        /// </summary>
        /// <param name="unit"></param>
        public void UpdateDgWrapperStowageConfilicts(DgWrapper unit)
        {
            ushort iterations = (ushort)this.Count;

            //Clear unit associated stowage conflicts
            for(ushort i=0, n=0; i < iterations; i++, n++)
            {
                var conflict = this[n];
                if(conflict.IsStowageConflict && conflict.DgID == unit.Model.ID)
                {
                    conflict.UnregisterInMessenger();
                    Remove(conflict);
                    n--;
                }
            }

            CreateStowageConflict(unit);
            CreateSwConflicts(unit);
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
                this[i - c].UnregisterInMessenger();
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
        /// Unregisters each conflict in DataMessenger
        /// </summary>
        private void UnregisterInMessenger()
        {
            foreach (var conflict in this)
            {
                conflict.UnregisterInMessenger();
            }
        }


        /// <summary>
        /// Method add a new ConflictPanelItem to ConflictList, if it does not already exist
        /// </summary>
        /// <param name="conf"></param>
        private void AddNewConflict(ConflictPanelItemViewModel conf)
        {
            if (Contains(conf)) return;
            if (!_isTempConflictsCreating) conf.RegisterInMessenger();
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
