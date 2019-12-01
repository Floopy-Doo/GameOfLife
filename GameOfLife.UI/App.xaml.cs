using System;
using System.Windows;
using GameOfLife.Core;

namespace GameOfLife.UI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindowViewModel = new MainWindowViewModel(Engine.Create(new GenerationGenerator()));
            var mainWindow = new MainWindow {DataContext = mainWindowViewModel};
            mainWindow.Show();
        }
    }
}