using System;
using System.Collections.Generic;

namespace Minesweeper
{
    public class GameBoard
    {
        public GameCell[,] Cells;
        public int Width = 16;
        public int Height = 16;

        public GameBoard()
        {
            CreateEmptyBoard();
        }

        private void CreateEmptyBoard()
        {
            Cells = new GameCell[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Cells[x, y] = new GameCell(x, y);
                }
            }
        }

        public GameCell GetCell(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return Cells[x, y];
            }
            return null;
        }

        public void ResetBoard()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Cells[x, y].Reset();
                }
            }
        }
    }
}