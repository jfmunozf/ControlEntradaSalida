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

        private EmpleadoData ConsultarDatosEmpleado(string id)
        {
            EmpleadoData ed = null;
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
                ed.numOfFace = DynamicDataEmployee.UserInfoSearch.UserInfo[0].numOfFace;
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
            lvi.SubItems.Add(ed.numOfFace);
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
                String[] numtarjetas = ConsultarTarjetas();

                if (numtarjetas != null)
                {
                    for (int i = 0; i < numtarjetas.Length; i++)
                    {
                        EmpleadoData ed = ConsultarDatosEmpleado(numtarjetas[i]);
                        if (ed != null)
                        {
                            AgregarDatosListView(ed);
                            ed = null;
                        }
                    }
                }
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
            }
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
        public string numOfFace { get; set; }

        public EmpleadoData()
        {
            this.employeeNum = null;
            this.name = null;
            this.userType = null;
            this.validEnable = null;
            this.beginTime = null;
            this.endTime = null;
            this.numOfFace = null;
        }
    }
}
