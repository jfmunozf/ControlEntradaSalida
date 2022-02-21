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
    public partial class MDIParent : Form
    {
        public MDIParent()
        {
            InitializeComponent();
            
        }

        private void gestiónDeDispositivosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionDispositivos frmGestionDispositivos = new GestionDispositivos();
            frmGestionDispositivos.MdiParent = this;
            frmGestionDispositivos.Show();
        }

        private void gestionDeEmpleadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionEmpleados frmGestionEmpleados = new GestionEmpleados();
            frmGestionEmpleados.MdiParent = this;
            frmGestionEmpleados.Show();
        }

        private void CapturarEntradaSalidaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CapturaEntradaSalida frmCapturaEntradaSalida = new CapturaEntradaSalida();
            frmCapturaEntradaSalida.MdiParent = this;
            frmCapturaEntradaSalida.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void eventosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamInformeEventos frmParamInformeEventos = new ParamInformeEventos();
            frmParamInformeEventos.MdiParent = this;
            frmParamInformeEventos.Show();
        }

        private void MDIParent_Load(object sender, EventArgs e)
        {
            if (!Common.InicializarSDKHikVision())
                MessageBox.Show("Error en la inicialización del SDK de HikVison", "Error inicialización SDK", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (!Common.CrearDirectorioData())
                MessageBox.Show("Error en creación del directorio de datos", "Error creación directorio de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MDIParent_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Common.m_UserID >= 0)
            {
                HCNetSDK.NET_DVR_Logout_V30(Common.m_UserID);
                Common.m_UserID = -1;
            }
            HCNetSDK.NET_DVR_Cleanup();
        }

        private void consultarDatosDispositivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionUsuariosDispositivo frmGestionUsuariosDispositivo = new GestionUsuariosDispositivo();
            frmGestionUsuariosDispositivo.MdiParent = this;
            frmGestionUsuariosDispositivo.Show();

        }

        private void entradasYSalidasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamInformeEntradaSalida frmParamInformeEntradaSalida = new ParamInformeEntradaSalida();
            frmParamInformeEntradaSalida.MdiParent = this;
            frmParamInformeEntradaSalida.Show();
        }
    }
}
