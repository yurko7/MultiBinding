using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace YuKu.Windows.Forms
{
    partial class MultiBinding
    {
        private sealed partial class BindingsHub : Control, ICustomTypeDescriptor, INotifyPropertyChanged
        {
            public BindingsHub()
            {
                PropertyDescriptorCollection defaultProperties = TypeDescriptor.GetProperties(this, true);
                _staticProperties = new[]
                {
                    defaultProperties.Find(nameof(DataValues), false)
                };
                _dynamicProperties = new Dictionary<String, PropertyDescriptor>();

                DataBindings.CollectionChanged += DataBindingsCollectionChanged;
            }

            public Object?[] DataValues
            {
                get => DataBindings.OfType<DataBinding>().Select(b => b.Value).ToArray();
                set
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }

                    DataBinding[] dataBindings = DataBindings.OfType<DataBinding>().ToArray();
                    if (value.Length != dataBindings.Length)
                    {
                        throw new ArgumentException(@"Number of values does not match number of bindings", nameof(value));
                    }

                    for (Int32 i = 0; i < dataBindings.Length; i++)
                    {
                        PropertyDescriptor valueProperty = _dynamicProperties[dataBindings[i].PropertyName];
                        valueProperty.SetValue(this, value[i]);
                    }
                }
            }

            public Binding AddBinding(
                Object dataSource,
                String dataMember,
                Boolean formattingEnabled,
                DataSourceUpdateMode dataSourceUpdateMode,
                Object? nullValue,
                String formatString,
                IFormatProvider? formatInfo)
            {
                String propertyName = GetUniquePropertyName();
                _dynamicProperties.Add(propertyName, new BoundPropertyDescriptor(propertyName));
                var binding = new DataBinding(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, nullValue, formatString, formatInfo);
                DataBindings.Add(binding);

                OnPropertyChanged(nameof(DataValues));

                return binding;
            }

            public void RemoveBinding(Binding binding)
            {
                DataBindings.Remove(binding);
                OnPropertyChanged(nameof(DataValues));
            }

            AttributeCollection ICustomTypeDescriptor.GetAttributes() => TypeDescriptor.GetAttributes(this, true);

            String ICustomTypeDescriptor.GetClassName() => TypeDescriptor.GetClassName(this, true);

            String ICustomTypeDescriptor.GetComponentName() => TypeDescriptor.GetComponentName(this, true);

            TypeConverter ICustomTypeDescriptor.GetConverter() => TypeDescriptor.GetConverter(this, true);

            EventDescriptor ICustomTypeDescriptor.GetDefaultEvent() => TypeDescriptor.GetDefaultEvent(this, true);

            PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty() => TypeDescriptor.GetDefaultProperty(this, true);

            Object ICustomTypeDescriptor.GetEditor(Type editorBaseType) => TypeDescriptor.GetEditor(this, editorBaseType, true);

            EventDescriptorCollection ICustomTypeDescriptor.GetEvents() => TypeDescriptor.GetEvents(this, true);

            EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes) => TypeDescriptor.GetEvents(this, attributes, true);

            PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
            {
                PropertyDescriptor[] properties = _staticProperties.Concat(_dynamicProperties.Values).ToArray();
                return new PropertyDescriptorCollection(properties, true);
            }

            PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes) => TypeDescriptor.GetProperties(this, attributes, true);

            Object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd) => this;

            public event PropertyChangedEventHandler? PropertyChanged;

            private Object? GetPropertyValue(String propertyName)
            {
                return GetDataBinding(propertyName).Value;
            }

            private void SetPropertyValue(String propertyName, Object? value)
            {
                GetDataBinding(propertyName).Value = value;
                OnPropertyChanged(propertyName);
                OnPropertyChanged(nameof(DataValues));
            }

            private DataBinding GetDataBinding(String propertyName)
            {
                return (DataBinding) DataBindings[propertyName];
            }

            private void OnPropertyChanged(String propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            private void DataBindingsCollectionChanged(Object sender, CollectionChangeEventArgs e)
            {
                switch (e.Action)
                {
                    case CollectionChangeAction.Refresh:
                        _dynamicProperties.Clear();
                        break;
                    case CollectionChangeAction.Remove when e.Element is DataBinding valueBinding:
                        _dynamicProperties.Remove(valueBinding.PropertyName);
                        break;
                }
            }

            private String GetUniquePropertyName()
            {
                Span<Char> propertyNameChars = stackalloc Char[8];
                propertyNameChars[0] = '_';
                propertyNameChars[1] = '_';
                var random = new Random();
                String propertyName;
                do
                {
                    for (Int32 i = 2; i < propertyNameChars.Length; i++)
                    {
                        propertyNameChars[i] = (Char) random.Next('a', 'z' + 1);
                    }
                    propertyName = new String(propertyNameChars);
                } while (_dynamicProperties.ContainsKey(propertyName));

                return propertyName;
            }

            private readonly PropertyDescriptor[] _staticProperties;
            private readonly Dictionary<String, PropertyDescriptor> _dynamicProperties;
        }
    }
}
