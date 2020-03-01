// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : Program.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : Aaru Server Task.
//
// --[ Description ] ----------------------------------------------------------
//
//     Runs time consuming server tasks.
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using Aaru.Server.Models;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;

namespace Aaru.Server.Task
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            DateTime start, end;

            start = DateTime.UtcNow;
            System.Console.WriteLine("{0}: Connecting to database...", DateTime.UtcNow);
            var ctx = new AaruServerContext();
            end = DateTime.UtcNow;
            System.Console.WriteLine("{0}: Took {1:F2} seconds", end, (end - start).TotalSeconds);

            System.Console.WriteLine("{0}: Migrating database to latest version...", DateTime.UtcNow);
            start = DateTime.UtcNow;
            ctx.Database.Migrate();
            end = DateTime.UtcNow;
            System.Console.WriteLine("{0}: Took {1:F2} seconds", DateTime.UtcNow, (end - start).TotalSeconds);

            WebClient client;

            try
            {
                System.Console.WriteLine("{0}: Retrieving USB IDs from Linux USB...", DateTime.UtcNow);
                start  = DateTime.UtcNow;
                client = new WebClient();
                var sr = new StringReader(client.DownloadString("http://www.linux-usb.org/usb.ids"));
                end = DateTime.UtcNow;
                System.Console.WriteLine("{0}: Took {1:F2} seconds", end, (end - start).TotalSeconds);

                UsbVendor vendor           = null;
                int       newVendors       = 0;
                int       newProducts      = 0;
                int       modifiedVendors  = 0;
                int       modifiedProducts = 0;
                int       counter          = 0;

                start = DateTime.UtcNow;
                System.Console.WriteLine("{0}: Adding and updating database entries...", DateTime.UtcNow);

                do
                {
                    if(counter == 1000)
                    {
                        DateTime start2 = DateTime.UtcNow;
                        System.Console.WriteLine("{0}: Saving changes", start2);
                        ctx.SaveChanges();
                        end = DateTime.UtcNow;
                        System.Console.WriteLine("{0}: Took {1:F2} seconds", end, (end - start2).TotalSeconds);
                        counter = 0;
                    }

                    string line = sr.ReadLine();

                    if(line is null)
                        break;

                    if(line.Length == 0 ||
                       line[0]     == '#')
                        continue;

                    ushort number;
                    string name;

                    if(line[0] == '\t')
                    {
                        try
                        {
                            number = Convert.ToUInt16(line.Substring(1, 4), 16);
                        }
                        catch(FormatException)
                        {
                            continue;
                        }

                        if(number == 0)
                            continue;

                        name = line.Substring(7);

                        UsbProduct product =
                            ctx.UsbProducts.FirstOrDefault(p => p.ProductId       == number && p.Vendor != null &&
                                                                p.Vendor.VendorId == vendor.VendorId);

                        if(product is null)
                        {
                            product = new UsbProduct(vendor, number, name);
                            ctx.UsbProducts.Add(product);

                            System.Console.WriteLine("{0}: Will add product {1} with ID {2:X4} and vendor {3} ({4:X4})",
                                                     DateTime.UtcNow, product.Product, product.ProductId,
                                                     product.Vendor?.Vendor ?? "null", product.Vendor?.VendorId ?? 0);

                            newProducts++;
                            counter++;
                        }
                        else if(name != product.Product)
                        {
                            System.Console.
                                   WriteLine("{0}: Will modify product with ID {1:X4} and vendor {2} ({3:X4}) from \"{4}\" to \"{5}\"",
                                             DateTime.UtcNow, product.ProductId, product.Vendor?.Vendor ?? "null",
                                             product.Vendor?.VendorId                                   ?? 0,
                                             product.Product, name);

                            product.Product      = name;
                            product.ModifiedWhen = DateTime.UtcNow;
                            modifiedProducts++;
                            counter++;
                        }

                        continue;
                    }

                    try
                    {
                        number = Convert.ToUInt16(line.Substring(0, 4), 16);
                    }
                    catch(FormatException)
                    {
                        continue;
                    }

                    if(number == 0)
                        continue;

                    name = line.Substring(6);

                    vendor = ctx.UsbVendors.FirstOrDefault(v => v.VendorId == number);

                    if(vendor is null)
                    {
                        vendor = new UsbVendor(number, name);
                        ctx.UsbVendors.Add(vendor);

                        System.Console.WriteLine("{0}: Will add vendor {1} with ID {2:X4}", DateTime.UtcNow,
                                                 vendor.Vendor, vendor.VendorId);

                        newVendors++;
                        counter++;
                    }
                    else if(name != vendor.Vendor)
                    {
                        System.Console.WriteLine("{0}: Will modify vendor with ID {1:X4} from \"{2}\" to \"{3}\"",
                                                 DateTime.UtcNow, vendor.VendorId, vendor.Vendor, name);

                        vendor.Vendor       = name;
                        vendor.ModifiedWhen = DateTime.UtcNow;
                        modifiedVendors++;
                        counter++;
                    }
                } while(true);

                end = DateTime.UtcNow;
                System.Console.WriteLine("{0}: Took {1:F2} seconds", end, (end - start).TotalSeconds);

                System.Console.WriteLine("{0}: Saving database changes...", DateTime.UtcNow);
                start = DateTime.UtcNow;
                ctx.SaveChanges();
                end = DateTime.UtcNow;
                System.Console.WriteLine("{0}: Took {1:F2} seconds", end, (end - start).TotalSeconds);

                System.Console.WriteLine("{0}: {1} vendors added.", DateTime.UtcNow, newVendors);
                System.Console.WriteLine("{0}: {1} products added.", DateTime.UtcNow, newProducts);
                System.Console.WriteLine("{0}: {1} vendors modified.", DateTime.UtcNow, modifiedVendors);
                System.Console.WriteLine("{0}: {1} products modified.", DateTime.UtcNow, modifiedProducts);

                System.Console.WriteLine("{0}: Looking up a vendor", DateTime.UtcNow);
                start  = DateTime.UtcNow;
                vendor = ctx.UsbVendors.FirstOrDefault(v => v.VendorId == 0x8086);

                if(vendor is null)
                    System.Console.WriteLine("{0}: Error, could not find vendor.", DateTime.UtcNow);
                else
                    System.Console.WriteLine("{0}: Found {1}.", DateTime.UtcNow, vendor.Vendor);

                end = DateTime.UtcNow;
                System.Console.WriteLine("{0}: Took {1:F2} seconds", end, (end - start).TotalSeconds);

                System.Console.WriteLine("{0}: Looking up a product", DateTime.UtcNow);
                start = DateTime.UtcNow;

                UsbProduct prd =
                    ctx.UsbProducts.FirstOrDefault(p => p.ProductId == 0x0001 && p.Vendor.VendorId == 0x8086);

                if(prd is null)
                    System.Console.WriteLine("{0}: Error, could not find product.", DateTime.UtcNow);
                else
                    System.Console.WriteLine("{0}: Found {1}.", DateTime.UtcNow, prd.Product);

                end = DateTime.UtcNow;
                System.Console.WriteLine("{0}: Took {1:F2} seconds", end, (end - start).TotalSeconds);
            }
            catch(Exception ex)
            {
            #if DEBUG
                if(Debugger.IsAttached)
                    throw;
            #endif
                System.Console.WriteLine("{0}: Exception {1} filling USB IDs...", DateTime.UtcNow, ex);
            }

            System.Console.WriteLine("{0}: Fixing all devices without modification time...", DateTime.UtcNow);
            start = DateTime.UtcNow;

            foreach(Device device in ctx.Devices.Where(d => d.ModifiedWhen == null))
                device.ModifiedWhen = device.AddedWhen;

            end = DateTime.UtcNow;
            System.Console.WriteLine("{0}: Took {1:F2} seconds", end, (end - start).TotalSeconds);

            System.Console.WriteLine("{0}: Committing changes...", DateTime.UtcNow);
            start = DateTime.UtcNow;
            ctx.SaveChanges();
            end = DateTime.UtcNow;
            System.Console.WriteLine("{0}: Took {1:F2} seconds", end, (end - start).TotalSeconds);

            try
            {
                System.Console.WriteLine("{0}: Retrieving CompactDisc read offsets from AccurateRip...",
                                         DateTime.UtcNow);

                start = DateTime.UtcNow;

                client = new WebClient();
                string html = client.DownloadString("http://www.accuraterip.com/driveoffsets.htm");
                end = DateTime.UtcNow;
                System.Console.WriteLine("{0}: Took {1:F2} seconds", end, (end - start).TotalSeconds);

                // The HTML is too malformed to process easily, so find start of table
                html = "<html><body><table><tr>" +
                       html.Substring(html.IndexOf("<td bgcolor=\"#000000\">", StringComparison.Ordinal));

                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                HtmlNode firstTable = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/table[1]");

                bool firstRow = true;

                int addedOffsets    = 0;
                int modifiedOffsets = 0;

                System.Console.WriteLine("{0}: Processing offsets...", DateTime.UtcNow);
                start = DateTime.UtcNow;

                foreach(HtmlNode row in firstTable.Descendants("tr"))
                {
                    HtmlNode[] columns = row.Descendants("td").ToArray();

                    if(columns.Length != 4)
                    {
                        System.Console.WriteLine("{0}: Row does not have correct number of columns...",
                                                 DateTime.UtcNow);

                        continue;
                    }

                    string column0 = columns[0].InnerText;
                    string column1 = columns[1].InnerText;
                    string column2 = columns[2].InnerText;
                    string column3 = columns[3].InnerText;

                    if(firstRow)
                    {
                        if(column0.ToLowerInvariant() != "cd drive")
                        {
                            System.Console.WriteLine("{0}: Unexpected header \"{1}\" found...", DateTime.UtcNow,
                                                     columns[0].InnerText);

                            break;
                        }

                        if(column1.ToLowerInvariant() != "correction offset")
                        {
                            System.Console.WriteLine("{0}: Unexpected header \"{1}\" found...", DateTime.UtcNow,
                                                     columns[1].InnerText);

                            break;
                        }

                        if(column2.ToLowerInvariant() != "submitted by")
                        {
                            System.Console.WriteLine("{0}: Unexpected header \"{1}\" found...", DateTime.UtcNow,
                                                     columns[2].InnerText);

                            break;
                        }

                        if(column3.ToLowerInvariant() != "percentage agree")
                        {
                            System.Console.WriteLine("{0}: Unexpected header \"{1}\" found...", DateTime.UtcNow,
                                                     columns[3].InnerText);

                            break;
                        }

                        firstRow = false;

                        continue;
                    }

                    string manufacturer;
                    string model;

                    if(column0[0] == '-' &&
                       column0[1] == ' ')
                    {
                        manufacturer = null;
                        model        = column0.Substring(2).Trim();
                    }
                    else
                    {
                        int cutOffset = column0.IndexOf(" - ", StringComparison.Ordinal);

                        if(cutOffset == -1)
                        {
                            manufacturer = null;
                            model        = column0;
                        }
                        else
                        {
                            manufacturer = column0.Substring(0, cutOffset).Trim();
                            model        = column0.Substring(cutOffset + 3).Trim();
                        }
                    }

                    switch(manufacturer)
                    {
                        case "Lite-ON":
                            manufacturer = "JLMS";

                            break;
                        case "LG Electronics":
                            manufacturer = "HL-DT-ST";

                            break;
                        case "Panasonic":
                            manufacturer = "MATSHITA";

                            break;
                    }

                    CompactDiscOffset cdOffset =
                        ctx.CdOffsets.FirstOrDefault(o => o.Manufacturer == manufacturer && o.Model == model);

                    if(column1.ToLowerInvariant() == "[purged]")
                    {
                        if(cdOffset != null)
                            ctx.CdOffsets.Remove(cdOffset);

                        continue;
                    }

                    if(!short.TryParse(column1, out short offset))
                        continue;

                    if(!int.TryParse(column2, out int submissions))
                        continue;

                    if(column3[column3.Length - 1] != '%')
                        continue;

                    column3 = column3.Substring(0, column3.Length - 1);

                    if(!float.TryParse(column3, out float percentage))
                        continue;

                    percentage /= 100;

                    if(cdOffset is null)
                    {
                        cdOffset = new CompactDiscOffset
                        {
                            AddedWhen    = DateTime.UtcNow, ModifiedWhen = DateTime.UtcNow, Agreement = percentage,
                            Manufacturer = manufacturer, Model           = model, Offset              = offset,
                            Submissions  = submissions
                        };

                        ctx.CdOffsets.Add(cdOffset);
                        addedOffsets++;
                    }
                    else
                    {
                        if(Math.Abs(cdOffset.Agreement - percentage) > 0)
                        {
                            cdOffset.Agreement    = percentage;
                            cdOffset.ModifiedWhen = DateTime.UtcNow;
                        }

                        if(cdOffset.Offset != offset)
                        {
                            cdOffset.Offset       = offset;
                            cdOffset.ModifiedWhen = DateTime.UtcNow;
                        }

                        if(cdOffset.Submissions != submissions)
                        {
                            cdOffset.Submissions  = submissions;
                            cdOffset.ModifiedWhen = DateTime.UtcNow;
                        }

                        if(Math.Abs(cdOffset.Agreement - percentage) > 0       ||
                           cdOffset.Offset                           != offset ||
                           cdOffset.Submissions                      != submissions)
                            modifiedOffsets++;
                    }

                    foreach(Device device in ctx.
                                             Devices.
                                             Where(d => d.Manufacturer == null && d.Model != null &&
                                                        d.Model.Trim() == model).
                                             Union(ctx.Devices.Where(d => d.Manufacturer        != null         &&
                                                                          d.Manufacturer.Trim() == manufacturer &&
                                                                          d.Model               != null         &&
                                                                          d.Model               == model)))
                    {
                        if(device.CdOffset     == cdOffset &&
                           device.ModifiedWhen == cdOffset.ModifiedWhen)
                            continue;

                        device.CdOffset     = cdOffset;
                        device.ModifiedWhen = cdOffset.ModifiedWhen;
                    }
                }

                end = DateTime.UtcNow;
                System.Console.WriteLine("{0}: Took {1:F2} seconds", end, (end - start).TotalSeconds);

                if(File.Exists("drive_offsets.json"))
                {
                    var sr = new StreamReader("drive_offsets.json");

                    CompactDiscOffset[] offsets = JsonSerializer.Deserialize<CompactDiscOffset[]>(sr.ReadToEnd());

                    if(offsets != null)
                    {
                        foreach(CompactDiscOffset offset in offsets)
                        {
                            CompactDiscOffset cdOffset =
                                ctx.CdOffsets.FirstOrDefault(o => o.Manufacturer == offset.Manufacturer &&
                                                                  o.Model        == offset.Model);

                            if(cdOffset is null)
                            {
                                offset.ModifiedWhen = DateTime.UtcNow;

                                ctx.CdOffsets.Add(offset);
                                addedOffsets++;
                            }
                            else
                            {
                                if(Math.Abs(cdOffset.Agreement - offset.Agreement) > 0 ||
                                   offset.Agreement                                < 0)
                                {
                                    cdOffset.Agreement    = offset.Agreement;
                                    cdOffset.ModifiedWhen = DateTime.UtcNow;
                                }

                                if(cdOffset.Offset != offset.Offset)
                                {
                                    cdOffset.Offset       = offset.Offset;
                                    cdOffset.ModifiedWhen = DateTime.UtcNow;
                                }

                                if(cdOffset.Submissions != offset.Submissions)
                                {
                                    cdOffset.Submissions  = offset.Submissions;
                                    cdOffset.ModifiedWhen = DateTime.UtcNow;
                                }

                                if(Math.Abs(cdOffset.Agreement - offset.Agreement) > 0              ||
                                   cdOffset.Offset                                 != offset.Offset ||
                                   cdOffset.Submissions                            != offset.Submissions)
                                    modifiedOffsets++;
                            }
                        }
                    }
                }

                System.Console.WriteLine("{0}: Committing changes...", DateTime.UtcNow);
                start = DateTime.UtcNow;
                ctx.SaveChanges();
                end = DateTime.UtcNow;
                System.Console.WriteLine("{0}: Took {1:F2} seconds", end, (end - start).TotalSeconds);

                System.Console.WriteLine("{0}: Added {1} offsets", end, addedOffsets);
                System.Console.WriteLine("{0}: Modified {1} offsets", end, modifiedOffsets);
            }
            catch(Exception ex)
            {
            #if DEBUG
                if(Debugger.IsAttached)
                    throw;
            #endif
                System.Console.WriteLine("{0}: Exception {1} filling CompactDisc read offsets...", DateTime.UtcNow, ex);
            }
        }
    }
}