using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.ViewModel;
using EasyJob_ProDG.UI.Wrapper;
using System.Windows.Threading;

namespace EasyJob_ProDG.UI.Data
{
    /// <summary>
    /// Handles various operations with <see cref="Data.ConflictsList"/> (extends it).
    /// </summary>
    internal static class ConflictsListHandler
    {

        // ---------------- Public constructors -------------------------------------
        static ConflictsListHandler()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
        }
        private static Dispatcher dispatcher;


        // ---------------- Public methods ------------------------------------------

        /// <summary>
        /// Creates or updates existing conflicts.
        /// </summary>
        /// <param name="dgList">DgWrapperList to be checked for conflicts</param>
        internal static void CreateConflictList(this ConflictsList conflictsList, DgWrapperList dgList)
        {
            dispatcher.Invoke(() =>
            { //When open new file
                if (dgList.IsCollectionNew)
                {
                    conflictsList.CreateConflictListForNewCollection(dgList);
                }
                //When modifying working dg list
                else
                {
                    conflictsList.UpdateConflictListForCurrentDgList(dgList);
                }
            });
        }

        /// <summary>
        /// Renews all unit stowage conflicts in ConflictList
        /// </summary>
        /// <param name="unit"></param>
        public static void UpdateDgWrapperStowageConfilicts(this ConflictsList conflictsList, DgWrapper unit)
        {
            conflictsList.UpdateUnitStowageConflicts(unit);
        }

        // ----- Private methods -----

        /// <summary>
        /// Checks all dg in dgList and adds stowage, segregation and SW conflicts to ConflictList
        /// </summary>
        /// <param name="dgList"></param>
        private static void CreateAllConflicts(this ConflictsList conflictsList, DgWrapperList dgList)
        {
            conflictsList.CreateStowageConflictList(dgList);
            conflictsList.CreateSegregationConflictList(dgList);
            conflictsList.CreateSwConflicts();
        }

        /// <summary>
        /// Updates existing ConflictsList with changes from DgList.
        /// </summary>
        /// <param name="dgList">Modified DgList.</param>
        /// <returns></returns>
        private static void UpdateConflictListForCurrentDgList(this ConflictsList conflictsList, DgWrapperList dgList)
        {
            //creating new conflict list to compare with the old one
            ConflictsList tempConflicts = new ConflictsList();

            //create temporary list of conflicts
            tempConflicts.CreateAllConflicts(dgList);

            //update conflict list and remove temp list
            conflictsList.UpdateConflictList(dgList, tempConflicts);
        }

        /// <summary>
        /// Creates new conflict list for new DgList.
        /// </summary>
        /// <param name="dgList">New DgList</param>
        /// <returns></returns>
        private static void CreateConflictListForNewCollection(this ConflictsList conflictsList, DgWrapperList dgList)
        {
            conflictsList.Clear();
            conflictsList.CreateAllConflicts(dgList);
            dgList.IsCollectionNew = false;
        }

        /// <summary>
        /// Updates only stowage conflicts for the selected DgWrapper.
        /// </summary>
        /// <param name="unit">DgWrapper which stowage conflicts shall be updated.</param>
        private static void UpdateUnitStowageConflicts(this ConflictsList conflictsList, DgWrapper unit)
        {
            ushort iterations = (ushort)conflictsList.Count;

            //Clear unit associated stowage conflicts
            for (ushort i = 0, n = 0; i < iterations; i++, n++)
            {
                var conflict = conflictsList[n];
                if (conflict.IsStowageConflict && conflict.DgID == unit.Model.ID)
                {
                    conflictsList.Remove(conflict);
                    n--;
                }
            }

            conflictsList.CreateStowageConflict(unit);
            conflictsList.CreateSwConflicts(unit);
        }

        /// <summary>
        /// Adds all stowage conflicts to Conflict list
        /// </summary>
        /// <param name="dgList">DgWrapperList</param>
        private static void CreateStowageConflictList(this ConflictsList conflictsList, DgWrapperList dgList)
        {
            foreach (DgWrapper dg in dgList)
                conflictsList.CreateStowageConflict(dg);
        }

        /// <summary>
        /// Adds stowage conflicts to ConflictList of a single dg
        /// </summary>
        /// <param name="dg"></param>
        private static void CreateStowageConflict(this ConflictsList conflictsList, DgWrapper dg)
        {
            if (dg.IsConflicted && dg.Conflicts.FailedStowage)
                foreach (string s in dg.Conflicts.StowageConflictsList)
                {
                    var newConflict = new ConflictPanelItemViewModel(dg, s);
                    conflictsList.AddNewConflict(newConflict);
                }
        }


        /// <summary>
        /// Adds SW conflicts to ConflictList 
        /// If dg is null, adds all the SW conflicts, otherwise adds SW conflicts of a selected unit only
        /// </summary>
        private static void CreateSwConflicts(this ConflictsList conflictsList, DgWrapper dg = null)
        {
            var specialGroups = Stowage.SWgroups;
            foreach (var unit in specialGroups.ListSW19List)
            {
                if (dg != null && unit.ID != dg.Model.ID) continue;
                var unitWrapper = new DgWrapper(unit);
                ConflictPanelItemViewModel conf = new ConflictPanelItemViewModel(unitWrapper, "SW19")
                {
                    GroupParam =
                        "SW19 For batteries transported in accordance with special provisions 376 or 377, category C, unless transported on a short international voyage. Please check cargo documents of the following units: "
                };
                conflictsList.AddNewConflict(conf);
            }
            foreach (var unit in specialGroups.ListSW22List)
            {
                if (dg != null && unit.ID != dg.Model.ID) continue;
                var unitWrapper = new DgWrapper(unit);
                ConflictPanelItemViewModel conf = new ConflictPanelItemViewModel(unitWrapper, "SW22")
                {
                    GroupParam =
                        "SW22 For WASTE AEROSOLS and WASTE GAS CARTRIDGES: category C, clear of living quarters. Please check cargo documents of the unit "
                };
                conflictsList.AddNewConflict(conf);
            }
        }

        /// <summary>
        /// Adds all segregation conflicts to Conflict list
        /// </summary>
        /// <param name="dgList">DgWrapperList</param>
        private static void CreateSegregationConflictList(this ConflictsList conflictsList, DgWrapperList dgList)
        {
            foreach (DgWrapper dg in dgList)
                if (dg.IsConflicted && dg.Conflicts.FailedSegregation)
                {
                    foreach (Conflicts.SegregationConflict c in dg.Conflicts.SegregationConflictsList)
                    {
                        var newConflict =
                            new ConflictPanelItemViewModel(dg, c.Code, true, new DgWrapper(c.DgInConflict));
                        conflictsList.AddNewConflict(newConflict);
                    }
                }
        }

        /// <summary>
        /// Removes conflicts which do not exist in ConflictList to compare, adds new conflicts from the list.
        /// </summary>
        /// <param name="dgList">DgWrapperList</param>
        /// <param name="tempConflicts">New conflict list to be compared with</param>
        private static void UpdateConflictList(this ConflictsList conflictsList, DgWrapperList dgList, ConflictsList tempConflicts)
        {
            //initializing axillary fields
            int c = 0;
            int count = conflictsList.Count;

            //Remove redundant conflicts
            for (int i = 0; i < count; i++)
            {
                var con = conflictsList[i - c];

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
                conflictsList.RemoveAt(i - c);
                c++;
            }

            //Add new conflicts
            foreach (var conf in tempConflicts)
            {
                conflictsList.AddNewConflict(conf);
            }
        }
    }
}
