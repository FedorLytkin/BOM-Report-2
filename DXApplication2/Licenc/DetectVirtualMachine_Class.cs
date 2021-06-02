using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

public class DetectVirtualMachine_Class
{
    public static bool DetectVirtualMachine()
    {
        bool result = false;
        const string MICROSOFTCORPORATION = "microsoft corporation";
        try
        {
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                result = queryObj["Manufacturer"].ToString().ToLower() == MICROSOFTCORPORATION.ToLower();
            }
            return result;
        }
        catch (ManagementException ex)
        {
            return result;
        }
    }
}
