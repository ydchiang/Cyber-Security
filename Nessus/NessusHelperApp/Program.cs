﻿using System;
using System.Collections.Generic;
using NessusHelperLib;
using NessusHelperLib.Common;
using NessusHelperLib.Model;

namespace NessusHelperApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Login Local Nessus Server
            var nessusHelper = new NessusHelper();

            // Login Linux Nessus Server
            //var nessusHelper = new NessusHelper("192.168.100.10",8834);
            try
            {
                var result = nessusHelper.Login("12345", "54321");

                if (!result.Success)
                {
                    Console.WriteLine(result.ErrorMessage);
                    return;
                }

                // Get all folders
                var folders = nessusHelper.GetFoldersList();
                foreach (var folder in folders)
                {
                    Console.WriteLine($"ID={folder.Id}, Name={folder.Name}, Type={folder.FolderType}.");
                }

                Console.WriteLine();

                // Get all scans
                var scans = nessusHelper.GetScansList();
                foreach (var scan in scans)
                {
                    Console.WriteLine($"ID={scan.Id}, Name={scan.Name}, Type={scan.ScanType}, Owner={scan.Owner}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine("Press any key to logout...");
                Console.ReadKey();
                nessusHelper.Logout();
            }

        }
    }
}
