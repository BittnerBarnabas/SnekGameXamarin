using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SnekGameWPF.ViewModel
{
    interface IMovableViewModel<out T> where T : ICommand
    {
        T WPressedCommand { get; }
        T APressedCommand { get; }
        T SPressedCommand { get; }
        T DPressedCommand { get; }
    }
}
