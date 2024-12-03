using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using VM_Detector.Module.System; 

namespace VM_Detector
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();


        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
        
            VM.DetectVM();
            
        }

      
    }
}
