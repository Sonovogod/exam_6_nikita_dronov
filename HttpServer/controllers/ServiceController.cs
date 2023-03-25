using System.Net;
using System.Text;
using HttpServer.managers;
using HttpServer.providers;

namespace HttpServer.controllers;

public class ServiceController : BaseController 
{
    private readonly FileManager _fileManager;

    public ServiceController(FileManager fileManager)
    {
        _fileManager = fileManager;
    }

    public override byte[] TryToProcessRequest(HttpListenerContext context, string fileName)
    {
        if (fileName.Contains(".css"))
        {
            string filePath = Path.Combine(RootDirectoryProvider.GetRootDirectoryPath(), fileName);
            var content = _fileManager.GetContent(filePath);

            return Encoding.UTF8.GetBytes(content);
        }
        
        if (Controller is not null) 
            return Controller.TryToProcessRequest(context, fileName);
        
        return Array.Empty<byte>();
    }
}