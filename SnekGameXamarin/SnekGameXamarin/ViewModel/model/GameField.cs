using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnekGameWPF.ViewModel.model
{
    class GameField : IGameField
    {
        public FieldType Owner { get; set; }
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }
    }
}
