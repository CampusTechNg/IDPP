using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDPP.LocalServer
{
    public partial class PanelForm : UserControl
    {
        public PanelForm(AppWindow owner)
        {
            InitializeComponent();

            this.AutoScroll = true;
            this.BackColor = owner.LightColor;
            this.Dock = DockStyle.Fill;
            Owner = owner;
        }

        public AppWindow Owner { get; private set; }
    }
}
