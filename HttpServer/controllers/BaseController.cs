using System.Net;

namespace HttpServer.controllers;

public abstract class BaseController
{
    public BaseController? Controller { get; set; }
    public abstract byte[] TryToProcessRequest(HttpListenerContext context, string fileName);
}