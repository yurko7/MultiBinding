using System.Windows.Forms;

namespace YuKu.Windows.Forms
{
    public static class BindingExtensions
    {
        public static void AddMultiBinding(this ControlBindingsCollection dataBindings, MultiBinding multiBinding)
        {
            multiBinding.AddTo(dataBindings);
        }

        public static void RemoveMultiBinding(this ControlBindingsCollection dataBindings, MultiBinding multiBinding)
        {
            multiBinding.RemoveFrom(dataBindings);
        }
    }
}
