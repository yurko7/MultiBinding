using System;
using System.ComponentModel;

namespace YuKu.Windows.Forms
{
    partial class MultiBinding
    {
        partial class BindingsHub
        {
            private sealed class BoundPropertyDescriptor : PropertyDescriptor
            {
                public BoundPropertyDescriptor(String name) : base(name, null) { }

                public override Type ComponentType => typeof(BindingsHub);

                public override Type PropertyType => typeof(Object);

                public override Boolean IsReadOnly => false;

                public override Object? GetValue(Object component) => ((BindingsHub) component).GetPropertyValue(Name);

                public override void SetValue(Object component, Object? value)
                {
                    ((BindingsHub) component).SetPropertyValue(Name, value);
                    OnValueChanged(component, EventArgs.Empty);
                }

                public override Boolean CanResetValue(Object component) => false;

                public override void ResetValue(Object component) { }

                public override Boolean ShouldSerializeValue(Object component) => false;
            }
        }
    }
}
