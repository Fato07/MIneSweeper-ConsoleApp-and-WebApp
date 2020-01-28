using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL;
using Domain;
using Newtonsoft.Json;

namespace GameEngine
{
    public static class ConstantSize
    {
        public static int _maximumBoardHeight = 24;
        public static int _maximumBoardWidth = 24;
        public static int _minimumBoardHeight = 10;
        public static int _minimumBoardWidth = 10;
    }

    public class Game
    {
        private readonly DAL.GameSaveDbContext _context;
        public string SaveName { get; set; } = default!;
        public CellStatus[,] Board { get; set; }
        public int BoardWidth { get; }
        public int BoardHeight { get; }
        public int NumberOfMines { get; }
        
        public GameSettings Settings { get; set; }


        public Game(GameSaveDbContext context)
        {
            Settings = new GameSettings();
            if (Settings.BoardHeight < ConstantSize._minimumBoardHeight ||
                Settings.BoardWidth < ConstantSize._minimumBoardWidth)
            {
                throw new ArgumentException(
                    $"Board size has to be at least {ConstantSize._minimumBoardHeight} x {ConstantSize._minimumBoardWidth} ");
            }

            if (Settings.BoardHeight > ConstantSize._maximumBoardHeight ||
                Settings.BoardWidth > ConstantSize._maximumBoardWidth )
            {
                throw new ArgumentException(
                    $"Board size cannot be greater than {ConstantSize._maximumBoardHeight} x {ConstantSize._maximumBoardWidth} ");
            }

            _context = context;
            BoardHeight = Settings.BoardHeight;
            BoardWidth = Settings.BoardWidth;
            NumberOfMines = Settings.NumberOfMines;

            if (Settings.isLoadedGame == true)
            {
                Board = Settings.Board;
            }
            else if (Settings.isLoadedGame == false)
            {
                InitializeNewBoard(BoardWidth, BoardHeight);
            }

            SetNumberOfMinesOnBoard();
        }
        
        public void InitializeNewBoard(int boardWidth, int boardHeight)
        {
            Board = new CellStatus[boardWidth, boardHeight];
        }
        
        public CellStatus[,] GetBoard()
        {
            var result = new CellStatus[BoardHeight, BoardWidth];
            Array.Copy(Board, result, Board.Length);
            return result;
        }

        public void SetNumberOfMinesOnBoard()
        {
            Random minePosition = new Random();

            for (int placed = 0; placed < NumberOfMines; placed++)
            {
                var minePositionYaxis = minePosition.Next(BoardHeight);
                var minePositionXaxis = minePosition.Next(BoardWidth);
                Board[minePositionYaxis, minePositionXaxis] = CellStatus.ClosedMine;
            }
        }

        public string MoveForWebApp(int positionY, int positionX, bool openCell)
        {
            if (openCell == true)
            {
                switch (Board[positionY, positionX])
                {
                    case CellStatus.ClosedAndNotAMine:
                        Board[positionY, positionX] = CellStatus.OpenedAndNotAMine;
                        return "";
                    case CellStatus.ClosedMine:
                        ShowBombs();
                        return "GameOver";
                    case CellStatus.OpenedAndNotAMine:
                        return "";
                    case CellStatus.FlaggedMine:
                        return "";
                    case CellStatus.FlaggedAndNotMine:
                        return "";
                }
            }
            else
            {
                switch (Board[positionY, positionX])
                {
                    case CellStatus.ClosedMine:
                        Board[positionY, positionX] = CellStatus.FlaggedMine;
                        return "";
                    case CellStatus.ClosedAndNotAMine:
                        Board[positionY, positionX] = CellStatus.FlaggedAndNotMine;
                        return "";
                    case CellStatus.FlaggedMine:
                        Board[positionY, positionX] = CellStatus.ClosedAndNotAMine;
                        return "";
                    case CellStatus.FlaggedAndNotMine:
                        Board[positionY, positionX] = CellStatus.ClosedAndNotAMine;
                        return "";
                    case CellStatus.OpenedAndNotAMine:
                        return "";
                }
            }

            return "";
        }
        
