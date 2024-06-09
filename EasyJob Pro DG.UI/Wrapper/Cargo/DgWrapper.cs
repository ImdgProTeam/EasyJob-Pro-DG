using EasyJob_ProDG.Data.Info_data;
using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Wrapper.Cargo;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.Wrapper
{
    public partial class DgWrapper : AbstractContainerWrapper<Dg>
    {

        #region Validation
        // --------------- Validation -----------------------------------------------

        /// <summary>
        /// Validation logic for properties
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(DgClass):
                    {
                        if (!DgClassValidator.IsValidDgClass(DgClass))
                        {
                            yield return "Dg class is invalid";
                        }
                        break;
                    }
                case nameof(ContainerType):
                    {
                        if (ContainerType.Length != 4)
                        {
                            yield return "Inappropriate unit type length";
                        }
                        break;
                    }
                case nameof(Unno):
                    {
                        if (Unno <= 0 || Unno > IMDGCode.MaxUnnoNumber)
                        {
                            yield return "Invalid UN number";
                        }
                        break;
                    }
                default:
                    break;
            }
        }
        #endregion


        #region Converters
        // --------------- Converters -----------------------------------------------

        /// <summary>
        /// Converts DgWrapper into a ContainerWrapper.
        /// Only ContainerNumber and Location will be saved.
        /// </summary>
        /// <returns>New ContainerWrapper instance.</returns>
        public ContainerWrapper ConvertToContainerWrapper()
        {
            Container newContainer = new Container { ContainerNumber = ContainerNumber, Location = Location };
            newContainer.DgCountInContainer++;
            return new ContainerWrapper(newContainer);
        }

        /// <summary>
        /// Converts DgWrapper to Dg (returns its model)
        /// </summary>
        /// <returns>Dg instance</returns>
        public Dg ConvertBackToDg()
        {
            return Model;
        }
        #endregion


        #region Constructors
        // --------------- Public constructors --------------------------------------

        public DgWrapper() : base(new Dg())
        {
            IsNameChanged = false;
            FlashPoint = "";
        }

        public DgWrapper(Dg model) : base(model)
        {
            IsNameChanged = false;
        }
        #endregion


        #region Override methods
        // -------------- Overriding methods and operators --------------------------

        public override string ToString()
        {
            return ContainerNumber + " in " + Location + " class " + DgClass + " (unno " + Unno + ")";
        }

        public bool Equals(DgWrapper dgWrapper)
        {
            return Model.ID == dgWrapper.Model.ID;
        }

        public override bool Equals(object obj)
        {
            if (obj is DgWrapper dgWrapper)
            {
                return this.Equals(dgWrapper);
            }
            return base.Equals(obj);
        }

        public static explicit operator Dg(DgWrapper dgWrapper)
        {
            return dgWrapper.ConvertBackToDg();
        }

        public static explicit operator ContainerWrapper(DgWrapper dgWrapper)
        {
            return dgWrapper.ConvertToContainerWrapper();
        }
        #endregion

    }
}
