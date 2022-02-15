using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace EquationLPP.ViewModel {
    public class BaseViewModel : INotifyPropertyChanged, IDisposable {
        public event PropertyChangedEventHandler? PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Dispose()   { this.OnDispose(); }
        public void OnDispose() { }
    }
}