        public string Move(int positionY, int positionX, ConsoleKey command)
        {
            if (command == ConsoleKey.Enter)
            {
                switch (Board[positionY, positionX])
                {
                    case CellStatus.ClosedAndNotAMine:
                        Board[positionY, positionX] = CellStatus.OpenedAndNotAMine;
                        return "";
                    case CellStatus.ClosedMine:
                        ShowBombs();
                        return "GameOver";
                    case CellStatus.OpenedAndNotAMine:
                        return "";
                    case CellStatus.FlaggedMine:
                        return "";
                    case CellStatus.FlaggedAndNotMine:
                        return "";
                }
            }
            else if (command == ConsoleKey.Spacebar)
            {
                switch (Board[positionY, positionX])
                {
                    case CellStatus.ClosedMine:
                        Board[positionY, positionX] = CellStatus.FlaggedMine;
                        return "";
                    case CellStatus.ClosedAndNotAMine:
                        Board[positionY, positionX] = CellStatus.FlaggedAndNotMine;
                        return "";
                    case CellStatus.FlaggedMine:
                        Board[positionY, positionX] = CellStatus.ClosedAndNotAMine;
                        return "";
                    case CellStatus.FlaggedAndNotMine:
                        Board[positionY, positionX] = CellStatus.ClosedAndNotAMine;
                        return "";
                    case CellStatus.OpenedAndNotAMine:
                        return "";
                    default:
                        throw new InvalidEnumArgumentException("Unknown enum option!");
                }
            }

            return "";
        }

        private void ShowBombs()
        {
            for (int xIndex = 0; xIndex < BoardHeight; xIndex++)
            {
                for (int yIndex = 0; yIndex < BoardWidth; yIndex++)
                {
                    if (Board[xIndex, yIndex] == CellStatus.ClosedMine)
                    {
                        Board[xIndex, yIndex] = CellStatus.OpenMine;
                    }
                }
            }
        }
        
        public string GetSerializedGameState()
        {
            return JsonConvert.SerializeObject(Board);
        }

        public void RestoreGameStateFromDb(int gameId)
        {
            var state = _context.GameSave.First(a => a.GameSaveId == gameId);
            Board = JsonConvert.DeserializeObject<CellStatus[,]>(state.BoardState);

            var settings = new GameSettings
            {
                BoardHeight = state.BoardHeight,
                BoardWidth = state.BoardWidth,
                NumberOfMines = state.NumberOfMines
            };
            SaveName = state.SaveName;
        }
        public CellStatus CellValue(in int yIndex, in int xIndex)
        {
            return Board[yIndex, xIndex];
        }
        
        public async Task SaveGame(int gameId)
        {
            var entity = _context.GameSave.First(a => a.GameSaveId == gameId);
            entity.BoardState = GetSerializedGameState();
            _context.GameSave.Update(entity);
            await _context.SaveChangesAsync();
        }
        
        public GameSettings SettingsBeginner()
        { 
            return new GameSettings();
        }
        public GameSettings SettingsIntermediate()
        {
            GameSettings gameSettings = new GameSettings
            {
                BoardHeight = 16,
                BoardWidth = 16,
                NumberOfMines = 40
            };

            return gameSettings;
        }
        public GameSettings SettingsExpert()
        {
            GameSettings gameSettings = new GameSettings
            {
                BoardHeight = 24,
                BoardWidth = 24,
                NumberOfMines = 99
            };
            return gameSettings;
        }

        public int SaveGameToDb(GameSettings gameSettings)
        {
            var entity = new GameSave()
            {
                SaveName = gameSettings.SaveName,
                BoardHeight = gameSettings.BoardHeight,
                BoardWidth = gameSettings.BoardWidth,
                NumberOfMines = gameSettings.NumberOfMines,
                BoardState = JsonConvert.SerializeObject(Board)
            };
            _context.GameSave.Add(entity);
            _context.SaveChangesAsync();
            return entity.GameSaveId;
        }
    }
}