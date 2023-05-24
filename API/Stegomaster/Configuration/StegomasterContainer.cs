namespace Stegomaster.Configuration;

public class StegomasterContainer
{
    public StegomasterContainer(string url, int width, int height) => (Url, Width, Height) = (url, width, height);

    public string Url { get; private init; }
    public int Width { get; private init;  }
    public int Height { get; private init; }
}
