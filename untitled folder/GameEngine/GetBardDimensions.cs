using System;

namespace GameEngine
{
    public class GetBardDimensions
    {
        public static int GetSizeofBoardHeight()
        {
            bool stateOfValue;
            int boardHeight;
            do
            {
                Console.Write("Enter Board Height: ");
                string input1 = Console.ReadLine();
                stateOfValue = int.TryParse(input1, out boardHeight);

                if (!stateOfValue) Console.WriteLine("Error: " + input1 + " is not a valid number");
                else if (boardHeight < ConstantSize._minimumBoardHeight)
                {
                    Console.WriteLine($"minimum height is {ConstantSize._minimumBoardHeight}");
                }
                else if (boardHeight > ConstantSize._maximumBoardHeight)
                {
                    Console.WriteLine($"maximum height is {ConstantSize._maximumBoardHeight}");
                }

            } while (!stateOfValue || boardHeight < ConstantSize._minimumBoardHeight ||
                     boardHeight > ConstantSize._maximumBoardHeight);

            return Math.Abs(boardHeight) + 1;
        }

        public static int GetSizeOfBoardWidth()
        {
            bool stateOfValue;
            int boardWidth;
            do
            {
                Console.Write("Enter Board Width: ");
                string input2 = Console.ReadLine();
                stateOfValue = int.TryParse(input2, out boardWidth);

                if (!stateOfValue) Console.WriteLine("Error: " + input2 + " is not a valid number");
                else if (boardWidth < ConstantSize._minimumBoardWidth)
                    Console.WriteLine($"minimum width is {ConstantSize._minimumBoardWidth}");
                else if (boardWidth > ConstantSize._maximumBoardWidth)
                    Console.WriteLine($"maximum width is {ConstantSize._maximumBoardWidth}");
            } while (!stateOfValue || boardWidth < ConstantSize._minimumBoardHeight ||
                     boardWidth > ConstantSize._maximumBoardHeight);

            return Math.Abs(boardWidth) + 1;
        }

        public static int GetNumberOfMines()
        {
            bool stateOfValue;
            int mineAmount;
            do
            {
                Console.Write("Enter Number Of mines: ");
                string input2 = Console.ReadLine();
                stateOfValue = int.TryParse(input2, out mineAmount);

                if (!stateOfValue) Console.WriteLine("Error: " + input2 + " is not a valid number");
            } while (!stateOfValue);

            return Math.Abs(mineAmount);
        }
    }
}