using Stegomaster.Configuration;
using System.Drawing;
using System.Text;
using System.Drawing.Imaging;

namespace Stegomaster;

#pragma warning disable CA1416
public class StegomasterService : IStegomasterService
{
    public List<string> ProcessRequest(string base64String)
    {
        var bitmap = Base64ToBitmap(base64String);
        var binaryData = GetBinaryDataFromBitmap(bitmap);
        var data = GetDataFromBinaryString(binaryData);

        return data;
    }

    public string ProcessResponse(List<string> data)
    {
        var bitmap = GetBitmapContainer(StegomasterConfiguration.CONTAINER_URL);
        var binaryData = GetBinaryData(data);
        var filledBase64Container = InjectDataIntoContainer(bitmap, binaryData);

        return $"{StegomasterConfiguration.BASE64_IMAGE_PREFIX}{filledBase64Container}";
    }

    private string InjectDataIntoContainer(Bitmap image, string binaryData)
    {
        var skipFirstBitsCounter = 0;

        for (int i = 0, k = 0; i < image.Height; i++)
        {
            var isFirstIteration = true;
            for (var j = skipFirstBitsCounter;;)
            {
                j += isFirstIteration ? 0 : StegomasterConfiguration.SKIP_BITS_COUNT;
                if (j >= image.Width)
                {
                    skipFirstBitsCounter = j - image.Width;
                    break;
                }

                var pixel = image.GetPixel(j, i);

                if (StegomasterConfiguration.ColorsDataHiding.Contains(RGBA.Red) && binaryData.Length > k)
                {
                    var redBinary = NumberToBinary(pixel.R);
                    redBinary = ReplaceAt(redBinary, StegomasterConfiguration.BITS_COUNT - StegomasterConfiguration.LSB_COUNT, binaryData.Substring(k, StegomasterConfiguration.LSB_COUNT));
                    image.SetPixel(j, i, Color.FromArgb(pixel.A, Convert.ToByte(redBinary, 2), pixel.G, pixel.B));
                    pixel = image.GetPixel(j, i);
                    k += StegomasterConfiguration.LSB_COUNT;
                }

                if (StegomasterConfiguration.ColorsDataHiding.Contains(RGBA.Green) && binaryData.Length > k)
                {
                    var greenBinary = NumberToBinary(pixel.G);
                    greenBinary = ReplaceAt(greenBinary, StegomasterConfiguration.BITS_COUNT - StegomasterConfiguration.LSB_COUNT, binaryData.Substring(k, StegomasterConfiguration.LSB_COUNT));
                    image.SetPixel(j, i, Color.FromArgb(pixel.A, pixel.R, Convert.ToByte(greenBinary, 2), pixel.B));
                    pixel = image.GetPixel(j, i);
                    k += StegomasterConfiguration.LSB_COUNT;
                }

                if (StegomasterConfiguration.ColorsDataHiding.Contains(RGBA.Blue) && binaryData.Length > k)
                {
                    var blueBinary = NumberToBinary(pixel.B);
                    blueBinary = ReplaceAt(blueBinary, StegomasterConfiguration.BITS_COUNT - StegomasterConfiguration.LSB_COUNT, binaryData.Substring(k, StegomasterConfiguration.LSB_COUNT));
                    image.SetPixel(j, i, Color.FromArgb(pixel.A, pixel.R, pixel.G, Convert.ToByte(blueBinary, 2)));
                    pixel = image.GetPixel(j, i);
                    k += StegomasterConfiguration.LSB_COUNT;
                }

                if (StegomasterConfiguration.ColorsDataHiding.Contains(RGBA.Alpha) && binaryData.Length > k)
                {
                    var alphaBinary = NumberToBinary(pixel.A);
                    alphaBinary = ReplaceAt(alphaBinary, StegomasterConfiguration.BITS_COUNT - StegomasterConfiguration.LSB_COUNT, binaryData.Substring(k, StegomasterConfiguration.LSB_COUNT));
                    image.SetPixel(j, i, Color.FromArgb(Convert.ToByte(alphaBinary, 2), pixel.R, pixel.G, pixel.B));
                    k += StegomasterConfiguration.LSB_COUNT;
                }

                if (binaryData.Length == k)
                {
                    return BitmapToBase64String(image);
                }

                if (isFirstIteration)
                {
                    isFirstIteration = false;
                }
            }
        }

        return string.Empty;
    }

    private string BitmapToBase64String(Bitmap bitmap) 
    {
        using var memoryStream = new MemoryStream();
        
        bitmap.Save(memoryStream, ImageFormat.Png);

        var imageBytes = memoryStream.ToArray();
        var base64String = Convert.ToBase64String(imageBytes);

        return base64String;
    }

    private string ReplaceAt(string data, int index, string replacement)
    {
        return $"{data.Substring(0, index)}{replacement}{data.Substring(index + replacement.Length)}";
    }

    private string NumberToBinary(byte number)
    {
        return Convert.ToString(number, 2).PadLeft(StegomasterConfiguration.BITS_COUNT, '0');
    }

