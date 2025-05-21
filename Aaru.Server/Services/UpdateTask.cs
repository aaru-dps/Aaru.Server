using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using Aaru.CommonTypes.Enums;
using Aaru.Server.Database.Models;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Services;

public class UpdateTask : IHostedService, IDisposable
{
    readonly ILogger<UpdateTask> _logger;
    readonly IServiceProvider    _serviceProvider;
    int                          _running;
    Timer?                       _timer;

    public UpdateTask(ILogger<UpdateTask> logger, IServiceProvider serviceProvider)
    {
        _logger          = logger;
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _timer?.Dispose();
    }

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.FromMinutes(1), TimeSpan.FromHours(1));

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        if(Interlocked.CompareExchange(ref _running, 1, 0) != 0) return;

        try
        {
            Stopwatch stopwatch = new();
            var       counter   = 0;

            stopwatch.Restart();
            _logger.LogInformation("Connecting to database...");
            using IServiceScope scope = _serviceProvider.CreateScope();
            using DbContext     ctx   = scope.ServiceProvider.GetRequiredService<DbContext>();
            stopwatch.Stop();
            _logger.LogDebug("Took {TotalSeconds:F2} seconds", stopwatch.Elapsed.TotalSeconds);

            try
            {
                _logger.LogInformation("Retrieving USB IDs from Linux USB...");
                stopwatch.Restart();
                using var client = new WebClient();
                var       sr     = new StringReader(client.DownloadString("http://www.linux-usb.org/usb.ids"));
                stopwatch.Stop();
                _logger.LogDebug("Took {TotalSeconds:F2} seconds", stopwatch.Elapsed.TotalSeconds);

                UsbVendor vendor           = null;
                var       newVendors       = 0;
                var       newProducts      = 0;
                var       modifiedVendors  = 0;
                var       modifiedProducts = 0;

                stopwatch.Restart();
                _logger.LogInformation("Adding and updating database entries...");

                do
                {
                    if(counter == 1000)
                    {
                        Stopwatch stopwatch2 = new();
                        stopwatch2.Start();
                        _logger.LogInformation("Saving changes");
                        ctx.SaveChanges();
                        stopwatch2.Stop();

                        _logger.LogDebug("Took {TotalSeconds:F2} seconds", stopwatch2.Elapsed.TotalSeconds);

                        counter = 0;
                    }

                    string line = sr.ReadLine();

                    if(line is null) break;

                    if(line.Length == 0 || line[0] == '#') continue;

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

                        if(number == 0) continue;

                        name = line[7..];

                        UsbProduct product = ctx.UsbProducts.Include(p => p.Vendor)
                                                .FirstOrDefault(p => p.ProductId       == number &&
                                                                     p.Vendor.VendorId == vendor.VendorId);

                        if(product is null)
                        {
                            product = new UsbProduct(vendor, number, name);
                            ctx.UsbProducts.Add(product);

                            _logger
                               .LogInformation("Will add product {Product} with ID {ProductId:X4} and vendor {Vendor} ({VendorId:X4})",
                                               product.Product,
                                               product.ProductId,
                                               product.Vendor?.Vendor   ?? "null",
                                               product.Vendor?.VendorId ?? 0);

                            newProducts++;
                            counter++;
                        }
                        else if(name != product.Product)
                        {
                            _logger
                               .LogInformation("Will modify product with ID {ProductId:X4} and vendor {Vendor} ({VendorId:X4}) from \"{Product}\" to \"{Name}\"",
                                               product.ProductId,
                                               product.Vendor?.Vendor   ?? "null",
                                               product.Vendor?.VendorId ?? 0,
                                               product.Product,
                                               name);

                            product.Product      = name;
                            product.ModifiedWhen = DateTime.UtcNow;
                            modifiedProducts++;
                            counter++;
                        }

                        continue;
                    }

                    try
                    {
                        number = Convert.ToUInt16(line[..4], 16);
                    }
                    catch(FormatException)
                    {
                        continue;
                    }

                    if(number == 0) continue;

                    name = line[6..];

                    vendor = ctx.UsbVendors.FirstOrDefault(v => v.VendorId == number);

                    if(vendor is null)
                    {
                        vendor = new UsbVendor(number, name);
                        ctx.UsbVendors.Add(vendor);

                        _logger.LogInformation("Will add vendor {Vendor} with ID {VendorId:X4}",
                                               vendor.Vendor,
                                               vendor.VendorId);

                        newVendors++;
                        counter++;
                    }
                    else if(name != vendor.Vendor)
                    {
                        _logger
                           .LogInformation("Will modify vendor with ID {VendorId:X4} from \"{VendorId}\" to \"{Name}\"",
                                           vendor.VendorId,
                                           vendor.Vendor,
                                           name);

                        vendor.Vendor       = name;
                        vendor.ModifiedWhen = DateTime.UtcNow;
                        modifiedVendors++;
                        counter++;
                    }
                } while(true);

                stopwatch.Stop();
                _logger.LogDebug("Took {TotalSeconds:F2} seconds", stopwatch.Elapsed.TotalSeconds);

                _logger.LogInformation("Saving database changes...");
                stopwatch.Restart();
                ctx.SaveChanges();
                stopwatch.Stop();
                _logger.LogDebug("Took {TotalSeconds:F2} seconds", stopwatch.Elapsed.TotalSeconds);

                _logger.LogInformation("{NewVendors} vendors added",           newVendors);
                _logger.LogInformation("{NewProducts} products added",         newProducts);
                _logger.LogInformation("{ModifiedVendors} vendors modified",   modifiedVendors);
                _logger.LogInformation("{ModifiedProducts} products modified", modifiedProducts);

                _logger.LogInformation("Looking up a vendor");
                stopwatch.Restart();
                vendor = ctx.UsbVendors.FirstOrDefault(v => v.VendorId == 0x8086);

                if(vendor is null)
                    _logger.LogError("Error, could not find vendor");
                else
                    _logger.LogInformation("Found {Vendor}", vendor.Vendor);

                stopwatch.Stop();
                _logger.LogDebug("Took {TotalSeconds:F2} seconds", stopwatch.Elapsed.TotalSeconds);

                _logger.LogInformation("Looking up a product");
                stopwatch.Restart();

                UsbProduct prd =
                    ctx.UsbProducts.FirstOrDefault(p => p.ProductId == 0x0001 && p.Vendor.VendorId == 0x8086);

                if(prd is null)
                    _logger.LogError("Error, could not find product");
                else
                    _logger.LogInformation("Found {Product}", prd.Product);

                stopwatch.Stop();
                _logger.LogDebug("Took {TotalSeconds:F2} seconds", stopwatch.Elapsed.TotalSeconds);
            }
            catch(Exception ex)
            {
#if DEBUG
                if(Debugger.IsAttached) throw;
#endif
                _logger.LogCritical("Exception {Ex} filling USB IDs...", ex);
            }

            _logger.LogInformation("Fixing all devices without modification time...");
            stopwatch.Restart();

            foreach(Device device in ctx.Devices.Where(d => d.ModifiedWhen == null))
                device.ModifiedWhen = device.AddedWhen;

            stopwatch.Stop();
            _logger.LogDebug("Took {TotalSeconds:F2} seconds", stopwatch.Elapsed.TotalSeconds);

            _logger.LogInformation("Committing changes...");
            stopwatch.Restart();
            ctx.SaveChanges();
            stopwatch.Stop();
            _logger.LogDebug("Took {TotalSeconds:F2} seconds", stopwatch.Elapsed.TotalSeconds);

            try
            {
                _logger.LogInformation("Retrieving CompactDisc read offsets from AccurateRip...");

                stopwatch.Restart();

                using var client = new WebClient();
                string    html   = client.DownloadString("http://www.accuraterip.com/driveoffsets.htm");
                stopwatch.Stop();
                _logger.LogDebug("Took {TotalSeconds:F2} seconds", stopwatch.Elapsed.TotalSeconds);

                // The HTML is too malformed to process easily, so find start of table
                html = "<html><body><table><tr>" +
                       html[html.IndexOf("<td bgcolor=\"#000000\">", StringComparison.Ordinal)..];

                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                HtmlNode firstTable = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/table[1]");

                var firstRow = true;

                var addedOffsets    = 0;
                var modifiedOffsets = 0;

                _logger.LogInformation("Processing offsets...");
                stopwatch.Restart();

                foreach(HtmlNode[] columns in firstTable.Descendants("tr")
                                                        .Select(row => row.Descendants("td").ToArray()))
                {
                    if(columns.Length != 4)
                    {
                        _logger.LogError("Row does not have correct number of columns...");

                        continue;
                    }

                    string column0 = columns[0].InnerText;
                    string column1 = columns[1].InnerText;
                    string column2 = columns[2].InnerText;
                    string column3 = columns[3].InnerText;

                    if(firstRow)
                    {
                        if(!string.Equals(column0, "cd drive", StringComparison.InvariantCultureIgnoreCase))
                        {
                            _logger.LogError("Unexpected header \"{InnerText}\" found...", columns[0].InnerText);

                            break;
                        }

                        if(!string.Equals(column1, "correction offset", StringComparison.InvariantCultureIgnoreCase))
                        {
                            _logger.LogError("Unexpected header \"{InnerText}\" found...", columns[1].InnerText);

                            break;
                        }

                        if(!string.Equals(column2, "submitted by", StringComparison.InvariantCultureIgnoreCase))
                        {
                            _logger.LogError("Unexpected header \"{InnerText}\" found...", columns[2].InnerText);

                            break;
                        }

                        if(!string.Equals(column3, "percentage agree", StringComparison.InvariantCultureIgnoreCase))
                        {
                            _logger.LogError("Unexpected header \"{InnerText}\" found...", columns[3].InnerText);

                            break;
                        }

                        firstRow = false;

                        continue;
                    }

                    string manufacturer;
                    string model;

                    if(column0[0] == '-' && column0[1] == ' ')
                    {
                        manufacturer = null;
                        model        = column0[2..].Trim();
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
                            manufacturer = column0[..cutOffset].Trim();
                            model        = column0[(cutOffset + 3)..].Trim();
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

                    if(string.Equals(column1, "[purged]", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if(cdOffset != null) ctx.CdOffsets.Remove(cdOffset);

                        continue;
                    }

                    if(!short.TryParse(column1, out short offset)) continue;

                    if(!int.TryParse(column2, out int submissions)) continue;

                    if(column3[^1] != '%') continue;

                    column3 = column3[..^1];

                    if(!float.TryParse(column3, out float percentage)) continue;

                    percentage /= 100;

                    if(cdOffset is null)
                    {
                        cdOffset = new CompactDiscOffset
                        {
                            AddedWhen    = DateTime.UtcNow,
                            ModifiedWhen = DateTime.UtcNow,
                            Agreement    = percentage,
                            Manufacturer = manufacturer,
                            Model        = model,
                            Offset       = offset,
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

                    foreach(Device device in ctx.Devices
                                                .Where(d => d.Manufacturer == null &&
                                                            d.Model        != null &&
                                                            d.Model.Trim() == model)
                                                .Union(ctx.Devices.Where(d => d.Manufacturer        != null         &&
                                                                              d.Manufacturer.Trim() == manufacturer &&
                                                                              d.Model               != null         &&
                                                                              d.Model               == model)))
                    {
                        if(device.CdOffset == cdOffset && device.ModifiedWhen == cdOffset.ModifiedWhen) continue;

                        device.CdOffset     = cdOffset;
                        device.ModifiedWhen = cdOffset.ModifiedWhen;
                    }
                }

                stopwatch.Stop();
                _logger.LogDebug("Took {TotalSeconds:F2} seconds", stopwatch.Elapsed.TotalSeconds);

                if(File.Exists("drive_offsets.json"))
                {
                    using var sr = new StreamReader("drive_offsets.json");

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
                                if(Math.Abs(cdOffset.Agreement - offset.Agreement) > 0 || offset.Agreement < 0)
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

                _logger.LogInformation("Committing changes...");
                stopwatch.Restart();
                ctx.SaveChanges();
                stopwatch.Stop();
                _logger.LogDebug("Took {TotalSeconds:F2} seconds", stopwatch.Elapsed.TotalSeconds);

                _logger.LogInformation("Added {AddedOffsets} offsets",       addedOffsets);
                _logger.LogInformation("Modified {ModifiedOffsets} offsets", modifiedOffsets);
            }
            catch(Exception ex)
            {
#if DEBUG
                if(Debugger.IsAttached) throw;
#endif
                _logger.LogCritical("Exception {Ex} filling CompactDisc read offsets...", ex);
            }

            if(!Directory.Exists("nes")) return;

            _logger.LogInformation("Reading iNES/NES 2.0 headers...");
            stopwatch.Restart();
            var newHeaders     = 0;
            var updatedHeaders = 0;
            counter = 0;

            foreach(string file in Directory.GetFiles("nes"))
            {
                try
                {
                    using var fs = new FileStream(file, FileMode.Open, FileAccess.Read);

                    if(fs.Length <= 16) continue;

                    var header = new byte[16];
                    var data   = new byte[fs.Length - 16];

                    fs.ReadExactly(header, 0, 16);
                    fs.ReadExactly(data,   0, data.Length);

                    bool ines;
                    bool nes20;

                    ines  = header[0] == 'N' && header[1]          == 'E' && header[2] == 'S' && header[3] == 0x1A;
                    nes20 = ines             && (header[7] & 0x0C) == 0x08;

                    if(!ines) continue;

                    counter++;

                    var info = new NesHeaderInfo
                    {
                        NametableMirroring = (header[6] & 0x1) == 0x1,
                        BatteryPresent     = (header[6] & 0x2) == 0x2,
                        FourScreenMode     = (header[6] & 0x8) == 0x8,
                        Mapper             = (ushort)(header[6] >> 4),
                        ConsoleType        = (NesConsoleType)(header[7] & 0x3)
                    };

                    info.Mapper += (ushort)(header[7] & 0xF0);

                    if(nes20)
                    {
                        info.Mapper     += (ushort)((header[8] & 0xF) << 8);
                        info.Submapper  =  (byte)(header[8]           >> 4);
                        info.TimingMode =  (NesTimingMode)(header[12] & 0x3);

                        switch(info.ConsoleType)
                        {
                            case NesConsoleType.Vs:
                                info.VsPpuType      = (NesVsPpuType)(header[13] & 0xF);
                                info.VsHardwareType = (NesVsHardwareType)(header[13] >> 4);

                                break;
                            case NesConsoleType.Extended:
                                info.ExtendedConsoleType = (NesExtendedConsoleType)(header[13] & 0xF);

                                break;
                        }

                        info.DefaultExpansionDevice = (NesDefaultExpansionDevice)header[15];
                    }

                    var    hasher    = SHA256.Create();
                    byte[] hashBytes = hasher.ComputeHash(data);
                    var    hashChars = new char[64];

                    for(var i = 0; i < 32; i++)
                    {
                        int a = hashBytes[i] >> 4;
                        int b = hashBytes[i] & 0xF;

                        hashChars[i * 2] = a > 9 ? (char)(a + 0x57) : (char)(a      + 0x30);
                        hashChars[i * 2                     + 1] = b > 9 ? (char)(b + 0x57) : (char)(b + 0x30);
                    }

                    info.Sha256 = new string(hashChars);

                    NesHeaderInfo existing = ctx.NesHeaders.FirstOrDefault(h => h.Sha256 == info.Sha256);

                    if(existing == null)
                    {
                        info.AddedWhen    = DateTime.UtcNow;
                        info.ModifiedWhen = info.AddedWhen;
                        ctx.NesHeaders.Add(info);
                        newHeaders++;

                        continue;
                    }

                    var modified = false;

                    if(existing.NametableMirroring != info.NametableMirroring)
                    {
                        existing.NametableMirroring = info.NametableMirroring;
                        modified                    = true;
                    }

                    if(existing.BatteryPresent != info.BatteryPresent)
                    {
                        existing.BatteryPresent = info.BatteryPresent;
                        modified                = true;
                    }

                    if(existing.FourScreenMode != info.FourScreenMode)
                    {
                        existing.FourScreenMode = info.FourScreenMode;
                        modified                = true;
                    }

                    if(existing.Mapper != info.Mapper)
                    {
                        existing.Mapper = info.Mapper;
                        modified        = true;
                    }

                    if(existing.ConsoleType != info.ConsoleType)
                    {
                        existing.ConsoleType = info.ConsoleType;
                        modified             = true;
                    }

                    if(existing.Submapper != info.Submapper)
                    {
                        existing.Submapper = info.Submapper;
                        modified           = true;
                    }

                    if(existing.TimingMode != info.TimingMode)
                    {
                        existing.TimingMode = info.TimingMode;
                        modified            = true;
                    }

                    if(existing.VsPpuType != info.VsPpuType)
                    {
                        existing.VsPpuType = info.VsPpuType;
                        modified           = true;
                    }

                    if(existing.VsHardwareType != info.VsHardwareType)
                    {
                        existing.VsHardwareType = info.VsHardwareType;
                        modified                = true;
                    }

                    if(existing.ExtendedConsoleType != info.ExtendedConsoleType)
                    {
                        existing.ExtendedConsoleType = info.ExtendedConsoleType;
                        modified                     = true;
                    }

                    if(existing.DefaultExpansionDevice != info.DefaultExpansionDevice)
                    {
                        existing.DefaultExpansionDevice = info.DefaultExpansionDevice;
                        modified                        = true;
                    }

                    if(!modified) continue;

                    existing.ModifiedWhen = DateTime.UtcNow;
                    updatedHeaders++;
                }
                catch(Exception ex)
                {
#if DEBUG
                    if(Debugger.IsAttached) throw;
#endif
                    _logger.LogCritical("Exception {Ex} with file {File}...", ex, file);
                }
            }

            stopwatch.Stop();
            _logger.LogDebug("Took {TotalSeconds:F2} seconds", stopwatch.Elapsed.TotalSeconds);

            _logger.LogInformation("Processed {Counter} iNES/NES 2.0 headers...",      counter);
            _logger.LogInformation("Added {NewHeaders} iNES/NES 2.0 headers...",       newHeaders);
            _logger.LogInformation("Updated {UpdatedHeaders} iNES/NES 2.0 headers...", updatedHeaders);

            _logger.LogInformation("Committing changes...");
            stopwatch.Restart();
            ctx.SaveChanges();
            stopwatch.Stop();
            _logger.LogDebug("Took {TotalSeconds:F2} seconds", stopwatch.Elapsed.TotalSeconds);
        }
        finally
        {
            Interlocked.Exchange(ref _running, 0);
        }
    }
}