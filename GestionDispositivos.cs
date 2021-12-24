using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ControlEntradaSalida
{
    public partial class GestionDispositivos : Form
    {
        public GestionDispositivos()
        {
            InitializeComponent();
        }
        private string ConsultarContrasenaDispositivo(string id)
        {
            string contrasena = null;
            Common cmn = new Common();
            String connstr = cmn.obtenerCadenaConexion();
            BaseDatosMySQL bd = new BaseDatosMySQL();
            bd.conectarMySQL(connstr);
            if (bd.conn != null)
            {
                string sql = "SELECT contrasena FROM dispositivos WHERE id = @id";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {

                        while (rdr.Read())
                        {
                            contrasena = rdr["contrasena"].ToString();
                        }
                    }
                    rdr.Close();
                    bd.desconectarMySQL();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            return contrasena;
        }

        private bool EliminarDispositivo(string id)
        {
            bool retval = false;
            Common cmn = new Common();
            string connstr = cmn.obtenerCadenaConexion();
            BaseDatosMySQL bd = new BaseDatosMySQL();
            bd.conectarMySQL(connstr);
            if (bd.conn != null)
            {
                string sql = "DELETE FROM dispositivos WHERE id = @id";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);
                    cmd.Parameters.AddWithValue("@id", id);                    
                    cmd.ExecuteNonQuery();
                    bd.desconectarMySQL();
                    retval = true;

                }
                catch (MySqlException ex)
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

        private void ConsultarDispositivos()
        {
            Common cmn = new Common();
            String connstr = cmn.obtenerCadenaConexion();
            BaseDatosMySQL bd = new BaseDatosMySQL();
            bd.conectarMySQL(connstr);
            if (bd.conn != null)
            {
                string sql = "SELECT * FROM dispositivos";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        listView.Items.Clear();
                        while (rdr.Read())
                        {
                            ListViewItem lvi = new ListViewItem(rdr["id"].ToString());
                            lvi.SubItems.Add(rdr["nombre"].ToString());
                            lvi.SubItems.Add(rdr["descripcion"].ToString());
                            lvi.SubItems.Add(rdr["direccionip"].ToString());
                            lvi.SubItems.Add(rdr["puerto"].ToString());
                            lvi.SubItems.Add(rdr["usuario"].ToString());
                            if (Common.m_UserID >= 0)
                            {
                                lvi.SubItems.Add("Conectado");
                            } else
                            {
                                lvi.SubItems.Add("Desconectado");
                            }
                            lvi.SubItems.Add(rdr["estado"].ToString());
                            lvi.SubItems.Add(rdr["predeterminado"].ToString());
                            lvi.SubItems.Add(rdr["lastimeused"].ToString());
                            listView.Items.Add(lvi);
                            lvi = null;
                        }
                    }
                    rdr.Close();
                    bd.desconectarMySQL();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void GestionDispositivos_Load(object sender, EventArgs e)
        {
            ConsultarDispositivos();
        }

        private void listView_DoubleClick(object sender, EventArgs e)
        {
            if (this.listView.SelectedIndices.Count != 0)
            {
                ListView.SelectedIndexCollection indexes = this.listView.SelectedIndices;
                foreach (int index in indexes)
                {
                    
                    string id = this.listView.Items[index].Text;
                    string nombre = this.listView.Items[index].SubItems[1].Text;
                    string descripcion = this.listView.Items[index].SubItems[2].Text;
                    string ip = this.listView.Items[index].SubItems[3].Text;
                    string puerto = this.listView.Items[index].SubItems[4].Text;
                    string usuario = this.listView.Items[index].SubItems[5].Text;
                    string contrasena = ConsultarContrasenaDispositivo(this.listView.Items[index].Text);
                    string activo = this.listView.Items[index].SubItems[7].Text;
                    string predeterminado = this.listView.Items[index].SubItems[8].Text;
                    string ultimavez = this.listView.Items[index].SubItems[9].Text;

                    LoginDevice frmLoginDevice = new LoginDevice();
                    frmLoginDevice.nuevo = false;
                    frmLoginDevice.id = id;
                    frmLoginDevice.nombre = nombre;
                    frmLoginDevice.descripcion = descripcion;
                    frmLoginDevice.ip = ip;
                    frmLoginDevice.puerto = puerto;
                    frmLoginDevice.usuario = usuario;
                    frmLoginDevice.contrasena = contrasena;
                    frmLoginDevice.activo = activo;
                    frmLoginDevice.predeterminado = predeterminado;
                    frmLoginDevice.ultimavez = ultimavez;
                    frmLoginDevice.ShowDialog();
                    if (Common.m_UserID >= 0)
                    {
                        this.listView.Items[index].SubItems[6].Text = "Conectado";
                    }
                    ConsultarDispositivos();

                }
            }
        }

        private void buttonNuevo_Click(object sender, EventArgs e)
        {
            LoginDevice frmLoginDevice = new LoginDevice();
            frmLoginDevice.nuevo = true;
            frmLoginDevice.ShowDialog();
            ConsultarDispositivos();
            if (Common.m_UserID >= 0)
            {
                int elementos = this.listView.Items.Count;
                this.listView.Items[elementos-1].SubItems[6].Text = "Conectado";

            }
        }

        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            if (this.listView.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Debe seleccionar al menos un dispositivo de la lista", "Eliminar dispositivo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            } else
            {
                DialogResult res = MessageBox.Show("¿Está seguro de eliminar el dispositivo seleccionado?", "Confirmar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    ListView.SelectedIndexCollection indexes = this.listView.SelectedIndices;
                    string id = this.listView.Items[indexes[0]].Text;
                    if (EliminarDispositivo(id))
                    {
                        ConsultarDispositivos();
                    }
                }
            }
        }
    }
}

