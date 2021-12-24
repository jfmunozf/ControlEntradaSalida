using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlEntradaSalida
{
    class BaseDatosMySQL
    {
       
        public MySqlConnection conn { get; set; }
        public string errormsg { get; set; }
        public string errornum { get; set; }
        public BaseDatosMySQL()
        {
            errormsg = null;
            errornum = null;
            conn = null;
        }
        public void conectarMySQL(string connStr)
        {
            /* Se crea el objeto conn con la
            * información de la cadena de
            * conexión pasada como parámetro */
            conn = new MySqlConnection(connStr);
            try
            {
                /* Abrir la conexión
                * a la base de datos */
                conn.Open();
            }
            catch (MySqlException ex)
            {
                /* Si ocurre algún error en la conexión
                * a la base de datos, almacenamos la
                * información del error en estas variables */
                errormsg = ex.Message;
                errornum = Convert.ToString(ex.Number);
                conn = null;
            }
        }
        public void desconectarMySQL()
        {
            conn.Close();
        }
    }
}

