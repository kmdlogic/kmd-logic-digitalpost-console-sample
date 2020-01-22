using System;
using System.IO;

namespace Kmd.Logic.Digitalpost.CallbackSample.Controllers
{
    public class FileSaver
    {
        private readonly string _directory;

        public FileSaver(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            this._directory = directory;
        }

        public void SaveFile(string filename, string contentInBase64)
        {
            this.SaveFile(filename, Convert.FromBase64String(contentInBase64));
        }

        public void SaveFile(string filename, byte[] bytes)
        {
            var filePath = Path.Combine(this._directory, filename);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            File.WriteAllBytes(filePath, bytes);
        }
    }
}
