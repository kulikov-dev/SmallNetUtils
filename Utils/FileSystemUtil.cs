using System.Diagnostics;
using System.Text;

namespace SmallNetUtils.Utils
{
    /// <summary>
    /// Utils to work with files/explorer
    /// </summary>
    public static class FileSystemUtil
    {
        /// <summary>
        /// The Explorer launch title
        /// </summary>
        private const string ExplorerLaunchTitle = "explorer.exe";

        /// <summary>
        /// Launch file in an assigned application or show it in the Explorer
        /// </summary>
        /// <param name="path"> File path </param>
        internal static void LaunchFile(string path)
        {
            try
            {
                Process.Start(path);
            }
            catch
            {
                //// There is no assigned application for this file
                ShowFileInExplorer(path);
            }
        }

        /// <summary>
        /// Show a file in the Explorer
        /// </summary>
        /// <param name="filePath"> File path </param>
        internal static void ShowFileInExplorer(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            var argument = "/select, \"" + filePath + "\"";

            Process.Start(ExplorerLaunchTitle, argument);
        }

        /// <summary>
        /// Open a folder in the Explorer
        /// </summary>
        /// <param name="folderPath"> Folder path </param>
        /// <param name="createNotExisted"> Flag if need to create a folder if not exists </param>
        public static void OpenFolder(string folderPath, bool createNotExisted = false)
        {
            if (!Directory.Exists(folderPath))
            {
                if (createNotExisted)
                {
                    Directory.CreateDirectory(folderPath);
                }
                else
                {
                    return;
                }
            }

            var processStartInfo = new ProcessStartInfo
            {
                FileName = ExplorerLaunchTitle,
                Arguments = folderPath
            };

            Process.Start(processStartInfo);
        }

        /// <summary>
        /// Validate filename and correct if necessary
        /// </summary>
        /// <param name="fileName"> Filename </param>
        /// <returns> Valid filename </returns>
        public static string ValidateFileName(string fileName)
        {
            var result = fileName;

            foreach (var notValidChar in Path.GetInvalidFileNameChars())
            {
                result = result.Replace(notValidChar.ToString(), "-");
            }

            return result;
        }

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filePath">The text file to analyze </param>
        /// <returns>The detected encoding </returns>
        /// <remarks> Source: https://stackoverflow.com/questions/3825390/effective-way-to-find-any-files-encoding </remarks>
        public static Encoding GetFileEncoding(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("Input file not found");
            }

            // Read the BOM
            var bom = new byte[4];

            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76)
            {
                return Encoding.UTF7;
            }

            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
            {
                return Encoding.UTF8;
            }

            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0)
            {
                // UTF-32LE
                return Encoding.UTF32;
            }

            if (bom[0] == 0xff && bom[1] == 0xfe)
            {
                // UTF-16LE
                return Encoding.Unicode;
            }

            if (bom[0] == 0xfe && bom[1] == 0xff)
            {
                // UTF-16BE
                return Encoding.BigEndianUnicode;
            }

            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)
            {
                // UTF-32BE
                return new UTF32Encoding(true, true);
            }

            // We actually have no idea what the encoding is if we reach this point, so
            // you may wish to return null instead of defaulting to ASCII
            return Encoding.Default;
        }
    }
}