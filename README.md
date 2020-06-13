# MultiBinding

[MultiBinding] class represents a collection of [Binding] objects bound to a single target property.
So it's quite similar to the WPF [MultiBinding][wpf-multibinding] but for Windows Forms.

Also it's a descendant of [Binding] class so it has all the same members but with the following differences:
* Constructor of [MultiBinding] does not allow to specify data source and data member.
Binding value retrieved from nested bindings as an array of values.
* Nested bindings should be added/removed using special methods:
  * `AddBinding` method creates and returns a nested binding.
  Signature of the method is similar to signature of Binding [constructor][binding-constructor] but without a name of target property.
  * `RemoveBinding` method removes nested binding created with the `AddBinding` method.
* Unfortunately multi-bindings can not be just added to [DataBindings] collection of controls,
`AddTo` and `RemoveFrom` methods should be used for that.

## Example

For example, MultiBinding can be used to enable button only if both text-boxes are not empty:
```csharp
// Create instance of one-way MultiBinding with "Enabled" target property
var multiBinding = new MultiBinding(nameof(Button.Enabled), dataSourceUpdateMode: DataSourceUpdateMode.Never);

// Add two nested bindings with text-boxes as data-sources and "Text" property as a data-member
multiBinding.AddBinding(textBoxA, nameof(TextBox.Text));
multiBinding.AddBinding(textBoxB, nameof(TextBox.Text));

// Convert array of values of nested bindings to a target value
multiBinding.Format += (sender, args) => args.Value = ((Object[]) args.Value).Cast<String>().All(value => !String.IsNullOrEmpty(value));

// Add multi-binding to the button
multiBinding.AddTo(button.DataBindings);
```

More examples can be found in the [demo-project](demos/WindowsFormsApp).

[binding]: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.binding
[binding-constructor]: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.binding.-ctor#System_Windows_Forms_Binding__ctor_System_String_System_Object_System_String_System_Boolean_System_Windows_Forms_DataSourceUpdateMode_System_Object_System_String_System_IFormatProvider_
[databindings]: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.control.databindings
[wpf-multibinding]: https://docs.microsoft.com/en-us/dotnet/api/system.windows.data.multibinding
[multibinding]: src/MultiBinding/MultiBinding.cs