namespace GameOfLife.Core
{
    public interface IGenerationGenerator
    {
        Board Generate(Board board);
    }
}