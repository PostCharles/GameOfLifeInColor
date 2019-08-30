using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeInColor.Infrastructure.RuleSets.Support
{
    public abstract class ObservableObjectBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (PropertyChanged != null)
            {
                var propertyName = GetPropertyName(propertyExpression);
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null) throw new ArgumentNullException("propertyExpression");

            var body = propertyExpression.Body as MemberExpression;

            if (body == null) throw new ArgumentException("Invalid argument", "propertyExpression");

            var property = body.Member as PropertyInfo;

            if (property == null) throw new ArgumentException("Argument is not a property", "propertyExpression");

            return property.Name;
        }
    }
}
            