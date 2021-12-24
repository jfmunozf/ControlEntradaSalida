using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ControlEntradaSalida
{ 
    public partial class CapturaEntradaSalida : Form
    {
        private HCNetSDK.MSGCallBack m_falarmData = null;
        private string path = null;
        private int m_lLogNum = 0;
        private int lAlarmHandle = -1;   
        private string cardnumber = null;

        
        BaseDatosMySQL bd;        
        

        public CapturaEntradaSalida()
        {
            InitializeComponent();
        }


        private bool InsertarEntradaSalida(string num, string fecha, string hora, string documento, string evento)
        {
            bool retval = false;
            Common cmn = new Common();
            string connstr = cmn.obtenerCadenaConexion();
            BaseDatosMySQL bd = new BaseDatosMySQL();
            bd.conectarMySQL(connstr);
            if (bd.conn != null)
            {
                string sql = "INSERT INTO entradas_salidas " +
                    "(num, fecha, hora, documento, evento, created) " +
                    "VALUES (@num, @fecha, @hora, @documento, @evento, @created)";
                try
                {

                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);
                    cmd.Parameters.AddWithValue("@num", num);
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    cmd.Parameters.AddWithValue("@hora", hora);
                    cmd.Parameters.AddWithValue("@documento", documento);
                    cmd.Parameters.AddWithValue("@evento", evento);                    
                    cmd.Parameters.AddWithValue("@created", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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



        private void Deploy()
        {
            if (Common.m_UserID < 0)
            {
                MessageBox.Show("Please Login First!");
                return;
            }
            else
            {
                HCNetSDK.NET_DVR_SETUPALARM_PARAM struSetupAlarmParam = new HCNetSDK.NET_DVR_SETUPALARM_PARAM();
                struSetupAlarmParam.dwSize = (uint)Marshal.SizeOf(struSetupAlarmParam);
                struSetupAlarmParam.byLevel = 1;
                struSetupAlarmParam.byAlarmInfoType = 1;
                //struSetupAlarmParam.byDeployType = (byte)cbDeployType.SelectedIndex;
                struSetupAlarmParam.byDeployType = (byte)1;

                this.lAlarmHandle = HCNetSDK.NET_DVR_SetupAlarmChan_V41(Common.m_UserID, ref struSetupAlarmParam);
                if (lAlarmHandle < 0)
                {
                    MessageBox.Show("NET_DVR_SetupAlarmChan_V41 fail error: " + HCNetSDK.NET_DVR_GetLastError(), "Setup alarm chan failed");
                    
                }
                else
                {
                    
                   //MessageBox.Show("Setup alarm chan succeed");
                }

                m_falarmData = new HCNetSDK.MSGCallBack(MsgCallback);
                if (HCNetSDK.NET_DVR_SetDVRMessageCallBack_V50(0, m_falarmData, IntPtr.Zero))
                {
                    //MessageBox.Show("NET_DVR_SetDVRMessageCallBack_V50 succ", "succ", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("NET_DVR_SetDVRMessageCallBack_V50 fail", "operation fail", MessageBoxButtons.OK);
                    
                }
            }
        }

        private void MsgCallback(int lCommand, ref HCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            switch (lCommand)
            {
                case HCNetSDK.COMM_ALARM_ACS:
                    ProcessCommAlarmACS(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                default:
                    break;
            }
        }

        private void ProcessCommAlarmACS(ref HCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            HCNetSDK.NET_DVR_ACS_ALARM_INFO struAcsAlarmInfo = new HCNetSDK.NET_DVR_ACS_ALARM_INFO();
            struAcsAlarmInfo = (HCNetSDK.NET_DVR_ACS_ALARM_INFO)Marshal.PtrToStructure(pAlarmInfo, typeof(HCNetSDK.NET_DVR_ACS_ALARM_INFO));
            HCNetSDK.NET_DVR_LOG_V30 struFileInfo = new HCNetSDK.NET_DVR_LOG_V30();
            struFileInfo.dwMajorType = struAcsAlarmInfo.dwMajor;
            struFileInfo.dwMinorType = struAcsAlarmInfo.dwMinor;
            char[] csTmp = new char[256];

            if (HCNetSDK.MAJOR_ALARM == struFileInfo.dwMajorType)
            {
                TypeMap.AlarmMinorTypeMap(struFileInfo, csTmp);
            }
            else if (HCNetSDK.MAJOR_OPERATION == struFileInfo.dwMajorType)
            {
                TypeMap.OperationMinorTypeMap(struFileInfo, csTmp);
            }
            else if (HCNetSDK.MAJOR_EXCEPTION == struFileInfo.dwMajorType)
            {
                TypeMap.ExceptionMinorTypeMap(struFileInfo, csTmp);
            }
            else if (HCNetSDK.MAJOR_EVENT == struFileInfo.dwMajorType)
            {
                TypeMap.EventMinorTypeMap(struFileInfo, csTmp);
            }

            String szInfo = new String(csTmp).TrimEnd('\0');
            String szInfoBuf = null;
            szInfoBuf = szInfo;
            /**************************************************/
            String name = System.Text.Encoding.UTF8.GetString(struAcsAlarmInfo.sNetUser).TrimEnd('\0');
            for (int i = 0; i < struAcsAlarmInfo.sNetUser.Length; i++)
            {
                if (struAcsAlarmInfo.sNetUser[i] == 0)
                {
                    name = name.Substring(0, i);
                    break;
                }
            }
            /**************************************************/

            string tipoevento = string.Format("{0}", szInfo);
            string fecha = string.Format("{0,4}-{1:D2}-{2}", struAcsAlarmInfo.struTime.dwYear, struAcsAlarmInfo.struTime.dwMonth, struAcsAlarmInfo.struTime.dwDay);
            string hora = string.Format("{0:D2}:{1:D2}:{2:D2}", struAcsAlarmInfo.struTime.dwHour, struAcsAlarmInfo.struTime.dwMinute, struAcsAlarmInfo.struTime.dwSecond);
            string numerotarjeta = null;

            szInfoBuf = string.Format("{0} time:{1,4}-{2:D2}-{3} {4:D2}:{5:D2}:{6:D2}, [{7}]({8})", szInfo, struAcsAlarmInfo.struTime.dwYear, struAcsAlarmInfo.struTime.dwMonth,
                struAcsAlarmInfo.struTime.dwDay, struAcsAlarmInfo.struTime.dwHour, struAcsAlarmInfo.struTime.dwMinute, struAcsAlarmInfo.struTime.dwSecond,
                struAcsAlarmInfo.struRemoteHostAddr.sIpV4, name);

            if (struAcsAlarmInfo.struAcsEventInfo.byCardNo[0] != 0)
            {
                this.cardnumber = System.Text.Encoding.UTF8.GetString(struAcsAlarmInfo.struAcsEventInfo.byCardNo).TrimEnd('\0');
                szInfoBuf = szInfoBuf + "+Card Number:" + cardnumber;
                numerotarjeta = cardnumber;
                
            }
            String[] szCardType = { "normal card", "disabled card", "blocklist card", "night watch card", "stress card", "super card", "guest card" };
            byte byCardType = struAcsAlarmInfo.struAcsEventInfo.byCardType;

            if (byCardType != 0 && byCardType <= szCardType.Length)
            {
                szInfoBuf = szInfoBuf + "+Card Type:" + szCardType[byCardType - 1];
            }

            if (struAcsAlarmInfo.struAcsEventInfo.dwCardReaderNo != 0)
            {
                szInfoBuf = szInfoBuf + "+Card Reader Number:" + struAcsAlarmInfo.struAcsEventInfo.dwCardReaderNo;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.dwDoorNo != 0)
            {
                szInfoBuf = szInfoBuf + "+Door Number:" + struAcsAlarmInfo.struAcsEventInfo.dwDoorNo;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.dwVerifyNo != 0)
            {
                szInfoBuf = szInfoBuf + "+Multiple Card Authentication Serial Number:" + struAcsAlarmInfo.struAcsEventInfo.dwVerifyNo;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.dwAlarmInNo != 0)
            {
                szInfoBuf = szInfoBuf + "+Alarm Input Number:" + struAcsAlarmInfo.struAcsEventInfo.dwAlarmInNo;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.dwAlarmOutNo != 0)
            {
                szInfoBuf = szInfoBuf + "+Alarm Output Number:" + struAcsAlarmInfo.struAcsEventInfo.dwAlarmOutNo;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.dwCaseSensorNo != 0)
            {
                szInfoBuf = szInfoBuf + "+Event Trigger Number:" + struAcsAlarmInfo.struAcsEventInfo.dwCaseSensorNo;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.dwRs485No != 0)
            {
                szInfoBuf = szInfoBuf + "+RS485 Channel Number:" + struAcsAlarmInfo.struAcsEventInfo.dwRs485No;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.dwMultiCardGroupNo != 0)
            {
                szInfoBuf = szInfoBuf + "+Multi Recombinant Authentication ID:" + struAcsAlarmInfo.struAcsEventInfo.dwMultiCardGroupNo;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.byCardReaderKind != 0)
            {
                szInfoBuf = szInfoBuf + "+CardReaderKind:" + struAcsAlarmInfo.struAcsEventInfo.byCardReaderKind.ToString();
            }
            if (struAcsAlarmInfo.struAcsEventInfo.wAccessChannel >= 0)
            {
                szInfoBuf = szInfoBuf + "+wAccessChannel:" + struAcsAlarmInfo.struAcsEventInfo.wAccessChannel;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.dwEmployeeNo != 0)
            {
                //employeenum = struAcsAlarmInfo.struAcsEventInfo.dwEmployeeNo.ToString();
                szInfoBuf = szInfoBuf + "+EmployeeNo:" + struAcsAlarmInfo.struAcsEventInfo.dwEmployeeNo;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.byDeviceNo != 0)
            {
                szInfoBuf = szInfoBuf + "+byDeviceNo:" + struAcsAlarmInfo.struAcsEventInfo.byDeviceNo.ToString();
            }
            if (struAcsAlarmInfo.struAcsEventInfo.wLocalControllerID >= 0)
            {
                szInfoBuf = szInfoBuf + "+wLocalControllerID:" + struAcsAlarmInfo.struAcsEventInfo.wLocalControllerID;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.byInternetAccess >= 0)
            {
                szInfoBuf = szInfoBuf + "+byInternetAccess:" + struAcsAlarmInfo.struAcsEventInfo.byInternetAccess.ToString();
            }
            if (struAcsAlarmInfo.struAcsEventInfo.byType >= 0)
            {
                szInfoBuf = szInfoBuf + "+byType:" + struAcsAlarmInfo.struAcsEventInfo.byType.ToString();
            }
            if (struAcsAlarmInfo.struAcsEventInfo.bySwipeCardType != 0)
            {
                szInfoBuf = szInfoBuf + "+bySwipeCardType:" + struAcsAlarmInfo.struAcsEventInfo.bySwipeCardType.ToString();
            }
            //其它消息先不罗列了......
            /*
            if (struAcsAlarmInfo.dwPicDataLen > 0)
            {
                path = null;
                Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
                path = string.Format(@"C:/Picture/ACS_LocalTime{0}_{1}.bmp", szInfo, rand.Next());
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    int iLen = (int)struAcsAlarmInfo.dwPicDataLen;
                    byte[] by = new byte[iLen];
                    Marshal.Copy(struAcsAlarmInfo.pPicData, by, 0, iLen);
                    fs.Write(by, 0, iLen);
                    fs.Close();
                }
                szInfoBuf = szInfoBuf + "SavePath:" + path;
            }*/
            
            
            this.listViewEventos.BeginInvoke(new Action(() =>
            {
                if (tipoevento == "MINOR_FACE_VERIFY_PASS")
                {
                    string nombreempleado = null;
                    if (numerotarjeta != null)
                        nombreempleado = ObtenerNombreEmpleado(numerotarjeta);
                    ListViewItem Item = new ListViewItem();
                    m_lLogNum += 1;
                    Item.Text = m_lLogNum.ToString();
                    //Item.SubItems.Add(DateTime.Now.ToString());
                    //Item.SubItems.Add(szInfoBuf);
                    Item.SubItems.Add(fecha);
                    Item.SubItems.Add(hora);
                    Item.SubItems.Add(tipoevento);
                    Item.SubItems.Add(numerotarjeta);
                    Item.SubItems.Add(nombreempleado);
                    this.listViewEventos.Items.Add(Item);

                    InsertarEntradaSalida(m_lLogNum.ToString(), fecha, hora, numerotarjeta, tipoevento);
                }
                
            })); 
        }

        private void ConectarBaseDatos()
        {
            Common cmn = new Common();            
            string connstr = cmn.obtenerCadenaConexion();
            bd = new BaseDatosMySQL();
            bd.conectarMySQL(connstr);
        }


        



        private string ObtenerNombreEmpleado(string documento)
        {
            string retval = null;
            
            if (this.bd.conn != null)
            {
                string sql = "SELECT * FROM empleados WHERE documento = @documento";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);
                    cmd.Parameters.AddWithValue("@documento", documento);                    
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        
                        while (rdr.Read())
                        {
                            retval = rdr["nombres"].ToString() + " " + rdr["apellidos"].ToString();
                        }
                    }
                    rdr.Close();
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

        private void GestionEventos_Load(object sender, EventArgs e)
        {
            ConectarBaseDatos();
            Deploy();
        }

        private void GestionEventos_FormClosing(object sender, FormClosingEventArgs e)
        {
            bd.desconectarMySQL();           
            bool ret = HCNetSDK.NET_DVR_CloseAlarmChan(this.lAlarmHandle);
            
        }
    }
        
    
}
