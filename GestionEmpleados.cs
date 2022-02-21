using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlEntradaSalida 
{ 
    public partial class GestionEmpleados : Form
    {
        private int m_lCapFaceCfgHandle = -1;
        private int m_lSetFaceCfgHandle = -1;
        public Int32 m_lSetCardCfgHandle = -1;
        public Int32 m_lDelCardCfgHandle = -1;
        public Int32 m_lGetCardCfgHandle = -1;


        private int m_lGetFaceCfgHandle = -1;

        private string url_imagen;
        private bool nuevo = false;

        public GestionEmpleados()
        {
            InitializeComponent();
        }



        private void ProcesarFotoCapturada(ref HCNetSDK_Facial.NET_DVR_CAPTURE_FACE_CFG struFaceCfg, ref bool flag)
        {
            if (0 == struFaceCfg.dwFacePicSize)
            {
                return;
            }
            DateTime dt = DateTime.Now;
            string filePath = null;
            if (Common.datadir != null)
            {
                filePath = Common.datadir + "\\" + dt.ToString("yyyy-MM-dd_HH-mm-ss") + ".jpg"; ;
            } else
            {
                if (!Directory.Exists(Environment.CurrentDirectory + "\\imagenes\\"))
                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\imagenes\\");

                filePath = Environment.CurrentDirectory + "\\imagenes\\" + dt.ToString("yyyy-MM-dd_HH-mm-ss") + ".jpg";
            }

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    int FaceLen = struFaceCfg.dwFacePicSize;
                    byte[] by = new byte[FaceLen];
                    Marshal.Copy(struFaceCfg.pFacePicBuffer, by, 0, FaceLen);
                    fs.Write(by, 0, FaceLen);
                    fs.Close();
                }

                pictureBoxUsuario.Image = Image.FromFile(filePath);
                this.url_imagen = filePath;
            }
            catch
            {
                flag = false;
                MessageBox.Show("Captura de datos incorrecto.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void CapturarFoto()
        {
            if (m_lCapFaceCfgHandle != -1)
            {
                HCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lCapFaceCfgHandle);
                m_lCapFaceCfgHandle = -1;
            }
            if (this.pictureBoxUsuario.Image != null)
            {
                this.pictureBoxUsuario.Image.Dispose();
                this.pictureBoxUsuario.Image = null;
            }
            url_imagen = null;

            HCNetSDK_Facial.NET_DVR_CAPTURE_FACE_COND struCond = new HCNetSDK_Facial.NET_DVR_CAPTURE_FACE_COND();
            struCond.init();
            struCond.dwSize = Marshal.SizeOf(struCond);
            int dwInBufferSize = struCond.dwSize;
            IntPtr ptrStruCond = Marshal.AllocHGlobal(dwInBufferSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);
            m_lCapFaceCfgHandle = HCNetSDK_Facial.NET_DVR_StartRemoteConfig(Common.m_UserID, HCNetSDK_Facial.NET_DVR_CAPTURE_FACE_INFO, ptrStruCond, dwInBufferSize, null, IntPtr.Zero);
            if (-1 == m_lCapFaceCfgHandle)
            {
                Marshal.FreeHGlobal(ptrStruCond);
                MessageBox.Show("NET_DVR_CAP_FACE_FAIL, ERROR CODE" + HCNetSDK_Facial.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                return;
            }

            HCNetSDK_Facial.NET_DVR_CAPTURE_FACE_CFG struFaceCfg = new HCNetSDK_Facial.NET_DVR_CAPTURE_FACE_CFG();
            struFaceCfg.init();
            int dwStatus = 0;
            int dwOutBuffSize = Marshal.SizeOf(struFaceCfg);
            bool Flag = true;
            while (Flag)
            {
                dwStatus = HCNetSDK_Facial.NET_DVR_GetNextRemoteConfig(m_lCapFaceCfgHandle, ref struFaceCfg, dwOutBuffSize);
                switch (dwStatus)
                {
                    case HCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_SUCCESS://成功读取到数据，处理完本次数据后需调用next
                        ProcesarFotoCapturada(ref struFaceCfg, ref Flag);
                        break;
                    case HCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_NEED_WAIT:
                        break;
                    case HCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_FAILED:
                        HCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lCapFaceCfgHandle);
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_FAILED" + HCNetSDK_Facial.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                        Flag = false;
                        break;
                    case HCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_FINISH:
                        HCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lCapFaceCfgHandle);
                        Flag = false;
                        break;
                    default:
                        MessageBox.Show("NET_SDK_GET_STATUS_UNKOWN" + HCNetSDK_Facial.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                        Flag = false;
                        HCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lCapFaceCfgHandle);
                        break;
                }
            }
            Marshal.FreeHGlobal(ptrStruCond);
        }


        public bool CrearUsuarioSDK(string numtarjeta, string permisostarjeta, string numerousuario, string nombreusuario)
        {
            bool retval = false;
            if (m_lSetCardCfgHandle != -1)
            {
                if (HCNetSDK_Tarjeta.NET_DVR_StopRemoteConfig(m_lSetCardCfgHandle))
                {
                    m_lSetCardCfgHandle = -1;
                }
            }

            HCNetSDK_Tarjeta.NET_DVR_CARD_COND struCond = new HCNetSDK_Tarjeta.NET_DVR_CARD_COND();
            struCond.Init();
            struCond.dwSize = (uint)Marshal.SizeOf(struCond);
            struCond.dwCardNum = 1;
            IntPtr ptrStruCond = Marshal.AllocHGlobal((int)struCond.dwSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);

            m_lSetCardCfgHandle = HCNetSDK_Tarjeta.NET_DVR_StartRemoteConfig(Common.m_UserID, HCNetSDK_Tarjeta.NET_DVR_SET_CARD, ptrStruCond, (int)struCond.dwSize, null, IntPtr.Zero);
            if (m_lSetCardCfgHandle < 0)
            {
                MessageBox.Show("NET_DVR_SET_CARD error:" + HCNetSDK_Tarjeta.NET_DVR_GetLastError());
                Marshal.FreeHGlobal(ptrStruCond);                
                return retval;
            }
            else
            {
                bool result;
                result = EnviarDatosTarjeta(numtarjeta, permisostarjeta, numerousuario, nombreusuario);
                if (result)
                    retval = true;
                Marshal.FreeHGlobal(ptrStruCond);
            }
            return retval;
        }

        private bool EnviarDatosTarjeta(string numtarjeta, string permisostarjeta, string numerousuario, string nombreusuario)
        {
            bool retval = false;
            HCNetSDK_Tarjeta.NET_DVR_CARD_RECORD struData = new HCNetSDK_Tarjeta.NET_DVR_CARD_RECORD();
            struData.Init();
            struData.dwSize = (uint)Marshal.SizeOf(struData);
            struData.byCardType = 1;
            byte[] byTempCardNo = new byte[HCNetSDK_Tarjeta.ACS_CARD_NO_LEN];
            byTempCardNo = System.Text.Encoding.UTF8.GetBytes(numtarjeta);
            for (int i = 0; i < byTempCardNo.Length; i++)
            {
                struData.byCardNo[i] = byTempCardNo[i];
            }
            ushort.TryParse(permisostarjeta, out struData.wCardRightPlan[0]);
            uint.TryParse(numerousuario, out struData.dwEmployeeNo);
            byte[] byTempName = new byte[HCNetSDK_Tarjeta.NAME_LEN];
            byTempName = System.Text.Encoding.Default.GetBytes(nombreusuario);
            for (int i = 0; i < byTempName.Length; i++)
            {
                struData.byName[i] = byTempName[i];
            }
            //periodo de validez del usuario
            struData.struValid.byEnable = 1;
            struData.struValid.struBeginTime.wYear = 2000;
            struData.struValid.struBeginTime.byMonth = 1;
            struData.struValid.struBeginTime.byDay = 1;
            struData.struValid.struBeginTime.byHour = 11;
            struData.struValid.struBeginTime.byMinute = 11;
            struData.struValid.struBeginTime.bySecond = 11;
            struData.struValid.struEndTime.wYear = 2030;
            struData.struValid.struEndTime.byMonth = 1;
            struData.struValid.struEndTime.byDay = 1;
            struData.struValid.struEndTime.byHour = 11;
            struData.struValid.struEndTime.byMinute = 11;
            struData.struValid.struEndTime.bySecond = 11;
            //permiso en puerta
            struData.byDoorRight[0] = 1;
            struData.wCardRightPlan[0] = 1;
            IntPtr ptrStruData = Marshal.AllocHGlobal((int)struData.dwSize);
            Marshal.StructureToPtr(struData, ptrStruData, false);

            HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS struStatus = new HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS();
            struStatus.Init();
            struStatus.dwSize = (uint)Marshal.SizeOf(struStatus);
            IntPtr ptrdwState = Marshal.AllocHGlobal((int)struStatus.dwSize);
            Marshal.StructureToPtr(struStatus, ptrdwState, false);

            int dwState = (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS;
            uint dwReturned = 0;
            while (true)
            {
                dwState = HCNetSDK_Tarjeta.NET_DVR_SendWithRecvRemoteConfig(m_lSetCardCfgHandle, ptrStruData, struData.dwSize, ptrdwState, struStatus.dwSize, ref dwReturned);
                struStatus = (HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS)Marshal.PtrToStructure(ptrdwState, typeof(HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS));
                if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_NEEDWAIT)
                {
                    Thread.Sleep(10);
                    continue;
                }
                else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FAILED)
                {
                    MessageBox.Show("NET_DVR_SET_CARD fail error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError());
                }
                else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS)
                {
                    if (struStatus.dwErrorCode != 0)
                    {
                        MessageBox.Show("NET_DVR_SET_CARD success but errorCode:" + struStatus.dwErrorCode);
                    }
                    else
                    {
                        //MessageBox.Show("NET_DVR_SET_CARD success");
                        retval = true;
                    }
                }
                else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FINISH)
                {
                    //MessageBox.Show("NET_DVR_SET_CARD finish"); 
                    break;
                }
                else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_EXCEPTION)
                {
                    MessageBox.Show("NET_DVR_SET_CARD exception error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError());
                    break;
                }
                else
                {
                    MessageBox.Show("unknown status error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError());
                    break;
                }
            }
            HCNetSDK_Tarjeta.NET_DVR_StopRemoteConfig(m_lSetCardCfgHandle);
            m_lSetCardCfgHandle = -1;
            Marshal.FreeHGlobal(ptrStruData);
            Marshal.FreeHGlobal(ptrdwState);
            return retval;
        }

        private bool EnviarImagen(string numlectora, string numtarjeta)
        {
            bool retval = false;
            if (this.pictureBoxUsuario.Image != null)
            {
                this.pictureBoxUsuario.Image.Dispose();
                this.pictureBoxUsuario.Image = null;
            }

            HCNetSDK_Facial.NET_DVR_FACE_COND struCond = new HCNetSDK_Facial.NET_DVR_FACE_COND();
            struCond.init();
            struCond.dwSize = Marshal.SizeOf(struCond);
            struCond.dwFaceNum = 1;
            int.TryParse(numlectora, out struCond.dwEnableReaderNo);
            byte[] byTemp = System.Text.Encoding.UTF8.GetBytes(numtarjeta);
            for (int i = 0; i < byTemp.Length; i++)
            {
                struCond.byCardNo[i] = byTemp[i];
            }

            int dwInBufferSize = Convert.ToInt32(struCond.dwSize);
            IntPtr ptrstruCond = Marshal.AllocHGlobal(dwInBufferSize);
            Marshal.StructureToPtr(struCond, ptrstruCond, false);
            m_lSetFaceCfgHandle = HCNetSDK_Facial.NET_DVR_StartRemoteConfig(Common.m_UserID, HCNetSDK_Facial.NET_DVR_SET_FACE, ptrstruCond, dwInBufferSize, null, IntPtr.Zero);
            if (-1 == m_lSetFaceCfgHandle)
            {
                Marshal.FreeHGlobal(ptrstruCond);
                MessageBox.Show("NET_DVR_SET_FACE_FAIL, ERROR CODE" + HCNetSDK_Facial.NET_DVR_GetLastError().ToString());
                return retval;
            }

            HCNetSDK_Facial.NET_DVR_FACE_RECORD struRecord = new HCNetSDK_Facial.NET_DVR_FACE_RECORD();
            struRecord.init();
            struRecord.dwSize = Marshal.SizeOf(struRecord);

            byte[] byRecordNo = System.Text.Encoding.UTF8.GetBytes(numtarjeta);
            for (int i = 0; i < byRecordNo.Length; i++)
            {
                struRecord.byCardNo[i] = byRecordNo[i];
            }
            bool result = false;
            result = LeerDatosImagen(ref struRecord);
            int dwInBuffSize = Marshal.SizeOf(struRecord);
            int dwStatus = 0;

            HCNetSDK_Facial.NET_DVR_FACE_STATUS struStatus = new HCNetSDK_Facial.NET_DVR_FACE_STATUS();
            struStatus.init();
            struStatus.dwSize = Marshal.SizeOf(struStatus);
            int dwOutBuffSize = Convert.ToInt32(struStatus.dwSize);
            IntPtr ptrOutDataLen = Marshal.AllocHGlobal(sizeof(int));
            bool Flag = true;

            while (Flag)
            {
                dwStatus = HCNetSDK_Facial.NET_DVR_SendWithRecvRemoteConfig(m_lSetFaceCfgHandle, ref struRecord, dwInBuffSize, ref struStatus, dwOutBuffSize, ptrOutDataLen);
                switch (dwStatus)
                {
                    case HCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_SUCCESS:
                        ResultadoEnviarDatosImagen(ref struStatus, ref Flag);
                        if (Flag)
                            retval = true;
                        break;
                    case HCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_NEED_WAIT:
                        break;
                    case HCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_FAILED:
                        HCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lSetFaceCfgHandle);
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_FAILED" + HCNetSDK_Facial.NET_DVR_GetLastError().ToString());
                        Flag = false;
                        break;
                    case HCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_FINISH:
                        HCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lSetFaceCfgHandle);
                        Flag = false;
                        break;
                    default:
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_UNKOWN" + HCNetSDK_Facial.NET_DVR_GetLastError().ToString());
                        Flag = false;
                        HCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lSetFaceCfgHandle);
                        break;
                }
            }
            Marshal.FreeHGlobal(ptrstruCond);
            Marshal.FreeHGlobal(ptrOutDataLen);
            return retval;
        }

        private void ResultadoEnviarDatosImagen(ref HCNetSDK_Facial.NET_DVR_FACE_STATUS struStatus, ref bool flag)
        {
            switch (struStatus.byRecvStatus)
            {
                case 1:
                    //MessageBox.Show("SetFaceDataSuccessful"); 
                    break;
                default:
                    flag = false;
                    MessageBox.Show("NET_SDK_SET_Face_DATA_FAILED" + struStatus.byRecvStatus.ToString());
                    break;
            }

        }
        private bool LeerDatosImagen(ref HCNetSDK_Facial.NET_DVR_FACE_RECORD struRecord)
        {
            bool retval = false;
            if (this.pictureBoxUsuario.Image != null)
            {
                this.pictureBoxUsuario.Image.Dispose();
                this.pictureBoxUsuario.Image = null;
            }

            if (!File.Exists(url_imagen))
            {
                MessageBox.Show("The face picture does not exist!");
                return retval;
            }
            FileStream fs = new FileStream(url_imagen, FileMode.OpenOrCreate);
            if (0 == fs.Length)
            {
                MessageBox.Show("The face picture is 0k,please input another picture!");
                return retval;
            }
            if (200 * 1024 < fs.Length)
            {
                MessageBox.Show("The face picture is larger than 200k,please input another picture!");
                return retval;
            }
            try
            {
                int.TryParse(fs.Length.ToString(), out struRecord.dwFaceLen);
                int iLen = Convert.ToInt32(struRecord.dwFaceLen);
                byte[] by = new byte[iLen];
                struRecord.pFaceBuffer = Marshal.AllocHGlobal(iLen);
                fs.Read(by, 0, iLen);
                Marshal.Copy(by, 0, struRecord.pFaceBuffer, iLen);
                fs.Close();
                retval = true;
            }
            catch
            {
                MessageBox.Show("Read Face Data failed");
                fs.Close();
                return retval;
            }
            return retval;
        }

        private void buttonCapturarFoto_Click(object sender, EventArgs e)
        {
            
            string msg;
            bool retval;
            Common cmn = new Common();
            
            /* Eliminar la imagen en disco si se hizo una captura previa y se quiere descartar */
            if ( url_imagen != null && this.pictureBoxUsuario.Image != null && nuevo)
            {
                this.pictureBoxUsuario.Image.Dispose();
                this.pictureBoxUsuario.Image = null;

                if (System.IO.File.Exists(url_imagen))
                {    
                    bool result = EliminarImagen(url_imagen);
                    if (!result)
                    {
                        MessageBox.Show("Error al eliminar el archivo de la captura previa: " + url_imagen, "Eliminar captura previa", MessageBoxButtons.OK);
                    }
                }
            }

            retval = cmn.Login(Common.ip, Common.puerto, Common.usuario, Common.contrasena, out msg);
            if (retval)
            {
                CapturarFoto();
                this.buttonCapturarFoto.Enabled = false;
            }
            cmn = null;
            
        }

        public bool EliminarUsuarioDispositivo(string _numero_lectora, string _numero_tarjeta)
        {
            bool retval = false;
            if (m_lDelCardCfgHandle != -1)
            {
                if (HCNetSDK_Tarjeta.NET_DVR_StopRemoteConfig(m_lDelCardCfgHandle))
                {
                    m_lDelCardCfgHandle = -1;
                }
            }
            HCNetSDK_Tarjeta.NET_DVR_CARD_COND struCond = new HCNetSDK_Tarjeta.NET_DVR_CARD_COND();
            struCond.Init();
            struCond.dwSize = (uint)Marshal.SizeOf(struCond);
            struCond.dwCardNum = 1;
            IntPtr ptrStruCond = Marshal.AllocHGlobal((int)struCond.dwSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);

            HCNetSDK_Tarjeta.NET_DVR_CARD_SEND_DATA struSendData = new HCNetSDK_Tarjeta.NET_DVR_CARD_SEND_DATA();
            struSendData.Init();
            struSendData.dwSize = (uint)Marshal.SizeOf(struSendData);
            byte[] byTempCardNo = new byte[HCNetSDK_Tarjeta.ACS_CARD_NO_LEN];
            byTempCardNo = System.Text.Encoding.UTF8.GetBytes(_numero_tarjeta);
            for (int i = 0; i < byTempCardNo.Length; i++)
            {
                struSendData.byCardNo[i] = byTempCardNo[i];
            }
            IntPtr ptrStruSendData = Marshal.AllocHGlobal((int)struSendData.dwSize);
            Marshal.StructureToPtr(struSendData, ptrStruSendData, false);

            HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS struStatus = new HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS();
            struStatus.Init();
            struStatus.dwSize = (uint)Marshal.SizeOf(struStatus);
            IntPtr ptrdwState = Marshal.AllocHGlobal((int)struStatus.dwSize);
            Marshal.StructureToPtr(struStatus, ptrdwState, false);

            m_lGetCardCfgHandle = HCNetSDK_Tarjeta.NET_DVR_StartRemoteConfig(Common.m_UserID, HCNetSDK_Tarjeta.NET_DVR_DEL_CARD, ptrStruCond, (int)struCond.dwSize, null, this.Handle);
            if (m_lGetCardCfgHandle < 0)
            {
                MessageBox.Show("NET_DVR_DEL_CARD error:" + HCNetSDK_Tarjeta.NET_DVR_GetLastError());
                Marshal.FreeHGlobal(ptrStruCond);
                return retval;
            }
            else
            {
                int dwState = (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS;
                uint dwReturned = 0;
                while (true)
                {
                    dwState = HCNetSDK_Tarjeta.NET_DVR_SendWithRecvRemoteConfig(m_lGetCardCfgHandle, ptrStruSendData, struSendData.dwSize, ptrdwState, struStatus.dwSize, ref dwReturned);
                    if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_NEEDWAIT)
                    {
                        Thread.Sleep(10);
                        continue;
                    }
                    else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FAILED)
                    {
                        MessageBox.Show("NET_DVR_DEL_CARD fail error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError());
                    }
                    else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS)
                    {
                        //MessageBox.Show("NET_DVR_DEL_CARD success");
                        retval = true;
                    }
                    else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FINISH)
                    {
                        //MessageBox.Show("NET_DVR_DEL_CARD finish");
                        break;
                    }
                    else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_EXCEPTION)
                    {
                        MessageBox.Show("NET_DVR_DEL_CARD exception error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError());
                        break;
                    }
                    else
                    {
                        MessageBox.Show("Unknown status error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError());
                        break;
                    }
                }
            }
            HCNetSDK_Tarjeta.NET_DVR_StopRemoteConfig(m_lDelCardCfgHandle);
            m_lDelCardCfgHandle = -1;
            Marshal.FreeHGlobal(ptrStruSendData);
            Marshal.FreeHGlobal(ptrdwState);
            return retval;
        }
        private int ConsultarUltimoID()
        {
            int retval = 0;
            Common cmn = new Common();
            string connstr = cmn.obtenerCadenaConexion();
            BaseDatosMySQL bd = new BaseDatosMySQL();
            bd.conectarMySQL(connstr);
            if (bd.conn != null)
            {
                string sql = "SELECT MAX(id) AS lastid FROM empleados";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        
                        while (rdr.Read())
                        {
                            retval = Convert.ToInt32(rdr["lastid"].ToString());
                        }
                        rdr.Close();
                        bd.desconectarMySQL();
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

        private bool AgregarUsuarioListView()
        {
            bool retval = false;
            //int lastid = 0;
            //lastid = ConsultarUltimoID();

            string nombrecompleto = this.textBoxNombres.Text + " " + this.textBoxApellidos.Text;
            if (nombrecompleto.Length > 30)
                nombrecompleto = nombrecompleto.Substring(0, 29);

            ListViewItem lvi = new ListViewItem(this.textBoxDocumento.Text);
            lvi.SubItems.Add(this.cmbEstado.Text);
            lvi.SubItems.Add(this.textBoxDocumento.Text);           
            lvi.SubItems.Add(this.textBoxNombres.Text);
            lvi.SubItems.Add(this.textBoxApellidos.Text);            
            lvi.SubItems.Add(this.url_imagen);
            listView.Items.Add(lvi);
            lvi = null;
            LimpiarControles(false);
            url_imagen = null;
             
            retval = true;
            return retval;

        }
        private bool CargarDatosTablaEmpleados()
        {
            bool retval = false;
            Common cmn = new Common();
            string connstr = cmn.obtenerCadenaConexion();
            BaseDatosMySQL bd = new BaseDatosMySQL();
            bd.conectarMySQL(connstr);
            if (bd.conn != null)
            {
                string sql = "SELECT * FROM empleados";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            ListViewItem lvi = new ListViewItem(rdr["documento"].ToString());
                            lvi.SubItems.Add(rdr["estado"].ToString());                           
                            lvi.SubItems.Add(rdr["numtarjeta"].ToString());
                            lvi.SubItems.Add(rdr["nombres"].ToString());
                            lvi.SubItems.Add(rdr["apellidos"].ToString());
                            lvi.SubItems.Add(rdr["foto"].ToString());                            
                            listView.Items.Add(lvi);
                            lvi = null;
                        }
                        rdr.Close();
                        bd.desconectarMySQL();
                        retval = true;
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

        private bool InsertarUsuario()
        {
            bool retval = false;
            Common cmn = new Common();
            string connstr = cmn.obtenerCadenaConexion();
            BaseDatosMySQL bd = new BaseDatosMySQL();
            bd.conectarMySQL(connstr);
            if (bd.conn != null)
            {
                string sql = "INSERT INTO empleados "+
                    "(documento, numtarjeta, nombres, apellidos, foto, estado, created)" +
                    "VALUES (@documento, @documento, @nombres, @apellidos, @foto, @estado, @created)";
                try
                {
                    string nombrecompleto = this.textBoxNombres.Text + " " + this.textBoxApellidos.Text;
                    if (nombrecompleto.Length > 30)                   
                        nombrecompleto = nombrecompleto.Substring(0, 29);                    
                    
                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);
                    cmd.Parameters.AddWithValue("@documento", this.textBoxDocumento.Text);
                    cmd.Parameters.AddWithValue("@nombres", this.textBoxNombres.Text);
                    cmd.Parameters.AddWithValue("@apellidos", this.textBoxApellidos.Text);
                    cmd.Parameters.AddWithValue("@nombrecompleto", nombrecompleto);
                    cmd.Parameters.AddWithValue("@foto", this.url_imagen);
                    cmd.Parameters.AddWithValue("@estado", this.cmbEstado.Text);
                    cmd.Parameters.AddWithValue("@created", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.ExecuteNonQuery();
                    bd.desconectarMySQL();
                    
                    retval = true;

                } catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            } else {
                MessageBox.Show(bd.errormsg);
            }        
            return retval;
        }

        private bool EliminarUsuario(string documento)
        {
            bool retval = false;
            Common cmn = new Common();
            string connstr = cmn.obtenerCadenaConexion();
            BaseDatosMySQL bd = new BaseDatosMySQL();
            bd.conectarMySQL(connstr);
            if (bd.conn != null)
            {
                string sql = "CALL ELIMINAR_EMPLEADO(@documento)";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);
                    cmd.Parameters.AddWithValue("@documento", documento);
                    cmd.ExecuteNonQuery();
                    bd.desconectarMySQL();
                    retval = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            } else
            {
                MessageBox.Show(bd.errormsg);
            }
            return retval;
        }


        private void ActualizarItemListView()
        {
            if (listView.SelectedItems.Count == 1)
            {
                ListViewItem item = this.listView.SelectedItems[0];
                item.SubItems[1].Text = cmbEstado.Text;
                item.SubItems[3].Text = this.textBoxNombres.Text;
                item.SubItems[4].Text = this.textBoxApellidos.Text;
                item.SubItems[5].Text = "";                
            }
        }

        private bool ActualizarInfoUsuarioBD(string documento)
        {
            bool retval = false;
            Common cmn = new Common();
            string connstr = cmn.obtenerCadenaConexion();
            BaseDatosMySQL bd = new BaseDatosMySQL();
            bd.conectarMySQL(connstr);
            if (bd.conn != null)
            {
                string sql = "UPDATE empleados SET nombres = @nombres, " +
                    "apellidos = @apellidos, estado = @estado, modified = @modified " +
                    "WHERE documento = @documento";
                try
                {
                    string nombrecompleto = this.textBoxNombres.Text + " " + this.textBoxApellidos.Text;
                    if (nombrecompleto.Length > 30)
                        nombrecompleto = nombrecompleto.Substring(0, 29);

                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);
                    cmd.Parameters.AddWithValue("@documento", this.textBoxDocumento.Text);
                    cmd.Parameters.AddWithValue("@nombres", this.textBoxNombres.Text);
                    cmd.Parameters.AddWithValue("@apellidos", this.textBoxApellidos.Text);                    
                    cmd.Parameters.AddWithValue("@estado", this.cmbEstado.Text);                    
                    cmd.Parameters.AddWithValue("@modified", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.ExecuteNonQuery();
                    bd.desconectarMySQL();

                    retval = true;
                    ActualizarItemListView();

                }
                catch (MySqlException ex)
                {
                    string errstr = ex.Number.ToString() + " " + ex.Message;
                    MessageBox.Show(errstr, "Error en ActualizarInfoUsuarioBD()", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            else
            {
                string errstr = bd.errornum + " " + bd.errormsg + "bd = null";
                MessageBox.Show(errstr, "Error en ActualizarInfoUsuarioBD()", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return retval;
        }

        private bool ActualizarInfoUsuarioDispositivo(string documento)
        {
            bool retval = false;

            string url = "PUT /ISAPI/AccessControl/UserInfo/Modify?format=json";
            string UserInfo = "{{ \"UserInfo\" : {{\"employeeNo\": \"{0}\",\"name\": \"{1}\",\"userType\": \"normal\",   \"Valid\" : {{ \"enable\": true,\"beginTime\": \"{2}\",\"endTime\": \"{3}\", \"timeType\": \"local\"}}  }}}}";

            string nombrecompleto = this.textBoxNombres.Text + " " + this.textBoxApellidos.Text;
            if (nombrecompleto.Length > 30)
                nombrecompleto = nombrecompleto.Substring(0, 29);

            string fechainicio = "2022-01-01T00:00:00";
            string fechafinal = "2031-01-01T23:59:00";
            string UserInfoValues = String.Format(UserInfo, documento, nombrecompleto, fechainicio, fechafinal);


            string outputString = null;
            string outputStatus = null;
            bool result = false;
            Common cmn = new Common();
            result = cmn.ISAPIQuery(url, UserInfoValues, out outputString, out outputStatus);
            string xmlresult = "";
            dynamic DynamicData;
            if (!result)
            {
                try
                {
                    xmlresult = outputStatus;
                    DynamicData = JsonConvert.DeserializeObject(xmlresult);
                    string statusCode = DynamicData.statusCode;
                    string statusString = DynamicData.statusString;
                    string subStatusCode = DynamicData.subStatusCode;
                    string errorCode = DynamicData.errorCode;
                    string errorMsg = DynamicData.errorMsg;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en ActualizarInfoUsuarioDispositivo()", ex.Message.ToString());
                }

            }
            else
            {
                try
                {
                    xmlresult = outputString;
                    DynamicData = JsonConvert.DeserializeObject(xmlresult);
                    string statusCode = DynamicData.statusCode;
                    string statusString = DynamicData.statusString;
                    string subStatusCode = DynamicData.subStatusCode;

                    if (statusCode == "1" && statusString == "OK" && subStatusCode == "ok")
                        retval = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en ActualizarInfoUsuarioDispositivo()", ex.Message.ToString());
                }
            }
            return retval;
        }
        private void buttonAgregar_Click(object sender, EventArgs e)
        {

            if (!ValidarDocumento() || !ValidarEstado() || !ValidarNombres() || !ValidarApellidos())
                return;

            string msg;
            bool retval;
            Common cmn = new Common();
            retval = cmn.Login(Common.ip, Common.puerto, Common.usuario, Common.contrasena, out msg);
            if (retval)
            {
                bool resultcrear = false;
                bool resultenviar = false;
                string nombrecompleto = this.textBoxNombres.Text + " " + this.textBoxApellidos.Text;
                if (nombrecompleto.Length > 30)
                    nombrecompleto = nombrecompleto.Substring(0, 29);

                

                if (nuevo)
                {
                    resultcrear = CrearUsuarioSDK(this.textBoxDocumento.Text, "1", this.textBoxDocumento.Text, nombrecompleto);

                    if (this.pictureBoxUsuario.Image != null)
                        resultenviar = EnviarImagen("1", this.textBoxDocumento.Text);
                    else
                        resultenviar = true;

                    if (resultcrear && resultenviar)
                    {
                        InsertarUsuario();
                        AgregarUsuarioListView();
                        LimpiarControles(false);
                        url_imagen = null;
                    }
                }
                if (!nuevo)
                {
                    bool resultactbd = false; ;
                    bool resultactdispo = ActualizarInfoUsuarioDispositivo(this.textBoxDocumento.Text);

                    if (this.pictureBoxUsuario.Image != null)
                        resultenviar = EnviarImagen("1", this.textBoxDocumento.Text);
                    else
                        resultenviar = true;

                    if (resultactdispo && resultenviar)
                        resultactbd = ActualizarInfoUsuarioBD(this.textBoxDocumento.Text);

                    if (!resultactdispo || !resultenviar)
                        MessageBox.Show("No se pudo actualizar el usuario (información y/o foto) en el dispositivo", "Error de actualizacion", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (!resultactbd)
                        MessageBox.Show("No se pudo actualizar el usuario en la base de datos local", "Error de actualizacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarControles(false);
                }
            }
            cmn = null;
        }

        private void GestionUsuarios_Load(object sender, EventArgs e)
        {
            if (Common.m_UserID < 0)
            {
                MessageBox.Show("Debe iniciar sesión en el dispositivo", "Error de inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.groupBox.Enabled = false;
                return;
            }
            this.groupBox.Enabled = true;
            LimpiarControles(true);
            CargarDatosTablaEmpleados();
        }

        private bool EliminarImagen(string filePath)
        {
            bool retval = false;
            try
            {
                System.IO.File.Delete(filePath);
                retval = true;
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                
            }
            return retval;
        }
        private void LimpiarControles(bool todos)
        {
            this.cmbEstado.Text = this.cmbEstado.Items[0].ToString();
            this.textBoxDocumento.Enabled = true;
            this.textBoxDocumento.Text = "";            
            this.textBoxNombres.Text = "";
            this.textBoxApellidos.Text = "";
            this.buttonAgregar.Enabled = true;

            if (this.pictureBoxUsuario.Image != null)
            {
                this.pictureBoxUsuario.Image.Dispose();
                this.pictureBoxUsuario.Image = null;
            }
            if (todos == true)
            {
                if (this.listView.Items.Count > 0)
                    this.listView.Items.Clear();
                
            }
            url_imagen = null;
            nuevo = true;
            this.buttonCapturarFoto.Enabled = true;
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
                string msg;
                bool retval;
                Common cmn = new Common();
                retval = cmn.Login(Common.ip, Common.puerto, Common.usuario, Common.contrasena, out msg);
                if (retval)
                {
                    ListView.CheckedListViewItemCollection itemColl = this.listView.CheckedItems;
                    foreach (ListViewItem item in itemColl) 
                    {
                        string documento = item.Text;
                        bool result = false;
                        result = EliminarUsuarioDispositivo("1", documento);
                        if (result)
                        {
                            result = EliminarUsuario(documento);
                            if (result)
                            {
                                /*result = EliminarImagen(item.SubItems[5].Text);
                                if (!result)
                                {
                                    MessageBox.Show("No se pudo eliminar la imagen del usuario: " + url_imagen + " por favor revise.", "Eliminar usuario", MessageBoxButtons.OK);
                                }*/
                            }
                            else
                            {
                                MessageBox.Show("Se eliminó el usuario del dispositivo pero no se logró eliminar de la base de datos de MySQL por favor revise.", "Eliminar usuario", MessageBoxButtons.OK);
                            }
                        }                  
                    }
                    LimpiarControles(true);
                    CargarDatosTablaEmpleados();
                    
                }
                    
            }
                
        }

        private bool BuscarImagenUsuario(string documento)
        {
            bool retval = false;
            if (m_lGetFaceCfgHandle != -1)
            {
                HCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lGetFaceCfgHandle);
                m_lGetFaceCfgHandle = -1;
            }

            if (this.pictureBoxUsuario.Image != null)
            {
                pictureBoxUsuario.Image.Dispose();
                pictureBoxUsuario.Image = null;
            }

            HCNetSDK_Facial.NET_DVR_FACE_COND struCond = new HCNetSDK_Facial.NET_DVR_FACE_COND();
            struCond.init();
            struCond.dwSize = Marshal.SizeOf(struCond);
            int dwSize = struCond.dwSize;
            struCond.dwEnableReaderNo = 1;
            struCond.dwFaceNum = 1;// The numbre of faces is 1
            byte[] byTemp = System.Text.Encoding.UTF8.GetBytes(documento);
            for (int i = 0; i < byTemp.Length; i++)
            {
                struCond.byCardNo[i] = byTemp[i];
            }

            IntPtr ptrStruCond = Marshal.AllocHGlobal(dwSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);

            m_lGetFaceCfgHandle = HCNetSDK_Facial.NET_DVR_StartRemoteConfig(Common.m_UserID, HCNetSDK_Facial.NET_DVR_GET_FACE, ptrStruCond, dwSize, null, IntPtr.Zero);
            if (m_lGetFaceCfgHandle == -1)
            {
                Marshal.FreeHGlobal(ptrStruCond);
                MessageBox.Show("NET_DVR_GET_FACE_FAIL, ERROR CODE" + HCNetSDK_Facial.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                return false;
            }

            bool Flag = true;
            int dwStatus = 0;

            HCNetSDK_Facial.NET_DVR_FACE_RECORD struRecord = new HCNetSDK_Facial.NET_DVR_FACE_RECORD();
            struRecord.init();
            struRecord.dwSize = Marshal.SizeOf(struRecord);
            int dwOutBuffSize = struRecord.dwSize;
            while (Flag)
            {
                dwStatus = HCNetSDK_Facial.NET_DVR_GetNextRemoteConfig(m_lGetFaceCfgHandle, ref struRecord, dwOutBuffSize);
                switch (dwStatus)
                {
                    case HCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_SUCCESS://The data is successfully read. After processing this data, you need to call next
                        ProcessFaceDataFromDevice(ref struRecord, ref Flag);
                        if (Flag)
                            retval = true;
                        break;
                    case HCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_NEED_WAIT:
                        break;
                    case HCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_FAILED:
                        HCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lGetFaceCfgHandle);
                        //MessageBox.Show("NET_SDK_GET_NEXT_STATUS_FAILED" + HCNetSDK_Facial.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                        Flag = false;
                        break;
                    case HCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_FINISH:
                        // MessageBox.Show("NET_SDK_GET_NEXT_STATUS_FINISH", "Tips", MessageBoxButtons.OK);
                        HCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lGetFaceCfgHandle);
                        Flag = false;
                        break;
                    default:
                        //MessageBox.Show("NET_SDK_GET_STATUS_UNKOWN" + HCNetSDK_Facial.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                        Flag = false;
                        HCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lGetFaceCfgHandle);
                        break;
                }
            }

            Marshal.FreeHGlobal(ptrStruCond);
            return retval;
        }

        private void ProcessFaceDataFromDevice(ref HCNetSDK_Facial.NET_DVR_FACE_RECORD struRecord, ref Boolean Flag)
        {
            string strpath = null;
            DateTime dt = DateTime.Now;
            string filePathName = null;
            if (Common.datadir != null)
            {
                filePathName = Common.datadir + "\\" + dt.ToString(this.textBoxDocumento.Text + "_yyyy-MM-dd_HH-mm-ss") + ".jpg"; ;
            }
            else
            {
                if (!Directory.Exists(Environment.CurrentDirectory + "\\imagenes\\"))
                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\imagenes\\");

                filePathName = Environment.CurrentDirectory + "\\imagenes\\" + dt.ToString(this.textBoxDocumento.Text + "_yyyy-MM-dd_HH-mm-ss") + ".jpg";
            }

            strpath = filePathName;

            if (0 == struRecord.dwFaceLen)
            {
                return;
            }

            if (pictureBoxUsuario.Image != null)
            {
                pictureBoxUsuario.Image.Dispose();
                pictureBoxUsuario.Image = null;
            }

            try
            {
                using (FileStream fs = new FileStream(strpath, FileMode.OpenOrCreate))
                {
                    int FaceLen = struRecord.dwFaceLen;
                    byte[] by = new byte[FaceLen];
                    Marshal.Copy(struRecord.pFaceBuffer, by, 0, FaceLen);
                    fs.Write(by, 0, FaceLen);
                    fs.Close();
                }
                pictureBoxUsuario.Image = Image.FromFile(strpath);
                this.url_imagen = filePathName;
            }
            catch
            {
                Flag = false;
                HCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lGetFaceCfgHandle);
                MessageBox.Show("ProcessFaceData failed", "Error", MessageBoxButtons.OK);
            }
        }

        private void listView_Click(object sender, EventArgs e)
        {
            if  (listView.SelectedItems.Count > 0)                
            {
                nuevo = false;
                //this.buttonCapturarFoto.Enabled = false;
                ListViewItem item = listView.SelectedItems[0];
                this.textBoxDocumento.Text = item.SubItems[0].Text;
                this.cmbEstado.Text = item.SubItems[1].Text;
                this.textBoxTarjeta.Text = item.SubItems[2].Text;
                this.textBoxNombres.Text = item.SubItems[3].Text;
                this.textBoxApellidos.Text = item.SubItems[4].Text;
                this.textBoxDocumento.Enabled = false;
                //this.pictureBoxUsuario.ImageLocation = item.SubItems[5].Text;
                //this.url_imagen = item.SubItems[5].Text;
                BuscarImagenUsuario(this.textBoxDocumento.Text);
                
            }
        }


        private string ObtenerExpresionQuery()
        {
            string retval = null;

            retval = "SELECT * FROM empleados WHERE 1 = 1 ";

            if (this.textBoxDocumento.Text.Trim() != "")
                retval += String.Format("AND documento LIKE '%{0}%' ", this.textBoxDocumento.Text);
            if (this.cmbEstado.Text.Trim() != "")
                retval += String.Format("AND estado LIKE '%{0}%' ", this.cmbEstado.Text);
            if (this.textBoxNombres.Text.Trim() != "")
                retval += String.Format("AND nombres LIKE '%{0}%' ", this.textBoxNombres.Text);
            if (this.textBoxApellidos.Text.Trim() != "")
                retval += String.Format("AND apellidos LIKE '%{0}%' ", this.textBoxApellidos.Text);           
            return retval;
        }

        private void filtrar(string query)
        {
            Common cmn = new Common();
            string connstr = cmn.obtenerCadenaConexion();
            BaseDatosMySQL bd = new BaseDatosMySQL();
            bd.conectarMySQL(connstr);
            if (bd.conn != null)
            {
                string sql = query;
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, bd.conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        if (this.listView.Items.Count > 0)
                            this.listView.Items.Clear();

                        while (rdr.Read())
                        {
                            
                            ListViewItem lvi = new ListViewItem(rdr["documento"].ToString());
                            lvi.SubItems.Add(rdr["estado"].ToString());
                            lvi.SubItems.Add(rdr["numtarjeta"].ToString());
                            lvi.SubItems.Add(rdr["nombres"].ToString());
                            lvi.SubItems.Add(rdr["apellidos"].ToString());
                            lvi.SubItems.Add(rdr["foto"].ToString());
                            listView.Items.Add(lvi);
                            lvi = null;
                        }
                        rdr.Close();
                        bd.desconectarMySQL();
                        
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
            
        }
        private void buttonFiltrar_Click(object sender, EventArgs e)
        {
            string query = ObtenerExpresionQuery().Trim();
            filtrar(query);
            Console.WriteLine(query);
        }

        private void buttonNuevo_Click(object sender, EventArgs e)
        {
            LimpiarControles(true);
        }

        private bool ValidarOpcionCombo(ComboBox cmb)
        {

            string textocombo = cmb.Text;

            ComboBox.ObjectCollection items = cmb.Items;
            bool encontrado = false;

            if (items.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].ToString() == textocombo)
                    {
                        encontrado = true;
                        break;
                    }

                }
            }
            return encontrado;

        }

        private bool ValidarEstado()
        {
            bool retval = false;
            if (string.IsNullOrEmpty(this.cmbEstado.Text))
            {
                errorProvider.SetError(this.cmbEstado, "No puede estar en blanco");
            }
            else
            {
                bool encontrado = ValidarOpcionCombo(this.cmbEstado);

                if (!encontrado)
                    errorProvider.SetError(this.cmbEstado, "Opción no válida para este campo");
                else
                {
                    errorProvider.SetError(this.cmbEstado, "");
                    retval = true;
                }
            }
            return retval;
        }


        private bool ValidarDocumento()
        {
            bool retval = false;

            try
            {
                if (string.IsNullOrEmpty(this.textBoxDocumento.Text) || this.textBoxDocumento.Text.Trim().Length == 0) 
                {
                    errorProvider.SetError(this.textBoxDocumento, "No puede estar en blanco");
                }
                else
                {
                    int valtemp = int.Parse(this.textBoxDocumento.Text);
                    if (valtemp <= 0)
                        errorProvider.SetError(this.textBoxDocumento, "Valor no permitido");
                    else
                    {
                        errorProvider.SetError(this.textBoxDocumento, "");
                        retval = true;
                    }
                }
            }
            catch
            {
                errorProvider.SetError(this.textBoxDocumento, "Debe ser un número");
            }

            return retval;

        }


        private void textBoxDocumento_Validating(object sender, CancelEventArgs e)
        {
            ValidarDocumento();
        }

        private void cmbEstado_Validating(object sender, CancelEventArgs e)
        {
            ValidarEstado();
        }


        private bool ValidarNombres()
        {
            bool retval = false;
            if (string.IsNullOrEmpty(this.textBoxNombres.Text))
                errorProvider.SetError(this.textBoxNombres, "No puede estar en blanco");
            else
            {
                errorProvider.SetError(this.textBoxNombres, "");
                retval = true;
            }


            return retval;
        }

        private bool ValidarApellidos()
        {
            bool retval = false;
            if (string.IsNullOrEmpty(this.textBoxApellidos.Text))
                errorProvider.SetError(this.textBoxApellidos, "No puede estar en blanco");
            else
            {
                errorProvider.SetError(this.textBoxApellidos, "");
                retval = true;
            }


            return retval;
        }

        private void textBoxNombres_Validating(object sender, CancelEventArgs e)
        {
            ValidarNombres();
        }

        private void textBoxApellidos_Validating(object sender, CancelEventArgs e)
        {
            ValidarApellidos();
        }
    }
}
