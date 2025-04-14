using System.Windows.Input;

namespace ForexFlow.ViewModel.RelayDelegats
{
    internal class RelayCommandCode : ICommand
    {
        private readonly Action<string> _executeWithGuid;
        private readonly Func<bool> _canExecute;

        public RelayCommandCode(Action<string> executeWithGuid, Func<bool> canExecute = null)
        {
            _executeWithGuid = executeWithGuid ?? throw new ArgumentNullException(nameof(executeWithGuid));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }
        public void Execute(object parameter)
        {
            if (parameter is string code)
            {
                _executeWithGuid(code);
            }
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
