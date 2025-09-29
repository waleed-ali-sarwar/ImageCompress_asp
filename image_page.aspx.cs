using ImageCompress_asp.Models;
using System;
using System.IO;
using System.IO.Compression;
using System.Web.UI;
namespace ImageCompress_asp
{
    public partial class image_page : Page
    {
        protected void BtnUpload_Click(object sender, EventArgs e)
        {
            if (!FileUpload1.HasFiles)
            {
                LitResult.Text = "⚠️ No files selected.";
                return;
            }

            int.TryParse(TxtMaxWidth.Text, out int maxW);
            int.TryParse(TxtMaxHeight.Text, out int maxH);

            if (FileUpload1.PostedFiles.Count == 1)
            {
                var file = FileUpload1.PostedFiles[0];
                if (file.ContentLength <= 0) return;

                string ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
                string safeName = Path.GetFileNameWithoutExtension(file.FileName) + "_processed" + ext;

                using (var memStream = new MemoryStream())
                {
                    ImageCompressor.CompressAndResize(file.InputStream, memStream, maxW, maxH);
                    SendToBrowser(memStream.ToArray(), safeName);
                }
            }
            else
            {
                using (var zipStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var file in FileUpload1.PostedFiles)
                        {
                            if (file.ContentLength <= 0) continue;

                            string ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
                            string safeName = Path.GetFileNameWithoutExtension(file.FileName) + "_processed" + ext;

                            using (var memStream = new MemoryStream())
                            {
                                ImageCompressor.CompressAndResize(file.InputStream, memStream, maxW, maxH);

                                var entry = archive.CreateEntry(safeName, CompressionLevel.Optimal);
                                using (var entryStream = entry.Open())
                                {
                                    memStream.Seek(0, SeekOrigin.Begin);
                                    memStream.CopyTo(entryStream);
                                }
                            }
                        }
                    }

                    SendToBrowser(zipStream.ToArray(), "processed_images.zip");
                }
            }
        }

        private void SendToBrowser(byte[] data, string fileName)
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
            Response.BinaryWrite(data);
            Response.End();
        }
    }
}
