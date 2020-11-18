using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor_Assign2
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }
        //closes the form when the button is clicked or escape is pressed or enter is pressed
        private void OkButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
