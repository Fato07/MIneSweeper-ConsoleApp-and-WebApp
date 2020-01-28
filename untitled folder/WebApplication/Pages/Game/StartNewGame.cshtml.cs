using System.Collections.Generic;
using System.Security.AccessControl;
using DAL;
using GameEngine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace WebApplication.Pages.Game
{
    public class StartNewGamePageModel : PageModel
    {
        private readonly DAL.GameSaveDbContext _context;
        public string SaveFileName { get; set; }
        [BindProperty] public string Difficulty { get; set; } = default!;
        
        
        public string? NameTaken { get; set; }

        public StartNewGamePageModel(GameSaveDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string dif, string? safeFileName)
        {
            Difficulty = dif;
            NameTaken = safeFileName;

            return Page();
        }
        
        public ActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var savedGames = new List<string>();
                foreach (var gameSave in _context.GameSave)
                {
                    savedGames.Add(gameSave.SaveName);
                }
                
                if (savedGames.Contains(SaveFileName))
                {
                    return RedirectToPage( new {dif=Difficulty, SaveFileName});
                }

                GameEngine.Game gameEngine = new GameEngine.Game(_context);
                //you need to fix the functions, maybe remove them entirely.
                switch (Difficulty)
                {
                    case "1":
                        gameEngine.Settings = new GameSettings();
                       
                        break;
                    case "2":
                        gameEngine.Settings = new GameSettings()
                        {
                            BoardHeight = 16,
                            BoardWidth = 16,
                            NumberOfMines = 40
                        };
                        break;
                    case "3":
                        gameEngine.Settings = new GameSettings()
                        {
                            BoardHeight = 24,
                            BoardWidth = 24,
                            NumberOfMines = 99
                        };
                        break;
                    default:
                        gameEngine.Settings = new GameSettings();
                        break;
                }
                
                gameEngine.InitializeNewBoard(gameEngine.Settings.BoardWidth, gameEngine.Settings.BoardHeight);

                var entity = new Domain.GameSave()
                {
                    SaveName = gameEngine.Settings.SaveName,
                    BoardHeight = gameEngine.Settings.BoardHeight,
                    BoardWidth = gameEngine.Settings.BoardWidth,
                    NumberOfMines = gameEngine.Settings.NumberOfMines,
                    BoardState = gameEngine.GetSerializedGameState()
                };
                _context.GameSave.Add(entity);
                _context.SaveChangesAsync();
                return RedirectToPage("./PlayGame", new {GameId = entity.GameSaveId});
            }

            return RedirectToPage("../Index");
        }
    }
}