// Decompiled with JetBrains decompiler
// Type: WeighBridgeApplication.ViewModel.LogonViewModel
// Assembly: WeighBridgeApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0008ECA4-ECDC-4057-A6F3-F7FD4CE2F23F
// Assembly location: C:\Users\Ndavhe\Desktop\Rustivia Weight Bridge\WeighBridgeApplication.exe

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using WeighBridgeApplication.OfflineMode;
using WeighBridgeApplication.Util;

namespace WeighBridgeApplication.ViewModel
{
    public class LogonViewModel : PropertyChangedNotify
    {
        private bool _isBusy;
        private bool _isServiceDown;
        private bool _isValidLogOn;
        private SecureString _password;
        private string _userName;

        public LogonViewModel()
        {
            this.IsBusy = false;
            this.IsValidLogOn = true;
            this.LogOnCommand = new DelegateCommand(new Action(this.CheckLogin));
            this.CancelOnCommand = new DelegateCommand(new Action(this.CancelOn));
        }

        public DelegateCommand LogOnCommand { get; set; }

        public DelegateCommand CancelOnCommand { get; set; }

        public string UserName
        {
            get
            {
                return this._userName;
            }
            set
            {
                this._userName = value;
                this.OnPropertyChanged(nameof(UserName));
            }
        }

        public bool IsBusy
        {
            get
            {
                return this._isBusy;
            }
            set
            {
                this._isBusy = value;
                this.OnPropertyChanged(nameof(IsBusy));
            }
        }

        public SecureString Password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
                this.OnPropertyChanged(nameof(Password));
            }
        }

        public bool IsValidLogOn
        {
            get
            {
                return this._isValidLogOn;
            }
            set
            {
                if (this._isValidLogOn == value)
                    return;
                this._isValidLogOn = value;
                this.OnPropertyChanged(nameof(IsValidLogOn));
            }
        }

        private void CancelOn()
        {
            Program.CloseLogon();
        }

        private void CheckLogin()
        {
            string password = string.Empty;
            if (this.Password != null && this.Password.Length > 0)
                password = this.PtrToStringBSTR(this.SecureStringToBSTR(this.Password));
            this.IsBusy = true;
            try
            {
                using (DataClassService dataClassService = new DataClassService())
                {
                    this.IsValidLogOn = dataClassService.Logon(this.UserName, password);
                    this.IsBusy = false;
                    if (!this.IsValidLogOn)
                        return;
                    Task.Factory.StartNew(new Action(this.RecoverWeighBridges)).Wait();
                    using (DataClassJson dataClassJson = new DataClassJson())
                        dataClassJson.CacheUser(this.UserName, password);
                    Program.ValidLogon(this.UserName);
                }
            }
            catch (Exception ex)
            {
                EventLogHelper.Log("Service is down, Using Cached Data " + ex.Message);
                using (DataClassJson dataClassJson = new DataClassJson())
                {
                    this.IsValidLogOn = dataClassJson.Logon(this.UserName, password);
                    this.IsBusy = false;
                    if (!this.IsValidLogOn)
                        return;
                    Program.IsServiceDown = true;
                    Program.ValidLogon(this.UserName);
                }
            }
        }

        private void RecoverWeighBridges()
        {
            using (WeighModeJson weighModeJson = new WeighModeJson())
                weighModeJson.RecoverWeighBridges();
        }

        private string PtrToStringBSTR(IntPtr ptr)
        {
            string stringBstr = Marshal.PtrToStringBSTR(ptr);
            Marshal.ZeroFreeBSTR(ptr);
            return stringBstr;
        }

        private IntPtr SecureStringToBSTR(SecureString password)
        {
            return Marshal.SecureStringToBSTR(password);
        }
    }
}
