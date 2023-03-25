namespace HttpServer.models;

public class Task
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Executor { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public DateTime DateOfCreate { get; set; }
    public DateTime? DateOfDone { get; set; }
}