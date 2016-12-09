using System.IO;
using System.Runtime.Serialization.Json;

namespace SnekGame.Model.Persistence
{
    public class GameMapFileIO : IGameMapIO
    {
        public string FilePath { get; set;}

        public void SaveMap(GameMap map)
        {
            var serializer = new DataContractJsonSerializer(typeof(GameMap));
            using (var fileStream = new FileStream(FilePath, FileMode.Create))
            {
                serializer.WriteObject(fileStream, map);
            }

        }

        public GameMap LoadMap()
        {
            var serializer = new DataContractJsonSerializer(typeof(GameMap));
            using (var fileStream = new FileStream(FilePath, FileMode.Open))
            {
                return (GameMap)serializer.ReadObject(fileStream);
            }
        }
    }
}
