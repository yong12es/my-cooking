using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.Storage;

namespace mycooking.Converters
{
    public static class CompresorImagen
    {
        public static async Task<byte[]> DecodeImageAsync(string fileUri, int regWidth, int regHeight, int compressionQuality)
        {
            byte[] returnVal;

            try
            {
                StorageFile file = await StorageFile.GetFileFromPathAsync(fileUri);

                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                    SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();

                    var qualityNum = (float)compressionQuality / 100;
                    var propertySet = new BitmapPropertySet();
                    var qualityValue = new BitmapTypedValue(
                        qualityNum,
                        Windows.Foundation.PropertyType.Single
                    );
                    propertySet.Add("ImageQuality", qualityValue);

                    var resizedStream = new InMemoryRandomAccessStream();

                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, resizedStream,
                        propertySet);
                    encoder.SetSoftwareBitmap(softwareBitmap);

                    double widthRatio = (double)regWidth / decoder.PixelWidth;
                    double heightRatio = (double)regHeight / decoder.PixelHeight;

                    double scaleRatio = Math.Min(widthRatio, heightRatio);

                    if (regWidth == 0)
                        scaleRatio = heightRatio;

                    if (regHeight == 0)
                        scaleRatio = widthRatio;

                    uint aspectHeight = (uint)Math.Floor(decoder.PixelHeight * scaleRatio);
                    uint aspectWidth = (uint)Math.Floor(decoder.PixelWidth * scaleRatio);

                    encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Linear;
                    encoder.BitmapTransform.ScaledHeight = aspectHeight;
                    encoder.BitmapTransform.ScaledWidth = aspectWidth;

                    await encoder.FlushAsync();
                    resizedStream.Seek(0);
                    returnVal = new byte[resizedStream.Size];
                    var dr = new DataReader(resizedStream.GetInputStreamAt(0));
                    await dr.LoadAsync((uint)resizedStream.Size);
                    dr.ReadBytes(returnVal);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return returnVal;
        }
    }
}
