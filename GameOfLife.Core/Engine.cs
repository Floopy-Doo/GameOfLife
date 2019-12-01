using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace GameOfLife.Core
{
    public class Engine : IInitializeableEngine, IStoppedEngine, IRunningEngine, INotifyPropertyChanged
    {
        private readonly IGenerationGenerator generator;
        private double calculationTimeInMs;
        private CancellationTokenSource caneclationTokenSource;
        private Board generation;
        private uint generationNumber;
        private Task runningTask;
        private byte speed = 1;

        private Engine(IGenerationGenerator generator)
        {
            this.generator = generator;
        }

        public byte SpeedMultiplier
        {
            get => speed;
            set
            {
                speed = value;
                OnPropertyChanged();
            }
        }

        public uint GenerationNumber
        {
            get => generationNumber;
            set
            {
                generationNumber = value;
                OnPropertyChanged();
            }
        }

        public double CalculationTimeInMs
        {
            get => calculationTimeInMs;
            set
            {
                calculationTimeInMs = value;
                OnPropertyChanged();
            }
        }

        public Board Generation
        {
            get => generation;
            set
            {
                generation = value;
                OnPropertyChanged();
            }
        }

        public IStoppedEngine Initialize(Board board)
        {
            Generation = board;
            GenerationNumber = 1;
            return this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task<IStoppedEngine> Stop()
        {
            caneclationTokenSource.Cancel();
            await (runningTask ?? Task.CompletedTask);

            return this;
        }

        public IRunningEngine Start()
        {
            caneclationTokenSource = new CancellationTokenSource();
            runningTask = Task.Run(() => RunningEngine(caneclationTokenSource.Token));

            return this;
        }

        public void Next()
        {
            var startTime = DateTime.Now;
            Generation = generator.Generate(generation);
            CalculationTimeInMs = (DateTime.Now - startTime).TotalMilliseconds;

            GenerationNumber++;
        }

        public static IInitializeableEngine Create(IGenerationGenerator generator)
        {
            return new Engine(generator);
        }

        private async Task RunningEngine(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                Next();
                try
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(1000 / (double) SpeedMultiplier), cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    // Cancaltion Is expected, and therefor the exception is dismissed
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface IEngine
    {
        byte SpeedMultiplier { get; set; }

        uint GenerationNumber { get; }

        double CalculationTimeInMs { get; }

        Board Generation { get; }
    }

    public interface IRunningEngine : IEngine
    {
        Task<IStoppedEngine> Stop();
    }

    public interface IStoppedEngine : IEngine
    {
        IRunningEngine Start();

        void Next();
    }

    public interface IInitializeableEngine : IEngine
    {
        IStoppedEngine Initialize(Board board);
    }
}