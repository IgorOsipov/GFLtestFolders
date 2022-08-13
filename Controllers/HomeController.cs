using GFLtestFolders.Models;
using GFLtestFolders.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GFLtestFolders.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFolderDirectoryService _folderDirectoryService;
        private readonly IConfiguration _config;

        public HomeController(
            ILogger<HomeController> logger,
            IFolderDirectoryService folderDirectoryService,
            IConfiguration config)
        {
            _logger = logger;
            _folderDirectoryService = folderDirectoryService;
            _config = config;
        }

        public async Task<IActionResult> Index(string? path)
        {
            try
            {
                string validatedPath = ValidatePathUrl(path);

                FolderDirectoryVM vm = new()
                {
                    PathUrl = validatedPath,
                    PathText = ValidatePathText(validatedPath),
                    FolderDirectories = await _folderDirectoryService.GetFolderDirectoriesAsync(validatedPath)
                };

                return View(model: vm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string ValidatePathText(string validatedPath)
        {
            if (string.IsNullOrEmpty(validatedPath))
                return "root";

            string[] splitArray = validatedPath.Split("/");

            return splitArray[^1];
        }

        private string ValidatePathUrl(string path)
        {
            if (path is null)
                return string.Empty;

            if (path.EndsWith("/") || path.EndsWith("\\"))
                return path[..^1];

            return path;
        }
    }
}