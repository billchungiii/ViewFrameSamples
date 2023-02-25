using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewFrameSample002
{
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            { return false; }
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        
        protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            { return false; }
            storage = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public class RelayCommand : ICommand
        {
            private Action<object> _executeHandler;
            private Func<object, bool> _canExecuteHandler;

            public event EventHandler CanExecuteChanged;            

            public RelayCommand(Action<object> executeHandler, Func<object, bool> canExecuteHandler)
            {
                _executeHandler = executeHandler ?? throw new ArgumentNullException("execute handler can not be null");
                _canExecuteHandler = canExecuteHandler ?? throw new ArgumentNullException("canExecute handler can not be null");
            }

            public RelayCommand(Action<object> execute) : this(execute, (x) => true)
            { }

            public bool CanExecute(object parameter)
            {
                return _canExecuteHandler(parameter);
            }

            public void Execute(object parameter)
            {
                _executeHandler(parameter);
            }
        }

    }
    public class MainViewModel
    {

    }


}
