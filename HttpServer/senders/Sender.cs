using System.Net;
using HttpServer.resolvers;

namespace HttpServer.senders;

public class Sender
{
    private readonly HttpListenerContext _context;

    public Sender(HttpListenerContext context)
    {
        _context = context;
    }

    public void Send(byte[] htmlBytes, string filename)
    {
        try
        {
            using Stream stream = new MemoryStream(htmlBytes);
            var contentTypeResolver = new ContentTypeResolver();

            _context.Response.ContentType = contentTypeResolver.ResolveContentType(filename);
            _context.Response.ContentLength64 = stream.Length;

            byte[] buffer = new byte[64 * 1024];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                _context.Response.OutputStream.Write(buffer, 0, bytesRead);
            }
        }
        catch (Exception ex)
        {
            _context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            _context.Response.StatusDescription = "Internal Server Error";
            _context.Response.OutputStream.Close();
            Console.WriteLine(ex.Message);
        }
    }
}