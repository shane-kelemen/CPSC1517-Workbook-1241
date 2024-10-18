namespace MyFirstBlazorApp.Data  // Note that the namespace assigned is the ProjectName/FolderName
                                 // A using or @using directive must be used wherever the class is being used
{
    // Class used in the Lists and Slider page
    public class SelectionList
    {
        public int ValueID { get; set; }
        public string DisplayText { get; set; }
    }
}
