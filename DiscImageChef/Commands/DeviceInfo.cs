﻿// /***************************************************************************
// The Disc Image Chef
// ----------------------------------------------------------------------------
//
// Filename       : DeviceInfo.cs
// Version        : 1.0
// Author(s)      : Natalia Portillo
//
// Component      : Component
//
// Revision       : $Revision$
// Last change by : $Author$
// Date           : $Date$
//
// --[ Description ] ----------------------------------------------------------
//
// Description
//
// --[ License ] --------------------------------------------------------------
//
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as
//     published by the Free Software Foundation, either version 3 of the
//     License, or (at your option) any later version.
//
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
//
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
// ----------------------------------------------------------------------------
// Copyright (C) 2011-2015 Claunia.com
// ****************************************************************************/
// //$Id$
using System;
using DiscImageChef.Devices;
using System.IO;
using DiscImageChef.Console;
using System.Text;

namespace DiscImageChef.Commands
{
    public static class DeviceInfo
    {
        public static void doDeviceInfo(DeviceInfoSubOptions options)
        {
            DicConsole.DebugWriteLine("Device-Info command", "--debug={0}", options.Debug);
            DicConsole.DebugWriteLine("Device-Info command", "--verbose={0}", options.Verbose);
            DicConsole.DebugWriteLine("Device-Info command", "--device={0}", options.DevicePath);

            if (options.DevicePath.Length == 2 && options.DevicePath[1] == ':' &&
                options.DevicePath[0] != '/' && Char.IsLetter(options.DevicePath[0]))
            {
                options.DevicePath = "\\\\.\\" + Char.ToUpper(options.DevicePath[0]) + ':';
            }

            Device dev = new Device(options.DevicePath);

            if (dev.Error)
            {
                DicConsole.ErrorWriteLine("Error {0} opening device.", dev.LastError);
                return;
            }

            switch (dev.Type)
            {
                case DeviceType.ATA:
                    {
                        AtaErrorRegistersCHS errorRegisters;

                        byte[] ataBuf;
                        bool sense = dev.AtaIdentify(out ataBuf, out errorRegisters);

                        if (sense)
                        {
                            DicConsole.DebugWriteLine("Device-Info command", "STATUS = 0x{0:X2}", errorRegisters.status);
                            DicConsole.DebugWriteLine("Device-Info command", "ERROR = 0x{0:X2}", errorRegisters.error);
                            DicConsole.DebugWriteLine("Device-Info command", "NSECTOR = 0x{0:X2}", errorRegisters.sectorCount);
                            DicConsole.DebugWriteLine("Device-Info command", "SECTOR = 0x{0:X2}", errorRegisters.sector);
                            DicConsole.DebugWriteLine("Device-Info command", "CYLHIGH = 0x{0:X2}", errorRegisters.cylinderHigh);
                            DicConsole.DebugWriteLine("Device-Info command", "CYLLOW = 0x{0:X2}", errorRegisters.cylinderLow);
                            DicConsole.DebugWriteLine("Device-Info command", "DEVICE = 0x{0:X2}", errorRegisters.deviceHead);
                            DicConsole.DebugWriteLine("Device-Info command", "COMMAND = 0x{0:X2}", errorRegisters.command);
                            DicConsole.DebugWriteLine("Device-Info command", "Error code = {0}", dev.LastError);
                            break;
                        }

                        DicConsole.WriteLine(Decoders.ATA.Identify.Prettify(ataBuf));
                        break;
                    }
                case DeviceType.ATAPI:
                    {
                        AtaErrorRegistersCHS errorRegisters;

                        byte[] ataBuf;
                        bool sense = dev.AtapiIdentify(out ataBuf, out errorRegisters);

                        if (sense)
                        {
                            DicConsole.DebugWriteLine("Device-Info command", "STATUS = 0x{0:X2}", errorRegisters.status);
                            DicConsole.DebugWriteLine("Device-Info command", "ERROR = 0x{0:X2}", errorRegisters.error);
                            DicConsole.DebugWriteLine("Device-Info command", "NSECTOR = 0x{0:X2}", errorRegisters.sectorCount);
                            DicConsole.DebugWriteLine("Device-Info command", "SECTOR = 0x{0:X2}", errorRegisters.sector);
                            DicConsole.DebugWriteLine("Device-Info command", "CYLHIGH = 0x{0:X2}", errorRegisters.cylinderHigh);
                            DicConsole.DebugWriteLine("Device-Info command", "CYLLOW = 0x{0:X2}", errorRegisters.cylinderLow);
                            DicConsole.DebugWriteLine("Device-Info command", "DEVICE = 0x{0:X2}", errorRegisters.deviceHead);
                            DicConsole.DebugWriteLine("Device-Info command", "COMMAND = 0x{0:X2}", errorRegisters.command);
                            DicConsole.DebugWriteLine("Device-Info command", "Error code = {0}", dev.LastError);
                            break;
                        }

                        DicConsole.WriteLine(Decoders.ATA.Identify.Prettify(ataBuf));

                        // ATAPI devices are also SCSI devices
                        goto case DeviceType.SCSI;
                    }
                case DeviceType.SCSI:
                    {
                        byte[] senseBuf;
                        byte[] inqBuf;

                        bool sense = dev.ScsiInquiry(out inqBuf, out senseBuf);

                        if (sense)
                        {
                            DicConsole.ErrorWriteLine("SCSI error. Sense decoding not yet implemented.");

                            #if DEBUG
                            FileStream senseFs = File.Open("sense.bin", FileMode.OpenOrCreate);
                            senseFs.Write(senseBuf, 0, senseBuf.Length);
                            #endif

                            break;
                        }

                        if (dev.Type != DeviceType.ATAPI)
                            DicConsole.WriteLine("SCSI device");

                        DicConsole.WriteLine(Decoders.SCSI.Inquiry.Prettify(inqBuf));

                        bool scsi83 = false;
                        string scsiSerial = null;
                        StringBuilder sb = null;

                        sense = dev.ScsiInquiry(out inqBuf, out senseBuf, 0x00);

                        if (!sense)
                        {
                            byte[] pages = Decoders.SCSI.EVPD.DecodePage00(inqBuf);

                            foreach (byte page in pages)
                            {
                                if (page >= 0x01 && page <= 0x7F)
                                {
                                    sense = dev.ScsiInquiry(out inqBuf, out senseBuf, page);
                                    if (!sense)
                                    {
                                        if (sb == null)
                                            sb = new StringBuilder();
                                        sb.AppendFormat("Page 0x{0:X2}: ", Decoders.SCSI.EVPD.DecodeASCIIPage(inqBuf)).AppendLine();
                                    }
                                }
                                else if (page == 0x80)
                                {
                                    sense = dev.ScsiInquiry(out inqBuf, out senseBuf, page);
                                    if (!sense)
                                    {
                                        scsi83 = true;
                                        scsiSerial = Decoders.SCSI.EVPD.DecodePage80(inqBuf);
                                    }
                                }
                                else
                                {
                                    if (page != 0x00)
                                        DicConsole.DebugWriteLine("Device-Info command", "Found undecoded SCSI VPD page 0x{0:X2}", page);
                                }
                            }
                        }

                        if (scsi83)
                            DicConsole.WriteLine("Unit Serial Number: {0}", scsiSerial);

                        if (sb != null)
                        {
                            DicConsole.WriteLine("ASCII VPDs:");
                            DicConsole.WriteLine(sb.ToString());
                        }

                        break;
                    }
                default:
                    DicConsole.ErrorWriteLine("Unknown device type {0}, cannot get information.", dev.Type);
                    break;
            }
        }
    }
}

