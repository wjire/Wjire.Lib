using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Wjire.Common
{
    /// <summary>
    /// 图片工具类
    /// </summary>
    public static class ImageHelper
    {

        /// <summary>
        /// 解析文件base64
        /// </summary>
        /// <param name="base64"></param>
        /// <returns>item1:文件字节,item2:文件格式</returns>
        public static Tuple<byte[], string> AnalyzeBase64(string base64)
        {
            string[] imgInfo = base64.Split(',');
            //图片base64
            string base64Code = imgInfo[1];

            //获取文件类型
            string[] tempArray = imgInfo[0].Split(';');
            string fileType = tempArray[0].Substring(tempArray[0].IndexOf('/') + 1);

            //base64转字节
            byte[] imgBytes = Convert.FromBase64String(base64Code);
            return new Tuple<byte[], string>(imgBytes, fileType);
        }

        /// <summary>  
        /// 合并图片  
        /// </summary>  
        /// <param name="imgBack">背景图片</param>  
        /// <param name="img">需要嵌套的图片</param>  
        /// <param name="position">需要嵌套的图片的位置</param>  
        /// <returns></returns>  
        public static Image CombineImage(Image imgBack, Image img, ImgPosition position, Graphics g = null, float? x = null, float? y = null)
        {
            bool isNeedDispose = false;
            if (g == null)
            {
                g = Graphics.FromImage(imgBack);
                isNeedDispose = true;
            }

            //呈现质量
            g.SmoothingMode = SmoothingMode.AntiAlias;

            //画需要嵌套的图片
            PointF point = GetPosition(imgBack, img, position, x, y);
            g.DrawImage(img, point.X, point.Y);

            img.Dispose();
            if (isNeedDispose)
            {
                g.Dispose();
            }
            return imgBack;
        }

        /// <summary>  
        /// 将二维码画在白色背景图上
        /// </summary>  
        /// <param name="oriFormat">二维码格式</param>
        /// <param name="width">白色背景图宽度</param>
        /// <param name="height">白色背景图高度</param>
        /// <param name="img">二维码</param>
        /// <returns></returns>  
        public static Bitmap CreateWhiteBmp(PixelFormat oriFormat, int width, int height, Image img)
        {
            Bitmap bmp = new Bitmap(width, height, oriFormat);
            Graphics g = Graphics.FromImage(bmp);

            //呈现质量
            g.SmoothingMode = SmoothingMode.AntiAlias;

            //定义画布颜色
            g.Clear(Color.White);

            //再画需要嵌套的图片
            PointF point = GetPosition(bmp, img, ImgPosition.居中);
            g.DrawImage(img, point.X, point.Y);

            img.Dispose();
            g.Dispose();

            return bmp;
        }

        /// <summary>  
        /// 将二维码画在白色背景图上
        /// </summary>  
        /// <param name="img">二维码</param>
        /// <returns></returns>  
        public static Image CreateWhiteBmp(Image bmp, Image img, ImgPosition imgPosition)
        {
            Graphics g = Graphics.FromImage(bmp);

            //呈现质量
            g.SmoothingMode = SmoothingMode.AntiAlias;

            //定义画布颜色
            g.Clear(Color.White);

            //再画需要嵌套的图片
            PointF point = GetPosition(bmp, img, imgPosition);
            g.DrawImage(img, point.X, point.Y);

            img.Dispose();
            g.Dispose();

            return bmp;
        }

        /// <summary>
        /// 获取需要嵌套的图片的左上角的坐标
        /// </summary>
        /// <param name="imgBack">背景图片</param>
        /// <param name="img">需要嵌套的图片</param>
        /// <param name="position">位置枚举</param>
        /// <returns></returns>
        private static PointF GetPosition(Image imgBack, Image img, ImgPosition position, float? x = null, float? y = null)
        {
            PointF point = new PointF
            {
                X = 0,
                Y = 0
            };
            switch (position)
            {
                case ImgPosition.居中:
                    point.X = imgBack.Width / 2 - img.Width / 2;
                    point.Y = imgBack.Height / 2 - img.Height / 2;
                    break;
                case ImgPosition.左上角:
                    break;
                case ImgPosition.右上角:
                    point.X = imgBack.Width - img.Width;
                    break;
                case ImgPosition.右下角:
                    point.X = imgBack.Width - img.Width;
                    point.Y = imgBack.Height - img.Height;
                    break;
                case ImgPosition.左下角:
                    point.Y = imgBack.Height - img.Height;
                    break;
                case ImgPosition.自定义:
                    point.X = (float)x;
                    point.Y = (float)y;
                    break;
            }
            return point;
        }

        /// <summary>
        /// 图片 转 字节, 并且根据图片内容来防止恶意脚本,因为文件扩展名有可能会被恶意更改。
        /// </summary>
        /// <param name="image">图片</param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image)
        {
            Guid format = image.RawFormat.Guid;
            MemoryStream ms = new MemoryStream();
            if (format.Equals(ImageFormat.Jpeg.Guid))
            {
                image.Save(ms, ImageFormat.Jpeg);
            }
            //用画笔画的 Bitmap 必须匹配这种类型( 这是一个深坑啊!!)
            else if (format.Equals(ImageFormat.MemoryBmp.Guid))
            {
                image.Save(ms, ImageFormat.Bmp);
            }
            else if (format.Equals(ImageFormat.Png.Guid))
            {
                image.Save(ms, ImageFormat.Png);
            }
            else if (format.Equals(ImageFormat.Bmp.Guid))
            {
                image.Save(ms, ImageFormat.Bmp);
            }
            else if (format.Equals(ImageFormat.Gif.Guid))
            {
                image.Save(ms, ImageFormat.Gif);
            }
            else if (format.Equals(ImageFormat.Icon.Guid))
            {
                image.Save(ms, ImageFormat.Icon);
            }
            else if (format.Equals(ImageFormat.Tiff.Guid))
            {
                image.Save(ms, ImageFormat.Tiff);
            }
            else
            {
                throw new ArgumentException("未知的图片格式");
            }
            //Save操作已经对流进行了一次写入操作,流的游标已经移到了最后,所以需要重新设置到起始位置
            ms.Seek(0, SeekOrigin.Begin);
            byte[] bytes = ms.ToArray();
            ms.Close();
            ms.Dispose();
            return bytes;
        }

        /// <summary>
        /// 字节数组 转 图片
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] buffer)
        {
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                return Image.FromStream(ms);
            }
        }

        /// <summary>
        /// 位置枚举
        /// </summary>
        public enum ImgPosition
        {
            居中 = 0,
            左上角 = 1,
            右上角 = 2,
            右下角 = 3,
            左下角 = 4,
            自定义 = 5
        }


        /// <summary>
        /// 将图片按百分比压缩，flag取值1到100，越小压缩比越大,如果 flag=100,则表示不压缩,直接返回原图流
        /// </summary>
        /// <param name="source">源图片字节</param>
        /// <param name="flag">压缩比例 1到100，越小压缩比越大</param>
        /// <returns>压缩后的图片流</returns>
        public static Stream Compress(byte[] source, int flag)
        {
            if (source == null || source.Length == 0)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (flag < 1 || flag > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(flag));
            }

            if (flag == 100)
            {
                return ToStreamFrom(source);
            }
            Image img = Image.FromStream(ToStreamFrom(source));
            return Compress(img, flag);
        }

        /// <summary>
        /// 将图片按百分比压缩，flag取值1到100，越小压缩比越大
        /// </summary>
        /// <param name="source">源图片</param>
        /// <param name="flag">压缩比例 1到100，越小压缩比越大</param>
        /// <returns>压缩后的图片流</returns>
        public static Stream Compress(Image source, int flag)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (flag < 1 || flag > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(flag));
            }

            if (flag == 100)
            {
                return ToStreamFrom(ImageToBytes(source));
            }

            ImageFormat tFormat = source.RawFormat;
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;
            EncoderParameter eParam = new EncoderParameter(Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageDecoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                MemoryStream stream = new MemoryStream();
                {
                    if (jpegICIinfo != null)
                    {
                        source.Save(stream, jpegICIinfo, ep);
                    }
                    else
                    {
                        source.Save(stream, tFormat);
                    }
                    stream.Seek(0, SeekOrigin.Begin);
                }
                return stream;
            }
            catch
            {
                return null;
            }
        }

        #region 缩略图

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="bytes">源图字节</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        /// <returns>裁剪后的图片</returns>
        public static Image MakeThumbnail(byte[] bytes, int width, int height, ThumbnailMode mode)
        {
            Image img = BytesToImage(bytes);
            return MakeThumbnail(img, width, height, mode);
        }


        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImage">源图</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        /// <returns>裁剪后的图片</returns>
        public static Image MakeThumbnail(Image originalImage, int width, int height, ThumbnailMode mode)
        {
            int towidth = width;//50
            int toheight = height;//50

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case ThumbnailMode.HW:  //指定高宽缩放（可能变形）                
                    break;
                case ThumbnailMode.W:   //指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ThumbnailMode.H:   //指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ThumbnailMode.Cut: //指定高宽裁减（不变形）                
                    if (originalImage.Width / (double)originalImage.Height > towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);

            try
            {
                return bitmap;
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                g.Dispose();
            }
        }

        #endregion

        public enum ThumbnailMode
        {
            /// <summary>
            ///  指定高宽缩放（可能变形）  
            /// </summary>
            HW,
            /// <summary>
            /// 指定宽，高按比例
            /// </summary>
            W,
            /// <summary>
            /// 指定高，宽按比例
            /// </summary>
            H,
            /// <summary>
            /// 指定高宽裁减（不变形）                
            /// </summary>
            Cut
        }


        /// <summary>
        /// 文字处理
        /// </summary>
        /// <param name="strr">字符串</param>
        /// <param name="fwith">开始宽度</param>
        /// <param name="autoHeight">开始高度</param>
        /// <param name="fontHeight">行高度</param>
        /// <param name="theGraphics">gdp+</param>
        /// <param name="theFontnormal">文字格式</param>
        /// <param name="newBrushnormal">画刷</param>
        /// <param name="kuandu" >宽度</param>
        /// <returns>返回高度</returns>
        public static int DrawFont(string strr, int fwith, int autoHeight, int fontHeight, Graphics theGraphics, Font theFontnormal, Brush newBrushnormal, int kuandu)
        {
            strr = strr.Replace(",", "@m@");
            //换行变，号
            strr = strr.Replace("<br/>", ",");

            string[] tag = strr.Split(new char[] { ',' });

            if (tag.Any())
            {
                foreach (string item in tag)
                {
                    string str = item.Replace("@m@", ",");
                    int thewight = Convert.ToInt16(Fontwigth(str, theFontnormal));
                    if (thewight > kuandu)
                    {
                        int cs = thewight / kuandu;//除数
                        int ys = thewight % kuandu;//余数
                        if (ys > 0)
                        {
                            cs = cs + 1;
                        }
                        int scs = str.Length / (cs - 1);
                        string newstr = string.Empty;

                        for (int i = 0; i < cs; i++)
                        {
                            if (scs > str.Length)
                            {
                                scs = str.Length;
                            }
                            if (Fontwigth(str, theFontnormal) >= kuandu)
                            {
                                do
                                {
                                    newstr = str.Substring(0, scs);
                                    if (Fontwigth(newstr, theFontnormal) > kuandu && scs < 100)
                                    {
                                        scs = scs - 1;
                                    }
                                } while (Fontwigth(newstr, theFontnormal) > kuandu && scs < 100);
                            }
                            else
                            {
                                do
                                {
                                    newstr = str.Substring(0, scs);
                                    if (scs < str.Length)
                                    {
                                        if (Fontwigth(newstr, theFontnormal) < kuandu && scs < 100 && scs < str.Length)
                                        {
                                            scs = scs + 1;
                                        }
                                    }
                                } while (Fontwigth(newstr, theFontnormal) < kuandu && scs < 100 && scs < str.Length);
                            }

                            theGraphics.DrawString(newstr, theFontnormal, newBrushnormal, fwith, autoHeight);
                            autoHeight += fontHeight;
                            if (scs > 0 && str.Length > scs)
                            {
                                str = str.Substring(scs, str.Length - scs);
                            }
                            newstr = string.Empty;
                        }
                    }
                    else
                    {
                        theGraphics.DrawString(str, theFontnormal, newBrushnormal, fwith, autoHeight);
                        autoHeight += fontHeight;
                    }
                }
            }
            return autoHeight;
        }

        /// <summary>
        /// 计算字符长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="theFontnormal"></param>
        /// <returns></returns>
        private static int Fontwigth(string str, Font theFontnormal)
        {
            int length = str.Length;
            float result = theFontnormal.Size * length;
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// 字节数组 转 流
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <returns></returns>
        public static Stream ToStreamFrom(byte[] buffer)
        {
            return new MemoryStream(buffer);
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="src"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static Image Tailor(Bitmap src, Rectangle range)
        {
            return src.Clone(range, PixelFormat.DontCare);
        }

        /// <summary>
        /// 流转字节
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="length">缓冲大小,默认4096</param>
        /// <returns></returns>
        public static byte[] ToBytesFrom(Stream stream, int length = 4096)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryReader reader = new BinaryReader(stream))
            {
                byte[] buffer = new byte[length];
                int count = 0;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                {
                    ms.Write(buffer, 0, count);
                }
                return ms.ToArray();
            }
        }
    }
}