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
    public partial class LoginDevice : Form
    {
        public bool nuevo = false;
        public string id = null;
        public string nombre = null;
        public string descripcion = null;
        public string ip = null;
        public string puerto = null;
        public string usuario = null;
        public string contrasena = null;
        public string ultimavez = null;
        public string predeterminado = null;
        public string activo = null;
        
        public LoginDevice()
        {
            InitializeComponent();   
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoginDevice_Load(object sender, EventArgs e)
        {
            if (this.id != null && this.nombre != null && this.ip != null && this.puerto != null && this.usuario != null && this.contrasena != null )
            {
                this.textBoxID.Text = this.id.ToString();
                this.textBoxNombre.Text = this.nombre.ToString();
                if (this.descripcion != null)
                    this.textBoxDescripcion.Text = this.descripcion.ToString().ToUpper();
                else
                    this.textBoxDescripcion.Text = "";
                if (this.activo.ToString() == "1")
                    this.checkBoxEstado.Checked = true;
                if (this.predeterminado.ToString() == "1")
                    this.checkBoxPredeterminado.Checked = true;

                
                
                this.txtDireccionIP.Text = this.ip.ToString();
                this.txtPuerto.Text = this.puerto.ToString();
                this.txtUsuario.Text = this.usuario.ToString();
                this.txtContrasena.Text = this.contrasena.ToString();
                this.textBoxUltimaVez.Text = this.ultimavez.ToString();

            } 
       
        }


        private bool InsertarDispositivo()
        {
            bool retval = false;
            Common cmn = new Common();
            string connstr = cmn.obtenerCadenaConexion();
            BaseDatosMySQL bd = new BaseDatosMySQL();
            bd.conectarMySQL(connstr);
            if (bd.conn != null)
            {
                string sql = "INSERT INTO dispositivos (nombre, " +
                    "descripcion, direccionip, puerto, usuario, " +
                    "contrasena, estado, predeterminado, lastimeused, created)" +
                    "VALUES (@nombre, @descripcion, @direccionip, @puerto, " +
                    "@usuario, @contrasena, @estado, @predeterminado, @lastimeused, @created)";
                try
                {
                    int estado = 0;
                    int predeterminado = 0;
                    if (this.checkBoxEstado.Checked)
                        estado = 1;
                    if (this.checkBoxPredeterminado.Checked)
                    {
                        predeterminado = 1;
                        ActualizarPredeterminado();
                    }


                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);
                    cmd.Parameters.AddWithValue("@nombre", this.textBoxNombre.Text);
                    cmd.Parameters.AddWithValue("@descripcion", this.textBoxDescripcion.Text);
                    cmd.Parameters.AddWithValue("@direccionip", this.txtDireccionIP.Text);
                    cmd.Parameters.AddWithValue("@puerto", this.txtPuerto.Text);
                    cmd.Parameters.AddWithValue("@usuario", this.txtUsuario.Text);
                    cmd.Parameters.AddWithValue("@contrasena", this.txtContrasena.Text);
                    cmd.Parameters.AddWithValue("@estado", estado);
                    cmd.Parameters.AddWithValue("@predeterminado", predeterminado);
                    cmd.Parameters.AddWithValue("@created", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@lastimeused", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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

        private bool ActualizarPredeterminado()
        {
            bool retval = false;
            Common cmn = new Common();
            string connstr = cmn.obtenerCadenaConexion();
            BaseDatosMySQL bd = new BaseDatosMySQL();
            bd.conectarMySQL(connstr);
            if (bd.conn != null)
            {
                string sql = "UPDATE dispositivos " +
                    "SET predeterminado = 0 " +                    
                    "WHERE predeterminado = 1";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);                   
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
        
        private bool ActualizarDispositivo(string id)
        {
            bool retval = false;
            Common cmn = new Common();
            string connstr = cmn.obtenerCadenaConexion();
            BaseDatosMySQL bd = new BaseDatosMySQL();
            bd.conectarMySQL(connstr);
            if (bd.conn != null)
            {
                string sql = "UPDATE dispositivos SET nombre = @nombre, " +
                    "descripcion = @descripcion, " +
                    "direccionip = @direccionip, " +
                    "puerto =  @puerto, " +
                    "usuario = @usuario, " +
                    "contrasena = @contrasena," +
                    "estado = @estado, " +
                    "predeterminado = @predeterminado, " +
                    "modified = @modified," +
                    "lastimeused = @lasttime " +
                    "WHERE id = @id";
                try
                {
                    int estado = 0;
                    int predeterminado = 0;
                    if (this.checkBoxEstado.Checked)
                        estado = 1;
                    if (this.checkBoxPredeterminado.Checked)
                    {
                        predeterminado = 1;
                        ActualizarPredeterminado();
                    }

                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);
                    cmd.Parameters.AddWithValue("@nombre", this.textBoxNombre.Text);
                    cmd.Parameters.AddWithValue("@descripcion", this.textBoxDescripcion.Text);
                    cmd.Parameters.AddWithValue("@direccionip", this.txtDireccionIP.Text);
                    cmd.Parameters.AddWithValue("@puerto", this.txtPuerto.Text);
                    cmd.Parameters.AddWithValue("@usuario", this.txtUsuario.Text);
                    cmd.Parameters.AddWithValue("@contrasena", this.txtContrasena.Text);
                    cmd.Parameters.AddWithValue("@estado", estado);
                    cmd.Parameters.AddWithValue("@predeterminado", predeterminado);
                    cmd.Parameters.AddWithValue("@modified", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@lasttime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@id", this.textBoxID.Text);
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

        private bool login()
        {
            bool retval = false;

            Common cmn = new Common();
            string msg = null;
            bool ret = false;
            ret = cmn.Login(this.txtDireccionIP.Text,
                this.txtPuerto.Text,
                this.txtUsuario.Text,
                this.txtContrasena.Text, out msg);
            if (!ret)
            {
                MessageBox.Show(msg,"Error en login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Common.ip = this.txtDireccionIP.Text;
                Common.puerto = this.txtPuerto.Text;
                Common.usuario = this.txtUsuario.Text;
                Common.contrasena = this.txtContrasena.Text;
                retval = true;                
            }
            return retval;
        }
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (nuevo)
            {
                if (login())
                {
                    InsertarDispositivo();
                }
                else 
                {
                    DialogResult res = MessageBox.Show("No se logró iniciar sesión en el dispositivo ¿Desea agregarlo de todas formas?", "Error en el login al dispositivo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (res == DialogResult.Yes)
                    {
                        InsertarDispositivo();
                    }

                }

            } 
            else
            {
                login();
                ActualizarDispositivo(this.textBoxID.Text);
            }
            this.Close();
        }

        private void txtPuerto_Enter(object sender, EventArgs e)
        {
            
        }
    }
}
