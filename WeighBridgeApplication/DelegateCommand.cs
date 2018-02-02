using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace WeighBridgeApplication
{
    public class DelegateCommand : ICommand
    {
        #region Constructors
        /// <summary>
        ///     Constructor
        /// </summary>
        public DelegateCommand(Action executeMethod)
            : this(executeMethod, null, false) {}

        /// <summary>
        ///     Constructor
        /// </summary>
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : this(executeMethod, canExecuteMethod, false) {}

        /// <summary>
        ///     Constructor
        /// </summary>
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null)
            {
                throw new ArgumentNullException("executeMethod");
            }
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
            _isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Method to determine if the command can be executed
        /// </summary>
        public bool CanExecute()
        {
            if (_canExecuteMethod != null)
            {
                return _canExecuteMethod();
            }
            return true;
        }

        /// <summary>
        ///     Execution of the command
        /// </summary>
        public void Execute()
        {
            if (_executeMethod != null)
            {
                _executeMethod();
            }
        }
        #endregion

        #region ICommand Members
        /// <summary>
        ///     ICommand.CanExecuteChanged implementation
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!_isAutomaticRequeryDisabled)
                {
                    CommandManager.RequerySuggested += value;
                }
                // CommandManagerHelper.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2);
            }
            remove
            {
                if (!_isAutomaticRequeryDisabled)
                {
                    CommandManager.RequerySuggested -= value;
                }
                // CommandManagerHelper.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        void ICommand.Execute(object parameter)
        {
            Execute();
        }
        #endregion

        #region Data
        private readonly Func<bool> _canExecuteMethod;
        private readonly Action _executeMethod;
        private readonly bool _isAutomaticRequeryDisabled;
        private List<WeakReference> _canExecuteChangedHandlers;
        #endregion
    }
}