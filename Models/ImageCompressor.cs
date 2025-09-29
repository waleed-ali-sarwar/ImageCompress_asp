using ImageMagick;
using System;
using System.IO;


namespace ImageCompress_asp.Models
{
    public static class ImageCompressor
    {
        private const int DefaultQuality = 75;
        private const int PngCompressionLevel = 6;
        private const int GifPaletteColors = 128;

        public static void CompressAndResize(Stream input, Stream output, int maxWidth = 0, int maxHeight = 0)
        {
            using (var image = new MagickImage(input))
            {
                // Resize if requested
                if (maxWidth > 0 || maxHeight > 0)
                {
                    var geo = new MagickGeometry(
                        (uint)Math.Max(0, maxWidth),
                        (uint)Math.Max(0, maxHeight))
                    {
                        IgnoreAspectRatio = false,
                        Greater = true // only shrink if larger
                    };
                    image.Resize(geo);
                }

                // Strip metadata
                image.Strip();

                switch (image.Format)
                {
                    case MagickFormat.Jpeg:
                    case MagickFormat.Jpg:
                        image.Quality = DefaultQuality;
                        image.Write(output, MagickFormat.Jpeg);
                        break;

                    case MagickFormat.Png:
                        image.Settings.SetDefine("png:compression-level", PngCompressionLevel.ToString());
                        image.Write(output, MagickFormat.Png);
                        break;

                    case MagickFormat.Gif:
                        image.ColorFuzz = new Percentage(2);
                        image.Quantize(new QuantizeSettings
                        {
                            Colors = GifPaletteColors,
                            DitherMethod = DitherMethod.FloydSteinberg
                        });
                        image.Write(output, MagickFormat.Gif);
                        break;

                    case MagickFormat.WebP:
                        image.Quality = DefaultQuality;
                        image.Write(output, MagickFormat.WebP);
                        break;

                    case MagickFormat.Heic:
                    case MagickFormat.Heif:
                        try
                        {
                            image.Quality = DefaultQuality;
                            image.Write(output, MagickFormat.Heic);
                        }
                        catch
                        {
                            // fallback to JPEG
                            image.Format = MagickFormat.Jpeg;
                            image.Quality = DefaultQuality;
                            image.Write(output, MagickFormat.Jpeg);
                        }
                        break;

                    default:
                        // fallback: JPEG
                        image.Format = MagickFormat.Jpeg;
                        image.Quality = DefaultQuality;
                        image.Write(output, MagickFormat.Jpeg);
                        break;
                }
            }
        }
    }
}