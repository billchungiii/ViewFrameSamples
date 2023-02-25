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
    public class MainViewModel : NotifyPropertyChangedBase
    {

        public MainViewModel()
        {
            Angle = 0;
            ScaleX = 1;
            ScaleY = 1;
            TranslateX = 0;
            TranslateY = 0;
        }

        private double _angle;

        public double Angle
        {
            get => _angle;
            set => SetProperty(ref _angle, value);
        }

        private double _scaleX;
        public double ScaleX
        {
            get => _scaleX;
            set => SetProperty(ref _scaleX, value);
        }

        private double _scaleY;
        public double ScaleY
        {
            get => _scaleY;
            set => SetProperty(ref _scaleY, value);
        }

        private double _translateX;
        public double TranslateX
        {
            get => _translateX;
            set => SetProperty(ref _translateX, value);
        }

        private double _translateY;


        public double TranslateY
        {
            get => _translateY;
            set => SetProperty(ref _translateY, value);
        }

        private Dictionary<string, Action> _operations;

        private ICommand _operationCommand;
        public ICommand OperationCommand
        {
            get
            {
                if (_operationCommand == null)
                {
                    _operationCommand = new RelayCommand((x) =>
                    {
                        string parameter = x as string;
                        if (parameter == null) { return; }
                        switch (parameter)
                        {
                            case "up":
                                TranslateY -= 5;
                                return;
                            case "down":
                                TranslateY += 5;
                                return;
                            case "left":
                                TranslateX -= 5;
                                return;
                            case "right":
                                TranslateX += 5;
                                return;
                            case "center":
                                TranslateX = 0;
                                TranslateY = 0;
                                return;
                            case "angle":
                                Angle = 0;
                                return;
                            case "size":
                                ScaleX = 1;
                                ScaleY = 1;
                                return;
                            default:
                                return;
                        }
                    });
                }
                return _operationCommand;
            }
        }
    }
}
