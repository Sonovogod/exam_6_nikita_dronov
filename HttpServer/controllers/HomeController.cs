using System.Net;
using System.Text;
using HttpServer.dto;
using HttpServer.providers;
using HttpServer.services;
using HttpServer.viewModels;

namespace HttpServer.controllers;

public class HomeController : BaseController
{
    private readonly TaskService _taskService;
    private readonly HtmlBuilderService<ResponseDto<List<TaskViewModel>>> _htmlBuilder;

    public HomeController(
        TaskService taskService, 
        HtmlBuilderService<ResponseDto<List<TaskViewModel>>> htmlBuilder)
    {
        _taskService = taskService;
        _htmlBuilder = htmlBuilder;
    }

    public override byte[] TryToProcessRequest(HttpListenerContext context, string fileName)
    {
        if (fileName.Contains("index.html"))
        {
            var htmlFilePath = $"{RootDirectoryProvider.GetRootDirectoryPath()}/views/{fileName}";
            ResponseDto<List<TaskViewModel>> response = _taskService.GetAll();
            string content = _htmlBuilder.BuildHtml(fileName, htmlFilePath, response);

            return Encoding.UTF8.GetBytes(content);
        }
        
        if (Controller is not null) 
            return Controller.TryToProcessRequest(context, fileName);
        
        return Array.Empty<byte>();
    }
}