using System;
using System.Collections.Generic;
using System.Linq;

namespace SnekGame.Model.Main
{
    using Coordinate = Tuple<int,int>;
    using SnekCoordinate = Tuple<Direction, Tuple<int, int>>;
    public class Snek
    {
        public event EventHandler<Coordinate> SnekAteTreat;
        private readonly List<SnekCoordinate> _moveVectorList;
        private SnekCoordinate _snekNewBodyPartCoordinate;

        public void CheckForTreat(Coordinate treatPosition)
        {
            if (_moveVectorList[0].Item2.Equals(treatPosition))
            {
                _snekNewBodyPartCoordinate = new SnekCoordinate(_moveVectorList[0].Item1,_moveVectorList[0].Item2);
            }
        }

        public void MoveSnek(Direction dir)
        {
            if (_snekNewBodyPartCoordinate == null)
            {
                for (var i = _moveVectorList.Count - 1; i > 0; i--)
                {
                    _moveVectorList[i] = new SnekCoordinate(_moveVectorList[i - 1].Item1, TransformCoordinate(_moveVectorList[i].Item2, _moveVectorList[i - 1].Item1));
                }
            }
            else
            {
                var x = _snekNewBodyPartCoordinate.Item2.Item1;
                var y = _snekNewBodyPartCoordinate.Item2.Item2;
                _moveVectorList.Insert(1,
                    new SnekCoordinate(_snekNewBodyPartCoordinate.Item1,new Coordinate(x, y)));
                SnekAteTreat?.Invoke(this,new Coordinate(x,y));
                _snekNewBodyPartCoordinate = null;
            }
            _moveVectorList[0] = new SnekCoordinate(dir,TransformCoordinate(_moveVectorList[0].Item2,dir));
        }

        public List<Coordinate> Coordinates
        {
            get
            {
                return _moveVectorList.Select(dirCoordPair => dirCoordPair.Item2).ToList();
            }
        }

        public Snek(int initSize, Direction initDir, Tuple<int,int> initCoordinate)
        {
            var length = initSize;
            _moveVectorList = new List<SnekCoordinate>(length);
            for (var i = 0; i < length; ++i)
            {
                _moveVectorList.Add(new SnekCoordinate(initDir,TransformCoordinate(initCoordinate,initDir.getInverse(),i)));
            }
        }

        private Coordinate TransformCoordinate(Coordinate baseCoord, Direction dirVector, int vectorSize = 1)
        {
            switch (dirVector)
            {
                case Direction.UP:
                    return new Coordinate(baseCoord.Item1, baseCoord.Item2 - vectorSize);
                case Direction.DOWN:
                    return new Coordinate(baseCoord.Item1, baseCoord.Item2 + vectorSize);
                case Direction.LEFT:
                    return new Coordinate(baseCoord.Item1 - vectorSize, baseCoord.Item2);
                case Direction.RIGHT:
                    return new Coordinate(baseCoord.Item1 + vectorSize, baseCoord.Item2);
                default:
                    throw new ArgumentOutOfRangeException(nameof(dirVector), dirVector, null);
            }
        }

        public SnekGame SnekGame
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
    }
}
