using System.Windows.Input;

namespace ForexFlow.ViewModel.RelayDelegats
{
    public class RelayCommandObject : ICommand
    {
        private readonly Action<object> _executeWithObject;
        private readonly Func<bool> _canExecute;

        public RelayCommandObject(Action<object> executeWithObject, Func<bool> canExecute = null)
        {
            _executeWithObject = executeWithObject ?? throw new ArgumentNullException(nameof(executeWithObject));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            if (parameter is object o)
            {
                _executeWithObject(o);
            }
        }

        // Event from ICommand interface
        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}

