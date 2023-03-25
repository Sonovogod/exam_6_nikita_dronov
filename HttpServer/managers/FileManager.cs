namespace HttpServer.managers;

public class FileManager
{
    public string GetContent( string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException($"Файл по пути {filePath} не найден");
        Console.WriteLine(filePath);

        return File.ReadAllText(filePath);
    }
    
    public byte[] GetImage(string filePath)
    {
        using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[1024 * 64];
        var _ = fileStream.Read(bytes);
        return bytes;
    }

    public void SaveData(string jsonString, string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException($"Файл по пути {filePath} не найден");
        Console.WriteLine(filePath);
        
        File.WriteAllText(filePath, jsonString);
    }
}