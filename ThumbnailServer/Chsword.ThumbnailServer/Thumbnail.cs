using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace Chsword.ThumbnailServer
{
    ///<summary>
    /// ��������ͼ����
    ///</summary>
    internal class Thumbnail
    {
        public static void CreateThumbnail(Image img, string newFile, Size size)
        {
            CreateThumbnail(img, newFile, size.Width, size.Height);
        }

        ///<summary>
        /// ��������ͼ�����Զ�����������
        ///</summary>
        ///<param name="img">ͼƬ</param>
        ///<param name="fileName">Ҫ��ĵ�ַ</param>
        ///<param name="maxWidth">����</param>
        ///<param name="maxHeight">����</param>
        public static void CreateThumbnail(Image img, string fileName, int maxWidth, int maxHeight)
        {
            Size newSize = NewSize(maxWidth, maxHeight, img.Width, img.Height);
            using (var outBmp = new Bitmap(newSize.Width, newSize.Height))
            {
                using (Graphics g = Graphics.FromImage(outBmp))
                {
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(img, new Rectangle(0, 0, newSize.Width, newSize.Height), 0, 0, img.Width, img.Height,
                        GraphicsUnit.Pixel);
                }
                var quality = new long[] { 100 };
                var encoderParam = new EncoderParameter(Encoder.Quality, quality);
                var encoderParams = new EncoderParameters();
                encoderParams.Param[0] = encoderParam;
                ImageCodecInfo jpegIci =
                    ImageCodecInfo.GetImageEncoders().Single(c => c.FormatDescription.Equals("JPEG"));
                if (jpegIci != null)
                    outBmp.Save(fileName, jpegIci, encoderParams);
                else
                    outBmp.Save(fileName, img.RawFormat);
            }
        }
        private static Size NewSize(int maxWidth, int maxHeight, int width, int height)
        {
            double w;
            double h;
            double sw = Convert.ToDouble(width);
            double sh = Convert.ToDouble(height);
            double mw = Convert.ToDouble(maxWidth);
            double mh = Convert.ToDouble(maxHeight);
            if (sw < mw && sh < mh)
            {
                w = sw;
                h = sh;
            }
            else if ((sw / sh) > (mw / mh))
            {
                w = maxWidth;
                h = (w * sh) / sw;
            }
            else
            {
                h = maxHeight;
                w = (h * sw) / sh;
            }
            return new Size(Convert.ToInt32(w), Convert.ToInt32(h));
        }

        /// <summary>
        /// ͼƬ���ݱȴ���
        /// </summary>
        /// <returns></returns>
        public static double ImageWhb(Image img)
        {
            if (img.Height == 0)
                return 0;
            //System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(fn, true);
            return ((double)img.Width) / img.Height;
        }
    }
}