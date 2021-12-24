using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlEntradaSalida
{
    class Common
    {
        public static int m_UserID = -1;
        public static string ip = null;
        public static string puerto = null;
        public static string usuario = null;
        public static string contrasena = null;
        public static string datadir = null;
        public string obtenerCadenaConexion()
        {
            string cadenaConexion = null;
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
            Console.WriteLine(settings.ToString());
            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    cadenaConexion = cs.ConnectionString;
                }
            }
            return cadenaConexion;
        }



        public static bool InicializarSDKHikVision()
        {
            bool retval = false;
            if (HCNetSDK.NET_DVR_Init() == true)
            {
                retval = true;
            }
            return retval;
        }

        public static bool CrearDirectorioData()
        {
            bool retval = false;

            string commonData = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            commonData += @"\Neapps\ControlEntradaSalida\data";
            try
            {
                if (!Directory.Exists(commonData))
                    Directory.CreateDirectory(commonData);
                retval = true;
                datadir = commonData;
            } catch (Exception ex)
            {
                retval = false;
            }

            return retval;

        }
        public bool Login(string ip, string puerto, string usuario, string contrasena, out string msg)
        {
            bool ret = false;
            msg = null;

            HCNetSDK.NET_DVR_USER_LOGIN_INFO struLoginInfo = new HCNetSDK.NET_DVR_USER_LOGIN_INFO();
            HCNetSDK.NET_DVR_DEVICEINFO_V40 struDeviceInfoV40 = new HCNetSDK.NET_DVR_DEVICEINFO_V40();
            struDeviceInfoV40.struDeviceV30.sSerialNumber = new byte[HCNetSDK.SERIALNO_LEN];

            struLoginInfo.sDeviceAddress = ip;
            struLoginInfo.sUserName = usuario;
            struLoginInfo.sPassword = contrasena;
            ushort.TryParse(puerto, out struLoginInfo.wPort);

            int lUserID = -1;
            lUserID = HCNetSDK.NET_DVR_Login_V40(ref struLoginInfo, ref struDeviceInfoV40);
            if (lUserID >= 0)
            {
                Common.m_UserID = lUserID;
                ret = true;
            }
            else
            {
                uint nErr = HCNetSDK.NET_DVR_GetLastError();
                if (nErr == HCNetSDK.NET_DVR_PASSWORD_ERROR)
                {
                    msg = "User name or password error!";
                    if (1 == struDeviceInfoV40.bySupportLock)
                    {
                        string strTemp1 = string.Format("Left {0} try opportunity", struDeviceInfoV40.byRetryLoginTime);
                        msg += " " + strTemp1;
                    }
                }
                else if (nErr == HCNetSDK.NET_DVR_USER_LOCKED)
                {
                    if (1 == struDeviceInfoV40.bySupportLock)
                    {
                        string strTemp1 = string.Format("User is locked, the remaining lock time is {0}", struDeviceInfoV40.dwSurplusLockTime);
                        msg = strTemp1;
                    }
                }
                else
                {
                    msg = "Login fail error: " + nErr.ToString();
                }
            }
            return ret;
        }
    }
}
