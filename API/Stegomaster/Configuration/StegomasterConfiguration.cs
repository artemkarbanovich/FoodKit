﻿namespace Stegomaster.Configuration;

public class StegomasterConfiguration
{
    public const string CONTAINER_URL = @"..\Stegomaster\Assets\container_200x300.jpg";
    public const string BASE64_IMAGE_PREFIX = "data:image/png;base64,";
    public const string DATA_DELIMETER = "11111111";
    public const string DATA_ENDING = "11111110";
    public const int BITS_COUNT = 8;
    public const int LSB_COUNT = 2; // 2 or 4 or 8

    public static IEnumerable<RGBA> ColorsDataHiding => new List<RGBA>() { RGBA.Red, RGBA.Green, RGBA.Blue };
}