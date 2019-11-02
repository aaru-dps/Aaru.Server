// /***************************************************************************
// The Disc Image Chef
// ----------------------------------------------------------------------------
//
// Filename       : UploadReportController.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : DiscImageChef Server.
//
// --[ Description ] ----------------------------------------------------------
//
//     Handles report uploads.
//
// --[ License ] --------------------------------------------------------------
//
//     This library is free software; you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as
//     published by the Free Software Foundation; either version 2.1 of the
//     License, or (at your option) any later version.
//
//     This library is distributed in the hope that it will be useful, but
//     WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//     Lesser General Public License for more details.
//
//     You should have received a copy of the GNU Lesser General Public
//     License along with this library; if not, see <http://www.gnu.org/licenses/>.
//
// ----------------------------------------------------------------------------
// Copyright © 2011-2019 Natalia Portillo
// ****************************************************************************/

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using Cinchoo.PGP;
using DiscImageChef.CommonTypes.Metadata;
using DiscImageChef.Server.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using MimeKit;
using Newtonsoft.Json;

namespace DiscImageChef.Server.Controllers
{
    public class UploadReportController : Controller
    {
        private DicServerContext ctx;
        private IWebHostEnvironment _environment;

        public UploadReportController(IWebHostEnvironment environment, DicServerContext _ctx)
        {
            _environment = environment;
            ctx = _ctx;
        }

        /// <summary>
        ///     Receives a report from DiscImageChef.Core, verifies it's in the correct format and stores it on the server
        /// </summary>
        /// <returns>HTTP response</returns>
        [Route("api/uploadreport")]
        [HttpPost]
        public async Task<IActionResult> UploadReport()
        {
            ContentResult response = new ContentResult {StatusCode = (int)HttpStatusCode.OK, ContentType = "text/plain"};

            try
            {
                DeviceReport newReport = new DeviceReport();
                HttpRequest  request   = HttpContext.Request;

                XmlSerializer xs = new XmlSerializer(newReport.GetType());
                newReport = (DeviceReport) xs.Deserialize(new StringReader(await new StreamReader(request.Body).ReadToEndAsync()));

                if(newReport == null)
                {
                    response.Content = "notstats";
                    return response;
                }

                DeviceReportV2 reportV2 = new DeviceReportV2(newReport);
                StringWriter   jsonSw   = new StringWriter();
                jsonSw.Write(JsonConvert.SerializeObject(reportV2, Formatting.Indented,
                                                         new JsonSerializerSettings
                                                         {
                                                             NullValueHandling = NullValueHandling.Ignore
                                                         }));
                string reportV2String = jsonSw.ToString();
                jsonSw.Close();

                ctx.Reports.Add(new UploadedReport(reportV2));
                ctx.SaveChanges();

                MemoryStream         pgpIn  = new MemoryStream(Encoding.UTF8.GetBytes(reportV2String));
                MemoryStream         pgpOut = new MemoryStream();
                ChoPGPEncryptDecrypt pgp    = new ChoPGPEncryptDecrypt();
                pgp.Encrypt(pgpIn, pgpOut,
                            Path.Combine(_environment.ContentRootPath ?? throw new InvalidOperationException(),
                                         "public.asc"), true);
                pgpOut.Position = 0;
                reportV2String  = Encoding.UTF8.GetString(pgpOut.ToArray());

                MimeMessage message = new MimeMessage
                {
                    Subject = "New device report (old version)",
                    Body    = new TextPart("plain") {Text = reportV2String}
                };
                message.From.Add(new MailboxAddress("DiscImageChef",  "dic@claunia.com"));
                message.To.Add(new MailboxAddress("Natalia Portillo", "claunia@claunia.com"));

                using(SmtpClient client = new SmtpClient())
                {
                    client.Connect("mail.claunia.com", 25, false);
                    client.Send(message);
                    client.Disconnect(true);
                }

                response.Content = "ok";
                return response;
            }
            // ReSharper disable once RedundantCatchClause
            catch
            {
                #if DEBUG
                if(Debugger.IsAttached) throw;
                #endif
                response.Content = "error";
                return response;
            }
        }

        /// <summary>
        ///     Receives a report from DiscImageChef.Core, verifies it's in the correct format and stores it on the server
        /// </summary>
        /// <returns>HTTP response</returns>
        [Route("api/uploadreportv2")]
        [HttpPost]
        public async Task<IActionResult> UploadReportV2()
        {
            ContentResult response = new ContentResult {StatusCode = (int)HttpStatusCode.OK, ContentType = "text/plain"};

            try
            {
                HttpRequest request = HttpContext.Request;

                StreamReader   sr         = new StreamReader(request.Body);
                string         reportJson = await sr.ReadToEndAsync();
                DeviceReportV2 newReport  = JsonConvert.DeserializeObject<DeviceReportV2>(reportJson);

                if(newReport == null)
                {
                    response.Content = "notstats";
                    return response;
                }

                ctx.Reports.Add(new UploadedReport(newReport));
                ctx.SaveChanges();

                MemoryStream         pgpIn  = new MemoryStream(Encoding.UTF8.GetBytes(reportJson));
                MemoryStream         pgpOut = new MemoryStream();
                ChoPGPEncryptDecrypt pgp    = new ChoPGPEncryptDecrypt();
                pgp.Encrypt(pgpIn, pgpOut,
                            Path.Combine(_environment.ContentRootPath ?? throw new InvalidOperationException(),
                                         "public.asc"), true);
                pgpOut.Position = 0;
                reportJson      = Encoding.UTF8.GetString(pgpOut.ToArray());

                MimeMessage message = new MimeMessage
                {
                    Subject = "New device report", Body = new TextPart("plain") {Text = reportJson}
                };
                message.From.Add(new MailboxAddress("DiscImageChef",  "dic@claunia.com"));
                message.To.Add(new MailboxAddress("Natalia Portillo", "claunia@claunia.com"));

                using(SmtpClient client = new SmtpClient())
                {
                    client.Connect("mail.claunia.com", 25, false);
                    client.Send(message);
                    client.Disconnect(true);
                }

                response.Content = "ok";
                return response;
            }
            // ReSharper disable once RedundantCatchClause
            catch
            {
                #if DEBUG
                if(Debugger.IsAttached) throw;
                #endif
                response.Content ="error";
                return response;
            }
        }
    }
}