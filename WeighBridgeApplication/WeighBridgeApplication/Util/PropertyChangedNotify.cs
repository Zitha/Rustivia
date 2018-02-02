using System.ComponentModel;
using System.Runtime.CompilerServices;
using WeighBridgeApplication.Annotations;

namespace WeighBridgeApplication.Util
{
    public abstract class PropertyChangedNotify : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}