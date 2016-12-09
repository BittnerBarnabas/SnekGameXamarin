using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using SnekGame.Model.Persistence;

namespace SnekGame.Model.Main
{
    using Coordinate = Tuple<int, int>;
    public class SnekGame
    {
        private Coordinate _snekTreatLocation;
        private BackgroundWorker _modelThread;
        private bool _isGamePaused;
        private uint _passedGameLoops;
        private readonly IGameSleeperHelper _sleeperHelper;
        public virtual IGameMapIO MapSerializer { get; set; }
        public int TreatsEaten { get; private set; }
        public Direction CurrentSnekDirection { get; set; }
        private Snek _snek;
        public List<Coordinate> SnekPosition => _snek?.Coordinates;
        public int? MapSize => GameMap?.MapSize;
        public List<Coordinate> ObstaclePosition => GameMap?.ObstacleList;
        public GameMap GameMap { get; set; }
        public double StepInterval { get; set; }

        public SnekGame(IGameSleeperHelper sleeperHelper)
        {
            this._sleeperHelper = sleeperHelper;
        }
        public void LoadGame()
        {
            ClearGame();

            GameMap = MapSerializer.LoadMap();
            _snek = new Snek(GameMap.SnekSize, GameMap.SnekDirection, GameMap.SnekPosition);
            _snek.SnekAteTreat += SnekOnSnekAteTreat;
            CurrentSnekDirection = GameMap.SnekDirection;
        }

        private void SnekOnSnekAteTreat(object sender, Coordinate treatCoordinates)
        {
            ++TreatsEaten;
            _snekTreatLocation = null;
            SnekAteTreat?.Invoke(this, treatCoordinates);
        }

        public void Move(Direction dir)
        {
            _snek.MoveSnek(dir);
            if (_snekTreatLocation != null)
            {
                _snek.CheckForTreat(_snekTreatLocation);
            }
        }

        private bool IsGameEnded()
        {
            var snekHeadX = _snek.Coordinates[0].Item1;
            var snekHeadY = _snek.Coordinates[0].Item2;
            if (!Enumerable.Contains(ObstaclePosition, SnekPosition[0]) && snekHeadX <= GameMap.MapSize &&
                snekHeadX >= 1 && snekHeadY <= GameMap.MapSize && snekHeadY >= 1 &&
                !Enumerable.Contains(SnekPosition.GetRange(1, SnekPosition.Count - 1), SnekPosition[0])) return false;
            GameEnded?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public void StartGame()
        {
            _isGamePaused = false;

            _modelThread = new BackgroundWorker { WorkerSupportsCancellation = true };
            _modelThread.DoWork += MainGameLoop;
            _modelThread.RunWorkerAsync();
        }

        private void MainGameLoop(object sender, DoWorkEventArgs args)
        {
            while (true)
            {
                if (((BackgroundWorker)sender).CancellationPending) return;
                _sleeperHelper.Sleep((int)StepInterval);
                if (_isGamePaused) continue;
                Move(CurrentSnekDirection);
                if (IsGameEnded())
                {
                    ClearGame();
                    return;
                }
                SpawnSnekTreat();
                ++_passedGameLoops;
                GameLoopElapsed?.Invoke(this, EventArgs.Empty);
            }

        }

        private void SpawnSnekTreat()
        {
            if (_passedGameLoops % (10 * MapSize.Value) == 0) _snekTreatLocation = null;
            var generator = new Random(DateTime.Now.GetHashCode());
            if (_snekTreatLocation != null || generator.Next(1, 12) != 1) return;
            _snekTreatLocation = GetValidRandomTreatLocation();
            SnekTreatSpawned?.Invoke(this, _snekTreatLocation);
        }

        private Coordinate GetValidRandomTreatLocation()
        {
            var generator = new Random(DateTime.Now.GetHashCode());
            var possibleX = 0;
            var possibleY = 0;
            var areCoordinatesValid = false;
            while (!areCoordinatesValid)
            {
                try
                {
                    possibleX = generator.Next(2, MapSize.Value - 1);
                    possibleY = generator.Next(2, MapSize.Value - 1);
                }
                catch (InvalidOperationException)
                {
                    return new Coordinate(0, 0);
                }
                areCoordinatesValid = CheckCoordinatesIfValid(possibleX, possibleY);
            }

            return new Coordinate(possibleX, possibleY);
        }

        private bool CheckCoordinatesIfValid(int possibleX, int possibleY)
        {
            return
                !(_snek.Coordinates.Any(tuple => tuple.Item1 == possibleX || tuple.Item2 == possibleY) &&
                  ObstaclePosition.Any(tuple => tuple.Item1 == possibleX || tuple.Item2 == possibleY));
        }

        public void PauseGame(bool choice)
        {
            _isGamePaused = choice;
        }

        private void ClearGame()
        {
            _modelThread?.CancelAsync();
            _snekTreatLocation = null;
            _isGamePaused = false;
            ObstaclePosition?.Clear();
            SnekPosition?.Clear();
            TreatsEaten = 0;
        }

        public event EventHandler GameEnded;
        public event EventHandler GameLoopElapsed;
        public event EventHandler<Coordinate> SnekTreatSpawned;
        public event EventHandler<Coordinate> SnekAteTreat;
    }
}
