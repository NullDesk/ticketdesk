using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketDesk.Domain.Models;
using System.Data.Objects;
using TicketDesk.Domain.Services;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Data.EntityClient;

namespace TicketDesk.Domain.Repositories
{
    [Export(typeof(IDatabaseSchemaManagerRepository))]
    public class DatabaseSchemaManagerRepository : IDatabaseSchemaManagerRepository
    {
        [ImportingConstructor]
        public DatabaseSchemaManagerRepository(IApplicationSettingsService appService)
        {
            AppSettings = appService;
        }

        public IApplicationSettingsService AppSettings { get; set; }

        public void EnsureSchemaVersion()
        {
            try
            {
                var currentVersionSetting = AppSettings.CurrentSettings.SingleOrDefault(s => s.SettingName == "Version");
                if (currentVersionSetting == null)//if no version is set in DB yet, create initial version idicator first
                {
                    currentVersionSetting = new Setting() { SettingName = "Version", SettingValue = "2.0.0", DefaultValue = "2.0.0", SettingDescription = "The version of the TicketDesk database. CHANGE AT YOUR OWN RISK!", SettingType = "SimpleString" };
                    AppSettings.SaveSettings(new[] { currentVersionSetting });
                }
                UpgradeSchema("2.0.3");
            }
            catch { }
        }

        public void UpgradeSchema(string finalVersionValue)
        {
            var currentVersionSetting = AppSettings.CurrentSettings.SingleOrDefault(s => s.SettingName == "Version");
               
            if (finalVersionValue != currentVersionSetting.SettingValue)
            {
                currentVersionSetting.SettingValue = RunUpgrade(currentVersionSetting.SettingValue);
                AppSettings.SaveSettings(new[] { currentVersionSetting });
                UpgradeSchema(finalVersionValue);
            }
        }

        public string RunUpgrade(string version)
        {
            string q = null;
            string upgradedToVersion = null;
            switch (version)
            {
                case "2.0.0":
                    q = @"INSERT Settings( SettingName, SettingValue, DefaultValue, SettingType, SettingDescription) 
                          VALUES(
                                    'HideHomePage',
                                    'false',
                                    'false',
                                    'BoolString',
                                    'Hide the home tab from the main menu and makes TicketCenter default home page'
                                );
                       
                    ";
                    upgradedToVersion = "2.0.2";
                    break;
                case "2.0.2":
                    q = @"INSERT Settings( SettingName, SettingValue, DefaultValue, SettingType, SettingDescription) 
                          VALUES(
                                    'HelpDeskBroadcastNotificationsEnabled',
                                    'true',
                                    'true',
                                    'BoolString',
                                    'Send broadcast notifications to helpdesk for all new tickets'
                                );
                       
                    ";
                    upgradedToVersion = "2.0.3";
                    break;
            }
            if (!string.IsNullOrEmpty(q))
            {
                ExecuteUpgrade(q);
            }
            return upgradedToVersion;
        }

        public void ExecuteUpgrade(string script)
        {
            
            using (var ctx = new TicketDeskEntities())
            {
                var cString = (ctx.Connection as EntityConnection).StoreConnection.ConnectionString;//get connection string from entity framework context
                //entity framework doesn't help us with executing DML and DDL; so we go back to old-skoo ado. 
                SqlConnection conn = new SqlConnection(cString);
                SqlCommand cmd = new SqlCommand(script, conn);
                conn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
