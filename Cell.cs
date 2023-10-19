public class Cell
{
    public int Row { get; set; } = -1;
    public int Col { get; set; } = -1;
    public bool Visited { get; set; } = false;
    public bool Live { get; set; } = false;
    public int LiveNeighbors { get; set; } = 0;

    public Cell(int row, int col)
    {
        Row = row;
        Col = col;
    }
}
