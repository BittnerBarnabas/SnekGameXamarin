using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnekGame.Model.Persistence
{
    public interface IGameMapIO
    {
        void SaveMap(GameMap map);
        GameMap LoadMap();
    }
}
