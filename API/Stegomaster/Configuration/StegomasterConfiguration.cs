namespace Stegomaster.Configuration;

public class StegomasterConfiguration
{
    public const bool USE_INFO = true;
    public const int INFO_DELAY = 300;
    public static readonly bool USE_DEFAULT_CONTAINER = false;
    public const string BASE64_IMAGE_PREFIX = "data:image/png;base64,";
    public const string DATA_DELIMETER = "11111111";
    public const string DATA_ENDING = "11111110";
    public const int BITS_COUNT = 8;
    public const int SKIP_BITS_COUNT = 37 + 1; // should be more or equals 1
    public const int LSB_COUNT = 2; // 2 or 4 or 8

    public static IEnumerable<RGBA> ColorsDataHiding => new List<RGBA>() { RGBA.Green };

    public static List<StegomasterContainer> AvailableContainers => new()
    {
        new StegomasterContainer(@"..\Stegomaster\Assets\container_10x10.jpg", 10, 10),
        new StegomasterContainer(@"..\Stegomaster\Assets\container_100x100.jpg", 100, 100),
        new StegomasterContainer(@"..\Stegomaster\Assets\container_365x261.jpg", 365, 261),
        new StegomasterContainer(@"..\Stegomaster\Assets\container_550x680.jpg", 550, 680),
        new StegomasterContainer(@"..\Stegomaster\Assets\container_1000x664.jpg", 1000, 664),
        new StegomasterContainer(@"..\Stegomaster\Assets\container_1560x1440.jpg", 1560, 1440),
        new StegomasterContainer(@"..\Stegomaster\Assets\container_1920x1080.jpg", 1920, 1080),
        new StegomasterContainer(@"..\Stegomaster\Assets\container_3600x2085.jpg", 3600, 2085),
    };

    public static StegomasterContainer DEFAULT_CONTAINER => AvailableContainers[6];
}
