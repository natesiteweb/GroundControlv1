using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroundControlv1
{
    public partial class AltitudeSetForm : Form
    {
        float altitude = 5f;

        public AltitudeSetForm()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void absAltTxtbox_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(absAltTxtbox.Text, out altitude))
            {
                button1.Enabled = true;

            }
            else
            {
                button1.Enabled = false;
            }
        }

        public float GetAltitude()
        {
            return altitude;
        }
    }
}
