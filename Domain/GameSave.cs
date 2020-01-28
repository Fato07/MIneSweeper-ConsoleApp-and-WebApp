using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameSave
    {
        [Key] public int GameSaveId { get; set; }
        public string SaveName { get; set; } = default!;

        [Range(10, 24, ErrorMessage = "{0} must be between {1} and {2}.")]
        public int BoardHeight { get; set; }

        [Range(10, 24, ErrorMessage = "{0} must be between {1} and {2}.")]
        public int BoardWidth { get; set; }

        public int NumberOfMines { get; set; }
        public string BoardState { get; set; } = default!;
    }
}