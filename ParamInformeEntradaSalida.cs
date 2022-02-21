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
    public partial class ParamInformeEntradaSalida : Form
    {
        private string comboboxplanid = null;
        public ParamInformeEntradaSalida()
        {
            InitializeComponent();
        }

        private bool CrearTablaInformeES()
        {
            bool retval = false;

            Common cmn = new Common();
            string connstr = cmn.obtenerCadenaConexion();
            BaseDatosMySQL bd = new BaseDatosMySQL();
            bd.conectarMySQL(connstr);
            if (bd.conn != null)
            {
                try
                {
                    string sql = "CALL CREAR_TABLA_INFORME_ES()";
                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);
                    cmd.ExecuteNonQuery();
                    bd.desconectarMySQL();
                    retval = true;
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
       

        private string ObtenerExpresionQuery()
        {
            
            string retval = null;
            retval = "SELECT temp_informe_es.id, temp_informe_es.documento, nombres, empleados.apellidos, temp_informe_es.fecha, temp_informe_es.hora_a, temp_informe_es.hora_b FROM empleados, temp_informe_es WHERE empleados.documento = temp_informe_es.documento ";

            if (this.radioButtonTodosEmpleados.Checked == false)
            {
                retval += String.Format("AND temp_informe_es.documento = '{0}' ", this.textBoxDocumentoEmpleado.Text);
            }
            
            if (this.radioButtonTodasFechas.Checked == false)
            {
                string fechainicial = dateTimePickerFechaInicial.Value.ToString("yyyy-MM-dd");
                string fechafinal = dateTimePickerFechaFinal.Value.ToString("yyyy-MM-dd");
                retval += String.Format("AND temp_informe_es.fecha BETWEEN CAST('{0}' AS DATE) AND CAST('{1}' AS DATE) ", fechainicial, fechafinal);
                
            }
            if (this.radioButtonTodasHoras.Checked == false)
            {
                string horainicial = dateTimePickerHoraInicial.Value.ToString("HH:MM:ss");
                string horafinal = dateTimePickerHoraFinal.Value.ToString("HH:MM:ss");
                retval += String.Format("AND temp_informe_es.hora_a AND temp_informe_es.hora_b BETWEEN CAST('{0}' AS TIME) AND CAST('{1}' AS TIME) ", horainicial, horafinal);
            }
            if (this.textBoxNombreEmpleado.Text.Length > 0)
            {
                retval += String.Format("AND empleados.nombres LIKE '%{0}%' ", this.textBoxNombreEmpleado.Text);
            }
            if (this.textBoxApellidosEmpleado.Text.Length > 0)
            {
                retval += String.Format("AND empleados.apellidos LIKE '%{0}%' ", this.textBoxApellidosEmpleado.Text);
            }
            retval += "ORDER BY fecha, documento ASC";

            return retval;
        }

        private bool GenerarConsulta(string sql, out List<InformeEntradaSalida> listaes)
        {
            listaes = new List<InformeEntradaSalida>();
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
                            DateTime fecha;
                            DateTime horaa;
                            DateTime horab;
                            string strfecha;
                            string strhoraa;
                            string strhorab;
                            try
                            {
                                fecha = DateTime.Parse(rdr["fecha"].ToString());
                                strfecha = fecha.ToString("yyyy-MM-dd");
                            }
                            catch
                            {
                                strfecha = "";
                            }
                            try
                            {
                                horaa = DateTime.Parse(rdr["hora_a"].ToString());
                                strhoraa = horaa.ToString("HH:MM:ss");
                            }
                            catch
                            {
                                strhoraa = "";
                            }
                            try
                            {
                                horab = DateTime.Parse(rdr["hora_b"].ToString());
                                strhorab = horab.ToString("HH:MM:ss");
                            }
                            catch
                            {
                                strhorab = "";
                            }
                            

                            InformeEntradaSalida ies = new InformeEntradaSalida();
                            ies.num = rdr["id"].ToString();
                            ies.documento = rdr["documento"].ToString();
                            ies.nombres = rdr["nombres"].ToString();
                            ies.apellidos = rdr["apellidos"].ToString();
                            ies.fecha = strfecha;
                            ies.horaa = strhoraa;
                            ies.horab = strhorab;
                            listaes.Add(ies);
                            ies = null;

                            ListViewItem lvi = new ListViewItem(rdr["id"].ToString());
                            lvi.SubItems.Add(rdr["documento"].ToString());
                            lvi.SubItems.Add(rdr["nombres"].ToString());
                            lvi.SubItems.Add(rdr["apellidos"].ToString());
                            lvi.SubItems.Add(strfecha);
                            lvi.SubItems.Add(strhoraa);
                            lvi.SubItems.Add(strhorab);
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
            if (CrearTablaInformeES())
            {
                string result = ObtenerExpresionQuery().Trim();
                if (this.listView.Items.Count > 0)
                    this.listView.Items.Clear();
                List<InformeEntradaSalida> objinf = null;
                bool res = GenerarConsulta(result, out objinf);
                if (res && objinf != null)
                {
                    Informe frmInforme = new Informe();
                    frmInforme.listads = objinf;
                    frmInforme.embeddedresource = "ControlEntradaSalida.InformeEntradaSalida.rdlc";
                    frmInforme.nombredatasource = "InformeEntradaSalida";
                    frmInforme.Show();
                }
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
