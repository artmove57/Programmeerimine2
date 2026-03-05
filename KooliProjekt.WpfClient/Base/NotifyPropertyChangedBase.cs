using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KooliProjekt.WpfClient.Base
{
    /// <summary>
    /// NotifyPropertyChangedBase - baasklass INotifyPropertyChanged implementatsiooniga
    /// Kõik ViewModelid peaksid sellest pärinema
    /// </summary>
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Teavitab UI'd, et property väärtus on muutunud
        /// </summary>
        /// <param name="propertyName">Property nimi (automaatne kui [CallerMemberName])</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Seab property väärtuse ja teavitab UI'd
        /// </summary>
        /// <typeparam name="T">Property tüüp</typeparam>
        /// <param name="field">Backing field viide</param>
        /// <param name="value">Uus väärtus</param>
        /// <param name="propertyName">Property nimi (automaatne)</param>
        /// <returns>True, kui väärtus muutus</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
