using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace NCopy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            CopyFile();
        }

        private void CopyFile()
        {
            var _source = new FileInfo("S:\\dotnet-sdk-3.0.100-rc1-014190-win-x64.exe");
            var _destination = new FileInfo("S:\\My Repos\\dotnet-sdk-3.0.100-rc1-014190-win-x64.exe");

            if (_destination.Exists)
            {
                _destination.Delete();
            }

            Task.Run(() =>
            {
                _source.XCopy(_destination, x => progressBar1.BeginInvoke(new Action(() => { progressBar1.Value = x; lbPercent.Text = x.ToString() + "%"; })));
            }).GetAwaiter().OnCompleted(() => progressBar1.BeginInvoke(new Action(() => { progressBar1.Value = 100; lbPercent.Text = "100%"; MessageBox.Show("Copy Completed"); })));
        }

    }
}
