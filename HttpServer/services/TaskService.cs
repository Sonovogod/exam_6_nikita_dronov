﻿using System.Text.Json;
using HttpServer.dto;
using HttpServer.managers;
using HttpServer.providers;
using HttpServer.viewModels;
using Task = HttpServer.models.Task;

namespace HttpServer.services;

public class TaskService
{
    private readonly FileManager _fileManager;
    private string _pathToJson;

    public TaskService(FileManager fileManager)
    {
        _fileManager = fileManager;
        _pathToJson = $"{RootDirectoryProvider.GetRootDirectoryPath()}/data/tasks.json";
    }

    public ResponseDto<List<TaskViewModel>> GetAll()
    {
        var jsonString = _fileManager.GetContent(_pathToJson);
        List<Task>? tasks = JsonSerializer.Deserialize<List<Task>>(jsonString);
        
        ResponseDto<List<TaskViewModel>> responseDto = new ResponseDto<List<TaskViewModel>>()
        {
            Result = tasks is null ? new List<TaskViewModel>() :
                tasks.Select(x => new TaskViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Executor = x.Executor,
                    Status = x.Status,
                    Description = x.Description,
                    DateOfCreate = x.DateOfCreate,
                    DateOfDone = x.DateOfDone
                }).ToList()
        };

        return responseDto;
    }

    public void Create(TaskViewModel taskViewModel)
    {
        var jsonString = _fileManager.GetContent(_pathToJson);
        List<Task>? tasks = JsonSerializer.Deserialize<List<Task>>(jsonString);
        Task newTask = new Task()
        {
            Id = taskViewModel.Id,
            Title = taskViewModel.Title,
            Executor = taskViewModel.Executor,
            Status = taskViewModel.Status,
            Description = taskViewModel.Description,
            DateOfCreate = taskViewModel.DateOfCreate,
            DateOfDone = taskViewModel.DateOfDone
        };

        tasks?.Add(newTask);
        jsonString = JsonSerializer.Serialize(tasks);
        _fileManager.SaveData(jsonString, _pathToJson);
    }
    
    public void Save(TaskViewModel taskViewModel)
    {
        var jsonString = _fileManager.GetContent(_pathToJson);
        List<Task>? tasks = JsonSerializer.Deserialize<List<Task>>(jsonString);

        var task = tasks.Find(x => x.Id == taskViewModel.Id);
        task.Description = taskViewModel.Description;
        task.Executor = taskViewModel.Executor;
        task.Status = taskViewModel.Status;
        task.Title = taskViewModel.Title;
        task.DateOfDone = taskViewModel.DateOfDone;
    
        jsonString = JsonSerializer.Serialize(tasks);
        _fileManager.SaveData(jsonString, _pathToJson);
    }
}