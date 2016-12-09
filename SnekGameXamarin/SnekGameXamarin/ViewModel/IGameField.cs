using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnekGameWPF.ViewModel.model
{
    interface IGameField
    {
        FieldType Owner { get; set; }
        Int32 RowNumber { get; set; }
        Int32 ColumnNumber { get; set; }
    }
}
