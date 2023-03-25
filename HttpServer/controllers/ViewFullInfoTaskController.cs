using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web;
using FluentValidation.Results;
using HttpServer.dto;
using HttpServer.providers;
using HttpServer.services;
using HttpServer.viewModels;

namespace HttpServer.controllers;

public class ViewFullInfoTaskController : BaseController
{
    private readonly HtmlBuilderService<ResponseDto<TaskViewModel>> _htmlBuilder;
    private readonly TaskService _taskService;
    
    public ViewFullInfoTaskController (HtmlBuilderService<ResponseDto<TaskViewModel>> htmlBuilder, TaskService taskService)
    {
        _htmlBuilder = htmlBuilder;
        _taskService = taskService;
    }

    public override byte[] TryToProcessRequest(HttpListenerContext context, string fileName)
    {
        string filePath = Path.Combine(RootDirectoryProvider.GetRootDirectoryPath(), $"views/{fileName}");
        
        if (fileName.Contains("task.html") && context.Request.HttpMethod.Equals("GET"))
        {
            var header = context.Request.QueryString;
            var tasks = _taskService.GetAll();
            TaskViewModel? task = tasks.Result.Find(x => x.Id == header["Id"]);
            ResponseDto<TaskViewModel> foundedTask = new ResponseDto<TaskViewModel>() { Result = task};
            var query = context.Request.QueryString;
            string content = _htmlBuilder.BuildHtml(fileName, filePath, foundedTask);

            return Encoding.UTF8.GetBytes(content);
        }
        
        
        if (Controller is not null) 
            return Controller.TryToProcessRequest(context, fileName);
        
        return Array.Empty<byte>();
    }
    
}