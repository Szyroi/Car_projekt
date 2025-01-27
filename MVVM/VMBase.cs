using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVM.VMBase
{
    public abstract class VMBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        //OnPropertyChanged benachrichtigt UI bei veränderungen
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
