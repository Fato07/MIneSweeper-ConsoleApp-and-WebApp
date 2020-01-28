using BLL;
using DAL;
using GameEngine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Game
{
    public class PlayGameModel : PageModel
    {
        private readonly DAL.GameSaveDbContext _context;
        
        public readonly GameEngine.Game GameEngine;
        public int GameId { get; set; }
        public string MoveState { get; set; }

        public string IsChecked { get; set; } = "true";
        
        
        public PlayGameModel(GameSaveDbContext context)
        {
            _context = context;
            GameEngine = new GameEngine.Game(_context);
        }

        public ActionResult OnPost(string? isChecked)
        {
            if (isChecked != null)
            {
                IsChecked = isChecked;
            }
            
            return Page();
        }
        
        public ActionResult OnGet(int? gameId, int? yIndex, int? xIndex)
        {
            if (gameId == null)
            {
                return RedirectToPage("./StartGame");
            }

            bool openCell = IsChecked == "true";

            GameId = gameId.Value;
            GameEngine.RestoreGameStateFromDb(gameId.Value);
            GameEngine.SetNumberOfMinesOnBoard();
            if (yIndex != null && xIndex != null)
            {
                MoveState = GameEngine.MoveForWebApp(yIndex.Value, xIndex.Value, openCell);
                GameEngine.SaveGame(gameId.Value);
            }
            return Page();
        }

    }
}