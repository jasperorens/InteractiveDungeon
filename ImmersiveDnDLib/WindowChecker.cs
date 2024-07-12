using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmersiveDnDLib
{
    public class WindowChecker
    {
        public static bool IsWindowOpen(string windowTitle)
        {
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                if (process.MainWindowTitle.Contains(windowTitle))
                {
                    // Window with matching title found
                    return true;
                }
            }

            // No window with matching title found
            return false;
        }
    }
}
