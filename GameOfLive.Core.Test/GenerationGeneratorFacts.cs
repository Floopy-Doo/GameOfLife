using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace GameOfLife.Core.Test
{
    public class GenerationGeneratorFacts
    {
        [Fact]
        public void WhenBoardWithFiveCellsInACrossConnectedThenBoardWithFour()
        {
            var expectation = new Board(new[]
            {
                new Cell(-1, 0), new Cell(-1, -2), new Cell(0, -1), new Cell(-2, -1)
            });
            var board = new Board(new[]
            {
                new Cell(0, 0), new Cell(-1, -1), new Cell(-2, 0), new Cell(0, -2), new Cell(-2, -2)
            });
            var generationGenerator = new GenerationGenerator();

            var result = generationGenerator.Generate(board);

            result.Should().BeEquivalentTo(expectation);
        }

        [Fact]
        public void WhenBoardWithBlockOf9CellsConnectedThenBoardWithFour()
        {
            var expectation = new Board(new[]
            {
                new Cell(-1,1), 
                new Cell(0,0), new Cell(0,2),
                new Cell(1,-1), new Cell(1,3),
                new Cell(2,0), new Cell(2,2),
                new Cell(3,1), 
            });
            var board = new Board(new[]
            {
                new Cell(0,0), new Cell(0,1),new Cell(0,2),
                new Cell(1,0), new Cell(1,1),new Cell(1,2),
                new Cell(2,0), new Cell(2,1),new Cell(2,2),
                new Cell(12,34), 
            });
            var generationGenerator = new GenerationGenerator();

            var result = generationGenerator.Generate(board);

            result.Should().BeEquivalentTo(expectation);
        }

        [Fact]
        public void WhenBoardWithFourCellsInALineConnectedThenBoardWithTwo()
        {
            var expectation = new Board(new[]
            {
                new Cell(-2, 0), new Cell(-1, 0), new Cell(-2, 1), new Cell(-1, 1), new Cell(-2, -1), new Cell(-1, -1)
            });
            var board = new Board(new[]
            {
                new Cell(0, 0), new Cell(-1, 0), new Cell(-2, 0), new Cell(-3, 0)
            });
            var generationGenerator = new GenerationGenerator();

            var result = generationGenerator.Generate(board);

            result.Should().BeEquivalentTo(expectation);
        }

        [Fact]
        public void WhenBoardWithOneCellThenEmptyBoard()
        {
            var board = new Board(new[] {new Cell(0, 0)});
            var generationGenerator = new GenerationGenerator();

            var result = generationGenerator.Generate(board);

            result.Should().BeEquivalentTo(new Board(new Cell[0]));
        }

        [Fact]
        public void WhenBoardWithThreeCellsInALineConnectedThenBoardWithOne()
        {
            var board = new Board(new[] {new Cell(0, 0), new Cell(-1, 0), new Cell(-2, 0)});
            var generationGenerator = new GenerationGenerator();

            var result = generationGenerator.Generate(board);

            result.Should().BeEquivalentTo(new Board(new[] {new Cell(-1, 0), new Cell(-1, 1), new Cell(-1, -1)}));
        }

        [Fact]
        public void WhenBoardWithTwoCellsConnectedThenEmptyBoard()
        {
            var board = new Board(new[] {new Cell(0, 0), new Cell(-1, 0)});
            var generationGenerator = new GenerationGenerator();

            var result = generationGenerator.Generate(board);

            result.Should().BeEquivalentTo(new Board(new Cell[0]));
        }

        [Fact]
        public void WhenEmptyBoardThenNextGenerationIsEmptyNewBoard()
        {
            var board = new Board(new List<Cell>());
            var generationGenerator = new GenerationGenerator();

            var result = generationGenerator.Generate(board);

            using (new AssertionScope())
            {
                result.Should().NotBe(board, "not same board");
                result.Should().BeEquivalentTo(new Board(new List<Cell>()), "be empty board");
            }
        }
    }
}