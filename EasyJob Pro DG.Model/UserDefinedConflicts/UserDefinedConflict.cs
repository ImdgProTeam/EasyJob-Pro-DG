namespace EasyJob_ProDG.Model.UserDefinedConflicts
{
    public class UserDefinedConflict
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsMultiUnitsConflict { get; set; }

        public UserDefinedConflict()
        {
            
        }

        public UserDefinedConflict(string title, string description, bool isMultiUnitsConflict = false)
        {
            Title = title;
            Description = description;
            IsMultiUnitsConflict = isMultiUnitsConflict;
        }
    }
}
