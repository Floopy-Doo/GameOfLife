using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace GameOfLife.Core.Test
{
    public class BoardFacts
    {
        public static IEnumerable<object[]> NeighboursCellData => new List<object[]>
        {
            new object[]
            {
                new[] {new Cell(0, 0), new Cell(0, 1)},
                new Cell(0, 0),
                new[] {new Cell(0, 1)}
            },
            new object[]
            {
                new[] {new Cell(0, 0), new Cell(0, 1), new Cell(0, 2)},
                new Cell(0, 0),
                new[] {new Cell(0, 1)}
            },
            new object[]
            {
                new[]
                {
                    new Cell(-1, -1), new Cell(-1, 0), new Cell(-1, 1),
                    new Cell(0, -1), new Cell(0, 0), new Cell(0, 1),
                    new Cell(1, -1), new Cell(1, 0), new Cell(1, 1),
                    new Cell(2, -1), new Cell(2, 0), new Cell(2, 1)
                },
                new Cell(0, 0),
                new[]
                {
                    new Cell(-1, -1), new Cell(-1, 0), new Cell(-1, 1),
                    new Cell(0, -1), new Cell(0, 1),
                    new Cell(1, -1), new Cell(1, 0), new Cell(1, 1)
                }
            }
        };

        public static IEnumerable<object[]> EmptyNeighboursCellData => new List<object[]>
        {
            new object[]
            {
                new Cell(0, 0),
                new[]
                {
                    new Cell(1, -1), new Cell(1, 0) , new Cell(1, 1),
                    new Cell(0, -1), new Cell(0, 1),
                    new Cell(-1, -1), new Cell(-1, 0) , new Cell(-1, 1),
                }
            },
            new object[]
            {
                new Cell(15, 3412),
                new[]
                {
                    new Cell(14, 3411), new Cell(14, 3412),new Cell(14, 3413),
                    new Cell(15, 3411), new Cell(15, 3413),
                    new Cell(16, 3411), new Cell(16, 3412),new Cell(16, 3413),
                }
            }
        };

        [Theory]
        [MemberData(nameof(NeighboursCellData))]
        public void WhenLookingForNeighboursAndThereAreCellsThenGetNeighebours(IEnumerable<Cell> cells, Cell cell,
            IEnumerable<Cell> expectedResult)
        {
            var board = new Board(cells.ToList());

            var result = board.GetNeigeboursOfCell(cell);

            result.Should().Equal(expectedResult);
        }

        [Theory]
        [MemberData(nameof(EmptyNeighboursCellData))]
        public void WhenLookingEmptyCellNeighbours(Cell cell, IEnumerable<Cell> expectedResult)
        {
            var board = new Board(new Cell[0]);

            var result = board.GetEmptyNigebhoursOfCells(cell);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void WhenLookingForNeighboursAndThereAreNoCellsAliveThenGetEmptyList()
        {
            var board = new Board(new Cell[0]);

            var result = board.GetNeigeboursOfCell(new Cell(0, 0));

            result.Should().BeEmpty();
        }
    }
}