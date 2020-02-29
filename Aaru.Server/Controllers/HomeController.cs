// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : HomeController.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : Aaru Server.
//
// --[ Description ] ----------------------------------------------------------
//
//     Provides documentation data for razor views.
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
// Copyright © 2011-2020 Natalia Portillo
// ****************************************************************************/

using System;
using System.IO;
using System.Reflection;
using Markdig;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DiscImageChef.Server.Controllers
{
    public class HomeController : Controller
    {
        readonly IWebHostEnvironment _environment;

        public HomeController(IWebHostEnvironment environment) => _environment = environment;

        [Route(""), Route("README")]
        public ActionResult Index()
        {
            var sr =
                new StreamReader(Path.Combine(_environment.ContentRootPath ?? throw new InvalidOperationException(),
                                              "docs", "README.md"));

            string mdcontent = sr.ReadToEnd();
            sr.Close();

            mdcontent = mdcontent.Replace(".md)", ")");

            ViewBag.Markdown = Markdown.ToHtml(mdcontent);

            ViewBag.lblVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            return View();
        }

        [Route("Changelog")]
        public ActionResult Changelog()
        {
            var sr =
                new StreamReader(Path.Combine(_environment.ContentRootPath ?? throw new InvalidOperationException(),
                                              "docs", "Changelog.md"));

            string mdcontent = sr.ReadToEnd();
            sr.Close();

            mdcontent = mdcontent.Replace(".md)", ")");

            ViewBag.Markdown = Markdown.ToHtml(mdcontent);

            ViewBag.lblVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            return View();
        }

        [Route("CODE_OF_CONDUCT")]
        public ActionResult CODE_OF_CONDUCT()
        {
            var sr =
                new StreamReader(Path.Combine(_environment.ContentRootPath ?? throw new InvalidOperationException(),
                                              "docs", "CODE_OF_CONDUCT.md"));

            string mdcontent = sr.ReadToEnd();
            sr.Close();

            mdcontent = mdcontent.Replace(".md)", ")").Replace("(.github/", "(");

            ViewBag.Markdown = Markdown.ToHtml(mdcontent);

            ViewBag.lblVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            return View();
        }

        [Route("PULL_REQUEST_TEMPLATE")]
        public ActionResult PULL_REQUEST_TEMPLATE()
        {
            var sr =
                new StreamReader(Path.Combine(_environment.ContentRootPath ?? throw new InvalidOperationException(),
                                              "docs", "PULL_REQUEST_TEMPLATE.md"));

            string mdcontent = sr.ReadToEnd();
            sr.Close();

            mdcontent = mdcontent.Replace(".md)", ")").Replace("(.github/", "(");

            ViewBag.Markdown = Markdown.ToHtml(mdcontent);

            ViewBag.lblVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            return View();
        }

        [Route("ISSUE_TEMPLATE")]
        public ActionResult ISSUE_TEMPLATE()
        {
            var sr =
                new StreamReader(Path.Combine(_environment.ContentRootPath ?? throw new InvalidOperationException(),
                                              "docs", "ISSUE_TEMPLATE.md"));

            string mdcontent = sr.ReadToEnd();
            sr.Close();

            mdcontent = mdcontent.Replace(".md)", ")").Replace("(.github/", "(");

            ViewBag.Markdown = Markdown.ToHtml(mdcontent);

            ViewBag.lblVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            return View();
        }

        [Route("CONTRIBUTING")]
        public ActionResult CONTRIBUTING()
        {
            var sr =
                new StreamReader(Path.Combine(_environment.ContentRootPath ?? throw new InvalidOperationException(),
                                              "docs", "CONTRIBUTING.md"));

            string mdcontent = sr.ReadToEnd();
            sr.Close();

            mdcontent = mdcontent.Replace(".md)", ")").Replace("(.github/", "(");

            ViewBag.Markdown = Markdown.ToHtml(mdcontent);

            ViewBag.lblVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            return View();
        }

        [Route("DONATING"), Route("NEEDED")]
        public ActionResult Needed()
        {
            var sr =
                new StreamReader(Path.Combine(_environment.ContentRootPath ?? throw new InvalidOperationException(),
                                              "docs", "NEEDED.md"));

            string mdcontent = sr.ReadToEnd();
            sr.Close();

            mdcontent = mdcontent.Replace(".md)", ")");

            ViewBag.Markdown = Markdown.ToHtml(mdcontent);

            ViewBag.lblVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            return View();
        }

        [Route("NEEDINFO")]
        public ActionResult NeedInfo()
        {
            var sr =
                new StreamReader(Path.Combine(_environment.ContentRootPath ?? throw new InvalidOperationException(),
                                              "docs", "NEEDINFO.md"));

            string mdcontent = sr.ReadToEnd();
            sr.Close();

            mdcontent = mdcontent.Replace(".md)", ")");

            ViewBag.Markdown = Markdown.ToHtml(mdcontent);

            ViewBag.lblVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            return View();
        }

        [Route("TODO")]
        public ActionResult TODO()
        {
            var sr =
                new StreamReader(Path.Combine(_environment.ContentRootPath ?? throw new InvalidOperationException(),
                                              "docs", "TODO.md"));

            string mdcontent = sr.ReadToEnd();
            sr.Close();

            mdcontent = mdcontent.Replace(".md)", ")");

            ViewBag.Markdown = Markdown.ToHtml(mdcontent);

            ViewBag.lblVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            return View();
        }
    }
}