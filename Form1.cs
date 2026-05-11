using System.Windows.Forms;
using System.Drawing;

namespace MiProyectoWeb
{
    // Simple Form1 para que Application.Run(new Form1()) compile y funcione.
    public class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "MiProyectoWeb - Cliente";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(900, 600);
        }
    }
}