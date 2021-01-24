using System.Linq;
using System.Management;
using System.Net.NetworkInformation;

namespace Easy_Licensing
{
    /// <summary>
    /// Class to search for and return hardware identity values
    /// </summary>
    public static class HardwareIdentityService
    {
        /// <summary>
        /// Checks to see whether the current machine is a virtual machine
        /// </summary>
        public static bool IsVirtualMachine()
        {
            var isVm = false;

            using (var searcher = new ManagementObjectSearcher("SELECT Manufacturer, Model FROM Win32_ComputerSystem"))
            {
                foreach (var item in searcher.Get())
                {
                    var manufacturer = item["Manufacturer"].ToString().ToLower();
                    var model = item["Model"].ToString();

                    if (
                        (manufacturer == "microsoft corporation" && model.ToUpperInvariant().Contains("VIRTUAL")) ||
                        manufacturer.Contains("vmware") ||
                        model == "VirtualBox"
                    )
                    {
                        isVm = true;
                    }
                }
            }

            return isVm;
        }

        /// <summary>
        /// Finds and returns the serial number for the CPU
        /// </summary>
        /// <param name="cpu">For machines with multiple CPUs this specifies which CPU serial to return</param>
        public static string GetCpuSerialNumber(int cpu = 0)
        {
            var cpuSerial = "";
            var i = 0;

            using (var searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_processor"))
            {
                foreach (var item in searcher.Get())
                {
                    if (cpu == i)
                    {
                        cpuSerial = item["ProcessorId"].ToString();
                        break;
                    }

                    i++;
                }
            }

            return cpuSerial;
        }

        /// <summary>
        /// Finds and returns the serial number for the disk drive
        /// </summary>
        /// <param name="drive">For machines with multiple disk drives this specifies which drive serial to return</param>
        public static string GetDriveSerialNumber(int drive = 0)
        {
            var driveSerial = "";

            using (var searcher = new ManagementObjectSearcher("SELECT SerialNumber, Tag FROM Win32_PhysicalMedia"))
            {
                foreach (var item in searcher.Get())
                {
                    var tag = item["Tag"].ToString();

                    if (tag.Contains((char)drive) && tag.Contains("PHYSICALDRIVE"))
                    {
                        driveSerial = item["SerialNumber"].ToString();
                        break;
                    }
                }
            }

            return driveSerial;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetInterfaceMacAddress()
        {
            var macAddress = "";

            var interfaces = NetworkInterface.GetAllNetworkInterfaces().Where(x =>
                x.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                x.NetworkInterfaceType == NetworkInterfaceType.FastEthernetT ||
                x.NetworkInterfaceType == NetworkInterfaceType.FastEthernetFx ||
                x.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                x.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet ||
                x.NetworkInterfaceType == NetworkInterfaceType.Wwanpp
            );

            foreach (var nic in interfaces)
            {
                macAddress = nic.GetPhysicalAddress().ToString();
            }

            return macAddress;
        }
    }
}
