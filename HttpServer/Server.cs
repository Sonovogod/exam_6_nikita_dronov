using System.Net;
using HttpServer.controllers;
using HttpServer.providers;
using HttpServer.senders;

namespace HttpServer;

public class Server
{
    private readonly Thread _serverThread;
    private readonly HttpListener _listener;
    private HttpListenerContext _context = null!;
    private readonly BaseController _baseController;

    public Server(
        HttpListener listener,
        BaseController baseController)
    {
        _listener = listener;
        _baseController = baseController;
        _serverThread = new Thread(Listen);
    }

    public void Start()
    {
        _serverThread.Start();
        Console.WriteLine($"Сервер запущен на порту: {AddressConnectionProvider.Port}");
        Console.WriteLine($"Файлы сайта лежат в папке: {RootDirectoryProvider.GetRootDirectoryPath()}");
    }
    
    private void Listen()
    {
        if (AddressConnectionProvider.Address != null)
        {
            _listener.Prefixes.Add(AddressConnectionProvider.Address);
            Console.WriteLine(AddressConnectionProvider.Address + "index.html");
        }

        _listener.Start();
        while (true)
        {
            try
            {
                _context = _listener.GetContext();
                string endpoint = _context.Request.Url.AbsolutePath[1..];
                Console.WriteLine(endpoint);
                byte[] result = _baseController.TryToProcessRequest(_context, endpoint);
                var handler = new Sender(_context);
                handler.Send(result, endpoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                _context.Response.StatusDescription = "Not Found";
            }
            finally
            {
                _context.Response.OutputStream.Close();
            }
        }
    }
}