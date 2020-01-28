using System.ComponentModel.DataAnnotations;

namespace GameEngine
{
    public class GameSettings
    {
        public string GameName { get; set; } = "Minesweeper";

        [Required]
        public string SaveName { get; set; } = "";
        
        [Range(10, 24, ErrorMessage = "{0} must be between {1} and {2}.")]
        public int BoardHeight { get; set; } = 10;
        
        
        [Range(10, 24, ErrorMessage = "{0} must be between {1} and {2}.")]
        public int BoardWidth { get; set; } = 10;
        
        [Range(10, 99, ErrorMessage = "{0} must be between {1} and {2}.")]
        public int NumberOfMines { get; set; } = 10;

        public bool isLoadedGame = false;

        public CellStatus[,] Board { get; set; } = new CellStatus[10, 10];
    }
}