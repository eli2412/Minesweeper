using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilestoneCST_250
{

    public class Board
    {
        public int Size { get; }
        public Cell[,] Grid { get; }
        public double Difficulty { get; set; }

        public Board(int size)
        {
            Size = size;
            Grid = new Cell[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Grid[i, j] = new Cell(i, j);
                }
            }
        }

        public void SetupLiveNeighbors()
        {
            int numberOfBombs = (int)(Size * Size * Difficulty);
            Random rand = new Random();
            while (numberOfBombs > 0)
            {
                int x = rand.Next(Size);
                int y = rand.Next(Size);
                if (!Grid[x, y].Live)
                {
                    Grid[x, y].Live = true;
                    numberOfBombs--;
                }
            }
        }

            public void CalculateLiveNeighbors()
        {
            int[] directions = { -1, 0, 1 };
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Grid[i, j].Live)
                    {
                        Grid[i, j].LiveNeighbors = 9;
                        continue;
                    }

                    int count = 0;
                    foreach (var x in directions)
                    {
                        foreach (var y in directions)
                        {
                            if (x == 0 && y == 0)
                                continue;

                            int newX = i + x;
                            int newY = j + y;

                            if (newX >= 0 && newX < Size && newY >= 0 && newY < Size && Grid[newX, newY].Live)
                            {
                                count++;
                            }
                        }
                    }
                    Grid[i, j].LiveNeighbors = count;
                }
            }
        }
        public void FloodFill(int row, int col)
        {
            // Base case, check for out-of-bounds
            if (row < 0 || row >= Size || col < 0 || col >= Size)
            {
                return;
            }

            Cell cell = Grid[row, col];

            // If the cell has been visited already or is live, return
            if (cell.Visited || cell.Live)
            {
                return;
            }

            // Mark the current cell as visited
            cell.Visited = true;

            // If the current cell has no live neighbors, recursively explore all neighbors
            if (cell.LiveNeighbors == 0)
            {
                FloodFill(row - 1, col - 1);
                FloodFill(row - 1, col);
                FloodFill(row - 1, col + 1);
                FloodFill(row, col - 1);
                FloodFill(row, col + 1);
                FloodFill(row + 1, col - 1);
                FloodFill(row + 1, col);
                FloodFill(row + 1, col + 1);
            }
        }
    }
}
