using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ForexFlow.ViewModel.RelayDelegats
{
    public class RelayCommandGuid : ICommand
    {
        private readonly Action<Guid> _executeWithGuid;
        private readonly Func<bool> _canExecute;

        public RelayCommandGuid(Action<Guid> executeWithGuid, Func<bool> canExecute = null)
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
            if (parameter is Guid guid)
            {
                _executeWithGuid(guid);
            }
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
