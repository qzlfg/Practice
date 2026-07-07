using System.IO;

[DisplayName("Класс, который показывает сколько весит переданная директория")]
[Version(2, 0)]
public class DirectorySizeCommand: ICommand
{
    private string _path;

    public DirectorySizeCommand(string directory)
    {
        _path = directory;
    }

    public void Execute()
    {
        if (!Directory.Exists(_path)) throw new DirectoryNotFoundException("Такой директории не существует");

        DirectoryInfo dirInfo = new DirectoryInfo(_path);

        FileInfo[] files = dirInfo.GetFiles("*", SearchOption.AllDirectories);

        long sum = 0;

        foreach (FileInfo file in files)
        {
            sum += file.Length;
        }

        Console.WriteLine($"Размер каталога составляет: {sum} байт");
            
    }
}


[DisplayName("Класс который показывает все найденные файлы по какому-то паттерну в каком-то конкретной директории")]
[Version(1, 1)]
public class FindFilesCommand: ICommand
{
    private string _path;
    private string _pattern;

    [DisplayName("Конструктор")]
    public FindFilesCommand(string directory, string pattern) 
    {
        _path = directory;
        _pattern = pattern;
    }

    [DisplayName("Метод с выполнением логики класса")]
    public void Execute()
    {
        if (!Directory.Exists(_path)) throw new DirectoryNotFoundException("Такой директории не существует");

        string[] TxtFiles = Directory.GetFiles(_path, _pattern, SearchOption.AllDirectories);

        if (TxtFiles.Length == 0) Console.WriteLine("Файлы не найдены");

        else Console.WriteLine(string.Join("\n", TxtFiles));
    }
}