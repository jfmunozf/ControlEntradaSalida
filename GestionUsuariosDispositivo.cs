using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ControlEntradaSalida
{
    public partial class GestionUsuariosDispositivo : Form
    {
        private uint iLastErr = 0;
        private string strErr;

        public GestionUsuariosDispositivo()
        {
            InitializeComponent();
        }


        private bool ISAPIQuery(string requestURL, string inputParam, out string outputResult, out string outputStatus)
        {
            bool retval = true;

            HCNetSDK.NET_DVR_XML_CONFIG_INPUT pInputXml = new HCNetSDK.NET_DVR_XML_CONFIG_INPUT();
            Int32 nInSize = Marshal.SizeOf(pInputXml);
            pInputXml.dwSize = (uint)nInSize;

            string strRequestUrl = requestURL;
            uint dwRequestUrlLen = (uint)strRequestUrl.Length;
            pInputXml.lpRequestUrl = Marshal.StringToHGlobalAnsi(strRequestUrl);
            pInputXml.dwRequestUrlLen = dwRequestUrlLen;

            string strInputParam = inputParam;

            pInputXml.lpInBuffer = Marshal.StringToHGlobalAnsi(strInputParam);
            pInputXml.dwInBufferSize = (uint)strInputParam.Length;

            HCNetSDK.NET_DVR_XML_CONFIG_OUTPUT pOutputXml = new HCNetSDK.NET_DVR_XML_CONFIG_OUTPUT();
            pOutputXml.dwSize = (uint)Marshal.SizeOf(pInputXml);
            pOutputXml.lpOutBuffer = Marshal.AllocHGlobal(3 * 1024 * 1024);
            pOutputXml.dwOutBufferSize = 3 * 1024 * 1024;
            pOutputXml.lpStatusBuffer = Marshal.AllocHGlobal(4096 * 4);
            pOutputXml.dwStatusSize = 4096 * 4;

            if (!HCNetSDK.NET_DVR_STDXMLConfig(Common.m_UserID, ref pInputXml, ref pOutputXml))
            {
                iLastErr = HCNetSDK.NET_DVR_GetLastError();
                outputResult = "NET_DVR_STDXMLConfig failed, error code= " + iLastErr;
                // Failed to send XML data and output the error code
                // MessageBox.Show(strErr);
                retval = false;
            }

            string strOutputParam = Marshal.PtrToStringAnsi(pOutputXml.lpOutBuffer);
            outputResult = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(strOutputParam));

            outputStatus = Marshal.PtrToStringAnsi(pOutputXml.lpStatusBuffer);

            Marshal.FreeHGlobal(pInputXml.lpRequestUrl);
            Marshal.FreeHGlobal(pOutputXml.lpOutBuffer);
            Marshal.FreeHGlobal(pOutputXml.lpStatusBuffer);
            return retval;
        }


        private bool AgregarTarjetaUsuario(string id)
        {
            bool retval = false;            
            string url = "POST /ISAPI/AccessControl/CardInfo/Record?format=json";
            string CardInfo = "{{\"CardInfo\" : {{\"employeeNo\": \"{0}\",\"cardNo\": \"{0}\",\"cardType\": \"normalCard\",\"checkCardNo\": true }}}}";
            string CardInfoValues = String.Format(CardInfo, id);

            string outputString = null;
            string outputStatus = null;
            Common cmn = new Common();
            string jsonresult = "";

            bool result = cmn.ISAPIQuery(url, CardInfoValues, out outputString, out outputStatus);
            bool flag = true;
            if (result)
            {
                jsonresult = outputString;
                
            }
            else
            {
                jsonresult = outputStatus;
                flag = false;

            }

            string statusCode = "";
            string subStatusCode = "";
            string statusString = "";
            try
            {
                dynamic DynamicData = JsonConvert.DeserializeObject(jsonresult);
                statusCode = DynamicData.statusCode;
                subStatusCode = DynamicData.subStatusCode;
                statusString = DynamicData.statusString;

                if (statusCode == "1" && statusString == "OK" && subStatusCode == "ok")
                {
                    retval = true;
                }
            } catch
            {
                MessageBox.Show("Ocurrió un error en al tratar de analizar la respuesta JSON de la consulta ISAPI en AgregarTarjetaUsuario()", "Error en análisis JSON", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!flag)
                MessageBox.Show("Ocurrió un error al intentar la consulta ISAPI en AgregarTarjetaUsuario. statusCode: " + statusCode + " subStatusCode: " + subStatusCode + " statusString: " + statusString, "Error en consulta ISAPI", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return retval;

        }


        private List<EmpleadoData> CargarInfoUsuariosDispositivo(out string responseStatusStrg, out int totalMatches, out int numOfMatches, int searchResultPosition)
        {
            bool flag = true;
            
            responseStatusStrg = null;
            totalMatches = 0;
            numOfMatches = 0;
            

            List<EmpleadoData> listauserdata = new List<EmpleadoData>();
            string url = "POST /ISAPI/AccessControl/UserInfo/Search?format=json";

            string UserInfoSearchCond = "{ \"UserInfoSearchCond\": { \"searchID\": \"100\", \"searchResultPosition\": " + searchResultPosition.ToString() + ", \"maxResults\": 5000 } }";

            string outputString = null;
            string outputStatus = null;
            
            Common cmn = new Common();
            string jsonresult = "";

            bool result = cmn.ISAPIQuery(url, UserInfoSearchCond, out outputString, out outputStatus);
            if (!result)
            {
                jsonresult = outputStatus;
                try 
                {
                    dynamic DynamicData = JsonConvert.DeserializeObject(jsonresult);
                    string statusCode = DynamicData.statusCode;
                    string subStatusCode = DynamicData.subStatusCode;
                    string statusString = DynamicData.statusString;
                    MessageBox.Show("Ocurrió un error al intentar la consulta ISAPI en CargarInfoUsuariosDispositivo(). statusCode: " + statusCode + " subStatusCode: " + subStatusCode + " statusString: " + statusString, "Error en consulta ISAPI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } catch
                {
                    MessageBox.Show("Ocurrió un error en al tratar de analizar la respuesta JSON de la consulta ISAPI en CargarInfoUsuariosDispositivo() con result = false", "Error en análisis JSON", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                return null;                
            }
            else
            {
                          
                jsonresult = outputString;
                try
                {
                    dynamic DynamicData = JsonConvert.DeserializeObject(jsonresult);
                    totalMatches = Convert.ToInt32(DynamicData.UserInfoSearch.totalMatches);
                    numOfMatches = Convert.ToInt32(DynamicData.UserInfoSearch.numOfMatches);
                    responseStatusStrg = DynamicData.UserInfoSearch.responseStatusStrg;


                    if (totalMatches > 0 && numOfMatches > 0)
                    {
                        for (int i = 0; i < numOfMatches; i++)
                        {
                            try
                            {
                                EmpleadoData ed = new EmpleadoData();
                                ed.employeeNum = DynamicData.UserInfoSearch.UserInfo[i].employeeNo;
                                ed.name = DynamicData.UserInfoSearch.UserInfo[i].name;
                                ed.userType = DynamicData.UserInfoSearch.UserInfo[i].userType;
                                ed.validEnable = DynamicData.UserInfoSearch.UserInfo[i].Valid.enable;
                                ed.beginTime = DynamicData.UserInfoSearch.UserInfo[i].Valid.beginTime;
                                ed.endTime = DynamicData.UserInfoSearch.UserInfo[i].Valid.endTime;
                                ed.numOfCard = DynamicData.UserInfoSearch.UserInfo[i].numOfCard;
                                ed.numOfFace = DynamicData.UserInfoSearch.UserInfo[i].numOfFace;
                                listauserdata.Add(ed);
                              
                                ed = null;
                            }
                            catch
                            {
                                MessageBox.Show("Ocurrió un error en al tratar de analizar la respuesta JSON de la consulta ISAPI en CargarInfoUsuariosDispositivo() con result = true. Error al tratar de hacer parsing de UserInfoSearch.UserInfo[i]", "Error en análisis JSON", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                flag = false;
                                break;
                            }
                        }


                    }
                } catch
                {
                    MessageBox.Show("Consulta ISAPI correcta pero ocurrió un error en al tratar de analizar la respuesta JSON de la respuesta ISAPI en CargarInfoUsuariosDispositivo() con result = true", "Error en análisis JSON", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                
            }
            if (flag)
                return listauserdata;
            else
                return null;
        }
        private String[] ConsultarTarjetas()
        {
            String[] retval = null;
            bool result = false;

            string url = "POST /ISAPI/AccessControl/CardInfo/Search?format=json";

            string CardInfoSearchCond = "{ \"CardInfoSearchCond\": { \"searchID\": \"100\", \"searchResultPosition\": 0, \"maxResults\": 600 } }";

            string resultJSON = null;
            string outputStatus = null;

            result = ISAPIQuery(url, CardInfoSearchCond, out resultJSON, out outputStatus);
            if (!result)
            {
                MessageBox.Show("Error en la consulta ISAPI: " + resultJSON, "Error en la consulta JSON", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return retval;
            }

            if (resultJSON != null && resultJSON.Length > 0)
            {
                int totalMatches = 0;
                dynamic DynamicData = JsonConvert.DeserializeObject(resultJSON);
                totalMatches = Convert.ToInt32(DynamicData.CardInfoSearch.totalMatches);
                List<string> listanumtarjetas = new List<string>();
                string employeeNum = null;
                for (int i = 0; i < totalMatches; i++)
                {
                    employeeNum = DynamicData.CardInfoSearch.CardInfo[i].employeeNo;
                    listanumtarjetas.Add(employeeNum);
                }
                retval = listanumtarjetas.ToArray();
            }
            return retval;
        }



        private EmpleadoData ConsultarDatosEmpleado(string id, out string jsondata)
        {
            EmpleadoData ed = null;
            jsondata = "";
            string url = "POST /ISAPI/AccessControl/UserInfo/Search?format=json";
            string UserInfoSearchCond = "{ \"UserInfoSearchCond\": { \"searchID\": \"200\", \"searchResultPosition\": 0, \"maxResults\": 1, \"EmployeeNoList\" : [ { \"employeeNo\": \"" + id + "\" } ] } }";
            string resultJSON = null;
            string outputStatus = null;
            bool result = false;
            result = ISAPIQuery(url, UserInfoSearchCond, out resultJSON, out outputStatus);
            if (!result)
            {
                MessageBox.Show("Error en la consulta ISAPI: " + resultJSON, "Error en la consulta JSON", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ed;
            }

            if (resultJSON != null && resultJSON.Length > 0)
            {
                dynamic DynamicDataEmployee = JsonConvert.DeserializeObject(resultJSON);
                ed = new EmpleadoData();
                ed.employeeNum = id;
                ed.name = DynamicDataEmployee.UserInfoSearch.UserInfo[0].name;
                ed.userType = DynamicDataEmployee.UserInfoSearch.UserInfo[0].userType;
                ed.validEnable = DynamicDataEmployee.UserInfoSearch.UserInfo[0].Valid.enable;
                ed.beginTime = DynamicDataEmployee.UserInfoSearch.UserInfo[0].Valid.beginTime;
                ed.endTime = DynamicDataEmployee.UserInfoSearch.UserInfo[0].Valid.endTime;
                ed.numOfCard = DynamicDataEmployee.UserInfoSearch.UserInfo[0].numOfCard;
                ed.numOfFace = DynamicDataEmployee.UserInfoSearch.UserInfo[0].numOfFace;
                jsondata = resultJSON;
            }
            return ed;
        }


        private void AgregarDatosListView(EmpleadoData ed)
        {
            ListViewItem lvi = new ListViewItem(ed.employeeNum);
            lvi.SubItems.Add(ed.name);
            lvi.SubItems.Add(ed.userType);
            lvi.SubItems.Add(ed.validEnable);
            lvi.SubItems.Add(ed.beginTime);
            lvi.SubItems.Add(ed.endTime);
            lvi.SubItems.Add(ed.numOfCard);
            lvi.SubItems.Add(ed.numOfFace);
            lvi.SubItems.Add("");
            listView.Items.Add(lvi);
            lvi = null;
        }

        private bool EliminarEmpleadoDispositivo(string id)
        {
            bool retval = false;
            string url = "PUT /ISAPI/AccessControl/UserInfo/Delete?format=json";
            string UserInfoDelCond = "{ \"UserInfoDelCond\" : {  \"EmployeeNoList\" : [ {   \"employeeNo\": \""+id+"\" } ] } }";
            string resultJSON = null;
            string outputStatus = null;
            bool result = false;
            result = ISAPIQuery(url, UserInfoDelCond, out resultJSON, out outputStatus);
            if (!result)
            {
                MessageBox.Show("Error en la consulta ISAPI: " + resultJSON, "Error en la consulta JSON", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return retval;
            }
            if (resultJSON != null && resultJSON.Length > 0)
            {
                dynamic DynamicData = JsonConvert.DeserializeObject(resultJSON);
                string statusCode = DynamicData.statusCode;
                if (statusCode == "1")
                    retval = true;
            }
            return retval;

        }

        private void buttonConsultar_Click(object sender, EventArgs e)
        {
            if (Common.m_UserID < 0)
            {
                MessageBox.Show("Primero inicie sesión en el dispositivo", "Se requiere inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool retval;
            string msg;
            Common cmn = new Common();
            retval = cmn.Login(Common.ip, Common.puerto, Common.usuario, Common.contrasena, out msg);
            if (retval)
            {
                if (this.listView.Items.Count > 0)
                    this.listView.Items.Clear();

                string responseStatusStrg = "MORE";
                int totalMatches = 0;
                int numOfMatches = 0;
                int searchResultPosition = numOfMatches;

             
                while (responseStatusStrg == "MORE")
                {
 
                    List<EmpleadoData> listaed = CargarInfoUsuariosDispositivo(out responseStatusStrg, out totalMatches, out numOfMatches, searchResultPosition);
                    searchResultPosition += numOfMatches;
                    
                    if (listaed != null)
                    {
                        foreach (EmpleadoData u in listaed)
                        {
                            AgregarDatosListView(u);
                        }
                    }
                }

                this.textBoxTotal.Text = this.listView.Items.Count.ToString();

            }
        }

        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            if (this.listView.CheckedItems.Count == 0)
            {
                MessageBox.Show("Debe marcar al menos un usuario de la lista", "Eliminar usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Los usuarios seleccionados serán eliminados sin posibilidad de recuperación. ¿Desea continuar?", "Confirmar acción de borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                this.listView.BeginInvoke(new Action(() =>
                {
                    ListView.CheckedListViewItemCollection itemColl = this.listView.CheckedItems;
                    foreach (ListViewItem item in itemColl)
                     {
                    
                        string id = item.Text;
                        bool result = false;
                        result = EliminarEmpleadoDispositivo(id);
                        if (result)
                        {
                            item.Remove();
                        }
                   

                    }
                }));
            }
        }


        private bool EliminarUsuariosTablaUsuariosDispositivo()
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
                    string sql = "DELETE FROM usuarios_dispositivo";
                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);
                    cmd.ExecuteNonQuery();
                    bd.desconectarMySQL();

                    retval = true;

                }
                catch (MySqlException ex)
                {
                    string errstr = ex.Number.ToString() + " " + ex.Message;
                    MessageBox.Show(errstr, "Error en InsertarUsuarioTablaUsuariosDispositivo()", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else
            {
                string errstr = bd.errornum + " " + bd.errormsg + "bd = null";
                MessageBox.Show(errstr, "Error en InsertarUsuarioTablaUsuariosDispositivo()", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return retval;
        }
        private bool InsertarUsuarioTablaUsuariosDispositivo(string jsondata, byte[] img = null)            
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
                    string sql = "INSERT INTO usuarios_dispositivo (userdata, image) " +
                                 "VALUES (@userdata, @image)";
                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);                    
                    
                    cmd.Parameters.AddWithValue("@userdata", jsondata);
                    if (img != null)
                        cmd.Parameters.Add("@image", MySqlDbType.Blob).Value = img;
                    else
                        cmd.Parameters.AddWithValue("@image", "");
                    cmd.ExecuteNonQuery();
                    bd.desconectarMySQL();

                    retval = true;

                }
                catch (MySqlException ex)
                {
                    string errstr = ex.Number.ToString() + " " + ex.Message;
                    MessageBox.Show(errstr, "Error en InsertarUsuarioTablaUsuariosDispositivo()", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else
            {
                string errstr = bd.errornum + " " + bd.errormsg + "bd = null";
                MessageBox.Show(errstr, "Error en InsertarUsuarioTablaUsuariosDispositivo()", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return retval;
        }

       
        private void buttonBackup_Click(object sender, EventArgs e)
        {
            /*if (this.listView.Items.Count > 0)
            {
                EliminarUsuariosTablaUsuariosDispositivo();
                ListView.ListViewItemCollection items = this.listView.Items;
                string jsondata = null;
                byte[] bytesimagen = null;
                Common cmn = new Common();
                string msg = null;
                foreach(ListViewItem item in items)
                {
                    bytesimagen = cmn.ObtenerImagenUsuario(item.Text, out msg);      
                    
                    ConsultarDatosEmpleado(item.Text, out jsondata);
                    if (InsertarUsuarioTablaUsuariosDispositivo(jsondata, bytesimagen))
                    {
                        item.SubItems[8].Text = "OK: Usuario respaldado";
                        item.ForeColor = Color.Green;
                    }
                    else 
                    {
                        item.SubItems[8].Text = "FALLO: Ocurrieron errores al intentar respaldar este usuario";
                        item.ForeColor = Color.Red;
                    }
                }
            }*/
        }

        private void buttonAgregarTarjeta_Click(object sender, EventArgs e)
        {
            if (this.listView.Items.Count > 0)
            {
                ListView.ListViewItemCollection items = this.listView.Items;

                foreach (ListViewItem item in items)
                {
                    if (item.SubItems[6].Text == "0")
                    {
                        AgregarTarjetaUsuario(item.Text);
                    }
                }
            }
        }

        private void checkBoxSeleccionarTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (this.listView.Items.Count > 0)
            {
                ListView.ListViewItemCollection items = this.listView.Items;

                foreach (ListViewItem item in items)
                {
                    if (this.checkBoxSeleccionarTodos.Checked == true)
                        item.Checked = true;
                    else
                        item.Checked = false;
                }
            }
           
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    class EmpleadoData
    {
        public string employeeNum { get; set; }
        public string name { get; set; }
        public string userType { get; set; }
        public string validEnable { get; set; }
        public string beginTime { get; set; }
        public string endTime { get; set; }
        public string numOfCard {get; set;}
        public string numOfFace { get; set; }

        public EmpleadoData()
        {
            this.employeeNum = null;
            this.name = null;
            this.userType = null;
            this.validEnable = null;
            this.beginTime = null;
            this.endTime = null;
            this.numOfCard = null;
            this.numOfFace = null;
        }
    }
}
