using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.MVVM.ViewModel
{
    public class VMNotifyErrorInfoBase: VMBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _PropertyErrors = new Dictionary<string, List<string>>();

        bool INotifyDataErrorInfo.HasErrors => _PropertyErrors.Any();
        public bool HasErrors => _PropertyErrors.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
        {
            if (propertyName != null) return _PropertyErrors.GetValueOrDefault(propertyName, null);
            else return null;
        }

        public void AddError(string propertyName, string errorMessage)
        {
            if (!_PropertyErrors.ContainsKey(propertyName))
            {
                _PropertyErrors.Add(propertyName, new List<string>());
            }

            _PropertyErrors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        public void ClearErrors(string propertyName)
        {
            if (_PropertyErrors.Remove(propertyName)) OnErrorsChanged(propertyName);
        }

        public void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(propertyName);
        }

        #region Helper methods
        protected bool IsNull(string? value)
        {
            return (value == null) ? true : false;
        }

        protected bool IsEmpty(string value)
        {
            if (value.Length == 0) return true;
            else for (int i = 0; i < value.Length; i++)
                {
                    if (!char.IsWhiteSpace(value[i])) return false;
                }
            return false;
        }

        protected bool IsEmptyOrNull(string? value)
        {
            return (IsNull(value) || IsEmpty(value)) ? true : false;
        }

        protected bool IsFloat(string value)
        {
            string formattedString = value.Replace(',', '.');
            float number;
            return float.TryParse(formattedString, out number);
        }

        #endregion Helper Methods
    }
}
