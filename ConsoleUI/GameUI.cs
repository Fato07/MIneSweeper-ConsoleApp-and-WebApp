using System;
using System.ComponentModel;
using GameEngine;

namespace ConsoleUI
{
    public static class GameUi
    {
        private static readonly string _verticalSeparator = "|";
        private static readonly string _horizontalSeparator = "-";
        private static readonly string _centerSeparator = "+";
        public static string _WhiteSpace = "  ";

        public static void PrintBoard(Game game)
        {
            CellStatus[,] board = game.GetBoard();

            PrintHorizontalBoarders(game, true);
            for (int yIndex = 0; yIndex < game.BoardHeight; yIndex++)
            {
                var line = _verticalSeparator + "";
                for (int xIndex = 0; xIndex < game.BoardWidth; xIndex++)
                {
                    line = line + _WhiteSpace + GetSingleState(board[yIndex, xIndex]) + " ";

                    if (xIndex < game.BoardWidth - 1)
                    {
                        line = line + _verticalSeparator;
                    }
                }

                Console.WriteLine($"{Alphabet(yIndex)} " + line + _verticalSeparator);

                if (yIndex < game.BoardHeight - 1)
                {
                    line = "";
                    for (int xIndex = 0; xIndex < game.BoardWidth; xIndex++)
                    {
                        line = line + _horizontalSeparator + _horizontalSeparator + _horizontalSeparator +
                               _horizontalSeparator;
                        if (xIndex < game.BoardWidth - 1)
                        {
                            line += _centerSeparator;
                        }
                    }

                    Console.WriteLine(_WhiteSpace + _centerSeparator + line + _centerSeparator);
                }
            }

            PrintHorizontalBoarders(game, false);
        }

        private static void PrintHorizontalBoarders(Game game, bool topHoprizontalBoarder)
        {
            for (int yIndex = 0; yIndex < game.BoardHeight; yIndex++)
            {
                if (yIndex == game.BoardHeight - 1)
                {
                    var line1 = "";
                    for (int xIndex1 = 0; xIndex1 < game.BoardWidth; xIndex1++)
                    {
                        line1 = line1 + _horizontalSeparator + _horizontalSeparator + _horizontalSeparator +
                                _horizontalSeparator;
                        if (xIndex1 < game.BoardWidth - 1)
                        {
                            line1 += _centerSeparator;
                        }
                    }

                    LabelBoardIndexes(game, topHoprizontalBoarder);
                    Console.WriteLine(_WhiteSpace + _centerSeparator + line1 + _centerSeparator);
                }
            }
        }

        private static void LabelBoardIndexes(Game game, bool topHoprizontalBoarder)
        {
            if (topHoprizontalBoarder == true)
            {
                for (int j = 0; j < game.BoardWidth; j++)
                {
                    Console.Write("    " + $"{Alphabet(j)}");
                }
            }
            else
            {
                return;
            }

            Console.WriteLine();
        }

        private static char Alphabet(int i)
        {
            char[] alphabet = new[]
            {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z'
            };

            return alphabet[i];
        }

        private static string GetSingleState(CellStatus status)
        {
            switch (status)
            {
                case CellStatus.ClosedMine:
                    return " ";
                case CellStatus.FlaggedMine:
                    return "F";
                case CellStatus.OpenMine:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    return "*";
                case CellStatus.ClosedAndNotAMine:
                    return " ";
                case CellStatus.OpenedAndNotAMine:
                    return "1";
                case CellStatus.FlaggedAndNotMine:
                    return "F";
                default:
                    throw new InvalidEnumArgumentException("Unknown enum option!");
            }
        }
    }
}