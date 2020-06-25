using System.Configuration;

namespace WebApp
{
    public class Common
    {
        public static string ProfileImagePath = ConfigurationManager.AppSettings["ProfileImagePath"];
        public static string SiteURL = ConfigurationManager.AppSettings["SiteURL"];
        public static string SampleImagePath = ConfigurationManager.AppSettings["SampleImagePath"];
        public static string EmployeeDocFolderName = ConfigurationManager.AppSettings["EmployeeDocFolderName"];
        public static string FileTypeIcons = ConfigurationManager.AppSettings["FileTypeIcons"];
        public static string TempEmployeeDocPath = ConfigurationManager.AppSettings["TempEmployeeDocPath"];
    }
}