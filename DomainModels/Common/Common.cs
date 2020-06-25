using DomainModels.Resources;
using System;
using System.Configuration;
using System.Net.NetworkInformation;

namespace DomainModels.Models
{
    public class Common
    {
        public const string ValidEmailValidation = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        public const string ValidIntegersValidation = "^[0-9]\\d*(\\.\\d+)?$";
        public const string DateFormat = "{0:yyyy-MM-dd}";
        public static string ValidDomain = EmployeeValidations.ValidDomain;

        #region [Password Encryption/Decryption]
        private static string _encryptedPassword = "";
        private static string _decryptedPassword = "";
        public static string EncryptionPrivateKey = ConfigurationManager.AppSettings["EncryptionPrivateKey"].ToString();

        /// <summary>
        /// Password Encryption
        /// </summary>
        public static string EncryptedPassword
        {
            get { return _encryptedPassword; }
            set { _encryptedPassword = CryptorEngine.Encrypt(value, true, EncryptionPrivateKey); }
        }

        /// <summary>
        /// Password Decryption
        /// </summary>
        public static string DecryptedPassword
        {
            get { return _decryptedPassword; }
            set { _decryptedPassword = CryptorEngine.Decrypt(value, true, EncryptionPrivateKey); }
        }
        #endregion

        #region [Get MAC Address of the Machine]
        /// <summary>
        /// Get MAC Address of the Machine
        /// </summary>
        /// <returns></returns>
        public static string GetMACAddress()
        {
            string macAddress = "";
            try
            {

                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType.ToString().Equals("ETHERNET", StringComparison.OrdinalIgnoreCase) && nic.Description.ToUpper().Contains("WIRELESS") == false && nic.Description.ToUpper().Contains("WIFI") == false)
                    {
                        macAddress = nic.GetPhysicalAddress().ToString();
                        break;
                    }
                }
                return macAddress;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        #endregion
    }
}

namespace DomainModels.Entities
{
    public struct UserInRoles
    {
        public static string SUPERADMIN = EmployeeValidations.RoleSuperAdmin;
        public static string ADMIN = EmployeeValidations.RoleAdmin;
        public static string DEV = EmployeeValidations.RoleDev;
        public static string HR = EmployeeValidations.RoleHR;
        public static string PM = EmployeeValidations.RolePM;
        public static string BDM = EmployeeValidations.RoleBDM;
    }
}