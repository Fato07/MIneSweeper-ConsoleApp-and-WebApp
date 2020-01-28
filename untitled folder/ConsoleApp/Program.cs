using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using BLL;
using ConsoleUI;
using DAL;
using Domain;
using Figgle;
using GameEngine;
using MenuSystem;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ConsoleApp
{
    class Program
    {
        static ConsoleColor _defaultForeGroundColor = Console.ForegroundColor;
        private static GameSettings _settings = new GameSettings();
        private static Game _game = default!;

        static void Main(string[] args)
        {
            Console.Write(FiggleFonts.Varsity.Render("MineSweeper"));

            var dbOption = new DbContextOptionsBuilder<GameSaveDbContext>()
                .UseSqlite(@"Data Source=/Users/fathindosunmu/OneDrive/Csharp/icd0008-2019f/MineSweeper/WebApplication/GameSave.db").Options;
            var context = new GameSaveDbContext(dbOption);
            _game = new Game(context);
            
            
            

            var gameMenu = new Menu(1)
            {
                Title = "Start a new game of MineSweeper",
                MenuItemsDictionary = new Dictionary<string, MenuItem>
                {
                    {
                        "P", new MenuItem
                        {
                            Title = "Play Customized Game",
                            CommandToExecute = () => NewGame(_settings, true, context)
                        }
                    },
                    {
                        "B", new MenuItem
                        {
                            Title = "Beginner",
                            CommandToExecute = () => Beginner(context)
                        }
                    }, 
                    {
                        "I", new MenuItem 
                        {
                            Title = "Intermediate",
                            CommandToExecute = () => InterMediate(context)
                        }
                    },
                    {
                        "E", new MenuItem
                        {
                            Title = "Expert",
                            CommandToExecute = () => Expert(context)
                        }
                    }
                }
            };

            var menu0 = new Menu
            {
                Title = "MineSweeper Main menu",
                MenuItemsDictionary = new Dictionary<string, MenuItem>
                {
                    {
                        "N", new MenuItem
                        {
                            Title = "Start game",
                            CommandToExecute = gameMenu.Run
                        }
                    },
                    {
                        "L", new MenuItem
                        {
                            Title = "LoadGame",
                            CommandToExecute = () => LoadGame(context)
                        }
                    }
                }
            };

            menu0.Run();
            Console.Clear();
        }
        
        /*private static void AutoSaveGame(Game gameContent, GameSaveDbContext context)
        {
            gameContent.SaveName = UniqueAutoSaveName();
            
            foreach (var gameSave in context.GameSave)
            {
                if (gameContent.SaveName == gameSave.SaveName)
                {
                   
                }
                context.GameSave.Add(new GameSave()
                {
                    SaveName = gameContent.SaveName,
                    BoardHeight = gameContent.BoardHeight,
                    BoardWidth = gameContent.BoardWidth,
                    NumberOfMines = gameContent.NumberOfMines,
                    BoardState =  JsonConvert.SerializeObject(gameContent.gamePlayBoard)

                });
            }
        }

        private static string UniqueAutoSaveName()
        {
            var AutoSaveName = "AutoSave";
            var i  = 1;
            AutoSaveName = AutoSaveName + $"{i}";
            i++;

            return AutoSaveName;
        }*/

        private static string Beginner(GameSaveDbContext context)
        {
            GameSettings settings = new GameSettings();
            settings.BoardHeight = 10;
            settings.BoardWidth = 10;
            settings.NumberOfMines = 10;

            return NewGame(settings, false, context);
        }
        private static string InterMediate(GameSaveDbContext context)
        {
            GameSettings settings = new GameSettings();
            _settings = settings;
            
            settings.BoardHeight = 16;
            settings.BoardWidth = 16;
            settings.NumberOfMines = 40;
            
            return NewGame(settings, false, context);
        }
        private static string Expert(GameSaveDbContext context)
        {
            GameSettings settings = new GameSettings();
            _settings = settings;
            
            settings.BoardHeight = 24;
            settings.BoardWidth = 24;
            settings.NumberOfMines = 99;

            return NewGame(settings, false, context);
        }
        
        private static void SaveGameToDb(Game gameContent, GameSaveDbContext context)
        {
            bool savedSuccessfully = true;
            
            string saveName;
            do
            {
                Console.Write("Enter name of Save: ");
                saveName = Console.ReadLine()?.Trim() ?? "";
                if (saveName == "") Console.WriteLine("Enter a file Name! File Name cannot be empty");
            } while (saveName == "");

            gameContent.SaveName = saveName;
            
            foreach (var gameSave in context.GameSave)
            {
                if (gameContent.SaveName == gameSave.SaveName)
                {
                    savedSuccessfully = false;
                    
                    Console.WriteLine($" \"{gameContent.SaveName}\" already exists!");
                    Console.WriteLine(
                        "O to OverWrite Existing Game|| R to Rename Game Name|| D(␡) to delete a Game");
                    ConsoleKeyInfo command;

                    do
                    {
                        command = Console.ReadKey(true);
                        switch (command.Key)
                        {
                            case ConsoleKey.O:
                                OverWriteGame(context, gameContent);
                                break;
                            case ConsoleKey.R:
                                //RenameGame();
                                break;
                            case ConsoleKey.D:
                                DeleteGame(context, gameContent);
                                break;
                            default:
                                Console.WriteLine("Invalid Input");
                                break;
                        }
                    } while (!(command.Key == ConsoleKey.O || command.Key == ConsoleKey.R ||
                               command.Key == ConsoleKey.Delete));
                }
            }

            if (savedSuccessfully)
            {
                context.GameSave.Add(new GameSave()
                {
                    SaveName = gameContent.SaveName,
                    BoardHeight = gameContent.BoardHeight,
                    BoardWidth = gameContent.BoardWidth,
                    NumberOfMines = gameContent.NumberOfMines,
                    BoardState = JsonConvert.SerializeObject(gameContent.Board)
                });

                context.SaveChanges();
            }
        }

        private static void OverWriteGame (GameSaveDbContext context, Game gameContent)
        {
            var overWriitenGame = new GameSave();
            {
                overWriitenGame.SaveName = gameContent.SaveName;
                overWriitenGame.BoardHeight = gameContent.BoardHeight;
                overWriitenGame.BoardWidth = gameContent.BoardWidth;
                overWriitenGame.NumberOfMines = gameContent.NumberOfMines;
                overWriitenGame.BoardState = JsonConvert.SerializeObject(gameContent.Board);

            }
            context.GameSave.Update(overWriitenGame);
            context.SaveChanges();
        }
        
        private static void RenameGame (GameSaveDbContext context, Game gameContent)
        {
            
        }

        private static void DeleteGame(GameSaveDbContext context, Game gameContent)
        {
            foreach (var gameSave in context.GameSave)
            {
                Console.WriteLine($"{gameSave.GameSaveId} {gameSave.SaveName}");
            }

            bool stateOfValue;
            int userInput;
            do
            {
                Console.Write("Select Game Number: ");
                string input2 = Console.ReadLine();
                stateOfValue = int.TryParse(input2, out userInput);

                if (!stateOfValue) Console.WriteLine("Error: " + input2 + " is not a valid number");
            } while (!stateOfValue);


            GameSave gameDelete = new GameSave();
            {
                gameDelete.GameSaveId = userInput;
            }
            
            context.GameSave.Remove(gameDelete);
            context.SaveChanges();
        }

        private static string LoadGame(GameSaveDbContext context)
        {
            _settings.isLoadedGame = true;
            
            Console.WriteLine("Select game to load");
            Console.WriteLine("=====================");

            foreach (var gameSave in context.GameSave)
            {
                Console.WriteLine($"{gameSave.GameSaveId} {gameSave.SaveName}");
            }

            bool stateOfValue;
            int userInput;
            do
            {
                Console.Write("Select Game Number: ");
                string input2 = Console.ReadLine();
                stateOfValue = int.TryParse(input2, out userInput);

                if (!stateOfValue) Console.WriteLine("Error: " + input2 + " is not a valid number");
            } while (!stateOfValue);

            foreach (var gameSave in context.GameSave)
            {
                if (gameSave.GameSaveId == userInput)
                {
                    _settings.BoardHeight = gameSave.BoardHeight;
                    _settings.BoardWidth = gameSave.BoardWidth;
                    _settings.NumberOfMines = gameSave.NumberOfMines;
                    _settings.Board = JsonConvert.DeserializeObject<CellStatus[,]>(gameSave.BoardState);


                    NewGame(_settings, false, context);
                }
            }

            return "";
        }
        
        private static string NewGame(GameSettings settings, bool needInput, GameSaveDbContext context)
        {
            if (needInput == true)
            {
                _game.Settings.BoardHeight = GetBardDimensions.GetSizeofBoardHeight();
                _game.Settings.BoardWidth = GetBardDimensions.GetSizeOfBoardWidth();
                _game.Settings.NumberOfMines = GetBardDimensions.GetNumberOfMines();
            }
            
            string moveState;

            Console.Clear();
            do
            {
                GameUi.PrintBoard(_game);

                if (Console.ForegroundColor != _defaultForeGroundColor)
                {
                    Console.ForegroundColor = _defaultForeGroundColor;
                }


                Console.WriteLine($"Enter X and Y coordinate (e.g \"A,G\") ");


                int xIndexOnBoard;
                string actualUserInputXindex;
                do
                {
                    var xCoordinateUserInput = CheckUserInput();
                    actualUserInputXindex = xCoordinateUserInput.ToString().ToUpper();
                    xIndexOnBoard = char.ToUpper(xCoordinateUserInput) - 64;

                    if (xIndexOnBoard >  _game.Settings.BoardWidth) Console.WriteLine("Input is out Of bound");
                } while (xIndexOnBoard >  _game.Settings.BoardWidth);


                int yIndexOnBoard;
                string actualUserInputYindex;
                do
                {
                    var yCoordinateUserInput = CheckUserInput();
                    actualUserInputYindex = yCoordinateUserInput.ToString().ToUpper();
                    yIndexOnBoard = char.ToUpper(yCoordinateUserInput) - 64;

                    if (yIndexOnBoard >  _game.Settings.BoardHeight) Console.WriteLine("Input is out Of bound");
                } while (yIndexOnBoard >  _game.Settings.BoardHeight);

                Console.WriteLine($"X:{actualUserInputXindex} Y:{actualUserInputYindex}");

                Console.WriteLine("SpaceBar to Flag|  Enter(⏎) to open|| S to save||esc to quit");
                ConsoleKeyInfo command;
                do
                {
                    command = Console.ReadKey(true);
                    if (command.Key == ConsoleKey.S)
                    {
                        SaveGameToDb(_game, context);
                    }

                    if (command.Key == ConsoleKey.Escape)
                    {
                        return "";
                    }
                } while (!(command.Key == ConsoleKey.Enter || command.Key == ConsoleKey.Spacebar ||
                           command.Key == ConsoleKey.S || command.Key == ConsoleKey.Escape));

                moveState = _game.Move(Math.Abs(yIndexOnBoard), Math.Abs(xIndexOnBoard), command.Key);
                //AutoSaveGame(game, context);
            } while (moveState != "GameOver");

            GameUi.PrintBoard(_game);

            if (moveState == "GameOver")
            {
                Console.WriteLine("=============");
                Console.WriteLine("GAME OVER");
                Console.WriteLine("=============");
            }


            if (Console.ForegroundColor != _defaultForeGroundColor)
            {
                Console.ForegroundColor = _defaultForeGroundColor;
            }

            Console.WriteLine("(Press any key to continue)");
            Console.ReadKey(true);
            Console.Clear();

            return "";
        }

        private static char CheckUserInput()
        {
            bool isUserInputLetter;
            char indexInput;

            do
            {
                indexInput = Console.ReadKey(true).KeyChar;

                isUserInputLetter = char.IsLetter(indexInput);

                if (isUserInputLetter == false) Console.WriteLine("Enter Valid Alphabet Index");
            } while (isUserInputLetter == false);

            return indexInput;
        }
    }
}