using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BLL;
using DAL;
using GameEngine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Game
{
    public class StartGameCustomModel : PageModel
    {
        private readonly DAL.GameSaveDbContext _context;

        public StartGameCustomModel(GameSaveDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public GameOptions Options { get; set; } = new GameOptions();
        
        public async Task<IActionResult> Onpost()
        {
            if (ModelState.IsValid)
            {
               GameEngine.Game gameEngine = new GameEngine.Game(_context);
               
               gameEngine.Settings = new GameSettings();
                
                gameEngine.InitializeNewBoard(Options.BoardWidth, Options.BoardHeight);
                
                var entity = new Domain.GameSave()
                {
                    SaveName = Options.SaveName,
                    BoardHeight = Options.BoardHeight,
                    BoardWidth = Options.BoardWidth,
                    NumberOfMines = Options.NumberOfMines,
                    BoardState = gameEngine.GetSerializedGameState()
                };

                _context.GameSave.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToPage("./PlayGame", new{GameId = entity.GameSaveId});
            }

            return Page();
        }
    }

    public class GameOptions
    {
        [Required]
        public string SaveName { get; set; }

        [Range(10, 24, ErrorMessage = "{0} must be between {1} and {2}.")]
        public int BoardHeight { get; set; } = 10;
        
        [Range(10, 24, ErrorMessage = "{0} must be between {1} and {2}.")]
        public int BoardWidth { get; set; } = 10;
        
        [Range(10, 99, ErrorMessage = "{0} must be between {1} and {2}.")]
        public int NumberOfMines { get; set; } = 10;
    }
}