﻿// /***************************************************************************
// The Disc Image Chef
// ----------------------------------------------------------------------------
//
// Filename       : Extern.cs
// Version        : 1.0
// Author(s)      : Natalia Portillo
//
// Component      : Linux direct device access
//
// Revision       : $Revision$
// Last change by : $Author$
// Date           : $Date$
//
// --[ Description ] ----------------------------------------------------------
//
// Contains the P/Invoke definitions of Linux syscalls used to directly interface devices
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

using System.Runtime.InteropServices;

namespace DiscImageChef.Devices.Linux
{
    static class Extern
    {
        [DllImport("libc", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern int open(
            string pathname,
            [MarshalAs(UnmanagedType.U4)]
            FileFlags flags);

        [DllImport("libc")]
        internal static extern int close(int fd);

        [DllImport("libc", EntryPoint="ioctl", SetLastError = true)]
        internal static extern int ioctlInt(int fd, LinuxIoctl request, out int value);

        [DllImport("libc", EntryPoint="ioctl", SetLastError = true)]
        internal static extern int ioctlSg(int fd, LinuxIoctl request, ref sg_io_hdr_t value);

        [DllImport("libc", EntryPoint="ioctl", SetLastError = true)]
        internal static extern int ioctlHdTaskfile(int fd, LinuxIoctl request, ref hd_drive_task_hdr value);

        [DllImport("libc", EntryPoint="ioctl", SetLastError = true)]
        internal static extern int ioctlHdTask(int fd, LinuxIoctl request, ref byte[] value);
    }
}
