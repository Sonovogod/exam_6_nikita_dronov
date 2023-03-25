using System.Net;
using System.Text;
using HttpServer;
using HttpServer.controllers;
using HttpServer.dto;
using HttpServer.managers;
using HttpServer.providers;
using HttpServer.services;
using HttpServer.validators;
using HttpServer.viewModels;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var fileManager = new FileManager();
var listener = new HttpListener();
AddressConnectionProvider.RootDomain = "http://localhost:";
AddressConnectionProvider.Port = 8000;
AddressConnectionProvider.Address = AddressConnectionProvider.RootDomain + AddressConnectionProvider.Port + "/";

var commonHtmlBuilder = new HtmlBuilderService<ResponseDto<IndexViewModel>>();
var tasksHtmlBuilder = new HtmlBuilderService<ResponseDto<List<TaskViewModel>>>();
var taskHtmlBuilder = new HtmlBuilderService<ResponseDto<TaskViewModel>>();
var taskService = new TaskService(fileManager);

var createTaskValidator = new CreateTaskValidator();


var imageController = new ImageController(fileManager);
var homeController = new HomeController(taskService, tasksHtmlBuilder);
var serviceController = new ServiceController(fileManager);
var addTaskController = new AddTaskController(tasksHtmlBuilder, createTaskValidator, taskService);
var viewFullInfoTaskController = new ViewFullInfoTaskController(taskHtmlBuilder, taskService);

addTaskController.Controller = homeController;
homeController.Controller = imageController;
imageController.Controller = serviceController;
serviceController.Controller = viewFullInfoTaskController;

Server server = new Server(listener, addTaskController);

server.Start();
