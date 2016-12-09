using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SnekGame.Model
{
    using Coordinate = Tuple<int, int>;
    [DataContract]
    public class GameMap
    {
        /// <summary>
        /// Size of the Map
        /// </summary>
        [DataMember]
        public int MapSize { get; set; }
        /// <summary>
        /// Coordinates of the obstacles on the map
        /// </summary>
        [DataMember]
        public List<Coordinate> ObstacleList { get; set; }

        /// <summary>
        /// Initial position of Snek
        /// </summary>
        [DataMember]
        public Coordinate SnekPosition { get; set; }

        /// <summary>
        /// Initial direction of Snek
        /// </summary>
        [DataMember]
        public Direction SnekDirection { get; set; }

        /// <summary>
        /// Size of Snek
        /// </summary>
        [DataMember]
        public int SnekSize { get; set; }

        protected bool Equals(GameMap other)
        {
            return MapSize == other.MapSize && ObstacleList.SequenceEqual(other.ObstacleList) && Equals(SnekPosition, other.SnekPosition) && SnekDirection == other.SnekDirection && SnekSize == other.SnekSize;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GameMap) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = MapSize;
                hashCode = (hashCode*397) ^ (ObstacleList != null ? ObstacleList.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (SnekPosition != null ? SnekPosition.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) SnekDirection;
                hashCode = (hashCode*397) ^ SnekSize;
                return hashCode;
            }
        }
    }
}
