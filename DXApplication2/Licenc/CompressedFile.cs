using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace SaveDXF
{
    class CompressedFile
    {

        public void AddDeCompressedFile(string FileName, bool DeletTempFile)
        {

            // создание НЕсжатого файла
            string TargetFileTmp = Path.Combine(Path.GetDirectoryName(FileName), "_" + Path.GetFileName(FileName));
            bool FileCompressed = false;
            Decompress(FileName, TargetFileTmp, out FileCompressed); 
            if (File.Exists(FileName) && DeletTempFile && FileCompressed == true)
                File.Delete(FileName);
            if (FileCompressed) File.Move(TargetFileTmp, FileName);
        }
        public void AddCompressedFile(string FileName, bool DeletTempFile)
        {
            // создание сжатого файла
            string FileNameTmp =Path.Combine(Path.GetDirectoryName(FileName), "_" + Path.GetFileName(FileName));
            Compress(FileName, FileNameTmp);
            if (File.Exists(FileName) && DeletTempFile)
                File.Delete(FileName);
            File.Move(FileNameTmp, FileName);
        }
        //-----------------------------------------------------------------------------

        public List<string> Decompress_to_(string compressedFile)
        {
            List<string> lst = new List<string>();

            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            {
                using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                {
                    StreamReader reader = new StreamReader(decompressionStream, Encoding.Default);

                    for (int i = 0; reader.EndOfStream == false; i++)
                    {
                        lst.Add(reader.ReadLine());

                    }
                    reader.Close();
                }
            } 
            return lst;
        }



        //-----------------------------------------------------------------------------
        public void Decompress(string compressedFile, string targetFile, out bool FileCompressed)
        {
            // поток для чтения из сжатого файла
            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            {
                // поток для записи восстановленного файла
                using (FileStream targetStream = File.Create(targetFile))
                {
                    // поток разархивации 
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        try
                        {
                            decompressionStream.CopyTo(targetStream);
                            FileCompressed = true;
                        }
                        catch (Exception Ex)
                        {
                            targetStream.Close();
                            File.Delete(targetFile);
                            FileCompressed = false;
                        }
                    } 
                }
            }
        }


        //-----------------------------------------------------------------------------

        public void Compress(string sourceFile, string compressedFile)
        {
            // поток для чтения исходного файла
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
            {
                // поток для записи сжатого файла
                using (FileStream targetStream = File.Create(compressedFile))
                {
                    // поток архивации
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой 
                    }
                }
            }
        }

    }
}
