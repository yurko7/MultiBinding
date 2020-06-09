using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using YuKu.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            InitializeButtonBinding();
            InitializeColorBinding();
        }

        private void InitializeButtonBinding()
        {
            var multiBinding = new MultiBinding(nameof(Button.Enabled), dataSourceUpdateMode: DataSourceUpdateMode.Never);
            multiBinding.AddBinding(checkBox1, nameof(CheckBox.Checked));
            multiBinding.AddBinding(checkBox2, nameof(CheckBox.Checked));
            multiBinding.AddBinding(checkBox3, nameof(CheckBox.Checked));
            multiBinding.Format += (sender, args) => args.Value = ((Object[]) args.Value).Cast<Boolean?>().All(value => value ?? false);
            multiBinding.AddTo(button1);
        }

        private void InitializeColorBinding()
        {
            var multiBinding = new MultiBinding(nameof(PictureBox.BackColor), dataSourceUpdateMode: DataSourceUpdateMode.OnPropertyChanged);
            multiBinding.AddBinding(textBoxR, nameof(TextBox.Text), dataSourceUpdateMode: DataSourceUpdateMode.OnPropertyChanged);
            multiBinding.AddBinding(textBoxG, nameof(TextBox.Text), dataSourceUpdateMode: DataSourceUpdateMode.OnPropertyChanged);
            multiBinding.AddBinding(textBoxB, nameof(TextBox.Text), dataSourceUpdateMode: DataSourceUpdateMode.OnPropertyChanged);
            multiBinding.Format += (sender, args) =>
            {
                var values = (Object[]) args.Value;
                Int32.TryParse((String?) values[0], out Int32 r);
                Int32.TryParse((String?) values[1], out Int32 g);
                Int32.TryParse((String?) values[2], out Int32 b);
                args.Value = Color.FromArgb(r, g, b);
            };
            multiBinding.Parse += (sender, args) =>
            {
                var color = (Color) args.Value;
                args.Value = new Object[] {color.R.ToString(), color.G.ToString(), color.B.ToString()};
            };
            multiBinding.AddTo(pictureBox);
        }

        private void pictureBox_Click(Object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog(this) == DialogResult.OK)
            {
                pictureBox.BackColor = colorDialog.Color;
            }
        }
    }
}
