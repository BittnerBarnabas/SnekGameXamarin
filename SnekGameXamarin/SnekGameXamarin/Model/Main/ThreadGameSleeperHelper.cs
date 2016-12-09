using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnekGame.Model.Main
{
    class ThreadGameSleeperHelper : IGameSleeperHelper
    {
        public void Sleep(int interval)
        {
            Thread.Sleep(interval);
        }
    }
}