    private string GetBinaryData(List<string> data)
    {
        var result = new StringBuilder(string.Empty);

        for (var i = 0; i < data.Count; i++)
        {
            var delimeter = i != data.Count - 1 ? StegomasterConfiguration.DATA_DELIMETER : StegomasterConfiguration.DATA_ENDING;
            result.Append($"{StringToBinary(data[i])}{delimeter}");
        }

        return result.ToString();
    }

    private string StringToBinary(string data)
    {
        var result = new StringBuilder(string.Empty);

        for (var i = 0; i < data.Length; i++)
        {
            result.Append(Convert.ToString(data[i], 2).PadLeft(StegomasterConfiguration.BITS_COUNT, '0'));
        }

        return result.ToString();
    }

    private Bitmap GetBitmapContainer(string imagePath)
    {
        using var fileStream = File.Open(imagePath, FileMode.Open);
        var image = Image.FromStream(fileStream);

        return new Bitmap(image);
    }

    private Bitmap Base64ToBitmap(string base64String)
    {
        if (base64String.Contains(StegomasterConfiguration.BASE64_IMAGE_PREFIX, StringComparison.OrdinalIgnoreCase))
        {
            base64String = base64String[StegomasterConfiguration.BASE64_IMAGE_PREFIX.Length..];
        }

        var bytes = Convert.FromBase64String(base64String);
        using var memoryStream = new MemoryStream(bytes);

        return (Bitmap)Image.FromStream(memoryStream);
    }

    private string GetBinaryDataFromBitmap(Bitmap image)
    {
        var data = new StringBuilder(string.Empty);
        var skipFirstBitsCounter = 0;

        for (var i = 0; i < image.Height; i++)
        {
            var isFirstIteration = true;
            for (var j = skipFirstBitsCounter;;)
            {
                j += isFirstIteration ? 0 : StegomasterConfiguration.SKIP_BITS_COUNT;
                if (j >= image.Width)
                {
                    skipFirstBitsCounter = j - image.Width;
                    break;
                }

                var pixel = image.GetPixel(j, i);

                if (StegomasterConfiguration.ColorsDataHiding.Contains(RGBA.Red))
                {
                    var redBinary = Convert.ToString(pixel.R, 2).PadLeft(StegomasterConfiguration.BITS_COUNT, '0');
                    data.Append(redBinary[^StegomasterConfiguration.LSB_COUNT..]);

                    if (IsDataEnd())
                    {
                        return data.ToString();
                    }
                }

                if (StegomasterConfiguration.ColorsDataHiding.Contains(RGBA.Green))
                {
                    var greenBinary = Convert.ToString(pixel.G, 2).PadLeft(StegomasterConfiguration.BITS_COUNT, '0');
                    data.Append(greenBinary[^StegomasterConfiguration.LSB_COUNT..]);

                    if (IsDataEnd())
                    {
                        return data.ToString();
                    }
                }

                if (StegomasterConfiguration.ColorsDataHiding.Contains(RGBA.Blue))
                {
                    var blueBinary = Convert.ToString(pixel.B, 2).PadLeft(StegomasterConfiguration.BITS_COUNT, '0');
                    data.Append(blueBinary[^StegomasterConfiguration.LSB_COUNT..]);

                    if (IsDataEnd())
                    {
                        return data.ToString();
                    }
                }

                if (StegomasterConfiguration.ColorsDataHiding.Contains(RGBA.Alpha))
                {
                    var alphaBinary = Convert.ToString(pixel.A, 2).PadLeft(StegomasterConfiguration.BITS_COUNT, '0');
                    data.Append(alphaBinary[^StegomasterConfiguration.LSB_COUNT..]);

                    if (IsDataEnd())
                    {
                        return data.ToString();
                    }
                }

                if (isFirstIteration)
                {
                    isFirstIteration = false;
                }
            }
        }

        return data.ToString();
    
        bool IsDataEnd()
        {
            return data.Length >= StegomasterConfiguration.DATA_ENDING.Length &&
                data.ToString(data.Length - StegomasterConfiguration.DATA_ENDING.Length, StegomasterConfiguration.DATA_ENDING.Length).Equals(StegomasterConfiguration.DATA_ENDING);
        }
    }

    private List<string> GetDataFromBinaryString(string binary)
    {
        var data = new List<string>();
        var list = new List<byte>();

        for (var i = 0; i < binary.Length; i += StegomasterConfiguration.BITS_COUNT)
        {
            var byteData = binary.Substring(i, StegomasterConfiguration.BITS_COUNT);

            if (byteData.Equals(StegomasterConfiguration.DATA_DELIMETER))
            {
                data.Add(Encoding.ASCII.GetString(list.ToArray()));
                list.Clear();
                continue;
            }
            else if (byteData.Equals(StegomasterConfiguration.DATA_ENDING))
            {
                data.Add(Encoding.ASCII.GetString(list.ToArray()));
                return data;
            }

            list.Add(Convert.ToByte(byteData, 2));
        }

        return data;
    }
}
#pragma warning restore CA1416