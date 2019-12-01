using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.Core
{
    public class GenerationGenerator : IGenerationGenerator
    {
        public Board Generate(Board board)
        {
            var activeCellsWithNeigbours = GetStillAliveCellsForNextGeneration(board);
            var newbornCellsForNextGeneration = GetNewbornCellsForNextGeneration(board);


            return new Board(activeCellsWithNeigbours.Concat(newbornCellsForNextGeneration).ToList());
        }

        private IEnumerable<Cell> GetNewbornCellsForNextGeneration(Board board)
        {
            var possibleNewbornCells = board.AliveCells
                .SelectMany(board.GetEmptyNigebhoursOfCells)
                .Except(board.AliveCells)
                .Distinct();

            var newboardCells = possibleNewbornCells
                .Select(x => (cell: x, neighbours: board.GetNeigeboursOfCell(x)))
                .Where(x => x.neighbours.Count() == 3)
                .Select(x => x.cell);

            return newboardCells;
        }

        private static IEnumerable<Cell> GetStillAliveCellsForNextGeneration(Board board)
        {
            return board
                .AliveCells
                .Select(x => (cell: x, neighbours: board.GetNeigeboursOfCell(x)))
                .Where(x => x.neighbours.Count() == 3 || x.neighbours.Count() == 2)
                .Select(x => x.cell);
        }
    }
}