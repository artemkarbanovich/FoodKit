namespace Stegomaster;

public interface IStegomasterService
{
    List<string> ProcessRequest(string base64String);
    string ProcessResponse(List<string> data);
    void LogInfo(string header, params string[] info);
    void LogError(string error);
}
