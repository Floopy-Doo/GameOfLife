namespace GameOfLife.Core
{
    public struct Cell
    {
        public Cell(long x, long y)
        {
            X = x;
            Y = y;
        }

        public long X { get; }

        public long Y { get; }
    }
}