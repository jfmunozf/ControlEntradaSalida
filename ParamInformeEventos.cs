using MySql.Data.MySqlClient;
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
    public partial class ParamInformeEventos : Form
    {
        private string comboboxplanid = null;
        public ParamInformeEventos()
        {
            InitializeComponent();
        }


       

        private string ObtenerExpresionQuery()
        {
            
            string retval = null;
            retval = "SELECT entradas_salidas.num, empleados.documento, empleados.nombres, empleados.apellidos, entradas_salidas.fecha, entradas_salidas.hora FROM empleados, entradas_salidas WHERE empleados.documento = entradas_salidas.documento ";

            if (this.radioButtonTodosEmpleados.Checked == false)
            {
                retval += String.Format("AND empleados.documento = '{0}' ", this.textBoxDocumentoEmpleado.Text);
            }
            
            if (this.radioButtonTodasFechas.Checked == false)
            {
                string fechainicial = dateTimePickerFechaInicial.Value.ToString("yyyy-MM-dd");
                string fechafinal = dateTimePickerFechaFinal.Value.ToString("yyyy-MM-dd");
                retval += String.Format("AND entradas_salidas.fecha BETWEEN CAST('{0}' AS DATE) AND CAST('{1}' AS DATE) ", fechainicial, fechafinal);
                
            }
            if (this.radioButtonTodasHoras.Checked == false)
            {
                string horainicial = dateTimePickerHoraInicial.Value.ToString("HH:MM:ss");
                string horafinal = dateTimePickerHoraFinal.Value.ToString("HH:MM:ss");
                retval += String.Format("AND entradas_salidas.hora BETWEEN CAST('{0}' AS TIME) AND CAST('{1}' AS TIME) ", horainicial, horafinal);
            }
            if (this.textBoxNombreEmpleado.Text.Length > 0)
            {
                retval += String.Format("AND empleados.nombres LIKE '%{0}%' ", this.textBoxNombreEmpleado.Text);
            }
            if (this.textBoxApellidosEmpleado.Text.Length > 0)
            {
                retval += String.Format("AND empleados.apellidos LIKE '%{0}%' ", this.textBoxApellidosEmpleado.Text);
            }
            retval += "ORDER BY fecha, hora DESC";

            return retval;
        }

        private bool GenerarConsulta(string sql, out List<InformeEventos> lieventos)
        {
            lieventos = new List<InformeEventos>();
            bool retval = false;
            Common cmn = new Common();
            string connstr = cmn.obtenerCadenaConexion();
            BaseDatosMySQL bd = new BaseDatosMySQL();
            bd.conectarMySQL(connstr);
            if (bd.conn != null)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {

                            DateTime fecha = DateTime.Parse(rdr["fecha"].ToString());
                            DateTime hora = DateTime.Parse(rdr["hora"].ToString());

                            InformeEventos ie = new InformeEventos();
                            ie.num = rdr["num"].ToString();
                            ie.documento = rdr["documento"].ToString();
                            ie.nombres = rdr["nombres"].ToString();
                            ie.apellidos = rdr["apellidos"].ToString();
                            ie.fecha = fecha.ToString("yyyy-MM-dd");
                            ie.hora = hora.ToString("HH:MM:ss");
                            lieventos.Add(ie);
                            ie = null;

                            ListViewItem lvi = new ListViewItem(rdr["num"].ToString());
                            lvi.SubItems.Add(rdr["documento"].ToString());
                            lvi.SubItems.Add(rdr["nombres"].ToString());
                            lvi.SubItems.Add(rdr["apellidos"].ToString());
                            lvi.SubItems.Add(rdr["hora"].ToString());
                            lvi.SubItems.Add(rdr["fecha"].ToString());
                            listView.Items.Add(lvi);
                            lvi = null;
                        }
                        rdr.Close();
                        bd.desconectarMySQL();
                        retval = true;
                    } else
                    {
                        MessageBox.Show("No hay registros para mostrar", "No hay registros para mostrar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show(bd.errormsg);
            }
            return retval;
        }
        private void buttonVerInforme_Click(object sender, EventArgs e)
        {
            string result = ObtenerExpresionQuery().Trim();
            if (this.listView.Items.Count > 0)
                this.listView.Items.Clear();
            List<InformeEventos> lieventos= null;
            bool res = GenerarConsulta(result, out lieventos);
            if (res && lieventos != null)
            {
                Informe frmInforme = new Informe();                
                frmInforme.lista = lieventos;                
                frmInforme.Show();
            }
        }

       
       
        private void radioButtonRangoFechas_Click(object sender, EventArgs e)
        {
            this.dateTimePickerFechaInicial.Enabled = true;
            this.dateTimePickerFechaFinal.Enabled = true;
        }

        private void radioButtonTodasFechas_Click(object sender, EventArgs e)
        {
            this.dateTimePickerFechaInicial.Enabled = false;
            this.dateTimePickerFechaFinal.Enabled = false;
        }

        private void ParamInformeConsumos_Load(object sender, EventArgs e)
        {
            this.radioButtonTodosEmpleados.Checked = true;


        }

        private void radioButtonRangoHoras_CheckedChanged(object sender, EventArgs e)
        {
            this.dateTimePickerHoraInicial.Enabled = true;
            this.dateTimePickerHoraFinal.Enabled = true;
        }

        private void radioButtonTodasHoras_CheckedChanged(object sender, EventArgs e)
        {
            this.dateTimePickerHoraInicial.Enabled = false;
            this.dateTimePickerHoraFinal.Enabled = false;
        }

        private void textBoxDocumentoEmpleado_Click(object sender, EventArgs e)
        {

            this.radioButtonTodosEmpleados.Checked = false;
        }

        private void radioButtonTodosEmpleados_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxDocumentoEmpleado.Text = "";
            this.textBoxNombreEmpleado.Text = "";
            this.textBoxApellidosEmpleado.Text = "";
        }
    }
}
