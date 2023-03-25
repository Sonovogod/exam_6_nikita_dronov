using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web;
using FluentValidation.Results;
using HttpServer.dto;
using HttpServer.providers;
using HttpServer.services;
using HttpServer.validators;
using HttpServer.viewModels;

namespace HttpServer.controllers;

public class AddTaskController : BaseController
{
    private readonly HtmlBuilderService<ResponseDto<List<TaskViewModel>>> _htmlBuilder;
    private readonly CreateTaskValidator _createTaskValidator;
    private readonly TaskService _taskService;
    public AddTaskController(HtmlBuilderService<ResponseDto<List<TaskViewModel>>> htmlBuilder, CreateTaskValidator createTaskValidator, TaskService taskService)
    {
        _htmlBuilder = htmlBuilder;
        _createTaskValidator = createTaskValidator;
        _taskService = taskService;
    }

    public override byte[] TryToProcessRequest(HttpListenerContext context, string fileName)
    {
        string filePath = Path.Combine(RootDirectoryProvider.GetRootDirectoryPath(), $"views/{fileName}");
        
        if (fileName.Contains("index.html") && context.Request.HttpMethod.Equals("GET"))
        {
            ResponseDto<List<TaskViewModel>> response = _taskService.GetAll();
            string content = _htmlBuilder.BuildHtml(fileName, filePath, response);
            if (response.Result.Any())
                return Encoding.UTF8.GetBytes(content);
            
            ResponseDto<List<TaskViewModel>> responseDto = new ResponseDto<List<TaskViewModel>> {Result = new List<TaskViewModel>()};
            var htmlString = _htmlBuilder.BuildHtml(fileName,filePath, responseDto);
            
            return Encoding.UTF8.GetBytes(htmlString);
        }

        if (fileName.Contains("index.html") && context.Request.HttpMethod.Equals("POST"))
        {
            if (context.Request.HasEntityBody)
            {
                using StreamReader streamReader = new StreamReader(context.Request.InputStream, Encoding.UTF8);
                var body = streamReader.ReadToEnd();
                NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(body);
                TaskViewModel employeeViewModel = new TaskViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = nameValueCollection["Title"],
                    Executor = nameValueCollection["Executor"],
                    Status = "new",
                    Description = nameValueCollection["Description"],
                    DateOfCreate = DateTime.Now,
                    DateOfDone = null
                };
                
                string content = Post(employeeViewModel, fileName, filePath);
                return Encoding.UTF8.GetBytes(content);
            }
        }
        
        if (Controller is not null) 
            return Controller.TryToProcessRequest(context, fileName);
        
        return Array.Empty<byte>();
    }

    private string Post(TaskViewModel model, string fileName, string filePath)
    {
        ValidationResult? validationResult = _createTaskValidator.Validate(model);

        if (validationResult.IsValid)
        {
            _taskService.Create(model);
            ResponseDto<List<TaskViewModel>> response = _taskService.GetAll();
            
            return _htmlBuilder.BuildHtml(fileName,filePath, response);
        }

        ResponseDto<List<TaskViewModel>> viewModel = new ResponseDto<List<TaskViewModel>>()
        {
            Result = new List<TaskViewModel>(),
            Errors = validationResult.Errors
        };

        return _htmlBuilder.BuildHtml("index.html", 
            $"{RootDirectoryProvider.GetRootDirectoryPath()}/views/index.html", viewModel);
    }
}