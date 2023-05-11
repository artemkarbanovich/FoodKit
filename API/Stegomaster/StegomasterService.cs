using Stegomaster.Configuration;
using System.Drawing;
using System.Text;

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

        for (var i = 0; i < image.Height; i++)
        {
            for (var j = 0; j < image.Width; j++)
            {
                var pixel = image.GetPixel(j, i);

                if (StegomasterConfiguration.ColorsDataHiding.Contains(RGBA.Red))
                {
                    var redBinary = Convert.ToString(pixel.R, 2).PadLeft(StegomasterConfiguration.BITS_COUNT, '0');
                    data.Append(redBinary[^StegomasterConfiguration.LSB_COUNT..]);
                }

                if (StegomasterConfiguration.ColorsDataHiding.Contains(RGBA.Green))
                {
                    var greenBinary = Convert.ToString(pixel.G, 2).PadLeft(StegomasterConfiguration.BITS_COUNT, '0');
                    data.Append(greenBinary[^StegomasterConfiguration.LSB_COUNT..]);
                }

                if (StegomasterConfiguration.ColorsDataHiding.Contains(RGBA.Blue))
                {
                    var blueBinary = Convert.ToString(pixel.B, 2).PadLeft(StegomasterConfiguration.BITS_COUNT, '0');
                    data.Append(blueBinary[^StegomasterConfiguration.LSB_COUNT..]);
                }

                if (StegomasterConfiguration.ColorsDataHiding.Contains(RGBA.Alpha))
                {
                    var alphaBinary = Convert.ToString(pixel.A, 2).PadLeft(StegomasterConfiguration.BITS_COUNT, '0');
                    data.Append(alphaBinary[^StegomasterConfiguration.LSB_COUNT..]);
                }

                if (data.Length >= StegomasterConfiguration.DATA_ENDING.Length && 
                    data.ToString(data.Length - StegomasterConfiguration.DATA_ENDING.Length, StegomasterConfiguration.DATA_ENDING.Length).Equals(StegomasterConfiguration.DATA_ENDING))
                {
                    return data.ToString();
                }
            }
        }

        return data.ToString();
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