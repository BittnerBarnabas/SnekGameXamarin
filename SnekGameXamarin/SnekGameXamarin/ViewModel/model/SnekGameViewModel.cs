using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using SnekGame.Model;
using SnekGame.Model.Persistence;
using SnekGameCross.ViewModel.model;
using SnekGameWPF.ViewModel.model;

namespace SnekGameWPF.ViewModel
{
    class SnekGameViewModel : ICommandableViewModel<DelegateCommand>, INotifyPropertyChanged, IMovableViewModel<DelegateCommand>
    {
        public string TestProperty => "CICA";
        private bool _isGamePaused;
        public DelegateCommand NewGameCommand { get; }
        public DelegateCommand LoadGameCommand { get; }
        public DelegateCommand PauseGameCommand { get; }
        public SnekGame.Model.Main.SnekGame Model { get; }
        public ObservableCollection<GameField> Fields { get; }
        public int MapSize => (int)Math.Sqrt(Fields.Count);

        private int _lastTreatLocation;
        public int EatenTreats { get; private set; }

        public int CurrentRoundTime => (int)_stopwatch.ElapsedMilliseconds/1000;

        private Stopwatch _stopwatch;

        public SnekGameViewModel(SnekGame.Model.Main.SnekGame model)
        {
            
            NewGameCommand = new DelegateCommand(NewGame);
            LoadGameCommand = new DelegateCommand(LoadGame);
            PauseGameCommand = new DelegateCommand(PauseGame);
            WPressedCommand = new DelegateCommand(()=>Model.CurrentSnekDirection=Direction.LEFT);
            SPressedCommand = new DelegateCommand(()=>Model.CurrentSnekDirection=Direction.RIGHT);
            APressedCommand = new DelegateCommand(()=>Model.CurrentSnekDirection = Direction.UP);
            DPressedCommand = new DelegateCommand(()=>Model.CurrentSnekDirection = Direction.DOWN);
            
            _isGamePaused = false;
            _stopwatch = new Stopwatch();
            
            Fields = new ObservableCollection<GameField>();
            Model = model;
            Model.GameLoopElapsed += (sender, args) => {
                UpdateCoordinates();
                OnPropertyChanged(nameof(CurrentRoundTime));

            };
            Model.GameEnded += (sender, args) =>
            {
                ResetGame();
                //MessageBox.Show("You lost", "Lost", MessageBoxButton.OK);
            };
            Model.SnekTreatSpawned += (sender, treatCoordinate) =>
            {
                UpdateTreat(treatCoordinate);
            };
            Model.SnekAteTreat += (sender, tuple) => SneakAteTreat(tuple);
        }

        private void UpdateTreat(Tuple<int, int> treatCoordinate)
        {
            _lastTreatLocation = (treatCoordinate.Item1 - 1) * MapSize + treatCoordinate.Item2 - 1;
            Fields[_lastTreatLocation] = new GameField() { Owner = FieldType.Treat };
        }

        private void SneakAteTreat(Tuple<int, int> treatCoordinate)
        {
            Fields[_lastTreatLocation] = new GameField() { Owner = FieldType.None };
            _lastTreatLocation = 0;
            EatenTreats++;
            OnPropertyChanged(nameof(EatenTreats));
        }

        private void NewGame()
        {
            _stopwatch = Stopwatch.StartNew();
            Model.StartGame();
        }

        private void UpdateCoordinates()
        {
            ResetGame();

            for (var i = 0; i < Model.MapSize; i++)
            {
                for (var j = 0; j < Model.MapSize; j++)
                {
                        Fields.Add(new GameField()
                        {
                            Owner = FieldType.None
                        });
                  }
            }
            UpdateCoordinates(Model.SnekPosition, FieldType.Snek);
            UpdateCoordinates(Model.ObstaclePosition, FieldType.Obstacle);
            if (_lastTreatLocation != 0)
            {
               Fields[_lastTreatLocation] = new GameField() { Owner = FieldType.Treat };
            }
        }

        private void UpdateCoordinates(IEnumerable<Tuple<int,int>> collection, FieldType newOwner)
        {
            foreach (var snekCoord in collection)
            {
                var x = snekCoord.Item1 - 1;
                var y = snekCoord.Item2 - 1;
                Fields[x*MapSize + y] = new GameField() {Owner = newOwner};
            }
        }

        private void LoadGame()
        {
            if (!_isGamePaused) Model.PauseGame(true);

            var pathToMap = GetMapPath();
            if (string.IsNullOrEmpty(pathToMap))
            {
                Model.PauseGame(false);
                return;
            }
            Model.MapSerializer = new GameMapFileIO() { FilePath = pathToMap };
            Model.LoadGame();


            UpdateCoordinates();
        }

        public void PauseGame()
        {
            _isGamePaused = !_isGamePaused;
            Model.PauseGame(_isGamePaused);
        }

        private void ResetGame()
        {
             Fields.Clear();
        }

        private static string GetMapPath()
        {
            /*var dialog = new OpenFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "SnekMap files(*.snekmap)|*.snekmap"
            };
            return (dialog.ShowDialog().HasValue) ? dialog.FileName : null;*/
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DelegateCommand WPressedCommand { get; }
        public DelegateCommand APressedCommand { get; }
        public DelegateCommand SPressedCommand { get; }
        public DelegateCommand DPressedCommand { get; }
    }
}
