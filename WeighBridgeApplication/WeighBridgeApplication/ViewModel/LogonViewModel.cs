using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Input;
using WeighBridgeApplication.RustiviaService.DbContext;

namespace WeighBridgeApplication.ViewModel
{
    public class LogonViewModel : ViewModelBase
    {
        private readonly DataContextClient _client;
        private bool _isValidLogOn;
        private SecureString _password;
        private string _userName;

        public LogonViewModel(DataContextClient client)
        {
            IsValidLogOn = true;
            _client = client;
            LogOnCommand = new DelegateCommand(CheckLogin);
            CancelOnCommand = new DelegateCommand(CancelOn);
        }

        public DelegateCommand LogOnCommand { get; set; }
        public DelegateCommand CancelOnCommand { get; set; }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public bool IsValidLogOn
        {
            get { return _isValidLogOn; }
            set
            {
                if (_isValidLogOn != value)
                {
                    _isValidLogOn = value;
                    OnPropertyChanged();
                }
            }
        }

        private void CancelOn()
        {
            Program.CloseLogon();
        }

        private void CheckLogin()
        {
            using (Cursors.Wait)
            {
                string passwordString = string.Empty;
                if (Password != null && Password.Length > 0)
                {
                    IntPtr ptr = SecureStringToBSTR(Password);
                    passwordString = PtrToStringBSTR(ptr);
                }

                IsValidLogOn = _client.IsValidUserAsync(UserName, passwordString).Result;
            }

            if (IsValidLogOn)
            {
                Program.ValidLogon(UserName);
            }
        }

        //Some Algorithm I do not know how it works, but it works
        //Resource Check this out 
        //http://social.msdn.microsoft.com/Forums/en-US/c6e3f4a4-3448-46c6-bbe5-c6415467c920/use-securestring-where-string-is-expectedconvert?forum=csharpgeneral
        private string PtrToStringBSTR(IntPtr ptr)
        {
            string s = Marshal.PtrToStringBSTR(ptr);
            Marshal.ZeroFreeBSTR(ptr);
            return s;
        }

        //Some Algorithm I do not know how it works, but it works
        //Resource Check this out 
        //http://social.msdn.microsoft.com/Forums/en-US/c6e3f4a4-3448-46c6-bbe5-c6415467c920/use-securestring-where-string-is-expectedconvert?forum=csharpgeneral
        private IntPtr SecureStringToBSTR(SecureString password)
        {
            IntPtr ptr = Marshal.SecureStringToBSTR(password);
            return ptr;
        }
    }
}