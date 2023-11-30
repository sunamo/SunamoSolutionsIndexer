using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SolutionsIndexerPaths
{
    public static readonly string listVpsNew = AppData.ci.GetFile(AppFolders.Data, "SlnVpsNew.txt");
    public static readonly string listSczAdmin64 = AppData.ci.GetFile(AppFolders.Data, "SlnSczAdmin64.txt");
}
