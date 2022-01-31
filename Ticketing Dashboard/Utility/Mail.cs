using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using Ticketing_Dashboard.Models;

namespace Ticketing_Dashboard.Utility
{
    public static class Mail
    {
        private static string GenerateViewURL(SR_Request sr) => "https://msftpmoservicexchange.azurewebsites.net/Home/UserSRDetail?token=" + CryptographyManager.ComputeSha265HashWithSalt(sr.requesterEmailID) + "&id=" + (object)sr.srPk;

        private static string GenerateLaptopViewURL(Laptop_Master laptop) => "https://msftpmoservicexchange.azurewebsites.net/Laptop/UserLaptopDetail?token=" + CryptographyManager.ComputeSha265HashWithSalt(laptop.requesterEmailID) + "&id=" + (object)laptop.srPk;

        private static List<string> GenerateForgotPasswordViewURL(string emailID)
        {
            string str1 = CryptographyManager.EncryptString(emailID + "~" + (object)(long)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            string str2 = "https://msftpmoservicexchange.azurewebsites.net/Admin/ResetForgotPassword?token=" + str1;
            return new List<string>() { str1, str2 };
        }

        public static void SendMail(
          SR_Request sr,
          string toMailID,
          List<string> ccMailIDs,
          CultureInfo cult)
        {
            List<string> ccMailID = new List<string>() { "mohammadzubair.n@hcl.com", "vishesh.dvivedi@hcl.com" };
            string str1 = "<!doctype html><html lang='en' xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office'><head><title>Welcome to [Coded Mails]</title><!--[if !mso]><!-- --><meta http-equiv='X-UA-Compatible' content='IE=edge'><!--<![endif]--><meta http-equiv='Content-Type' content='text/html; charset=UTF-8'><meta name='viewport' content='width=device-width,initial-scale=1'><style type='text/css'>#outlook a{padding:0}body{margin:0;padding:0;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{border-collapse:collapse;mso-table-lspace:0;mso-table-rspace:0}img{border:0;height:auto;line-height:100%;outline:0;text-decoration:none;-ms-interpolation-mode:bicubic}p{display:block;margin:13px 0}</style><!--[if mso]><xml><o:officedocumentsettings><o:allowpng><o:pixelsperinch>96</o:pixelsperinch></o:officedocumentsettings></xml><![endif]--><!--[if lte mso 11]><style type='text/css'>.mj-outlook-group-fix{width:100%!important}</style><![endif]--><!--[if !mso]><!--><link href='https://fonts.googleapis.com/css?family=Muli:300,400,700' rel='stylesheet' type='text/css'><style type='text/css'>@import url(https://fonts.googleapis.com/css?family=Muli:300,400,700);</style><!--<![endif]--><style type='text/css'>@media only screen and (min-width:480px){.mj-column-per-100{width:100%!important;max-width:100%}}</style><style type='text/css'>@media only screen and (max-width:480px){table.mj-full-width-mobile{width:100%!important}td.mj-full-width-mobile{width:auto!important}}</style><style type='text/css'>a,span,td,th{-webkit-font-smoothing:antialiased!important;-moz-osx-font-smoothing:grayscale!important}</style></head><body style='background-color:#006db6'><div style='display:none;font-size:1px;color:#fff;line-height:1px;max-height:0;max-width:0;opacity:0;overflow:hidden'></div><div style='background-color:#006db6'><!--[if mso | IE]><table align='center' border='0' cellpadding='0' cellspacing='0' class='' style='width:900px' width='600'><tr><td style='line-height:0;font-size:0;mso-line-height-rule:exactly'><![endif]--><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='' style='vertical-align:top;width:900px'><![endif]--><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td style='font-size:0;word-break:break-word'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td height='20' style='vertical-align:top;height:20px'><![endif]--><div style='height:20px'>&nbsp;</div><!--[if mso | IE]><![endif]--></td></tr><tr></tr></table></div><!--[if mso | IE]><![endif]--></td></tr></tbody></table></div><!--[if mso | IE]><table align='center' border='0' cellpadding='0' cellspacing='0' class='' style='width:900px' width='600'><tr><td style='line-height:0;font-size:0;mso-line-height-rule:exactly'><![endif]--><div style='background:#fff;background-color:#fff;margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='background:#fff;background-color:#fff;width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='' width='900px'><table align='center' border='0' cellpadding='0' cellspacing='0' class='' style='width:900px' width='600'><tr><td style='line-height:0;font-size:0;mso-line-height-rule:exactly'><![endif]--><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:0;text-align:center'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='' style='vertical-align:top;width:900px'><![endif]--><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td align='left' style='font-size:0;padding:10px 25px;word-break:break-word'><div style='font-family:Muli,Arial,sans-serif;font-size:20px;font-weight:400;line-height:30px;text-align:left;color:#333'><h1 style='margin:0;font-size:24px;line-height:normal;font-weight:700'>SR Request Created Successfully</h1></div></td></tr><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'><p style='border-top:solid 1px #f4f5fb;font-size:1px;margin:0 auto;width:100%'></p><!--[if mso | IE]><table align='center' border='0' cellpadding='0' cellspacing='0' style='border-top:solid 1px #f4f5fb;font-size:1px;margin:0 auto;width:550px' role='presentation' width='550px'><tr><td style='height:0;line-height:0'>&nbsp;</td></tr></table><![endif]--></td></tr><tr><td align='left' style='font-size:0;padding:10px 25px;word-break:break-word'><div style='font-family:Muli,Arial,sans-serif;font-size:16px;font-weight:400;line-height:20px;text-align:left;color:#333'><p style='margin:0'>Dear User,</p><br/><p style='margin:0'>Following are the details of your created SR request:</p></div></td></tr></table></div><!--[if mso | IE]><![endif]--></td></tr></tbody></table></div><!--[if mso | IE]><tr><td class='' width='900px'><table align='center' border='0' cellpadding='0' cellspacing='0' class='' style='width:900px' width='600'><tr><td style='line-height:0;font-size:0;mso-line-height-rule:exactly'><![endif]--><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;padding-bottom:0;padding-top:0;text-align:center'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='' style='vertical-align:top;width:900px'><![endif]--><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' width='100%'><tbody><tr><td style='vertical-align:top;padding:10px 25px'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='background-color:#f5f5f5' width='100%'><tr><td align='left' class='receipt-table' style='font-size:0;padding:20px;word-break:break-word'><table cellpadding='0' cellspacing='0' width='100%' border='0' style='color:#333;font-family:Muli,Arial,sans-serif;font-size:13px;line-height:22px;table-layout:auto;width:100%;border:none'><tr><th colspan='2' align='left' style='padding-bottom:10px;color:#7e7e7e;font-size:12px;line-height:16px;font-weight:700;text-transform:uppercase'>Created Date: ";
            string str2 = "'>View</a></td></tr><tr><td style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal;border-bottom-width:1px;border-bottom-color:#eaeeeb;border-bottom-style:solid;padding:5px 0' colspan='3'></td></tr></table></td></tr></table></td></tr></tbody></table></div></td></tr></tbody></table></div><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:0;text-align:center'><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td align='left' style='font-size:0;padding:10px 25px;word-break:break-word'><div style='font-family:Muli,Arial,sans-serif;font-size:16px;font-weight:400;line-height:20px;text-align:left;color:#333'><p style='margin:0'>If you have any questions or need further information, you can <a href='#' style='color:#ac7b4c;text-decoration:none'>contact administrator.</a></p></div></td></tr></table></div></td></tr></tbody></table></div><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;padding-bottom:0;padding-top:0;text-align:center'></td></tr></tbody></table></div></td></tr></tbody></table></div><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td align='center' style='font-size:0;padding:10px 25px;word-break:break-word'><div style='font-family:Muli,Arial,sans-serif;font-size:14px;font-weight:400;line-height:20px;text-align:center;color:white'>© 2021 HCL Technologies Inc.</div></td></tr></table></div></td></tr></tbody></table></div><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td style='font-size:0;word-break:break-word'><div style='height:1px'>&nbsp;</div></td></tr></table></div></td></tr></tbody></table></div></div></body></html>";
            string str3 = str1;
            string str4 = "msftpmoservicexchange@outlook.com";
            string password = "Msft@1234!";
            string str5 = sr.sapid != null ? sr.sapid.ToString() : "NA";
            string str6 = str3 + Convert.ToDateTime((object)sr.createdDate, (IFormatProvider)cult).ToString("dd/MM/yyyy") + "</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Request ID<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>" + (object)sr.srPk + "</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Name<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>" + sr.name + "</td></tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>SAP ID<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>" + str5 + "</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Project Name<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>" + sr.projectName + "</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Experience<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>" + (object)sr.experience + "</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Status<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>" + sr.status + "</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>View Request Details<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><a href='" + Ticketing_Dashboard.Utility.Mail.GenerateViewURL(sr) + str2;
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.office365.com";
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential(str4, password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage message = new MailMessage();
                message.From = new MailAddress(str4);
                message.To.Add(new MailAddress(toMailID));
                foreach (var mail in ccMailID)
                {
                    message.CC.Add(new MailAddress(mail));
                }
                message.Subject = "SR Request Created Successfully";
                message.IsBodyHtml = true;
                message.Body = str6;
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
            }
        }

        public static void SendCloseMail(
          SR_Request sr,
          string toMailID,
          List<string> ccMailIDs,
          CultureInfo cult)
        {
            List<string> ccMailID = new List<string>() { "mohammadzubair.n@hcl.com", "vishesh.dvivedi@hcl.com" };
            string str1 = "<!doctype html><html lang='en' xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office'><head><title>Welcome to [Coded Mails]</title><!--[if !mso]><!-- --><meta http-equiv='X-UA-Compatible' content='IE=edge'><!--<![endif]--><meta http-equiv='Content-Type' content='text/html; charset=UTF-8'><meta name='viewport' content='width=device-width,initial-scale=1'><style type='text/css'>#outlook a{padding:0}body{margin:0;padding:0;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{border-collapse:collapse;mso-table-lspace:0;mso-table-rspace:0}img{border:0;height:auto;line-height:100%;outline:0;text-decoration:none;-ms-interpolation-mode:bicubic}p{display:block;margin:13px 0}</style><!--[if mso]><xml><o:officedocumentsettings><o:allowpng><o:pixelsperinch>96</o:pixelsperinch></o:officedocumentsettings></xml><![endif]--><!--[if lte mso 11]><style type='text/css'>.mj-outlook-group-fix{width:100%!important}</style><![endif]--><!--[if !mso]><!--><link href='https://fonts.googleapis.com/css?family=Muli:300,400,700' rel='stylesheet' type='text/css'><style type='text/css'>@import url(https://fonts.googleapis.com/css?family=Muli:300,400,700);</style><!--<![endif]--><style type='text/css'>@media only screen and (min-width:480px){.mj-column-per-100{width:100%!important;max-width:100%}}</style><style type='text/css'>@media only screen and (max-width:480px){table.mj-full-width-mobile{width:100%!important}td.mj-full-width-mobile{width:auto!important}}</style><style type='text/css'>a,span,td,th{-webkit-font-smoothing:antialiased!important;-moz-osx-font-smoothing:grayscale!important}</style></head><body style='background-color:#006db6'><div style='display:none;font-size:1px;color:#fff;line-height:1px;max-height:0;max-width:0;opacity:0;overflow:hidden'></div><div style='background-color:#006db6'><!--[if mso | IE]><table align='center' border='0' cellpadding='0' cellspacing='0' class='' style='width:900px' width='600'><tr><td style='line-height:0;font-size:0;mso-line-height-rule:exactly'><![endif]--><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='' style='vertical-align:top;width:900px'><![endif]--><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td style='font-size:0;word-break:break-word'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td height='20' style='vertical-align:top;height:20px'><![endif]--><div style='height:20px'>&nbsp;</div><!--[if mso | IE]><![endif]--></td></tr><tr></tr></table></div><!--[if mso | IE]><![endif]--></td></tr></tbody></table></div><!--[if mso | IE]><table align='center' border='0' cellpadding='0' cellspacing='0' class='' style='width:900px' width='600'><tr><td style='line-height:0;font-size:0;mso-line-height-rule:exactly'><![endif]--><div style='background:#fff;background-color:#fff;margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='background:#fff;background-color:#fff;width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='' width='900px'><table align='center' border='0' cellpadding='0' cellspacing='0' class='' style='width:900px' width='600'><tr><td style='line-height:0;font-size:0;mso-line-height-rule:exactly'><![endif]--><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:0;text-align:center'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='' style='vertical-align:top;width:900px'><![endif]--><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td align='left' style='font-size:0;padding:10px 25px;word-break:break-word'><div style='font-family:Muli,Arial,sans-serif;font-size:20px;font-weight:400;line-height:30px;text-align:left;color:#333'><h1 style='margin:0;font-size:24px;line-height:normal;font-weight:700'>SR Request Closed Successfully</h1></div></td></tr><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'><p style='border-top:solid 1px #f4f5fb;font-size:1px;margin:0 auto;width:100%'></p><!--[if mso | IE]><table align='center' border='0' cellpadding='0' cellspacing='0' style='border-top:solid 1px #f4f5fb;font-size:1px;margin:0 auto;width:550px' role='presentation' width='550px'><tr><td style='height:0;line-height:0'>&nbsp;</td></tr></table><![endif]--></td></tr><tr><td align='left' style='font-size:0;padding:10px 25px;word-break:break-word'><div style='font-family:Muli,Arial,sans-serif;font-size:16px;font-weight:400;line-height:20px;text-align:left;color:#333'><p style='margin:0'>Dear User,</p><br/><p style='margin:0'>Following are the details of closed SR request:</p></div></td></tr></table></div><!--[if mso | IE]><![endif]--></td></tr></tbody></table></div><!--[if mso | IE]><tr><td class='' width='900px'><table align='center' border='0' cellpadding='0' cellspacing='0' class='' style='width:900px' width='600'><tr><td style='line-height:0;font-size:0;mso-line-height-rule:exactly'><![endif]--><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;padding-bottom:0;padding-top:0;text-align:center'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='' style='vertical-align:top;width:900px'><![endif]--><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' width='100%'><tbody><tr><td style='vertical-align:top;padding:10px 25px'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='background-color:#f5f5f5' width='100%'><tr><td align='left' class='receipt-table' style='font-size:0;padding:20px;word-break:break-word'><table cellpadding='0' cellspacing='0' width='100%' border='0' style='color:#333;font-family:Muli,Arial,sans-serif;font-size:13px;line-height:22px;table-layout:auto;width:100%;border:none'><tr><th colspan='2' align='left' style='padding-bottom:10px;color:#7e7e7e;font-size:12px;line-height:16px;font-weight:700;text-transform:uppercase'>Created Date: ";
            string str2 = "'>View</a></td></tr><tr><td style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal;border-bottom-width:1px;border-bottom-color:#eaeeeb;border-bottom-style:solid;padding:5px 0' colspan='3'></td></tr></table></td></tr></table></td></tr></tbody></table></div></td></tr></tbody></table></div><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:0;text-align:center'><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td align='left' style='font-size:0;padding:10px 25px;word-break:break-word'><div style='font-family:Muli,Arial,sans-serif;font-size:16px;font-weight:400;line-height:20px;text-align:left;color:#333'><p style='margin:0'>If you have any questions or need further information, you can <a href='#' style='color:#ac7b4c;text-decoration:none'>contact administrator.</a></p></div></td></tr></table></div></td></tr></tbody></table></div><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;padding-bottom:0;padding-top:0;text-align:center'></td></tr></tbody></table></div></td></tr></tbody></table></div><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td align='center' style='font-size:0;padding:10px 25px;word-break:break-word'><div style='font-family:Muli,Arial,sans-serif;font-size:14px;font-weight:400;line-height:20px;text-align:center;color:white'>© 2021 HCL Technologies Inc.</div></td></tr></table></div></td></tr></tbody></table></div><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td style='font-size:0;word-break:break-word'><div style='height:1px'>&nbsp;</div></td></tr></table></div></td></tr></tbody></table></div></div></body></html>";
            string str3 = str1;
            string str4 = "msftpmoservicexchange@outlook.com";
            string password = "Msft@1234!";
            string str5 = sr.sapid != null ? sr.sapid.ToString() : "NA";
            object[] objArray = new object[22];
            objArray[0] = (object)str3;
            DateTime dateTime = Convert.ToDateTime((object)sr.createdDate, (IFormatProvider)cult);
            objArray[1] = (object)dateTime.ToString("dd/MM/yyyy");
            objArray[2] = (object)"</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Request ID<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>";
            objArray[3] = (object)sr.srPk;
            objArray[4] = (object)"</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Name<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>";
            objArray[5] = (object)sr.name;
            objArray[6] = (object)"</td></tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>SAP ID<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>";
            objArray[7] = (object)str5;
            objArray[8] = (object)"</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Project Name<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>";
            objArray[9] = (object)sr.projectName;
            objArray[10] = (object)"</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Experience<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>";
            objArray[11] = (object)sr.experience;
            objArray[12] = (object)"</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>SR No.<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>";
            objArray[13] = (object)sr.srNo;
            objArray[14] = (object)"</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Remarks<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>";
            objArray[15] = (object)sr.remarks;
            objArray[16] = (object)"</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Ticket Closing Date<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>";
            dateTime = Convert.ToDateTime((object)sr.closingDate.Value, (IFormatProvider)cult);
            objArray[17] = (object)dateTime.ToString("dd/MM/yyyy");
            objArray[18] = (object)"</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Status<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>";
            objArray[19] = (object)sr.status;
            objArray[20] = (object)"</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>View Request Details<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><a href='";
            objArray[21] = (object)Ticketing_Dashboard.Utility.Mail.GenerateViewURL(sr);
            string str6 = string.Concat(objArray) + str2;
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.office365.com";
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential(str4, password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage message = new MailMessage();
                message.From = new MailAddress(str4);
                message.To.Add(new MailAddress(toMailID));
                foreach (var mail in ccMailID)
                {
                    message.CC.Add(new MailAddress(mail));
                }
                message.Subject = "SR Request Closed Successfully";
                message.IsBodyHtml = true;
                message.Body = str6;
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
            }
        }

        public static string TestMail()
        {
            try
            {
                List<string> ccMailID = new List<string>() { "mohammadzubair.n@hcl.com", "vishesh.dvivedi@hcl.com" };
                string str = "msftpmoservicexchange@outlook.com";
                string password = "Msft@1234!";
                List<string> stringList = new List<string>()
        {
          "vishesh.dvivedi@hcl.com",
          "mohammadzubair.n@hcl.com"
        };
                new SmtpClient()
                {
                    Port = 587,
                    Host = "smtp.office365.com",
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = ((ICredentialsByHost)new NetworkCredential(str, password)),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                }.Send(new MailMessage()
                {
                    From = new MailAddress(str),
                    To = {
            "mohammadzubair.n@hcl.com"
          },
                    Subject = "Test Mail",
                    IsBodyHtml = true,
                    Body = "Hi, this is a test mail"
                });
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "Mail Send Successfully";
        }

        public static void SendPendingMail(
          List<SR_Request> srlist,
          List<string> toMailIDs,
          CultureInfo cult)
        {
            List<string> ccMailID = new List<string>() { "mohammadzubair.n@hcl.com", "vishesh.dvivedi@hcl.com" };
            string str1 = "<!DOCTYPE html><html lang=en xmlns=http://www.w3.org/1999/xhtml xmlns:o=urn:schemas-microsoft-com:office:office xmlns:v=urn:schemas-microsoft-com:vml><title>Welcome to [Coded Mails]</title><meta content='IE=edge'http-equiv=X-UA-Compatible><meta content='text/html; charset=UTF-8'http-equiv=Content-Type><meta content='width=device-width,initial-scale=1'name=viewport><style>#outlook a{padding:0}body{margin:0;padding:0;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{border-collapse:collapse;mso-table-lspace:0;mso-table-rspace:0}img{border:0;height:auto;line-height:100%;outline:0;text-decoration:none;-ms-interpolation-mode:bicubic}p{display:block;margin:13px 0}</style><!--[if mso]><xml><o:officedocumentsettings><o:allowpng><o:pixelsperinch>96</o:pixelsperinch></o:officedocumentsettings></xml><![endif]--><!--[if lte mso 11]><style>.mj-outlook-group-fix{width:100%!important}</style><![endif]--><link href='https://fonts.googleapis.com/css?family=Muli:300,400,700'rel=stylesheet><style>@import url(https://fonts.googleapis.com/css?family=Muli:300,400,700);</style><style>@media only screen and (min-width:480px){.mj-column-per-100{width:100%!important;max-width:100%}}</style><style>@media only screen and (max-width:480px){table.mj-full-width-mobile{width:100%!important}td.mj-full-width-mobile{width:auto!important}}</style><style>a,span,td,th{-webkit-font-smoothing:antialiased!important;-moz-osx-font-smoothing:grayscale!important}</style><body style=background-color:#006db6><div style=display:none;font-size:1px;color:#fff;line-height:1px;max-height:0;max-width:0;opacity:0;overflow:hidden></div><div style=background-color:#006db6><!--[if mso | IE]><table border=0 cellpadding=0 cellspacing=0 align=center style=width:600px width=600><tr><td style=line-height:0;font-size:0;mso-line-height-rule:exactly><![endif]--><div style='margin:0 auto;'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style=font-size:0;word-break:break-word><div style=height:20px></div><tr></table></div></table></div><div style='background:#fff;background-color:#fff;margin:0 auto;max-width: 1000px'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=background:#fff;background-color:#fff;width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div style='margin:0 auto;'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style=direction:ltr;font-size:0;padding:0;text-align:center><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'align=left><div style=font-family:Muli,Arial,sans-serif;font-size:20px;font-weight:400;line-height:30px;text-align:left;color:#333><h1 style=margin:0;font-size:24px;line-height:normal;font-weight:700>Pending SR Requests</h1></div><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'><p style='border-top:solid 1px #f4f5fb;font-size:1px;margin:0 auto;width:100%'><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'align=left><div style=font-family:Muli,Arial,sans-serif;font-size:16px;font-weight:400;line-height:20px;text-align:left;color:#333><p style=margin:0>Dear User,</p><br/><p style=margin:0>Following requests are still in the pending state. Here are the details of the same:</p><br><table border=1 cellpadding=12 class=table id=data-table><thead><tr style='background-color: #00AAE4; font-weight: bold; color: white;'><td>#<td>Request ID<td>Name<td>SAP ID<td>Project Name<td>Requisition Source<td>Created Date<td>Status</tr><tbody>";
            string str2 = "</tbody></table></div></table></div></table></div><div style='margin:0 auto'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style=direction:ltr;font-size:0;padding:0;text-align:center><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'align=left><div style=font-family:Muli,Arial,sans-serif;font-size:16px;font-weight:400;line-height:20px;text-align:left;color:#333><p><a href='https://msftpmoservicexchange.azurewebsites.net/Home/AdminLogin'>Click here for more details<a><br /><p style=margin:0>If you have any questions or need further information, you can <a href=# style=color:#ac7b4c;text-decoration:none>contact administrator.</a></div></table></div></table></div><div style='margin:0 auto;max-width:600px'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;padding-bottom:0;padding-top:0;text-align:center'></table></div></table></div><div style='margin:0 auto;max-width:600px'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'align=center><div style=font-family:Muli,Arial,sans-serif;font-size:14px;font-weight:400;line-height:20px;text-align:center;color:white>© 2021 HCL Technologies Inc.</div></table></div></table></div><div style='margin:0 auto;max-width:600px'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style=font-size:0;word-break:break-word><div style=height:1px></div></table></div></table></div></div>";
            string str3 = str1;
            string str4 = "msftpmoservicexchange@outlook.com";
            string password = "Msft@1234!";
            int num = 1;
            foreach (SR_Request sr in srlist)
            {
                string str5 = sr.sapid != null ? sr.sapid.ToString() : "NA";
                if (SLAManager.CheckSRSLAExceed(sr))
                    str3 = str3 + "<tr style='white-space: nowrap; background-color: #F0FFFF;'><td>" + Convert.ToString(num) + "</td><td>" + (object)sr.srPk + "</td><td>" + sr.name + "</td><td nowrap>" + str5 + "</td><td>" + sr.projectName + "</td><td>" + sr.requisitionSource + "</td><td>" + Convert.ToDateTime((object)sr.createdDate, (IFormatProvider)cult).ToString("dd/MM/yyyy") + "</td><td style='color: white!important; background-color: red!important;'>" + sr.status + "</td></tr>";
                else
                    str3 = str3 + "<tr style='white-space: nowrap; background-color: #F0FFFF;'><td>" + Convert.ToString(num) + "</td><td>" + (object)sr.srPk + "</td><td>" + sr.name + "</td><td nowrap>" + str5 + "</td><td>" + sr.projectName + "</td><td>" + sr.requisitionSource + "</td><td>" + Convert.ToDateTime((object)sr.createdDate, (IFormatProvider)cult).ToString("dd/MM/yyyy") + "</td><td style='color: white!important; background-color: orange!important;'>" + sr.status + "</td></tr>";
                ++num;
            }
            string str6 = str3 + str2;
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.office365.com";
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential(str4, password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage message = new MailMessage();
                message.From = new MailAddress(str4);
                foreach (string toMailId in toMailIDs)
                    message.To.Add(new MailAddress(toMailId));
                foreach (var mail in ccMailID)
                {
                    message.CC.Add(new MailAddress(mail));
                }
                message.Subject = "Pending SR Requests";
                message.IsBodyHtml = true;
                message.Body = str6;
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
            }
        }

        public static void SendLaptopMail(
          Laptop_Master laptop,
          string toMailID,
          List<string> ccMailIDs,
          CultureInfo cult)
        {
            List<string> ccMailID = new List<string>() { "mohammadzubair.n@hcl.com", "vishesh.dvivedi@hcl.com" };
            string str1 = "<!doctype html><html lang='en' xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office'><head><title>Welcome to [Coded Mails]</title><!--[if !mso]><!-- --><meta http-equiv='X-UA-Compatible' content='IE=edge'><!--<![endif]--><meta http-equiv='Content-Type' content='text/html; charset=UTF-8'><meta name='viewport' content='width=device-width,initial-scale=1'><style type='text/css'>#outlook a{padding:0}body{margin:0;padding:0;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{border-collapse:collapse;mso-table-lspace:0;mso-table-rspace:0}img{border:0;height:auto;line-height:100%;outline:0;text-decoration:none;-ms-interpolation-mode:bicubic}p{display:block;margin:13px 0}</style><!--[if mso]><xml><o:officedocumentsettings><o:allowpng><o:pixelsperinch>96</o:pixelsperinch></o:officedocumentsettings></xml><![endif]--><!--[if lte mso 11]><style type='text/css'>.mj-outlook-group-fix{width:100%!important}</style><![endif]--><!--[if !mso]><!--><link href='https://fonts.googleapis.com/css?family=Muli:300,400,700' rel='stylesheet' type='text/css'><style type='text/css'>@import url(https://fonts.googleapis.com/css?family=Muli:300,400,700);</style><!--<![endif]--><style type='text/css'>@media only screen and (min-width:480px){.mj-column-per-100{width:100%!important;max-width:100%}}</style><style type='text/css'>@media only screen and (max-width:480px){table.mj-full-width-mobile{width:100%!important}td.mj-full-width-mobile{width:auto!important}}</style><style type='text/css'>a,span,td,th{-webkit-font-smoothing:antialiased!important;-moz-osx-font-smoothing:grayscale!important}</style></head><body style='background-color:#006db6'><div style='display:none;font-size:1px;color:#fff;line-height:1px;max-height:0;max-width:0;opacity:0;overflow:hidden'></div><div style='background-color:#006db6'><!--[if mso | IE]><table align='center' border='0' cellpadding='0' cellspacing='0' class='' style='width:900px' width='600'><tr><td style='line-height:0;font-size:0;mso-line-height-rule:exactly'><![endif]--><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='' style='vertical-align:top;width:900px'><![endif]--><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td style='font-size:0;word-break:break-word'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td height='20' style='vertical-align:top;height:20px'><![endif]--><div style='height:20px'>&nbsp;</div><!--[if mso | IE]><![endif]--></td></tr><tr></tr></table></div><!--[if mso | IE]><![endif]--></td></tr></tbody></table></div><!--[if mso | IE]><table align='center' border='0' cellpadding='0' cellspacing='0' class='' style='width:900px' width='600'><tr><td style='line-height:0;font-size:0;mso-line-height-rule:exactly'><![endif]--><div style='background:#fff;background-color:#fff;margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='background:#fff;background-color:#fff;width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='' width='900px'><table align='center' border='0' cellpadding='0' cellspacing='0' class='' style='width:900px' width='600'><tr><td style='line-height:0;font-size:0;mso-line-height-rule:exactly'><![endif]--><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:0;text-align:center'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='' style='vertical-align:top;width:900px'><![endif]--><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td align='left' style='font-size:0;padding:10px 25px;word-break:break-word'><div style='font-family:Muli,Arial,sans-serif;font-size:20px;font-weight:400;line-height:30px;text-align:left;color:#333'><h1 style='margin:0;font-size:24px;line-height:normal;font-weight:700'>Laptop Request Created Successfully</h1></div></td></tr><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'><p style='border-top:solid 1px #f4f5fb;font-size:1px;margin:0 auto;width:100%'></p><!--[if mso | IE]><table align='center' border='0' cellpadding='0' cellspacing='0' style='border-top:solid 1px #f4f5fb;font-size:1px;margin:0 auto;width:550px' role='presentation' width='550px'><tr><td style='height:0;line-height:0'>&nbsp;</td></tr></table><![endif]--></td></tr><tr><td align='left' style='font-size:0;padding:10px 25px;word-break:break-word'><div style='font-family:Muli,Arial,sans-serif;font-size:16px;font-weight:400;line-height:20px;text-align:left;color:#333'><p style='margin:0'>Dear User,</p><br/><p style='margin:0'>Following are the details of your created Laptop request:</p></div></td></tr></table></div><!--[if mso | IE]><![endif]--></td></tr></tbody></table></div><!--[if mso | IE]><tr><td class='' width='900px'><table align='center' border='0' cellpadding='0' cellspacing='0' class='' style='width:900px' width='600'><tr><td style='line-height:0;font-size:0;mso-line-height-rule:exactly'><![endif]--><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;padding-bottom:0;padding-top:0;text-align:center'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='' style='vertical-align:top;width:900px'><![endif]--><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' width='100%'><tbody><tr><td style='vertical-align:top;padding:10px 25px'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='background-color:#f5f5f5' width='100%'><tr><td align='left' class='receipt-table' style='font-size:0;padding:20px;word-break:break-word'><table cellpadding='0' cellspacing='0' width='100%' border='0' style='color:#333;font-family:Muli,Arial,sans-serif;font-size:13px;line-height:22px;table-layout:auto;width:100%;border:none'><tr><th colspan='2' align='left' style='padding-bottom:10px;color:#7e7e7e;font-size:12px;line-height:16px;font-weight:700;text-transform:uppercase'>Created Date: ";
            string str2 = "'>View</a></td></tr><tr><td style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal;border-bottom-width:1px;border-bottom-color:#eaeeeb;border-bottom-style:solid;padding:5px 0' colspan='3'></td></tr></table></td></tr></table></td></tr></tbody></table></div></td></tr></tbody></table></div><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:0;text-align:center'><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td align='left' style='font-size:0;padding:10px 25px;word-break:break-word'><div style='font-family:Muli,Arial,sans-serif;font-size:16px;font-weight:400;line-height:20px;text-align:left;color:#333'><p style='margin:0'>If you have any questions or need further information, you can <a href='#' style='color:#ac7b4c;text-decoration:none'>contact administrator.</a></p></div></td></tr></table></div></td></tr></tbody></table></div><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;padding-bottom:0;padding-top:0;text-align:center'></td></tr></tbody></table></div></td></tr></tbody></table></div><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td align='center' style='font-size:0;padding:10px 25px;word-break:break-word'><div style='font-family:Muli,Arial,sans-serif;font-size:14px;font-weight:400;line-height:20px;text-align:center;color:white'>© 2021 HCL Technologies Inc.</div></td></tr></table></div></td></tr></tbody></table></div><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td style='font-size:0;word-break:break-word'><div style='height:1px'>&nbsp;</div></td></tr></table></div></td></tr></tbody></table></div></div></body></html>";
            string str3 = str1;
            string str4 = "msftpmoservicexchange@outlook.com";
            string password = "Msft@1234!";
            string str5 = laptop.sapid.HasValue ? laptop.sapid.ToString() : "NA";
            string str6 = str3 + Convert.ToDateTime((object)laptop.createdDate, (IFormatProvider)cult).ToString("dd/MM/yyyy") + "</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Request ID<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>" + (object)laptop.srPk + "</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Name<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>" + laptop.name + "</td></tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>SAP ID<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>" + str5 + "</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Email ID<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>" + laptop.requesterEmailID + "</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Request Type<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>" + laptop.requestType + "</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Status<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>" + laptop.status + "</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>View Request Details<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><a href='" + Ticketing_Dashboard.Utility.Mail.GenerateLaptopViewURL(laptop) + str2;
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.office365.com";
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential(str4, password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage message = new MailMessage();
                message.From = new MailAddress(str4);
                message.To.Add(new MailAddress(toMailID));
                foreach (string ccMailId in ccMailIDs)
                    message.CC.Add(new MailAddress(ccMailId));
                foreach (var mail in ccMailID)
                {
                    message.CC.Add(new MailAddress(mail));
                }
                message.Subject = "Laptop Request Created Successfully";
                message.IsBodyHtml = true;
                message.Body = str6;
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
            }
        }

        public static void SendLaptopCloseMail(
          Laptop_Master laptop,
          string toMailID,
          List<string> ccMailIDs,
          CultureInfo cult)
        {
            List<string> ccMailID = new List<string>() { "mohammadzubair.n@hcl.com", "vishesh.dvivedi@hcl.com" };
            string str1 = "<!doctype html><html lang='en' xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office'><head><title>Welcome to [Coded Mails]</title><!--[if !mso]><!-- --><meta http-equiv='X-UA-Compatible' content='IE=edge'><!--<![endif]--><meta http-equiv='Content-Type' content='text/html; charset=UTF-8'><meta name='viewport' content='width=device-width,initial-scale=1'><style type='text/css'>#outlook a{padding:0}body{margin:0;padding:0;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{border-collapse:collapse;mso-table-lspace:0;mso-table-rspace:0}img{border:0;height:auto;line-height:100%;outline:0;text-decoration:none;-ms-interpolation-mode:bicubic}p{display:block;margin:13px 0}</style><!--[if mso]><xml><o:officedocumentsettings><o:allowpng><o:pixelsperinch>96</o:pixelsperinch></o:officedocumentsettings></xml><![endif]--><!--[if lte mso 11]><style type='text/css'>.mj-outlook-group-fix{width:100%!important}</style><![endif]--><!--[if !mso]><!--><link href='https://fonts.googleapis.com/css?family=Muli:300,400,700' rel='stylesheet' type='text/css'><style type='text/css'>@import url(https://fonts.googleapis.com/css?family=Muli:300,400,700);</style><!--<![endif]--><style type='text/css'>@media only screen and (min-width:480px){.mj-column-per-100{width:100%!important;max-width:100%}}</style><style type='text/css'>@media only screen and (max-width:480px){table.mj-full-width-mobile{width:100%!important}td.mj-full-width-mobile{width:auto!important}}</style><style type='text/css'>a,span,td,th{-webkit-font-smoothing:antialiased!important;-moz-osx-font-smoothing:grayscale!important}</style></head><body style='background-color:#006db6'><div style='display:none;font-size:1px;color:#fff;line-height:1px;max-height:0;max-width:0;opacity:0;overflow:hidden'></div><div style='background-color:#006db6'><!--[if mso | IE]><table align='center' border='0' cellpadding='0' cellspacing='0' class='' style='width:900px' width='600'><tr><td style='line-height:0;font-size:0;mso-line-height-rule:exactly'><![endif]--><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='' style='vertical-align:top;width:900px'><![endif]--><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td style='font-size:0;word-break:break-word'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td height='20' style='vertical-align:top;height:20px'><![endif]--><div style='height:20px'>&nbsp;</div><!--[if mso | IE]><![endif]--></td></tr><tr></tr></table></div><!--[if mso | IE]><![endif]--></td></tr></tbody></table></div><!--[if mso | IE]><table align='center' border='0' cellpadding='0' cellspacing='0' class='' style='width:900px' width='600'><tr><td style='line-height:0;font-size:0;mso-line-height-rule:exactly'><![endif]--><div style='background:#fff;background-color:#fff;margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='background:#fff;background-color:#fff;width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='' width='900px'><table align='center' border='0' cellpadding='0' cellspacing='0' class='' style='width:900px' width='600'><tr><td style='line-height:0;font-size:0;mso-line-height-rule:exactly'><![endif]--><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:0;text-align:center'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='' style='vertical-align:top;width:900px'><![endif]--><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td align='left' style='font-size:0;padding:10px 25px;word-break:break-word'><div style='font-family:Muli,Arial,sans-serif;font-size:20px;font-weight:400;line-height:30px;text-align:left;color:#333'><h1 style='margin:0;font-size:24px;line-height:normal;font-weight:700'>Laptop Request Closed Successfully</h1></div></td></tr><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'><p style='border-top:solid 1px #f4f5fb;font-size:1px;margin:0 auto;width:100%'></p><!--[if mso | IE]><table align='center' border='0' cellpadding='0' cellspacing='0' style='border-top:solid 1px #f4f5fb;font-size:1px;margin:0 auto;width:550px' role='presentation' width='550px'><tr><td style='height:0;line-height:0'>&nbsp;</td></tr></table><![endif]--></td></tr><tr><td align='left' style='font-size:0;padding:10px 25px;word-break:break-word'><div style='font-family:Muli,Arial,sans-serif;font-size:16px;font-weight:400;line-height:20px;text-align:left;color:#333'><p style='margin:0'>Dear User,</p><br/><p style='margin:0'>Following are the details of closed Laptop request:</p></div></td></tr></table></div><!--[if mso | IE]><![endif]--></td></tr></tbody></table></div><!--[if mso | IE]><tr><td class='' width='900px'><table align='center' border='0' cellpadding='0' cellspacing='0' class='' style='width:900px' width='600'><tr><td style='line-height:0;font-size:0;mso-line-height-rule:exactly'><![endif]--><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;padding-bottom:0;padding-top:0;text-align:center'><!--[if mso | IE]><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='' style='vertical-align:top;width:900px'><![endif]--><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' width='100%'><tbody><tr><td style='vertical-align:top;padding:10px 25px'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='background-color:#f5f5f5' width='100%'><tr><td align='left' class='receipt-table' style='font-size:0;padding:20px;word-break:break-word'><table cellpadding='0' cellspacing='0' width='100%' border='0' style='color:#333;font-family:Muli,Arial,sans-serif;font-size:13px;line-height:22px;table-layout:auto;width:100%;border:none'><tr><th colspan='2' align='left' style='padding-bottom:10px;color:#7e7e7e;font-size:12px;line-height:16px;font-weight:700;text-transform:uppercase'>Created Date: ";
            string str2 = "'>View</a></td></tr><tr><td style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal;border-bottom-width:1px;border-bottom-color:#eaeeeb;border-bottom-style:solid;padding:5px 0' colspan='3'></td></tr></table></td></tr></table></td></tr></tbody></table></div></td></tr></tbody></table></div><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:0;text-align:center'><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td align='left' style='font-size:0;padding:10px 25px;word-break:break-word'><div style='font-family:Muli,Arial,sans-serif;font-size:16px;font-weight:400;line-height:20px;text-align:left;color:#333'><p style='margin:0'>If you have any questions or need further information, you can <a href='#' style='color:#ac7b4c;text-decoration:none'>contact administrator.</a></p></div></td></tr></table></div></td></tr></tbody></table></div><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;padding-bottom:0;padding-top:0;text-align:center'></td></tr></tbody></table></div></td></tr></tbody></table></div><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td align='center' style='font-size:0;padding:10px 25px;word-break:break-word'><div style='font-family:Muli,Arial,sans-serif;font-size:14px;font-weight:400;line-height:20px;text-align:center;color:white'>© 2021 HCL Technologies Inc.</div></td></tr></table></div></td></tr></tbody></table></div><div style='margin:0 auto;max-width:900px'><table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'><tbody><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'><table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'><tr><td style='font-size:0;word-break:break-word'><div style='height:1px'>&nbsp;</div></td></tr></table></div></td></tr></tbody></table></div></div></body></html>";
            string str3 = str1;
            string str4 = "msftpmoservicexchange@outlook.com";
            string password = "Msft@1234!";
            string str5 = laptop.sapid.HasValue ? laptop.sapid.ToString() : "NA";
            object[] objArray = new object[18];
            objArray[0] = (object)str3;
            DateTime dateTime = Convert.ToDateTime((object)laptop.createdDate, (IFormatProvider)cult);
            objArray[1] = (object)dateTime.ToString("dd/MM/yyyy");
            objArray[2] = (object)"</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Request ID<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>";
            objArray[3] = (object)laptop.srPk;
            objArray[4] = (object)"</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Name<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>";
            objArray[5] = (object)laptop.name;
            objArray[6] = (object)"</td></tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>SAP ID<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>";
            objArray[7] = (object)str5;
            objArray[8] = (object)"</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Email ID<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>";
            objArray[9] = (object)laptop.requesterEmailID;
            objArray[10] = (object)"</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Action Taken<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>";
            objArray[11] = (object)laptop.actionTaken;
            objArray[12] = (object)"</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Closing Date<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'>";
            dateTime = Convert.ToDateTime((object)laptop.dateOfClosing, (IFormatProvider)cult);
            objArray[13] = (object)dateTime.ToString("dd/MM/yyyy");
            objArray[14] = (object)"</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>Status<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='background-color: orange!important; color: white!important; font-size:15px;line-height:24px;word-break:normal'>";
            objArray[15] = (object)laptop.status;
            objArray[16] = (object)"</td></tr><tr><td colspan='2' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><p style='margin:0'>View Request Details<span style='color:#a7a7a7;font-size:14px;line-height:14px'></span></p></td><td align='right' valign='top' style='color:#525f7f;font-size:15px;line-height:24px;word-break:normal'><a href='";
            objArray[17] = (object)Ticketing_Dashboard.Utility.Mail.GenerateLaptopViewURL(laptop);
            string str6 = string.Concat(objArray) + str2;
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.office365.com";
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential(str4, password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage message = new MailMessage();
                message.From = new MailAddress(str4);
                message.To.Add(new MailAddress(toMailID));
                foreach (string ccMailId in ccMailIDs)
                    message.CC.Add(new MailAddress(ccMailId));
                foreach (var mail in ccMailID)
                {
                    message.CC.Add(new MailAddress(mail));
                }
                message.Subject = "Laptop Request Closed Successfully";
                message.IsBodyHtml = true;
                message.Body = str6;
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
            }
        }

        public static void SendLaptopPendingMail(
          List<Laptop_Master> laptoplist,
          List<string> toMailIDs,
          CultureInfo cult)
        {
            List<string> ccMailID = new List<string>() { "mohammadzubair.n@hcl.com", "vishesh.dvivedi@hcl.com" };
            string str1 = "<!DOCTYPE html><html lang=en xmlns=http://www.w3.org/1999/xhtml xmlns:o=urn:schemas-microsoft-com:office:office xmlns:v=urn:schemas-microsoft-com:vml><title>Welcome to [Coded Mails]</title><meta content='IE=edge'http-equiv=X-UA-Compatible><meta content='text/html; charset=UTF-8'http-equiv=Content-Type><meta content='width=device-width,initial-scale=1'name=viewport><style>#outlook a{padding:0}body{margin:0;padding:0;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{border-collapse:collapse;mso-table-lspace:0;mso-table-rspace:0}img{border:0;height:auto;line-height:100%;outline:0;text-decoration:none;-ms-interpolation-mode:bicubic}p{display:block;margin:13px 0}</style><!--[if mso]><xml><o:officedocumentsettings><o:allowpng><o:pixelsperinch>96</o:pixelsperinch></o:officedocumentsettings></xml><![endif]--><!--[if lte mso 11]><style>.mj-outlook-group-fix{width:100%!important}</style><![endif]--><link href='https://fonts.googleapis.com/css?family=Muli:300,400,700'rel=stylesheet><style>@import url(https://fonts.googleapis.com/css?family=Muli:300,400,700);</style><style>@media only screen and (min-width:480px){.mj-column-per-100{width:100%!important;max-width:100%}}</style><style>@media only screen and (max-width:480px){table.mj-full-width-mobile{width:100%!important}td.mj-full-width-mobile{width:auto!important}}</style><style>a,span,td,th{-webkit-font-smoothing:antialiased!important;-moz-osx-font-smoothing:grayscale!important}</style><body style=background-color:#006db6><div style=display:none;font-size:1px;color:#fff;line-height:1px;max-height:0;max-width:0;opacity:0;overflow:hidden></div><div style=background-color:#006db6><!--[if mso | IE]><table border=0 cellpadding=0 cellspacing=0 align=center style=width:600px width=600><tr><td style=line-height:0;font-size:0;mso-line-height-rule:exactly><![endif]--><div style='margin:0 auto;'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style=font-size:0;word-break:break-word><div style=height:20px></div><tr></table></div></table></div><div style='background:#fff;background-color:#fff;margin:0 auto;max-width: 1000px'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=background:#fff;background-color:#fff;width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div style='margin:0 auto;'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style=direction:ltr;font-size:0;padding:0;text-align:center><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'align=left><div style=font-family:Muli,Arial,sans-serif;font-size:20px;font-weight:400;line-height:30px;text-align:left;color:#333><h1 style=margin:0;font-size:24px;line-height:normal;font-weight:700>Pending Laptop Requests</h1></div><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'><p style='border-top:solid 1px #f4f5fb;font-size:1px;margin:0 auto;width:100%'><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'align=left><div style=font-family:Muli,Arial,sans-serif;font-size:16px;font-weight:400;line-height:20px;text-align:left;color:#333><p style=margin:0>Dear User,</p><br/><p style=margin:0>Following requests are still in the pending state. Here are the details of the same:</p><br><table border=1 cellpadding=12 class=table id=data-table><thead><tr style='background-color: #00AAE4; font-weight: bold; color: white;'><td>#<td>Request ID<td>Name<td>SAP ID<td>Email ID<td>Created Date<td>Request Type<td>Status</tr><tbody>";
            string str2 = "</tbody></table></div></table></div></table></div><div style='margin:0 auto'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style=direction:ltr;font-size:0;padding:0;text-align:center><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'align=left><div style=font-family:Muli,Arial,sans-serif;font-size:16px;font-weight:400;line-height:20px;text-align:left;color:#333><p><a href='https://msftpmoservicexchange.azurewebsites.net/Home/AdminLogin'>Click here for more details<a><br /><p style=margin:0>If you have any questions or need further information, you can <a href=# style=color:#ac7b4c;text-decoration:none>contact administrator.</a></div></table></div></table></div><div style='margin:0 auto;max-width:600px'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;padding-bottom:0;padding-top:0;text-align:center'></table></div></table></div><div style='margin:0 auto;max-width:600px'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'align=center><div style=font-family:Muli,Arial,sans-serif;font-size:14px;font-weight:400;line-height:20px;text-align:center;color:white>© 2021 HCL Technologies Inc.</div></table></div></table></div><div style='margin:0 auto;max-width:600px'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style=font-size:0;word-break:break-word><div style=height:1px></div></table></div></table></div></div>";
            string str3 = str1;
            string str4 = "msftpmoservicexchange@outlook.com";
            string password = "Msft@1234!";
            int num = 1;
            foreach (Laptop_Master laptop in laptoplist)
            {
                string str5 = !laptop.sapid.HasValue ? "NA" : Convert.ToString((object)laptop.sapid);
                DateTime dateTime;
                if (SLAManager.CheckLaptopSLAExceed(laptop))
                {
                    object[] objArray = new object[18]
                    {
            (object) str3,
            (object) "<tr style='white-space: nowrap; background-color: #F0FFFF;'><td>",
            (object) Convert.ToString(num),
            (object) "</td><td>",
            (object) laptop.srPk,
            (object) "</td><td>",
            (object) laptop.name,
            (object) "</td><td nowrap>",
            (object) str5,
            (object) "</td><td>",
            (object) laptop.requesterEmailID,
            (object) "</td><td>",
            null,
            null,
            null,
            null,
            null,
            null
                    };
                    dateTime = Convert.ToDateTime((object)laptop.createdDate, (IFormatProvider)cult);
                    objArray[12] = (object)dateTime.ToString("dd/MM/yyyy");
                    objArray[13] = (object)"</td><td>";
                    objArray[14] = (object)laptop.requestType;
                    objArray[15] = (object)"</td><td style='background-color: red; color: white;'>";
                    objArray[16] = (object)laptop.status;
                    objArray[17] = (object)"</td></tr>";
                    str3 = string.Concat(objArray);
                }
                else
                {
                    object[] objArray = new object[18]
                    {
            (object) str3,
            (object) "<tr style='white-space: nowrap; background-color: #F0FFFF;'><td>",
            (object) Convert.ToString(num),
            (object) "</td><td>",
            (object) laptop.srPk,
            (object) "</td><td>",
            (object) laptop.name,
            (object) "</td><td nowrap>",
            (object) str5,
            (object) "</td><td>",
            (object) laptop.requesterEmailID,
            (object) "</td><td>",
            null,
            null,
            null,
            null,
            null,
            null
                    };
                    dateTime = Convert.ToDateTime((object)laptop.createdDate, (IFormatProvider)cult);
                    objArray[12] = (object)dateTime.ToString("dd/MM/yyyy");
                    objArray[13] = (object)"</td><td>";
                    objArray[14] = (object)laptop.requestType;
                    objArray[15] = (object)"</td><td style='background-color: orange!important; color: white!important;'>";
                    objArray[16] = (object)laptop.status;
                    objArray[17] = (object)"</td></tr>";
                    str3 = string.Concat(objArray);
                }
                ++num;
            }
            string str6 = str3 + str2;
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.office365.com";
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential(str4, password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage message = new MailMessage();
                message.From = new MailAddress(str4);
                foreach (string toMailId in toMailIDs)
                    message.To.Add(new MailAddress(toMailId));
                foreach (var mail in ccMailID)
                {
                    message.CC.Add(new MailAddress(mail));
                }
                message.Subject = "Pending Laptop Requests";
                message.IsBodyHtml = true;
                message.Body = str6;
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
            }
        }

        public static void SendOnboardingPendingMail(
          List<Onboarding_Master> onboardingList,
          List<string> toMailIDs,
          CultureInfo cult)
        {
            List<string> ccMailID = new List<string>() { "mohammadzubair.n@hcl.com", "vishesh.dvivedi@hcl.com" };
            string str1 = "<!DOCTYPE html><html lang=en xmlns=http://www.w3.org/1999/xhtml xmlns:o=urn:schemas-microsoft-com:office:office xmlns:v=urn:schemas-microsoft-com:vml><title>Welcome to [Coded Mails]</title><meta content='IE=edge'http-equiv=X-UA-Compatible><meta content='text/html; charset=UTF-8'http-equiv=Content-Type><meta content='width=device-width,initial-scale=1'name=viewport><style>#outlook a{padding:0}body{margin:0;padding:0;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{border-collapse:collapse;mso-table-lspace:0;mso-table-rspace:0}img{border:0;height:auto;line-height:100%;outline:0;text-decoration:none;-ms-interpolation-mode:bicubic}p{display:block;margin:13px 0}</style><!--[if mso]><xml><o:officedocumentsettings><o:allowpng><o:pixelsperinch>96</o:pixelsperinch></o:officedocumentsettings></xml><![endif]--><!--[if lte mso 11]><style>.mj-outlook-group-fix{width:100%!important}</style><![endif]--><link href='https://fonts.googleapis.com/css?family=Muli:300,400,700'rel=stylesheet><style>@import url(https://fonts.googleapis.com/css?family=Muli:300,400,700);</style><style>@media only screen and (min-width:480px){.mj-column-per-100{width:100%!important;max-width:100%}}</style><style>@media only screen and (max-width:480px){table.mj-full-width-mobile{width:100%!important}td.mj-full-width-mobile{width:auto!important}}</style><style>a,span,td,th{-webkit-font-smoothing:antialiased!important;-moz-osx-font-smoothing:grayscale!important}</style><body style=background-color:#006db6><div style=display:none;font-size:1px;color:#fff;line-height:1px;max-height:0;max-width:0;opacity:0;overflow:hidden></div><div style=background-color:#006db6><!--[if mso | IE]><table border=0 cellpadding=0 cellspacing=0 align=center style=width:600px width=600><tr><td style=line-height:0;font-size:0;mso-line-height-rule:exactly><![endif]--><div style='margin:0 auto;'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style=font-size:0;word-break:break-word><div style=height:20px></div><tr></table></div></table></div><div style='background:#fff;background-color:#fff;margin:0 auto;max-width: 1000px'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=background:#fff;background-color:#fff;width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div style='margin:0 auto;'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style=direction:ltr;font-size:0;padding:0;text-align:center><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'align=left><div style=font-family:Muli,Arial,sans-serif;font-size:20px;font-weight:400;line-height:30px;text-align:left;color:#333><h1 style=margin:0;font-size:24px;line-height:normal;font-weight:700>Upcoming (7 Days Joining) Onboarding Requests</h1></div><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'><p style='border-top:solid 1px #f4f5fb;font-size:1px;margin:0 auto;width:100%'><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'align=left><div style=font-family:Muli,Arial,sans-serif;font-size:16px;font-weight:400;line-height:20px;text-align:left;color:#333><p style=margin:0>Dear User,</p><br/><p style=margin:0>Following requests have joining date within 7 days. Here are the details of the same:</p><br><table border=1 cellpadding=12 class=table id=data-table><thead><tr style='background-color: #00AAE4; font-weight: bold; color: white;'><td>#<td>Request ID<td>First Name<td>Last Name<td>Email ID<td>Contact No.<td>Created Date<td>Joining Date</tr><tbody>";
            string str2 = "</tbody></table></div></table></div></table></div><div style='margin:0 auto'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style=direction:ltr;font-size:0;padding:0;text-align:center><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'align=left><div style=font-family:Muli,Arial,sans-serif;font-size:16px;font-weight:400;line-height:20px;text-align:left;color:#333><p><a href='https://msftpmoservicexchange.azurewebsites.net/Home/AdminLogin'>Click here for more details<a><br /><p style=margin:0>If you have any questions or need further information, you can <a href=# style=color:#ac7b4c;text-decoration:none>contact administrator.</a></div></table></div></table></div><div style='margin:0 auto;max-width:600px'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;padding-bottom:0;padding-top:0;text-align:center'></table></div></table></div><div style='margin:0 auto;max-width:600px'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'align=center><div style=font-family:Muli,Arial,sans-serif;font-size:14px;font-weight:400;line-height:20px;text-align:center;color:white>© 2021 HCL Technologies Inc.</div></table></div></table></div><div style='margin:0 auto;max-width:600px'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style=font-size:0;word-break:break-word><div style=height:1px></div></table></div></table></div></div>";
            string str3 = str1;
            string str4 = "msftpmoservicexchange@outlook.com";
            string password = "Msft@1234!";
            int num = 1;
            foreach (Onboarding_Master onboarding in onboardingList)
            {
                DateTime dateTime;
                object[] objArray = new object[18]
                    {
            (object) str3,
            (object) "<tr style='white-space: nowrap; background-color: #F0FFFF;'><td>",
            (object) Convert.ToString(num),
            (object) "</td><td>",
            (object) onboarding.pkID,
            (object) "</td><td>",
            (object) onboarding.firstName,
            (object) "</td><td nowrap>",
            (object) onboarding.lastName,
            (object) "</td><td>",
            (object) onboarding.emailID,
            (object) "</td><td style='font-size: 14;'>",
            null,
            null,
            null,
            null,
            null,
            null
                    };
                objArray[12] = (object)onboarding.contactNumber;
                objArray[13] = (object)"</td><td>";
                objArray[14] = (object)Convert.ToDateTime(onboarding.createdDate, cult).ToString("dd/MM/yyyy");
                objArray[15] = (object)"</td><td>";
                if(onboarding.actualJoiningDate != null)
                {
                    objArray[16] = (object)Convert.ToDateTime(onboarding.actualJoiningDate, cult).ToString("dd/MM/yyyy");
                }
                else
                {
                    objArray[16] = (object)Convert.ToDateTime(onboarding.expectedJoiningDate, cult).ToString("dd/MM/yyyy");
                }
                objArray[17] = (object)"</td></tr>";
                str3 = string.Concat(objArray);
                ++num;
            }
            string str6 = str3 + str2;
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.office365.com";
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential(str4, password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage message = new MailMessage();
                message.From = new MailAddress(str4);
                foreach (string toMailId in toMailIDs)
                    message.To.Add(new MailAddress(toMailId));
                foreach (var mail in ccMailID)
                {
                    message.CC.Add(new MailAddress(mail));
                }
                message.Subject = "Upcoming (7 Days Joining) Onboarding Requests";
                message.IsBodyHtml = true;
                message.Body = str6;
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
            }
        }

        public static void SendForgotPasswordMail(string toMailID)
        {
            List<string> ccMailID = new List<string>() { "mohammadzubair.n@hcl.com", "vishesh.dvivedi@hcl.com" };
            string str1 = "<!DOCTYPE html><html lang=en xmlns=http://www.w3.org/1999/xhtml xmlns:o=urn:schemas-microsoft-com:office:office xmlns:v=urn:schemas-microsoft-com:vml><title>Welcome to [Coded Mails]</title><meta content='IE=edge' http-equiv=X-UA-Compatible><meta content='text/html; charset=UTF-8' http-equiv=Content-Type><meta content='width=device-width,initial-scale=1' name=viewport><style>#outlook a{padding: 0}body{margin: 0;padding: 0;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%}table,td{border-collapse: collapse;mso-table-lspace: 0;mso-table-rspace: 0}img{border: 0;height: auto;line-height: 100%;outline: 0;text-decoration: none;-ms-interpolation-mode: bicubic}p{display: block;margin: 13px 0}</style><link href='https://fonts.googleapis.com/css?family=Muli:300,400,700' rel=stylesheet><style>@import url(https://fonts.googleapis.com/css?family=Muli:300,400,700);</style><style>@media only screen and (min-width:480px){.mj-column-per-100{width: 100%!important;max-width: 100%}}</style><style>@media only screen and (max-width:480px){table.mj-full-width-mobile{width: 100%!important}td.mj-full-width-mobile{width: auto!important}}</style><style>a,span,td,th{-webkit-font-smoothing: antialiased!important;-moz-osx-font-smoothing: grayscale!important}</style><body style=background-color:#006db6><div style=display:none;font-size:1px;color:#fff;line-height:1px;max-height:0;max-width:0;opacity:0;overflow:hidden></div><div style=background-color:#006db6><div style='margin:0 auto;'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style=font-size:0;word-break:break-word><div style=height:20px></div><tr></table></div></table></div><div style='background:#fff;background-color:#fff;margin:0 auto;max-width: 1000px'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=background:#fff;background-color:#fff;width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div style='margin:0 auto;'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style=direction:ltr;font-size:0;padding:0;text-align:center><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style='font-size:0;padding:10px 25px;word-break:break-word' align=left><div style=font-family:Muli,Arial,sans-serif;font-size:20px;font-weight:400;line-height:30px;text-align:left;color:#333><h1 style=margin:0;font-size:24px;line-height:normal;font-weight:700>Forgot Password</h1></div><tr><td style='font-size:0;padding:10px 25px;word-break:break-word'><p style='border-top:solid 1px #f4f5fb;font-size:1px;margin:0 auto;width:100%'><tr><td style='font-size:0;padding:10px 25px;word-break:break-word' align=left><div style=font-family:Muli,Arial,sans-serif;font-size:16px;font-weight:400;line-height:20px;text-align:left;color:#333><p style=margin:0>Dear User,</p><br/><p style=margin:0>Click on the below link to change your account password</p><br><a style='background-color: #0066AE; color: white; padding: 7px; text-decoration: none;' href='" + Ticketing_Dashboard.Utility.Mail.GenerateForgotPasswordViewURL(toMailID)[1] + "'>Click Here</a><br/></div></table></div></table></div><div style='margin:0 auto'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style=direction:ltr;font-size:0;padding:0;text-align:center><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style='font-size:0;padding:10px 25px;word-break:break-word' align=left><div style=font-family:Muli,Arial,sans-serif;font-size:16px;font-weight:400;line-height:20px;text-align:left;color:#333><p><p style=margin:0>If you have any questions or need further information, you can <a href=# style=color:#ac7b4c;text-decoration:none>contact administrator.</a></div></table></div></table></div><div style='margin:0 auto;max-width:600px'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;padding-bottom:0;padding-top:0;text-align:center'></table></div></table></div><div style='margin:0 auto;max-width:600px'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style='font-size:0;padding:10px 25px;word-break:break-word' align=center><div style=font-family:Muli,Arial,sans-serif;font-size:14px;font-weight:400;line-height:20px;text-align:center;color:white>© 2021 HCL Technologies Inc.</div></table></div></table></div><div style='margin:0 auto;max-width:600px'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=width:100% align=center><tr><td style='direction:ltr;font-size:0;padding:20px 0;text-align:center'><div style=font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100% class='mj-column-per-100 mj-outlook-group-fix'><table border=0 cellpadding=0 cellspacing=0 role=presentation style=vertical-align:top width=100%><tr><td style=font-size:0;word-break:break-word><div style=height:1px></div></table></div></table></div></div>";
            string str2 = "msftpmoservicexchange@outlook.com";
            string password = "Msft@1234!";
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.office365.com";
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential(str2, password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage message = new MailMessage();
                message.From = new MailAddress(str2);
                message.To.Add(new MailAddress(toMailID));
                foreach (var mail in ccMailID)
                {
                    message.CC.Add(new MailAddress(mail));
                }
                message.Subject = "Forgot Password";
                message.IsBodyHtml = true;
                message.Body = str1;
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
