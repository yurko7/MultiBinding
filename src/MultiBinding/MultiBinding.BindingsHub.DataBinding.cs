using System;
using System.Windows.Forms;

namespace YuKu.Windows.Forms
{
    partial class MultiBinding
    {
        partial class BindingsHub
        {
            private sealed class DataBinding : Binding
            {
                public DataBinding(String propertyName, Object dataSource, String dataMember, Boolean formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, Object? nullValue, String formatString, IFormatProvider? formatInfo)
                    : base(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, nullValue, formatString, formatInfo)
                { }

                public Object? Value { get; set; }
            }
        }
    }
}
