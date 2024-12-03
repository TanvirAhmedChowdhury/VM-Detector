using System; // Provides fundamental classes and base functionality for .NET applications.
using System.Drawing; // Contains classes for working with graphics, including drawing and imaging.
using System.Drawing.Imaging; // Provides advanced imaging functionality.
using System.Management; // Provides access to Windows Management Instrumentation (WMI) for querying system information.
using System.Windows.Forms; // Provides classes for creating Windows-based applications with graphical user interfaces (GUIs).

namespace VM_Detector.Module.System // Defines the namespace, grouping related classes for VM detection.
{
    internal class VM // Declares an internal class named `VM` for virtual machine detection.
    {
        public static void DetectVM() // Declares a static method to detect virtual machine environments.
        {
            try
            {
                // Screenshot capturing
                using (Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
                {
                    // Creates a Bitmap object with dimensions equal to the primary screen's resolution.
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        // Creates a Graphics object for drawing on the Bitmap.
                        g.CopyFromScreen(0, 0, 0, 0, bmp.Size);
                        // Copies the entire screen's content to the Bitmap starting at the top-left corner (0, 0).
                        bmp.Save("Screenshot.jpeg", ImageFormat.Jpeg);
                        // Saves the captured screen image as a JPEG file in the current working directory.
                    }
                }
                //MessageBox.Show("Screenshot Captured Successfully!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Displays a message box confirming that the screenshot was successfully captured.

                // VM Detection Variables
                string systemManufacturer = string.Empty; // Initializes a variable to hold the system manufacturer name.
                string systemModel = string.Empty; // Initializes a variable to hold the system model name.
                string gpuName = string.Empty; // Initializes a variable to hold the GPU (graphics card) name.

                // Querying Computer System Information
                using (ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem"))
                {
                    // Creates a WMI query to retrieve system information (manufacturer and model).
                    foreach (ManagementObject obj in searcher1.Get())
                    {
                        // Iterates through the results of the query.
                        systemManufacturer = obj["Manufacturer"]?.ToString() ?? string.Empty;
                        // Extracts the "Manufacturer" property, or sets it to an empty string if null.
                        systemModel = obj["Model"]?.ToString() ?? string.Empty;
                        // Extracts the "Model" property, or sets it to an empty string if null.
                    }
                }

                // Querying GPU Information
                using (ManagementObjectSearcher searcher2 = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController"))
                {
                    // Creates a WMI query to retrieve GPU information.
                    foreach (ManagementObject obj in searcher2.Get())
                    {
                        // Iterates through the results of the query.
                        gpuName = obj["Name"]?.ToString() ?? string.Empty;
                        // Extracts the "Name" property of the GPU, or sets it to an empty string if null.
                    }
                }

                // Check for VM indicators
                if (systemManufacturer.ToLower().Contains("vmware") ||
                    // Checks if the system manufacturer contains "vmware" (common in virtual environments).
                    systemModel.ToLower().Contains("virtual") ||
                    // Checks if the system model contains "virtual" (another VM indicator).
                    gpuName.ToLower().Contains("vmware"))
                // Checks if the GPU name contains "vmware" (common virtual GPU).
                {
                    MessageBox.Show("Virtual Environment Detected!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // Displays a warning message if any VM indicator is found.
                }
                else
                {
                    MessageBox.Show("No Virtual Environment Detected!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Displays an informational message if no VM indicators are detected.
                }
            }
            catch (ManagementException ex)
            {
                // Catches errors related to WMI queries.
                MessageBox.Show($"WMI query error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Displays an error message if a WMI exception occurs.
            }
            catch (Exception ex)
            {
                // Catches any other general exceptions.
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Displays an error message for general exceptions.
            }
        }
    }
}
