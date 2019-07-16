using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class ProcessBarForm : Form
    {
        public ProcessBarForm()
        {
            InitializeComponent();
        }

        public bool Increase(int nValue)
        {
            if(nValue>0)
            {
                if (this.progressBar1.Value + nValue < this.progressBar1.Maximum)
                {
                    this.progressBar1.Value += nValue;
                    return true;
                }
                else
                {
                    this.progressBar1.Value = this.progressBar1.Maximum;
                    this.Close();
                    return false;
                }
            }
            return false;
        }
    }

   

}