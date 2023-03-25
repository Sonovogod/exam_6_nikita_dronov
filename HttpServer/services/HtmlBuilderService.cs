using HttpServer.providers;
using RazorEngine;
using RazorEngine.Templating;

namespace HttpServer.services;

public class HtmlBuilderService<T>
{
    public string BuildHtml(string filename, string filePath, T entity)
    {
        Console.WriteLine(filePath);
        string layoutPath = Path.Combine(RootDirectoryProvider.GetRootDirectoryPath(), "views/layout.html");
        IRazorEngineService? razorService = Engine.Razor;
        
        if (!razorService.IsTemplateCached("layout", typeof(T))) // Проверяем наличие базового шаблона в кэше
            razorService.AddTemplate("layout", File.ReadAllText(layoutPath)); //Добавляем его если отсутствует

        if (!razorService.IsTemplateCached(filename, typeof(T)))//Находим шаблон страницы который будет вложен в базовый
        {
            razorService.AddTemplate(filename, File.ReadAllText(filePath));
            razorService.Compile(filename, typeof(T));
        }
        var key = razorService.GetKey(filename);

        return razorService.Run(key.Name, typeof(T), entity);
    }
}