using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AmnesiaManager.Models
{
    // TODO: Implement getting the saved mode from the settings
    internal enum SecurityMode
    {
        /// <summary>
        /// Entering a password is required only after the program is started
        /// The list of passwords and the master password are stored in RAM
        /// </summary>
        Usual = 0,

        /// <summary>
        /// After each collapse of the window, you need to enter the master password
        /// Each time the window is minimized, the list of passwords and the master password are deleted from RAM
        /// </summary>
        Elevated = 1,
    }

    internal static class Product
    {
        public static string Name { get; set; } = AppDomain.CurrentDomain.FriendlyName;
        
        public static SecurityMode Mode = SecurityMode.Usual;

        public static string? GetVersion()
        {
            if (typeof(App).GetTypeInfo().Assembly.GetName().Version is not { } version)
                return null;

            var formattedVersion = Regex.Replace(
                version.ToString(),
                @".\d$",
                ""
            );

            return formattedVersion;
        }
    }
}
