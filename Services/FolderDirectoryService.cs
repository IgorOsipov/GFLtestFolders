using GFLtestFolders.Data;
using GFLtestFolders.Models;
using Microsoft.EntityFrameworkCore;

namespace GFLtestFolders.Services
{
    public interface IFolderDirectoryService
    {
        Task<List<FolderDirectory>> GetFolderDirectoriesAsync(string path);
    }

    public class FolderDirectoryService : IFolderDirectoryService
    {
        private readonly DataContext _context;

        public FolderDirectoryService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<FolderDirectory>> GetFolderDirectoriesAsync(string path)
        {
            // need to iterate through each node to ensure all sub-nodes are correct
            // to avoid some wrong nodes in url by typing it manually
            int parentId = 0;
            List<FolderDirectory> result = new List<FolderDirectory>();
            string[] splitArray = path.Split("/");
            if (!string.IsNullOrWhiteSpace(path))
            {
                var directoryEntities = await _context.FolderDirectories
                    .Where(x => splitArray.Contains(x.Title))
                    .ToListAsync();

                if (directoryEntities.Count < splitArray.Length)
                    return result;

                foreach (var urlSubItem in splitArray)
                {
                    var item = directoryEntities.FirstOrDefault(x => x.Title.ToLower() == urlSubItem.ToLower() &&
                                                                     x.ParentId == parentId);

                    if (item is null) return result;
                    parentId = item.Id;
                }
            }

            result = _context.FolderDirectories
                .ToLookup(x => x.ParentId)
                .Where(x => x.Key == parentId)
                .SelectMany(x => x)
                .ToList();

            return result;
        }

    }
}
