using DevExpress.DashboardCommon;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SimpleMailExport {
    public partial class Form1 : DevExpress.XtraEditors.XtraForm {
        public Form1() {
            InitializeComponent();
        }
        private static MimeMessage CreateMimeMessage() {
            try {
                DashboardExporter exporter = new DashboardExporter();
                exporter.ConnectionError += Exporter_ConnectionError;
                exporter.DashboardItemDataLoadingError += Exporter_DashboardItemDataLoadingError;
                exporter.DataLoadingError += Exporter_DataLoadingError;

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Someone", "someone@somewhere.com"));
                message.To.Add(new MailboxAddress("Someone Else", "someone.else@somewhere.com"));
                message.Subject = "Dashboard";
                var builder = new BodyBuilder();
                builder.TextBody = "This is a test e-mail message sent by an application.";
                // Create a new attachment and add the PDF document.
                using(MemoryStream stream = new MemoryStream()) {

                    exporter.ExportToPdf("Data/MailDashboard.xml", stream, new System.Drawing.Size(2000, 1000));
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    builder.Attachments.Add("Dashboard.pdf", stream.ToArray(), new ContentType("application", "pdf"));
                }
                message.Body = builder.ToMessageBody();
                return message;
            }
            catch { return null; }
        }

        private async void btnSend_Click(object sender, EventArgs e) {
            string SmtpHost = edtHost.EditValue.ToString();
            int SmtpPort = Int32.Parse(edtPort.EditValue.ToString());
            string SmtpUserName = edtUsername.EditValue.ToString();
            string SmtpUserPassword = edtPassword.EditValue.ToString();
            lblProgress.Text = "Sending mail...";
            lblProgress.Text = await SendAsync(SmtpHost, SmtpPort, SmtpUserName, SmtpUserPassword);
        }

        private static async Task<string> SendAsync(string smtpHost, int smtpPort, string userName, string password) {
            string result = "OK";
            // Create a new memory stream and export the dashboard in PDF.
            using(MimeMessage mail = CreateMimeMessage()) {
                if(mail == null)
                    return "An error occured when export a dashboard. See console for details.";
                using(var client = new SmtpClient()) {
                    try {
                        client.Connect(smtpHost, smtpPort, SecureSocketOptions.Auto);
                        client.Authenticate(userName, password);
                        await client.SendAsync(mail);
                    }
                    catch(Exception ex) {
                        result = ex.Message;
                    }
                    client.Disconnect(true);
                }
            }

            return result;
        }

        static void Exporter_ConnectionError(object sender,
            DashboardExporterConnectionErrorEventArgs e) {
            Console.WriteLine(
                $"The following error occurs in {e.DataSourceName}: {e.Exception.Message}");
            throw new Exception();
        }
        static void Exporter_DataLoadingError(object sender,
            DataLoadingErrorEventArgs e) {
            foreach(DataLoadingError error in e.Errors)
                Console.WriteLine(
                    $"The following error occurs in {error.DataSourceName}: {error.Error}");
            throw new Exception();
        }
        static void Exporter_DashboardItemDataLoadingError(object sender,
            DashboardItemDataLoadingErrorEventArgs e) {
            foreach(DashboardItemDataLoadingError error in e.Errors)
                Console.WriteLine(
                    $"The following error occurs in {error.DashboardItemName}: {error.Error}");
            throw new Exception();
        }
    }
}
