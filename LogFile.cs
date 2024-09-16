using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaultProjectUploader
{
    public class LogFile
    {
        string LOG_PATH = @"S:\Development\Box.com Logs\";
        string DATE = DateTime.Now.ToString("MM-dd-yyyy");
        string TIME = DateTime.Now.ToString("hh:mm tt");

        public void CreateLogFile()
        {
            // Code will be written so that if file already exists, a boolean value will be set for StreamWriter
            // The border text will always display going forward.
            string outputFile = $"Box Automation - {DATE}.txt";
            string[] files = Directory.GetFiles(LOG_PATH, outputFile, SearchOption.TopDirectoryOnly);
            
            bool append = files.Length > 0;

            string[] lines =
                {
                "BOX AUTOMATION RAN",
                $"AT {TIME} ON {DATE}",
                "LOGGED UPLOAD STATUS"
            };

            int width = 60; // Total width of the line including '|'
            string border = new string('-', width);

            using (StreamWriter writer = new StreamWriter(LOG_PATH + outputFile, append))
            {
                writer.WriteLine(border);
                foreach (string line in lines)
                {
                    string centeredLine = CenterText(line, width - 2); // Subtract 2 for the '|' characters
                    writer.WriteLine($"|{centeredLine}|");
                }
                writer.WriteLine($"{border}");
                // Separate Function Call
                writer.WriteLine();
                writer.WriteLine("Successful Uploads:");
                foreach (string file in VaultFileCheckin.UploadStatus.SuccessUploads)
                {
                    writer.WriteLine(file);
                }
                writer.WriteLine();
                writer.WriteLine("Failed Uploads:");
                foreach (string file in VaultFileCheckin.UploadStatus.FailedUploads)
                {
                    writer.WriteLine(file);
                }
                writer.Flush();
            }
        }

        static string CenterText(string text, int width)
        {
            if (text.Length >= width)
                return text;

            int leftPadding = (width - text.Length) / 2;
            int rightPadding = width - text.Length - leftPadding;

            return new string(' ', leftPadding) + text + new string(' ', rightPadding);
        }
    }
}
