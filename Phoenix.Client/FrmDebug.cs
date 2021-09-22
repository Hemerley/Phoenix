using System.Drawing;
using System.Windows.Forms;

namespace Phoenix.Client
{
    public partial class FrmDebug : Form
    {
        public string Packet { get; set; }

        readonly FrmClient formClient;

        public FrmDebug(FrmClient client)
        {
            InitializeComponent();
            this.formClient = client;
            this.formClient.NewPacket += UpdateText;
        }

        private void UpdateText(object sender, string e)
        {
            this.rtbDebug.ForeColor = Color.Red;
            this.rtbDebug.AppendText(">>New Packet<<\n\n");
            this.rtbDebug.ForeColor = Color.BlanchedAlmond;
            this.rtbDebug.AppendText(e + "\n\n");
            this.rtbDebug.SelectionStart = this.rtbDebug.Text.Length;
            this.rtbDebug.ScrollToCaret();
        }
    }
}
