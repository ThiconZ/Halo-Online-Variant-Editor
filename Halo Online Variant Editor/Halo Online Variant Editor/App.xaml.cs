using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Halo_Online_Variant_Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            for (int i = 0; i < e.Args.Count(); i++)
            {
                string arg = e.Args[i].ToString();
                if (arg.ToLower().StartsWith("--") == true)
                {
                    switch (arg.ToLower())
                    {
                        case "--miniupdater":
                            this.Properties["MiniUpdater"] = arg;
                            break;
                        case "--nevernewvariant":
                            this.Properties["NeverNewVariant"] = arg;
                            break;
                        case "--verbose":
                            this.Properties["Verbose"] = arg;
                            break;
                        default:
                            break;
                    }
                }
                else if (i == 0 || i == e.Args.Count() - 1)
                {
                    this.Properties["OpenFilePath"] = e.Args[i].ToString();
                }
                    
            }
            base.OnStartup(e);
        }
    }
}
