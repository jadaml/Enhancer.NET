/* Copyright (c) 2018, Ádám L. Juhász
 *
 * This file is part of Enhancer.NativeBridge.
 *
 * Enhancer.NativeBridge is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Enhancer.NativeBridge is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Enhancer.NativeBridge.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Enhancer
{
    public static class SystemResources
    {
        private enum DialogResultCaption : uint
        {
            Ok       = 800,
            Cancel   = 801,
            Abort    = 802,
            Retry    = 803,
            Ignore   = 804,
            Yes      = 805,
            No       = 806,
            Close    = 807,
            Help     = 808,
            TryAgain = 809,
            Continue = 810,
        }

        public static string Ok => LoadDialogResultCaption(DialogResultCaption.Ok);

        public static string Cancel => LoadDialogResultCaption(DialogResultCaption.Cancel);

        public static string Abort => LoadDialogResultCaption(DialogResultCaption.Abort);

        public static string Retry => LoadDialogResultCaption(DialogResultCaption.Retry);

        public static string Ignore => LoadDialogResultCaption(DialogResultCaption.Ignore);

        public static string Yes => LoadDialogResultCaption(DialogResultCaption.Yes);

        public static string No => LoadDialogResultCaption(DialogResultCaption.No);

        public static string Close => LoadDialogResultCaption(DialogResultCaption.Close);

        public static string Help => LoadDialogResultCaption(DialogResultCaption.Help);

        public static string TryAgain => LoadDialogResultCaption(DialogResultCaption.TryAgain);

        public static string Continue => LoadDialogResultCaption(DialogResultCaption.Continue);

        private static readonly IntPtr user32lib = LoadLibrary(Environment.SystemDirectory + "\\User32.dll");

        /// <summary>
        /// Loads a string resource from the executable file associated with a specified module,
        /// copies the string into a buffer, and appends a terminating null character.
        /// </summary>
        /// <param name="hInstance">A handle to an instance of the module whose executable file
        /// contains the string resource. To get the handle to the application itself, call the
        /// <see cref="GetModuleHandle"/> function with <c>NULL</c>.</param>
        /// <param name="uID">The identifier of the string to be loaded.</param>
        /// <param name="lpBuffer">The buffer is to receive the string.</param>
        /// <param name="nBufferMax">The size of the buffer, in characters. The string is truncated
        /// and null-terminated if it is longer than the number of characters specified. If this
        /// parameter is 0, then <i>lpBuffer</i> receives a read-only pointer to the resource itself.</param>
        /// <returns>If the function succeeds, the return value is the number of characters
        /// copied into the buffer, not including the terminating null character, or zero if the
        /// string resource does not exist. To get extended error information, call <see cref="GetLastError"/>.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int LoadString(IntPtr hInstance, uint uID, StringBuilder lpBuffer, int nBufferMax);
        /// <summary>
        /// Loads the specified module into the address space of the calling process. The specified
        /// module may cause other modules to be loaded.<br/>
        /// For additional load options, use the <see cref="LoadLibraryEx"/> function.
        /// </summary>
        /// <param name="lpFileName">The name of the module. This can be either a library module
        /// (a .dll file) or an executable module (an .exe file). The name specified is the file
        /// name of the module and is not related to the name stored in the library module itself,
        /// as specified by the LIBRARY keyword in the module-definition (.def) file.<br/>
        /// If the string specifies a full path, the function searches only that path for the module.<br/>
        /// If the string specifies a relative path or a module name without a path, the function
        /// uses a standard search strategy to find the module; for more information, see the Remarks.<br/>
        /// If the function cannot find the module, the function fails. When specifying a path, be
        /// sure to use backslashes (\), not forward slashes (/). For more information about
        /// paths, see Naming a File or Directory.<br/>
        /// If the string specifies a module name without a path and the file name extension is
        /// omitted, the function appends the default library extension .dll to the module name.
        /// To prevent the function from appending .dll to the module name, include a trailing
        /// point character (.) in the module name string.</param>
        /// <returns>If the function succeeds, the return value is a handle to the module.<br/>
        /// If the function fails, the return value is NULL. To get extended error information,
        /// call <see cref="GetLastError"/>.</returns>
        [DllImport("kernel32")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        /// <summary>
        /// Loads a resource string from user32.dll
        /// </summary>
        /// <param name="caption">The resource to load.</param>
        /// <returns>The string stored by that resource.</returns>
        private static string LoadDialogResultCaption(DialogResultCaption caption)
        {
            StringBuilder stringb = new StringBuilder(256);
            LoadString(user32lib, (uint)caption, stringb, stringb.Capacity);
            return stringb.ToString();
        }
    }
}
