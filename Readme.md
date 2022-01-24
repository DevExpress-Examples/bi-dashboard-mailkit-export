<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/450493383/21.2.5%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1062073)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# BI Dashboard - How to Use MailKit to Send a Dashboard as a Document in PDF

This example demonstrates how to email a dashboard with the MailKit email client library. Run the application, enter the SMTP host, port, smtp credentials, and click Send to email a document to the address specified in the report export options.

## Files to Look at

[Form1.cs](./CS/SimpleMailExport/Form1.cs) ([Form1.vb](./VB/SimpleMailExport/Form1.vb))

## Example Overview


The project code that sends a report as a PDF attachment includes the following steps:

- Create a [DashboardExporter](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.DashboardExporter) instance and call the [DashboardExporter.ExportToPdf](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.DashboardExporter.ExportToPdf(Dashboard--Stream--Nullable-Size---DashboardState--DashboardPdfExportOptions)) method to export a dashboard to a PDF document.
- Attach the PDF document to the [MimeMessage](http://www.mimekit.net/docs/html/T_MimeKit_MimeMessage.htm) object.
- Create the [SmtpClient](http://www.mimekit.net/docs/html/T_MailKit_Net_Smtp_SmtpClient.htm) instance and call its [SendAsync](http://www.mimekit.net/docs/html/M_MailKit_MailTransport_SendAsync_3.htm) method.

## Documentation

- [DashboardExporter](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.DashboardExporter)

## Examples
