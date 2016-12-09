using System.Windows.Input;

namespace SnekGameWPF.ViewModel
{
    interface ICommandableViewModel<out T> where T: ICommand
    {
        T NewGameCommand { get; }
        T LoadGameCommand { get; }
        T PauseGameCommand { get; }
    }
}
