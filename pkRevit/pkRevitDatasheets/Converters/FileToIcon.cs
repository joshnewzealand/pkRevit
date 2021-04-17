using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows;
using System.Runtime.InteropServices;
using vbAccelerator.Components.ImageList;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using Microsoft.WindowsAPICodePack.Shell;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace QuickZip.Tools
{

    public static class helpers
    {
        private static System.Windows.Media.PixelFormat ConvertPixelFormat(System.Drawing.Imaging.PixelFormat sourceFormat)
        {
            switch (sourceFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    return PixelFormats.Bgr24;

                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    return PixelFormats.Bgra32;

                case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                    return PixelFormats.Bgr32;
            }

            return new System.Windows.Media.PixelFormat();
        }

        public static BitmapSource GetBitmapSource(this Bitmap image)
        {
            var rect = new Rectangle(0, 0, image.Width, image.Height);
            var bitmap_data = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);

            try
            {
                BitmapPalette palette = null;

                if (image.Palette.Entries.Length > 0)
                {
                    var palette_colors = image.Palette.Entries.Select(entry => System.Windows.Media.Color.FromArgb(entry.A, entry.R, entry.G, entry.B)).ToList();
                    palette = new BitmapPalette(palette_colors);
                }

                return BitmapSource.Create(
                    image.Width,
                    image.Height,
                    image.HorizontalResolution,
                    image.VerticalResolution,
                    ConvertPixelFormat(image.PixelFormat),
                    palette,
                    bitmap_data.Scan0,
                    bitmap_data.Stride * image.Height,
                    bitmap_data.Stride
                );
            }
            finally
            {
                image.UnlockBits(bitmap_data);
            }
        }
    }


    [ValueConversion(typeof(string), typeof(ImageSource))]
    public class FileToIconConverter : IMultiValueConverter
    {
        //private static string imageFilter = ".jpg,.jpeg,.png,.gif";
        private static string imageFilter = ".jpg,.jpeg,.gif";
        //private static string exeFilter = ".exe,.lnk";
        private static string exeFilter = ".exe";
        private int defaultsize;

        public int DefaultSize { get { return defaultsize; } set { defaultsize = value; } }

        public enum IconSize
        {
            small, large, extraLarge, jumbo, thumbnail
        }

        private class thumbnailInfo
        {
            public IconSize iconsize;
            public WriteableBitmap bitmap;
            public string fullPath;
            public thumbnailInfo(WriteableBitmap b, string path, IconSize size)
            {
                bitmap = b;
                fullPath = path;
                iconsize = size;
                //iconsize = IconSize.extraLarge;
            }
        }


        #region Win32api
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [StructLayout(LayoutKind.Sequential)]
        internal struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        internal const uint SHGFI_ICON = 0x100;
        internal const uint SHGFI_TYPENAME = 0x400;
        internal const uint SHGFI_LARGEICON = 0x0; // 'Large icon
        internal const uint SHGFI_SMALLICON = 0x1; // 'Small icon
        internal const uint SHGFI_SYSICONINDEX = 16384;
        internal const uint SHGFI_USEFILEATTRIBUTES = 16;

        // <summary>
        /// Get Icons that are associated with files.
        /// To use it, use (System.Drawing.Icon myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon));
        /// hImgSmall = SHGetFileInfo(fName, 0, ref shinfo,(uint)Marshal.SizeOf(shinfo),Win32.SHGFI_ICON |Win32.SHGFI_SMALLICON);
        /// </summary>
        [DllImport("shell32.dll")]
        internal static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes,
                                                  ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        // <summary>
        /// Return large file icon of the specified file.
        /// </summary>
        internal static Icon GetFileIcon(string fileName, IconSize size)
        {
            SHFILEINFO shinfo = new SHFILEINFO();

            uint flags = SHGFI_SYSICONINDEX;
            if (fileName.IndexOf(":") == -1)
                flags = flags | SHGFI_USEFILEATTRIBUTES;
            if (size == IconSize.small)
                flags = flags | SHGFI_ICON | SHGFI_SMALLICON;
            else flags = flags | SHGFI_ICON;

            SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);
            return Icon.FromHandle(shinfo.hIcon);
        }
        #endregion

        #region Static Tools

        private static void copyBitmap(BitmapSource source, WriteableBitmap target, bool dispatcher)
        {
            int width = source.PixelWidth;
            int height = source.PixelHeight;
            int stride = width * ((source.Format.BitsPerPixel + 7) / 8);

            byte[] bits = new byte[height * stride];
            source.CopyPixels(bits, stride, 0);
            source = null;


            if (dispatcher)
            {
                target.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new ThreadStart(delegate
                {
                    //UI Thread
                    var delta = target.Height - height;
                    var newWidth = width > target.Width ? (int)target.Width : width;
                    var newHeight = height > target.Height ? (int)target.Height : height;
                    Int32Rect outRect = new Int32Rect(0, (int)(delta >= 0 ? delta : 0) / 2, newWidth, newWidth);
                    try
                    {
                        target.WritePixels(outRect, bits, stride, 0);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debugger.Break();
                    }
                }));
            }
            else
            {
                var delta = target.Height - height;
                var newWidth = width > target.Width ? (int)target.Width : width;
                var newHeight = height > target.Height ? (int)target.Height : height;
                Int32Rect outRect = new Int32Rect(0, (int)(delta >= 0 ? delta : 0) / 2, newWidth, newWidth);
                try
                {
                    target.WritePixels(outRect, bits, stride, 0);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }


        private static System.Drawing.Size getDefaultSize(IconSize size)
        {
            switch (size)
            {
                case IconSize.jumbo: return new System.Drawing.Size(256, 256);
                case IconSize.thumbnail: return new System.Drawing.Size(256, 256);
                case IconSize.extraLarge: return new System.Drawing.Size(48, 48);
                case IconSize.large: return new System.Drawing.Size(32, 32);
                default: return new System.Drawing.Size(16, 16);
            }
        }

        public static Bitmap Bolden(Bitmap bmp0)
        {
            float f = 2f;

            Bitmap bmp = new Bitmap(bmp0.Width, bmp0.Height);
            using (Bitmap bmp1 = new Bitmap(bmp0, new System.Drawing.Size((int)(bmp0.Width * f),
                                                           (int)(bmp0.Height * f))))
            {
                float contrast = 1f;

                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                        {
            new float[] {contrast, 0, 0, 0, 0},
            new float[] {0,contrast, 0, 0, 0},
            new float[] {0, 0, contrast, 0, 0},
            new float[] {0, 0, 0, 1, 0},
            new float[] {0, 0, 0, 0, 1}
                        });

                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default,
                                                        ColorAdjustType.Bitmap);
                attributes.SetGamma(7.5f, ColorAdjustType.Bitmap);
                using (Graphics g = Graphics.FromImage(bmp))
                    g.DrawImage(bmp1, new Rectangle(0, 0, bmp.Width, bmp.Height),
                             0, 0, bmp1.Width, bmp1.Height, GraphicsUnit.Pixel, attributes);

            }
            return bmp;
        }

        //http://blog.paranoidferret.com/?p=11 , modified a little.
        public static Bitmap resizeImage(Bitmap imgToResize, System.Drawing.Size size, int spacing, bool bool_darken)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)((sourceWidth * nPercent) - spacing * 4);
            int destHeight = (int)((sourceHeight * nPercent) - spacing * 4);

            int leftOffset = (int)((size.Width - destWidth) / 2);
            int topOffset = (int)((size.Height - destHeight) / 2);


            Bitmap b = new Bitmap(size.Width, size.Height);
            
            Graphics g = Graphics.FromImage((Image)b);
            g.Clear(Color.White);
            //g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //////g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            ////g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

     
            ////////////g.DrawLines(System.Drawing.Pens.Silver, new PointF[] {
            ////////////    new PointF(leftOffset - spacing, topOffset + destHeight + spacing), //BottomLeft
            ////////////    new PointF(leftOffset - spacing, topOffset -spacing),                 //TopLeft
            ////////////    new PointF(leftOffset + destWidth + spacing, topOffset - spacing)});//TopRight

            ////////////g.DrawLines(System.Drawing.Pens.Gray, new PointF[] {
            ////////////    new PointF(leftOffset + destWidth + spacing, topOffset - spacing),  //TopRight
            ////////////    new PointF(leftOffset + destWidth + spacing, topOffset + destHeight + spacing), //BottomRight
            ////////////    new PointF(leftOffset - spacing, topOffset + destHeight + spacing),}); //BottomLeft

            g.DrawImage(imgToResize, leftOffset, topOffset, destWidth, destHeight);
            g.Dispose();

            if(bool_darken) b = Bolden(b);
            return b;
        }

        private static Bitmap resizeJumbo(Bitmap imgToResize, System.Drawing.Size size, int spacing)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = 80;
            int destHeight = 80;

            int leftOffset = (int)((size.Width - destWidth) / 2);
            int topOffset = (int)((size.Height - destHeight) / 2);


            Bitmap b = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.DrawLines(System.Drawing.Pens.Silver, new PointF[] {
                new PointF(0 + spacing, size.Height - spacing), //BottomLeft
                new PointF(0 + spacing, 0 + spacing),                 //TopLeft
                new PointF(size.Width - spacing, 0 + spacing)});//TopRight

            g.DrawLines(System.Drawing.Pens.Gray, new PointF[] {
                new PointF(size.Width - spacing, 0 + spacing),  //TopRight
                new PointF(size.Width - spacing, size.Height - spacing), //BottomRight
                new PointF(0 + spacing, size.Height - spacing)}); //BottomLeft

            g.DrawImage(imgToResize, leftOffset, topOffset, destWidth, destHeight);
            g.Dispose();

            return b;
        }

        private static BitmapSource loadBitmap(Bitmap source)
        {
            IntPtr hBitmap = source.GetHbitmap();
            //Memory Leak fixes, for more info : http://social.msdn.microsoft.com/forums/en-US/wpf/thread/edcf2482-b931-4939-9415-15b3515ddac6/
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }

        private static bool isImage(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            if (ext == "")
                return false;
            return (imageFilter.IndexOf(ext) != -1 && File.Exists(fileName));
        }

        private static bool isExecutable(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            if (ext == "")
                return false;
            return (exeFilter.IndexOf(ext) != -1 && File.Exists(fileName));
        }

        private static bool isFolder(string path)
        {
            return path.EndsWith("\\") || Directory.Exists(path);
        }

        private static string returnKey(string fileName, IconSize size)
        {
            string key = Path.GetFileName(fileName).ToLower();

            if (isExecutable(fileName))
                key = fileName.ToLower();
            if (isImage(fileName) && size == IconSize.thumbnail)
                key = fileName.ToLower();
            if (isFolder(fileName))
                key = fileName.ToLower();

            switch (size)
            {
                case IconSize.thumbnail: key += isImage(fileName) ? "+T" : "+J"; break;
                case IconSize.jumbo: key += "+J"; break;
                case IconSize.extraLarge: key += "+XL"; break;
                case IconSize.large: key += "+L"; break;
                case IconSize.small: key += "+S"; break;
            }
            return key;
        }
        #endregion

        #region Static Cache
        private static Dictionary<string, ImageSource> iconDic = new Dictionary<string, ImageSource>();
        private static SysImageList _imgList = new SysImageList(SysImageListSize.jumbo);

        private Bitmap loadJumbo(string lookup)
        {
            _imgList.ImageListSize = isVistaUp() ? SysImageListSize.jumbo : SysImageListSize.extraLargeIcons;
            Icon icon = _imgList.Icon(_imgList.IconIndex(lookup, isFolder(lookup)));
            Bitmap bitmap = icon.ToBitmap();
            icon.Dispose();

            System.Drawing.Color empty = System.Drawing.Color.FromArgb(0, 0, 0, 0);

            if (bitmap.Width < 256)
                bitmap = resizeImage(bitmap, new System.Drawing.Size(256, 256), 0, false);
            else
                if (bitmap.GetPixel(100, 100) == empty && bitmap.GetPixel(200, 200) == empty && bitmap.GetPixel(200, 200) == empty)
            {
                _imgList.ImageListSize = SysImageListSize.largeIcons;
                bitmap = resizeJumbo(_imgList.Icon(_imgList.IconIndex(lookup)).ToBitmap(), new System.Drawing.Size(200, 200), 5);
            }
            return bitmap;
        }

        #endregion

        #region Instance Cache
        private static Dictionary<string, ImageSource> thumbDic = new Dictionary<string, ImageSource>();

        public void ClearInstanceCache()
        {
            thumbDic.Clear();
            //System.GC.Collect();
        }

        private void PollIconCallback(object state)
        {
            thumbnailInfo input = state as thumbnailInfo;
            string fileName = input.fullPath;
            WriteableBitmap writeBitmap = input.bitmap;
            IconSize size = input.iconsize;

            Bitmap origBitmap = GetFileIcon(fileName, size).ToBitmap();
            Bitmap inputBitmap = origBitmap;
            if (size == IconSize.jumbo || size == IconSize.thumbnail)
                inputBitmap = resizeJumbo(origBitmap, getDefaultSize(size), 5);
            else inputBitmap = resizeImage(origBitmap, getDefaultSize(size), 0, false);

            BitmapSource inputBitmapSource = loadBitmap(inputBitmap);
            origBitmap.Dispose();
            inputBitmap.Dispose();

            copyBitmap(inputBitmapSource, writeBitmap, true);
        }

        private void PollThumbnailCallback(object state)
        {
            //Non UIThread
            thumbnailInfo input = state as thumbnailInfo;
            string fileName = input.fullPath;
            WriteableBitmap writeBitmap = input.bitmap;
            IconSize size = input.iconsize;

            try
            {
                Bitmap origBitmap = new Bitmap(fileName);
                Bitmap inputBitmap = resizeImage(origBitmap, getDefaultSize(size), 5, false);
                BitmapSource inputBitmapSource = loadBitmap(inputBitmap);
                origBitmap.Dispose();
                inputBitmap.Dispose();

                copyBitmap(inputBitmapSource, writeBitmap, true);
            }
            catch { }
        }

        private ImageSource addToDic(string fileName, IconSize size)
        {
            string key = returnKey(fileName, size);
            //    string stringstring = @"C:\Users\Joshua\Dropbox\pkRevit Storage (do not edit directly)\Database File\Admin Storage\20201229 1732 57\002 LR LR install studio and microsoft installer projects.pdf";

            //MessageBox.Show(fileName);

            if (size == IconSize.large || size == IconSize.extraLarge || size == IconSize.jumbo || size == IconSize.thumbnail || isExecutable(fileName))
            {
                int eL = -1;

                try
                {
                    if (!thumbDic.ContainsKey(key))
                        lock (thumbDic)
                        {
                            if (fileName == "")
                            {
                                thumbDic.Add(key, getImage(fileName, size));
                            }
                            else if ((File.GetAttributes(fileName) & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                thumbDic.Add(key, ShellFolder.FromParsingName(fileName).Thumbnail.BitmapSource);
                            }
                            else
                            {
                                ShellFile sf = ShellFile.FromFilePath(fileName);

                                ///so I am completely drawing a blank on where, where, where, where was the original code that gave me the full side icon
                                ///full size icon
                                ///full size icon
                                ///full size icon
                                ///

                                if (ShellFile.FromFilePath(fileName).IsLink)
                                {
                                    //resizeImage(ShellFile.FromFilePath(fileName).Thumbnail.CurrentSize.Width < 256)

                                    Bitmap bitmap = resizeImage(ShellFile.FromParsingName(fileName).Thumbnail.SmallBitmap, new System.Drawing.Size(256, 256), 0, false);
                                    thumbDic.Add(key, helpers.GetBitmapSource(bitmap));

                                    //BitmapSource src = ShellFile.FromFilePath(fileName).Thumbnail.SmallBitmapSource;
                                    //thumbDic.Add(key, src);

                                } else if (fileName.Substring(fileName.Length - 3) == "jpg" | fileName.Substring(fileName.Length - 3) == "png" )
                                {
                                    if (System.IO.File.Exists(fileName))
                                    {
                                        ////DateTime modDate = System.IO.File.GetLastWriteTime(fileName);
                                        ////System.IO.File.SetLastWriteTime(fileName, modDate.AddSeconds(1.0));
                                        ////System.IO.File.SetLastWriteTime(fileName, modDate.AddSeconds(-1.0));
                                        //  Bitmap bitmap2 = GetFileIcon(fileName, IconSize.large).ToBitmap();

                                        ShellThumbnail shellthumbnail = ShellFile.FromParsingName(fileName).Thumbnail;
                                        shellthumbnail.FormatOption = ShellThumbnailFormatOption.ThumbnailOnly;

                                        int timeOut = 0;
                                        LoopLoop:
                                        Bitmap bm;
                                        try
                                        {
                                            bm = shellthumbnail.Bitmap;
                                            System.Threading.Thread.Sleep(2);
                                        }
                                        catch // errors can occur with windows api calls so just skip
                                        {
                                            timeOut++;
                                            if(timeOut < 50)
                                            {
                                                goto LoopLoop;
                                            } else
                                            {
                                                shellthumbnail.FormatOption = ShellThumbnailFormatOption.IconOnly;
                                                bm = shellthumbnail.Bitmap;
                                            }
                                        }

                                        Bitmap bitmap = resizeImage(bm, new System.Drawing.Size(256, 256), 0, false);

                                        //////int THUMB_SIZE = 256;
                                        //////Bitmap thumbnail = WindowsThumbnailProvider.GetThumbnail(
                                        //////   fileName, THUMB_SIZE, THUMB_SIZE, ThumbnailOptions.InMemoryOnly);

                                        //////////////Thread.Sleep(200);

                                        //////////////shellobject = ShellFile.FromParsingName(fileName);
                                        //////////////bitmap = resizeImage(shellobject.Thumbnail.Bitmap, new System.Drawing.Size(256, 256), 0);
                                        //bitmap = resizeImage(ShellFile.FromParsingName(fileName).Thumbnail.Bitmap, new System.Drawing.Size(256, 256), 0);
                                        thumbDic.Add(key, helpers.GetBitmapSource(bitmap));
                                    }

                                }
                                else if (sf.Thumbnail.BitmapSource.PixelWidth != sf.Thumbnail.BitmapSource.PixelHeight)
                                {
                                    ////DateTime modDate = System.IO.File.GetLastWriteTime(fileName);
                                    ////System.IO.File.SetLastWriteTime(fileName, modDate.AddSeconds(1.0));
                                    ////System.IO.File.SetLastWriteTime(fileName, modDate.AddSeconds(-1.0));
                                    //Bitmap bitmap2 = GetFileIcon(fileName, IconSize.large).ToBitmap();

                                    ShellObject shellobject = ShellFile.FromParsingName(fileName);
                                    Bitmap bitmap = resizeImage(shellobject.Thumbnail.Bitmap, new System.Drawing.Size(256, 256), 0, false);

                                    //////////////////Thread.Sleep(200);

                                    //////////////////shellobject = ShellFile.FromParsingName(fileName);
                                    //////////////////bitmap = resizeImage(shellobject.Thumbnail.Bitmap, new System.Drawing.Size(256, 256), 0);

                                    // bitmap = resizeImage(ShellFile.FromParsingName(fileName).Thumbnail.Bitmap, new System.Drawing.Size(256, 256), 0);
                                    thumbDic.Add(key, helpers.GetBitmapSource(bitmap));
                                }
                                else
                                {
                                    ////////ShellObject shellobject = ShellFile.FromParsingName(fileName);
                                    ////////Bitmap bitmap = resizeImage((Bitmap)Image.FromFile(fileName), new System.Drawing.Size(256, 256), 0);
                                    //////////thumbDic.Add(key, helpers.GetBitmapSource((Bitmap)Image.FromFile(fileName)));

                                    Icon appIcon = Icon.ExtractAssociatedIcon(fileName);
                                    

                                    BitmapSource src = ShellFile.FromParsingName(fileName).Thumbnail.BitmapSource;

                                    ///////////////////////////thumbDic.Add(key, helpers.GetBitmapSource(appIcon.ToBitmap()));
                                    thumbDic.Add(key, src);



                                    //////                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>

                                    //////                                    {
                                    //////                                        ////////ShellObject shellobject = null;
                                    //////                                        ////////shellobject = ShellFile.FromFilePath(fileName);
                                    //////                                        ////////shellobject.Thumbnail.FormatOption = ShellThumbnailFormatOption.ThumbnailOnly;

                                    //////                                        ////////////ShellObject shellobject = ShellFile.FromParsingName(fileName);
                                    //////                                        ////////////Bitmap bitmap = resizeImage(shellobject.Thumbnail.Bitmap, new System.Drawing.Size(256, 256), 0);

                                    //////                                        BitmapSource src = ShellFile.FromParsingName(fileName).Thumbnail.SmallBitmapSource;

                                    //////                                        thumbDic.Add(key, src);
                                    //////                                        // BitmapSource src = helpers.GetBitmapSource(bitmap));
                                    //////                                        // bitmap.Save(@"C:\sdf.sdf");


                                    //////                                    }
                                    //////)); ;
                                    ////////////////////BitmapSource src = ShellFile.FromParsingName(fileName).Thumbnail.SmallBitmapSource;
                                    ////////////////////thumbDic.Add(key, src);


                                    ////Bitmap bitmap = resizeImage(ShellFile.FromParsingName(fileName).Thumbnail.Bitmap, new System.Drawing.Size(256, 256), 0);
                                    ////thumbDic.Add(key, helpers.GetBitmapSource(bitmap));

                                    //MessageBox.Show("hello");

                                    //////Icon icon;
                                    //////string lookup = "aaa" + Path.GetExtension(fileName).ToLower();
                                    //////if (!key.StartsWith("."))
                                    //////    lookup = fileName;

                                    //////_imgList.ImageListSize = SysImageListSize.smallIcons;
                                    //////icon = _imgList.Icon(_imgList.IconIndex(lookup, isFolder(fileName)));

                                    ////thumbDic.Add(key, loadBitmap(bitmap));
                                }
                            }
                        }
                }

                #region catch and finally
                catch (Exception ex)
                {
                    _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("addToDic, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
                }
                finally
                {
                }
                #endregion

                return thumbDic[key];

            }
            else
            {
                if (!iconDic.ContainsKey(key))
                    lock (iconDic)
                        if ((File.GetAttributes(fileName) & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            iconDic.Add(key, ShellFolder.FromParsingName(fileName).Thumbnail.BitmapSource);
                        }
                        else
                        {
                            iconDic.Add(key, ShellFile.FromFilePath(fileName).Thumbnail.BitmapSource);
                        }

                return iconDic[key];
            }

            //////if (size == IconSize.thumbnail || isExecutable(fileName))
            //////{
            //////    if (!thumbDic.ContainsKey(key))
            //////        lock (thumbDic)
            //////            thumbDic.Add(key, loadBitmap(icon.ToBitmap()));

            //////    return thumbDic[key];
            //////}
            //////else
            //////{
            //////    if (!iconDic.ContainsKey(key))
            //////        lock (iconDic)
            //////            iconDic.Add(key, getImage(fileName, size));
            //////    return iconDic[key];
            //////}
        }


        public static byte[] ConvertImageToByteArray(Image imageToConvert)
        {
            using (var ms = new MemoryStream())
            {
                ImageFormat format;
                format = ImageFormat.Bmp;
                //////switch (imageToConvert.MimeType())	
                //////{	
                //////    case "image/png":	
                //////        format = ImageFormat.Png;	
                //////        break;	
                //////    case "image/gif":	
                //////        format = ImageFormat.Gif;	
                //////        break;	
                //////    default:	
                //////        format = ImageFormat.Jpeg;	
                //////        break;	
                //////}	
                imageToConvert.Save(ms, format);
                return ms.ToArray();
            }
        }
        public static System.Drawing.Icon GetRegisteredIcon(string filePath)
        {
            var shinfo = new SHfileInfo();
            Win32.SHGetFileInfo(filePath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_SMALLICON);
            return System.Drawing.Icon.FromHandle(shinfo.hIcon);
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct SHfileInfo
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }
        internal sealed class Win32
        {
            public const uint SHGFI_ICON = 0x100;
            public const uint SHGFI_LARGEICON = 0x0; // large	
            public const uint SHGFI_SMALLICON = 0x1; // small	
            [System.Runtime.InteropServices.DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHfileInfo psfi, uint cbSizeFileInfo, uint uFlags);
        }


        //////////public Dictionary<string, ImageSource> GetValueById(Dictionary<string, ImageSource> thumbDic, string key, string fileName)
        //////////{
        //////////    return Application.Current.Dispatcher.Invoke(() =>
        //////////    {
        //////////        var main = Application.Current.MainWindow as pkRevitDatasheets.MainWindow;
        //////////        if (main != null)
        //////////        {
        //////////            BitmapSource src = ShellFile.FromParsingName(fileName).Thumbnail.SmallBitmapSource;

        //////////            thumbDic.Add(key, src);

        //////////            return thumbDic;
        //////////        }
        //////////    });
        //////////}


        public ImageSource GetImage(string fileName, int iconSize)
        {
            IconSize size;

            if (iconSize <= 16) size = IconSize.small;
            else if (iconSize <= 32) size = IconSize.large;
            else if (iconSize <= 48) size = IconSize.extraLarge;
            else if (iconSize <= 72) size = IconSize.jumbo;
            else size = IconSize.thumbnail;

            //size = IconSize.jumbo;
            return addToDic(fileName, size);
        }

        #endregion

        #region Instance Tools

        public static bool isVistaUp()
        {
            return (Environment.OSVersion.Version.Major >= 6);
        }

        public static void SaveClipboardImageToFile(string filePath)
        {
            var image = Clipboard.GetImage();
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }

        //////private BitmapImage GetThumbnail(string filePath)
        //////{
        //////    ShellFile shellFile = ShellFile.FromFilePath(filePath);
        //////    BitmapSource shellThumb = shellFile.Thumbnail.ExtraLargeBitmapSource;

        //////    BitmapImage bImg = new BitmapImage();
        //////    PngBitmapEncoder encoder = new PngBitmapEncoder();

        //////    var memoryStream = new MemoryStream();
        //////    encoder.Frames.Add(BitmapFrame.Create(shellThumb));
        //////    encoder.Save(memoryStream);
        //////    bImg.BeginInit();
        //////    bImg.StreamSource = memoryStream;
        //////    bImg.EndInit();
        //////    return bImg;
        //////}

        private BitmapSource getImage(string fileName, IconSize size)
        {
            Icon icon;
            string key = returnKey(fileName, size);
            string lookup = "aaa" + Path.GetExtension(fileName).ToLower();
            if (!key.StartsWith("."))
                lookup = fileName;


            ///it is here somewhere
            ///
            //////////string stringstring = @"C:\Users\Joshua\Dropbox\pkRevit Storage (do not edit directly)\Database File\Admin Storage\20201229 1732 57\002 LR LR install studio and microsoft installer projects.pdf";
            //////////ShellThumbnail mtSource = ShellFile.FromFilePath(@"C:\Users\Joshua\Dropbox\pkRevit Storage (do not edit directly)\Database File\Admin Storage\20201229 1732 57\002 LR LR install studio and microsoft installer projects.pdf").Thumbnail;
            //////////string stringstring2 = @"C:\Users\Joshua\Documents\93 saturday processing\20191123 1359\" + Path.GetFileNameWithoutExtension(fileName) + ".bmp";
            //////////mtSource.LargeBitmap.Save(stringstring2);

            if (isExecutable(fileName))
            {

                WriteableBitmap bitmap = new WriteableBitmap(addToDic("aaa.exe", size) as BitmapSource);
                ThreadPool.QueueUserWorkItem(new WaitCallback(PollIconCallback), new thumbnailInfo(bitmap, fileName, size));
                return bitmap;
                ////return null;
            }

            else
                switch (size)
                {
                    case IconSize.thumbnail:
                        if (isImage(fileName))
                        {

                            //Load as jumbo icon first.                         
                            WriteableBitmap bitmap = new WriteableBitmap(addToDic(fileName, IconSize.jumbo) as BitmapSource);
                            //WriteableBitmap bitmap = ShellFile.FromFilePath(fileName).Thumbnail.BitmapSource; 

                            //BitmapSource bitmapSource = addToDic(fileName, IconSize.jumbo) as BitmapSource;                            
                            //WriteableBitmap bitmap = new WriteableBitmap(256, 256, 96, 96, PixelFormats.Bgra32, null);
                            //copyBitmap(bitmapSource, bitmap, false);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(PollThumbnailCallback), new thumbnailInfo(bitmap, fileName, size));
                            return bitmap;
                        }
                        else
                        {
                            return getImage(lookup, IconSize.jumbo);
                        }

                    case IconSize.jumbo:
                        return loadBitmap(loadJumbo(lookup));
                    case IconSize.extraLarge:
                        _imgList.ImageListSize = SysImageListSize.extraLargeIcons;
                        icon = _imgList.Icon(_imgList.IconIndex(lookup, isFolder(fileName)));
                        return loadBitmap(icon.ToBitmap());
                    default:
                        icon = GetFileIcon(lookup, size);
                        return loadBitmap(icon.ToBitmap());
                }
        }
        #endregion

        public FileToIconConverter()
        {
            this.defaultsize = 48;
        }

        #region IMultiValueConverter Members
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int eL = -1;
            int size = defaultsize;

            try
            {
                if (values.Length > 1 && values[1] is double)
                    size = (int)(float)(double)values[1];

                if (values[0] is string)
                    return GetImage(values[0] as string, size);
                else return GetImage("", size);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Convert, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
            return GetImage("", size);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

   }
