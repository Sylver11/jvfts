using System;
using System.IO;

namespace jvfts
{
    public class FileCopy
    {
        public FileCopy(string path)
        {          
            string fileName = Path.GetFileName(path);
            string sourcePath = @path;
            string targetPath = System.IO.Directory.GetCurrentDirectory() + "/Processing";
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            System.IO.Directory.CreateDirectory(targetPath);

            System.IO.File.Copy(sourcePath, destFile, true);
        }
    }
}
