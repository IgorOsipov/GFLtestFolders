namespace GFLtestFolders.Models
{
    public class FolderDirectoryVM
    {
        public string PathUrl { get; set; } = string.Empty;
        public string PathText { get; set; }
        public IEnumerable<FolderDirectory> FolderDirectories { get; set; } = Enumerable.Empty<FolderDirectory>();
    }
}