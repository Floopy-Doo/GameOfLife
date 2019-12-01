using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.Core
{
    public class Board
    {
        public Board(IReadOnlyCollection<Cell> aliveCells)
        {
            AliveCells = aliveCells;
        }

        public IReadOnlyCollection<Cell> AliveCells { get; }

        public IEnumerable<Cell> GetNeigeboursOfCell(Cell cell)
        {
            return AliveCells
                .Except(new[] {cell})
                .Where(x => Math.Abs(x.X - cell.X) <= 1 && Math.Abs(x.Y - cell.Y) <= 1);
        }

        public IEnumerable<Cell> GetEmptyNigebhoursOfCells(Cell cell)
        {
            var initialX = cell.X - 1;
            var initialY = cell.Y - 1;

            return Enumerable.Range(0,9).Select(i => new Cell(initialX + (i / 3), initialY + (i % 3))).Except(new []{cell});
        }
    }
}