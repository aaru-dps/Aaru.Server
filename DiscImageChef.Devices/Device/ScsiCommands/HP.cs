﻿// /***************************************************************************
// The Disc Image Chef
// ----------------------------------------------------------------------------
//
// Filename       : HP.cs
// Version        : 1.0
// Author(s)      : Natalia Portillo
//
// Component      : Hewlett-Packard vendor commands
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
using DiscImageChef.Console;

namespace DiscImageChef.Devices
{
    public partial class Device
    {
        /// <summary>
        /// Sends the HP READ LONG vendor command
        /// </summary>
        /// <returns><c>true</c> if the command failed and <paramref name="senseBuffer"/> contains the sense buffer.</returns>
        /// <param name="buffer">Buffer where the SCSI READ LONG response will be stored</param>
        /// <param name="senseBuffer">Sense buffer.</param>
        /// <param name="relAddr">If set to <c>true</c> address contain two's complement offset from last read address.</param>
        /// <param name="address">PBA/LBA to read.</param>
        /// <param name="blockBytes">How many bytes per block.</param>
        /// <param name="pba">If set to <c>true</c> address contain physical block address.</param>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <param name="duration">Duration in milliseconds it took for the device to execute the command.</param>
        public bool HPReadLong(out byte[] buffer, out byte[] senseBuffer, bool relAddr, uint address, ushort blockBytes, bool pba, uint timeout, out double duration)
        {
            return HPReadLong(out buffer, out senseBuffer, relAddr, address, 0, blockBytes, pba, false, timeout, out duration);
        }

        /// <summary>
        /// Sends the HP READ LONG vendor command
        /// </summary>
        /// <returns><c>true</c> if the command failed and <paramref name="senseBuffer"/> contains the sense buffer.</returns>
        /// <param name="buffer">Buffer where the SCSI READ LONG response will be stored</param>
        /// <param name="senseBuffer">Sense buffer.</param>
        /// <param name="relAddr">If set to <c>true</c> address contain two's complement offset from last read address.</param>
        /// <param name="address">PBA/LBA to read.</param>
        /// <param name="transferLen">How many blocks/bytes to read.</param>
        /// <param name="blockBytes">How many bytes per block.</param>
        /// <param name="pba">If set to <c>true</c> address contain physical block address.</param>
        /// <param name="sectorCount">If set to <c>true</c> <paramref name="transferLen"/> is a count of secors to read. Otherwise it will be ignored</param>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <param name="duration">Duration in milliseconds it took for the device to execute the command.</param>
        public bool HPReadLong(out byte[] buffer, out byte[] senseBuffer, bool relAddr, uint address, ushort transferLen, ushort blockBytes, bool pba, bool sectorCount, uint timeout, out double duration)
        {
            senseBuffer = new byte[32];
            byte[] cdb = new byte[10];
            bool sense;

            cdb[0] = (byte)ScsiCommands.ReadLong;
            if(relAddr)
                cdb[1] += 0x01;
            cdb[2] = (byte)((address & 0xFF000000) >> 24);
            cdb[3] = (byte)((address & 0xFF0000) >> 16);
            cdb[4] = (byte)((address & 0xFF00) >> 8);
            cdb[5] = (byte)(address & 0xFF);
            cdb[7] = (byte)((transferLen & 0xFF00) >> 8);
            cdb[8] = (byte)(transferLen & 0xFF);
            if (pba)
                cdb[9] += 0x80;
            if (sectorCount)
                cdb[9] += 0x40;

            if(sectorCount)
                buffer = new byte[blockBytes * transferLen];
            else
                buffer = new byte[transferLen];

            lastError = SendScsiCommand(cdb, ref buffer, out senseBuffer, timeout, ScsiDirection.In, out duration, out sense);
            error = lastError != 0;

            DicConsole.DebugWriteLine("SCSI Device", "HP READ LONG took {0} ms.", duration);

            return sense;
        }
    }
}

