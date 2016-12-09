using System;
using System.Windows.Input;

namespace SnekGameWPF.ViewModel
{
    class DelegateCommand : ICommand
    {
        private readonly Action _methodToExecute;
        private readonly Func<object, bool> _canExecuteFunc;
        public DelegateCommand(Action methodToExecute) : this(null, methodToExecute) { }

        public DelegateCommand(Func<object, bool> canExecuteFunc, Action methodToExecute)
        {
            _methodToExecute = methodToExecute;
            _canExecuteFunc = canExecuteFunc;
        }

        public bool CanExecute(object parameter)
        {
           return _canExecuteFunc?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("Not permitted to excute!");
            }
            _methodToExecute.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}
