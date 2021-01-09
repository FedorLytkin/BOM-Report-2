using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNRM_Kompas.Controllers
{
    class ConnectionString
    {
        public static string ConnStr
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DXApplication2.Properties.Settings.ConStr"].ConnectionString;
            }
        }
        private void NewPatch()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings.Remove("DXApplication2.Properties.Settings.ConStr");
            config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("DXApplication2.Properties.Settings.ConStr", "new connection string"));
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}
