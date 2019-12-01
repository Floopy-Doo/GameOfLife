using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using GameOfLife.Core;
using WpfAsyncPack.Command;

namespace GameOfLife.UI
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel(IInitializeableEngine engine)
        {
            Engine = engine;

            var random = new Random();
            engine.Initialize(new Board(
                Enumerable.Repeat(new Cell(random.Next(-10, 10), random.Next(-10, 10)), 10).Distinct().ToArray()));
            StartCommand = new AsyncCommand(Start);
            StopCommand = new AsyncCommand(Stop);
            NextCommand = new AsyncCommand(Next);
        }

        public ICommand NextCommand { get; }

        public IEngine Engine { get; }

        public ICommand StopCommand { get; }

        public ICommand StartCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private Task Next(object arg)
        {
            var stoppedEngine = Engine as IStoppedEngine;
            stoppedEngine?.Next();

            return Task.CompletedTask;
        }

        private async Task Stop(object arg)
        {
            var stoppedEngine = Engine as IRunningEngine;
            await (stoppedEngine?.Stop() ?? Task.CompletedTask);
        }

        private Task Start(object arg)
        {
            var stoppedEngine = Engine as IStoppedEngine;
            stoppedEngine?.Start();

            return Task.CompletedTask;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}