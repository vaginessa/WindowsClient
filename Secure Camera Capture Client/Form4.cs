using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;

namespace Secure_Camera_Capture_Client
{
    public partial class Form4 : Form
    {
        string gURI;
        string gMyParamters;
        Form1 gForm1;
        public Form4(string URI, string myParamters, Form1 form1)
        {
            gURI = URI;
            gMyParamters = myParamters;
            gForm1 = form1;

            InitializeComponent();
        }

        public void downloadPicture()
        {
            Cursor.Current = Cursors.WaitCursor;
            gForm1.Enabled = false;
            Thread a = new Thread(() =>
            {
            ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        try
                        {
                            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                            string HtmlResult = wc.UploadString(gURI, gMyParamters);
                            byte[] tempImg = Convert.FromBase64String(HtmlResult);
                            using (var ms = new MemoryStream(tempImg))
                            {   
                                gForm1.setImage(Image.FromStream(ms));
                            }
                            Cursor.Current = Cursors.Default;
                            this.DialogResult = DialogResult.OK;
                        }
                        catch
                        {
                            Cursor.Current = Cursors.Default;
                            this.DialogResult = DialogResult.Cancel;
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                    }
                    Cursor.Current = Cursors.Default;
                    this.DialogResult = DialogResult.OK;

                }
                catch
                {
                    MessageBox.Show("Unable to connect to Remote Server", "Connection Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
            a.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (error == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }
#if DEBUG
            Console.WriteLine("X509Certificate [{0}] Policy Error: '{1}'",
                cert.Subject,
                error.ToString());
#endif
            return true;

        }
    }
}
