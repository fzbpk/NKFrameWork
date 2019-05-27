using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing.QrCode;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace NK.DataWork
{
    /// <summary>
    /// 图形码
    /// </summary>
    public partial class CODEC
    {
        /// <summary>
        /// 图形码解码
        /// </summary>
        /// <param name="img">图形</param>
        /// <param name="BarcodeFormat">返回图形码类型</param>
        /// <param name="Code">原始数据</param>
        /// <returns></returns>
        public static bool Decode(Image img, out string BarcodeFormat, out string Code)
        {
            Code = "";
            BarcodeFormat = "";
            IBarcodeReader reader = new BarcodeReader();
            var result = reader.Decode((Bitmap)img);
            if (result != null)
            {
                BarcodeFormat = result.BarcodeFormat.ToString();
                Code = result.Text;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="Code">原始数据</param>
        /// <param name="Width">图片宽度</param>
        /// <param name="Height">图片高度</param>
        /// <returns></returns>
        public static Image EncodeQRCode(string Code, int Width = 200, int Height = 200)
        {
            EncodingOptions options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = Width,
                Height = Height
            };
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;
            return writer.Write(Code);
        }

        /// <summary>
        /// 条形码
        /// </summary>
        /// <param name="Code">原始数据</param>
        /// <param name="Width">图形宽度</param>
        /// <param name="Height">图形高度</param>
        /// <returns></returns>
        public static Image EncodeBarCode(string Code, int Width = 200, int Height = 100)
        {
            EncodingOptions options = new QrCodeEncodingOptions
            {
                Width = Width,
                Height = Height
            };
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.CODE_128;
            writer.Options = options;
            return writer.Write(Code);
        }

    }

}
