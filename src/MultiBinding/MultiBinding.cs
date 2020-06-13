using System;
using System.Windows.Forms;

namespace YuKu.Windows.Forms
{
    public partial class MultiBinding : Binding
    {
        public MultiBinding(
            String propertyName,
            Boolean formattingEnabled = false,
            DataSourceUpdateMode dataSourceUpdateMode = DataSourceUpdateMode.OnValidation,
            Object? nullValue = null,
            String? formatString = @"",
            IFormatProvider? formatInfo = null)
            : base(
                propertyName,
                new BindingsHub(),
                nameof(BindingsHub.DataValues),
                formattingEnabled,
                dataSourceUpdateMode,
                nullValue,
                formatString,
                formatInfo)
        { }

        public Binding AddBinding(
            Object dataSource,
            String dataMember,
            Boolean formattingEnabled = false,
            DataSourceUpdateMode dataSourceUpdateMode = DataSourceUpdateMode.OnValidation,
            Object? nullValue = null,
            String formatString = @"",
            IFormatProvider? formatInfo = null)
        {
            return Hub.AddBinding(dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, nullValue, formatString, formatInfo);
        }

        public void RemoveBinding(Binding binding)
        {
            Hub.RemoveBinding(binding);
        }

        public void AddTo(ControlBindingsCollection dataBindings)
        {
            if (dataBindings == null) throw new ArgumentNullException(nameof(dataBindings));

            dataBindings.Control.Controls.Add(Hub);
            dataBindings.Add(this);
        }

        public void RemoveFrom(ControlBindingsCollection dataBindings)
        {
            if (dataBindings == null) throw new ArgumentNullException(nameof(dataBindings));

            dataBindings.Remove(this);
            dataBindings.Control.Controls.Remove(Hub);
        }

        private BindingsHub Hub => (BindingsHub) DataSource;
    }
}
