namespace GFLtestFolders.Models
{
    public class FolderDirectory
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ParentId { get; set; }
    }
}
