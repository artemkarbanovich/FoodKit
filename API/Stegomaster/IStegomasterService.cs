namespace Stegomaster;

public interface IStegomasterService
{
    List<string> ProcessRequest(string base64String);
    string ProcessResponse(List<string> data);
}
