using System;
using System.Collections.Generic;

namespace Saper
{
    public class MinesweeperGame
    {
        public GameBoard Board;

        public int GameStatus = 0;
        public int MinesLeft = 40;
        public int GameTime = 0;
        public int TotalMines = 40;

        private bool firstMove = true;
        private Random random = new Random();

        public MinesweeperGame()
        {
            Board = new GameBoard();
            MinesLeft = TotalMines;
            GameStatus = 0;
        }

        public void NewGame()
        {
            Board.ResetBoard();
            MinesLeft = TotalMines;
            GameTime = 0;
            GameStatus = 0;
            firstMove = true;
        }

        public bool MakeMove(int x, int y)
        {
            GameCell cell = Board.GetCell(x, y);
            if (cell == null || cell.IsOpened() || cell.IsFlagged() || cell.HasQuestion())
                return false;

            if (firstMove)
            {
                GenerateMines(x, y);
                CalculateNumbers();
                firstMove = false;
                GameStatus = 1;
            }

            if (cell.HasMine)
            {
                cell.Open();
                GameStatus = 2;
                return true;
            }

            if (cell.MinesAround == 0)
            {
                OpenArea(x, y);
            }
            else
            {
                cell.Open();
            }

            CheckWin();

            return true;
        }


        public void ToggleFlag(int x, int y)
        {
            GameCell cell = Board.GetCell(x, y);
            if (cell != null && !cell.IsOpened())
            {
                bool wasFlagged = cell.IsFlagged();
                cell.ToggleMark();

                if (wasFlagged)
                {
                    MinesLeft++;
                }
                else if (cell.IsFlagged())
                {
                    MinesLeft--;
                }
            }
        }

        private void GenerateMines(int safeX, int safeY)
        {
            int minesPlaced = 0;

            while (minesPlaced < TotalMines)
            {
                int x = random.Next(Board.Width);
                int y = random.Next(Board.Height);

                if ((x == safeX && y == safeY) || Board.Cells[x, y].HasMine)
                    continue;

                Board.Cells[x, y].HasMine = true;
                minesPlaced++;
            }
        }

        private void CalculateNumbers()
        {
            for (int x = 0; x < Board.Width; x++)
            {
                for (int y = 0; y < Board.Height; y++)
                {
                    if (!Board.Cells[x, y].HasMine)
                    {
                        Board.Cells[x, y].MinesAround = CountMinesAround(x, y);
                    }
                }
            }
        }

        private int CountMinesAround(int cellX, int cellY)
        {
            int count = 0;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int x = cellX + dx;
                    int y = cellY + dy;

                    if (x >= 0 && x < Board.Width &&
                        y >= 0 && y < Board.Height &&
                        Board.Cells[x, y].HasMine)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private void OpenArea(int startX, int startY)
        {
            Queue<(int, int)> queue = new Queue<(int, int)>();
            HashSet<(int, int)> visited = new HashSet<(int, int)>();

            queue.Enqueue((startX, startY));
            visited.Add((startX, startY));

            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();
                GameCell cell = Board.GetCell(x, y);

                if (cell == null || cell.IsFlagged())
                    continue;

                if (cell.HasMine)
                    continue;

                if (!cell.IsOpened())
                    cell.Open();

                if (cell.MinesAround == 0)
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (dx == 0 && dy == 0) continue;

                            int nx = x + dx;
                            int ny = y + dy;

                            if (nx >= 0 && nx < Board.Width &&
                                ny >= 0 && ny < Board.Height)
                            {
                                var neighborPos = (nx, ny);

                                if (!visited.Contains(neighborPos))
                                {
                                    visited.Add(neighborPos);
                                    queue.Enqueue(neighborPos);
                                }
                            }
                        }
                    }
                }
            }
        }


        private void CheckWin()
        {
            for (int x = 0; x < Board.Width; x++)
            {
                for (int y = 0; y < Board.Height; y++)
                {
                    GameCell cell = Board.Cells[x, y];
                    if (!cell.IsOpened() && !cell.HasMine)
                    {
                        return;
                    }
                }
            }

            GameStatus = 3;
        }

        public void UpdateTimer()
        {
            if (GameStatus == 1)
            {
                GameTime++;
            }
        }
    }
}