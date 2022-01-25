Imports DevExpress.DashboardCommon
Imports MailKit.Net.Smtp
Imports MailKit.Security
Imports MimeKit
Imports System
Imports System.IO
Imports System.Threading.Tasks

Namespace SimpleMailExport

    Public Partial Class Form1
        Inherits DevExpress.XtraEditors.XtraForm

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Shared Function CreateMimeMessage() As MimeMessage
            Try
                Dim exporter As DashboardExporter = New DashboardExporter()
                AddHandler exporter.ConnectionError, AddressOf Exporter_ConnectionError
                AddHandler exporter.DashboardItemDataLoadingError, AddressOf Exporter_DashboardItemDataLoadingError
                AddHandler exporter.DataLoadingError, AddressOf Exporter_DataLoadingError
                Dim message = New MimeMessage()
                message.From.Add(New MailboxAddress("Someone", "someone@somewhere.com"))
                message.To.Add(New MailboxAddress("Someone Else", "someone.else@somewhere.com"))
                message.Subject = "Dashboard"
                Dim builder = New BodyBuilder()
                builder.TextBody = "This is a test e-mail message sent by an application."
                ' Create a new attachment and add the PDF document.
                Using stream As MemoryStream = New MemoryStream()
                    exporter.ExportToPdf("Data/MailDashboard.xml", stream, New System.Drawing.Size(2000, 1000))
                    stream.Seek(0, SeekOrigin.Begin)
                    builder.Attachments.Add("Dashboard.pdf", stream.ToArray(), New ContentType("application", "pdf"))
                End Using

                message.Body = builder.ToMessageBody()
                Return message
            Catch
                Return Nothing
            End Try
        End Function

        Private Async Sub btnSend_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim SmtpHost As String = edtHost.EditValue.ToString()
            Dim SmtpPort As Integer = Integer.Parse(edtPort.EditValue.ToString())
            Dim SmtpUserName As String = edtUsername.EditValue.ToString()
            Dim SmtpUserPassword As String = edtPassword.EditValue.ToString()
            lblProgress.Text = "Sending mail..."
            lblProgress.Text = Await SendAsync(SmtpHost, SmtpPort, SmtpUserName, SmtpUserPassword)
        End Sub

        Private Shared Async Function SendAsync(ByVal smtpHost As String, ByVal smtpPort As Integer, ByVal userName As String, ByVal password As String) As Task(Of String)
            Dim result As String = "OK"
            ' Create a new memory stream and export the dashboard in PDF.
            Using mail As MimeMessage = CreateMimeMessage()
                If mail Is Nothing Then Return "An error occured when export a Dashboard. See console for details."
                Using client = New SmtpClient()
                    Try
                        client.Connect(smtpHost, smtpPort, SecureSocketOptions.Auto)
                        client.Authenticate(userName, password)
                        Await client.SendAsync(mail)
                    Catch ex As Exception
                        result = ex.Message
                    End Try

                    client.Disconnect(True)
                End Using
            End Using

            Return result
        End Function

        Private Shared Sub Exporter_ConnectionError(ByVal sender As Object, ByVal e As DashboardExporterConnectionErrorEventArgs)
            Console.WriteLine($"The following error occurs in {e.DataSourceName}: {e.Exception.Message}")
            Throw New Exception()
        End Sub

        Private Shared Sub Exporter_DataLoadingError(ByVal sender As Object, ByVal e As DataLoadingErrorEventArgs)
            For Each [error] As DataLoadingError In e.Errors
                Console.WriteLine($"The following error occurs in {[error].DataSourceName}: {[error].Error}")
            Next

            Throw New Exception()
        End Sub

        Private Shared Sub Exporter_DashboardItemDataLoadingError(ByVal sender As Object, ByVal e As DashboardItemDataLoadingErrorEventArgs)
            For Each [error] As DashboardItemDataLoadingError In e.Errors
                Console.WriteLine($"The following error occurs in {[error].DashboardItemName}: {[error].Error}")
            Next

            Throw New Exception()
        End Sub
    End Class
End Namespace
