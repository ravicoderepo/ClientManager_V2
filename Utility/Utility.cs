﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Globalization;

namespace Utility
{

    public static class Error
    {
        public static List<string> GetEntityErrors(Exception e)
        {
            List<string> errList = new List<string>();

            DbEntityValidationException ex = e is DbEntityValidationException ?
                e as DbEntityValidationException : null;

            if (ex == null) { return errList; }

            foreach (var eve in ex.EntityValidationErrors)
            {
                var Errors = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);

                foreach (var ve in eve.ValidationErrors)
                {
                    errList.Add(string.Format("- Field: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                }
            }

            return errList;
        }

        public static string GetEntityErrorsAsSting(Exception e)
        {
            List<string> errList = new List<string>();

            DbEntityValidationException ex = e is DbEntityValidationException ?
                e as DbEntityValidationException : null;

            if (ex == null) { return string.Empty; }

            StringBuilder sbErrors = new StringBuilder();

            foreach (var eve in ex.EntityValidationErrors)
            {
                var Errors = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);

                foreach (var ve in eve.ValidationErrors)
                {
                    sbErrors.AppendLine(string.Format("- Field: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                }
            }

            return sbErrors.ToString();
        }


    }

    public static class DefaultList
    {
        public static List<SelectListItem> GetUnitList()
        {
            List<SelectListItem> unitList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="No.", Value="No."},
                new SelectListItem() {Text="KW", Value="KW"},
            };

            return unitList;
        }

        public static List<SelectListItem> GetPaymentModeList()
        {
            List<SelectListItem> items = new System.Collections.Generic.List<SelectListItem>();
            items.Insert(0, new SelectListItem()
            {
                Text = "",
                Value = ""
            });
            items.Insert(1, new SelectListItem()
            {
                Text = "ATM",
                Value = "ATM"
            });
            items.Insert(2, new SelectListItem()
            {
                Text = "Account Transfer",
                Value = "Account Transfer"
            });
            items.Insert(3, new SelectListItem()
            {
                Text = "Cash",
                Value = "Cash"
            });
            items.Insert(4, new SelectListItem()
            {
                Text = "Other Receivables",
                Value = "Other Receivables"
            });

            return items;
        }

        public static List<SelectListItem> GetDocumentStatusList()
        {
            List<SelectListItem> items = new System.Collections.Generic.List<SelectListItem>();
            items.Insert(0, new SelectListItem()
            {
                Text = "Draft",
                Value = "Draft"
            });
            items.Insert(1, new SelectListItem()
            {
                Text = "Published",
                Value = "Published"
            });
            items.Insert(2, new SelectListItem()
            {
                Text = "Archived",
                Value = "Archived"
            });

            return items;
        }

        public static List<SelectListItem> GetDocumentTypeList()
        {
            List<SelectListItem> items = new System.Collections.Generic.List<SelectListItem>();
            items.Insert(0, new SelectListItem()
            {
                Text = "Image",
                Value = "Image"
            });
            items.Insert(1, new SelectListItem()
            {
                Text = "PDF",
                Value = "PDF"
            });
            items.Insert(2, new SelectListItem()
            {
                Text = "Document",
                Value = "Document"
            });
            items.Insert(3, new SelectListItem()
            {
                Text = "Zip",
                Value = "Zip"
            });
            items.Insert(4, new SelectListItem()
            {
                Text = "Others",
                Value = "Others"
            });

            return items;
        }

        public static List<SelectListItem> GetModuleList()
        {
            List<SelectListItem> items = new System.Collections.Generic.List<SelectListItem>();
            items.Insert(0, new SelectListItem()
            {
                Text = "Expence Tracker",
                Value = "Expence Tracker"
            });

            return items;
        }

        public static List<SelectListItem> GetStatusList()
        {
            List<SelectListItem> items = new System.Collections.Generic.List<SelectListItem>();
            items.Insert(0, new SelectListItem()
            {
                Text = "De-Active",
                Value = "0"
            });
            items.Insert(1, new SelectListItem()
            {
                Text = "Active",
                Value = "1"
            });
            return items;
        }

        public static List<SelectListItem> GetPaymentStatusList()
        {
            List<SelectListItem> items = new System.Collections.Generic.List<SelectListItem>();
            items.Insert(0, new SelectListItem()
            {
                Text = "Approved",
                Value = "Approved"
            });
            items.Insert(1, new SelectListItem()
            {
                Text = "Pending",
                Value = "Pending"
            });
            items.Insert(2, new SelectListItem()
            {
                Text = "Verified",
                Value = "Verified"
            });

            return items;
        }
    }

    public static class Emails
    {
        public static bool SendEmail(string to, string subject, string body)
        {
            //File as Param
            //HttpPostedFile postedFile
            try
            {
                string from = ConfigSettings.ReadSetting("EmailFrom");
                string password = ConfigSettings.ReadSetting("EmailFromPassword").ToString();
                string emailHost = ConfigSettings.ReadSetting("EmailHost").ToString();

                using (MailMessage mm = new MailMessage(from, to))
                {
                    mm.Subject = subject;
                    mm.Body = body;
                    //if (postedFile.ContentLength > 0)
                    //{
                    //    string fileName = Path.GetFileName(postedFile.FileName);
                    //    mm.Attachments.Add(new Attachment(postedFile.InputStream, fileName));
                    //}
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = emailHost;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    NetworkCredential NetworkCred = new NetworkCredential(from, password);
                    smtp.EnableSsl = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static string GetEmailTemplate(string EmailTemplateType)
        {

            if (EmailTemplateType == "PettyCashAdded")
            {
                return @"<!DOCTYPE html>
                            <html>
                                <body style='font-size:12px'>
                                    <p>Dear Accounts Team, </p>
                                    <p>The amount of  <b>Rs.{PettyCashValue}</b> has been sent/added as PettyCash by <b>{PaymentMode} </b> on <b>{AmountReceivedDate}</b>.<p>
                                    <p>The Overall due PettyCash <b>Rs.{PendingPettyCash}</b> as on <b>" + DateTime.Now.ToShortDateString() + @"</b>.</p>
                                    <p>Description : <b> {Description} </b> </p>
                                    Regards,</br>
                                    Admin Team
                                    <p>
                                    <hr>
                                        <span style='font-size:10px'>This is auto generated email, please do not reply.</small>
                                    <p>
                                </body>
                            </html>";
            }
            else
            {
                return "";
            }
        }
    }

    public static class ConstantData
    {
        public static string ToMonthName(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
        }

        public static string ToShortMonthName(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dateTime.Month);
        }
    }

    public static class ConfigSettings
    {
        public static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                return "";
            }
        }

        public static void ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Console.WriteLine("AppSettings is empty.");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        //Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException)
            {
                //Console.WriteLine("Error reading app settings");
            }
        }

        public static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {

            }
        }
    }

    public static class FileProcess
    {
        static string base64String = null;
        public static string ImageToBase64(string filePath)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(filePath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }
        public static System.Drawing.Image Base64ToImage(string base64Text)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }                      
    }
}
