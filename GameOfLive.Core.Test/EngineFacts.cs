using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace GameOfLife.Core.Test
{
    public class EngineFacts
    {
        public static IEnumerable<object[]> IterationData =>
            new List<object[]>
            {
                new object[] {2, 2, 4},
                new object[] {3, 3, 6},
                new object[] {4, 4, 8},
                new object[] {5, 5, 10},
                new object[] {6, 6, 12},
                new object[] {7, 7, 14},
                new object[] {8, 8, 16}
            };

        [Theory(Timeout = 2000)]
        [MemberData(nameof(IterationData))]
        public async Task ShouldDoMultipleIterationWithASecondWithSpeedOne(byte speedMultiplier,
            uint minExpectedIteration, uint maxExpectedIteration)
        {
            var initialBoard = new Board(new Cell[0]);
            var intermediat = new Board(new Cell[0]);
            var expecteBoard = new Board(new Cell[0]);

            var generationGenerator = A.Fake<IGenerationGenerator>(o => o.Strict());
            A.CallTo(() => generationGenerator.Generate(expecteBoard)).Returns(expecteBoard);
            A.CallTo(() => generationGenerator.Generate(initialBoard)).Returns(intermediat);
            A.CallTo(() => generationGenerator.Generate(intermediat)).ReturnsNextFromSequence(Enumerable
                .Repeat(intermediat, (int) minExpectedIteration - 1).Concat(new[] {expecteBoard}).ToArray());

            var initializeable = Engine.Create(generationGenerator);
            var testee = initializeable.Initialize(initialBoard);
            testee.SpeedMultiplier = speedMultiplier;

            var runningEngine = testee.Start();
            await Task.Delay(TimeSpan.FromSeconds(1.1));
            var result = await runningEngine.Stop();

            result.GenerationNumber.Should().BeInRange(minExpectedIteration, maxExpectedIteration);
            result.Generation.Should().Be(expecteBoard);
        }

        [Fact]
        public void ShouldGenerateCalculationTime()
        {
            var initialBoard = new Board(new Cell[0]);
            var expecteBoard = new Board(new Cell[0]);

            var generationGenerator = A.Fake<IGenerationGenerator>(o => o.Strict());
            A.CallTo(() => generationGenerator.Generate(initialBoard)).ReturnsNextFromSequence(expecteBoard);

            var initializeable = Engine.Create(generationGenerator);
            var testee = initializeable.Initialize(initialBoard);

            testee.Next();

            testee.CalculationTimeInMs.Should().BeGreaterThan(0);
        }

        [Fact(Timeout = 2000)]
        public async Task ShouldDoOneIterationWithASecondWithSpeedOne()
        {
            var initialBoard = new Board(new Cell[0]);
            var expecteBoard = new Board(new Cell[0]);

            var generationGenerator = A.Fake<IGenerationGenerator>(o => o.Strict());
            A.CallTo(() => generationGenerator.Generate(expecteBoard)).Returns(expecteBoard);
            A.CallTo(() => generationGenerator.Generate(initialBoard)).Returns(expecteBoard);

            var initializeable = Engine.Create(generationGenerator);
            var testee = initializeable.Initialize(initialBoard);
            testee.SpeedMultiplier = 1;

            var x = testee.Start();
            await Task.Delay(TimeSpan.FromSeconds(1.1));
            var result = await x.Stop();

            result.GenerationNumber.Should().BeInRange(1, 2);
            result.Generation.Should().Be(expecteBoard);
        }

        [Fact]
        public void ShouldStepOneGenerationForward()
        {
            var initialBoard = new Board(new Cell[0]);
            var expecteBoard = new Board(new Cell[0]);

            var generationGenerator = A.Fake<IGenerationGenerator>(o => o.Strict());
            A.CallTo(() => generationGenerator.Generate(initialBoard)).ReturnsNextFromSequence(expecteBoard);

            var initializeable = Engine.Create(generationGenerator);
            var testee = initializeable.Initialize(initialBoard);

            testee.Next();

            testee.GenerationNumber.Should().Be(1);
            testee.Generation.Should().Be(expecteBoard);
        }

        [Fact]
        public void ShouldStepTwoGenerationForward()
        {
            var initialBoard = new Board(new Cell[0]);
            var expecteBoard = new Board(new Cell[0]);

            var generationGenerator = A.Fake<IGenerationGenerator>(o => o.Strict());
            A.CallTo(() => generationGenerator.Generate(A<Board>.Ignored))
                .ReturnsNextFromSequence(A.Dummy<Board>(), expecteBoard);

            var initializeable = Engine.Create(generationGenerator);
            var testee = initializeable.Initialize(initialBoard);

            testee.Next();
            testee.Next();

            testee.GenerationNumber.Should().Be(2);
            testee.Generation.Should().Be(expecteBoard);
        }

        [Fact(Timeout = 2000)]
        public async Task ShouldNotCreateNewGenerationWhenStopped()
        {
            var initialBoard = new Board(new Cell[0]);

            var generationGenerator = A.Fake<IGenerationGenerator>(o => o.Strict());
            A.CallTo(() => generationGenerator.Generate(initialBoard)).Returns(initialBoard);
               

            var initializeable = Engine.Create(generationGenerator);
            var testee = initializeable.Initialize(initialBoard);
            testee.SpeedMultiplier = 8;

            var runningEngine = testee.Start();
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            var result = await runningEngine.Stop();
            var expectedGeneration = result.GenerationNumber;

            await Task.Delay(TimeSpan.FromSeconds(0.5));

            result.GenerationNumber.Should().Be(expectedGeneration);
        }
    }
}