using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ControlEntradaSalida
{
    public partial class Informe : Form
    {
        public object listads = null;
        public string nombredatasource = null;
        public string embeddedresource = null;

        public Informe()
        {
            InitializeComponent();
        }

        private void Informe_Load(object sender, EventArgs e)
        {


            //ReportDataSource rds = new ReportDataSource("InformeEventos", listads);
            ReportDataSource rds = new ReportDataSource(this.nombredatasource, listads);
            //this.reportViewer1.LocalReport.ReportEmbeddedResource = "ControlEntradaSalida.InformeEventos.rdlc";
            this.reportViewer1.LocalReport.ReportEmbeddedResource = this.embeddedresource;
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(rds);
            this.reportViewer1.Width = this.Width - 5;
            this.reportViewer1.Height = this.Height - 5;
            this.reportViewer1.RefreshReport();
        }
    }
}
