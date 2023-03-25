namespace HttpServer.providers;

public static class RootDirectoryProvider
{
    public static string GetRootDirectoryPath()
    {
        string currentDir = Directory.GetCurrentDirectory();
        return currentDir + "/www";
    }
}