
using System.IO;
using System.Xml.Serialization;

public class FileWatcher
{
    public event Action<MultiplayerCarnageReport>? ReportParsed;
    private string? _latestFilePath;
    private FileSystemWatcher? _watcher;

    public void WatchFolder()
    {
        string watchFolder = Environment.ExpandEnvironmentVariables(@"%userprofile%\AppData\LocalLow\MCC\Temporary\");

        if (!Directory.Exists(watchFolder))
        {
            Console.WriteLine($"Folder not found: {watchFolder}");
            return;
        }

        var latestFile = GetLatestXmlFile(watchFolder);

        if (latestFile != null)
        {
            _latestFilePath = latestFile.FullName;
            MultiplayerCarnageReport report = DeserializeReport(_latestFilePath);
            ReportParsed?.Invoke(report);
        }

        _watcher = new FileSystemWatcher
        {
            Path = watchFolder,
            Filter = "*.xml",
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
            EnableRaisingEvents = true
        };

        _watcher.Created += (s, e) => OnFileEvent(e.FullPath);
        _watcher.Changed += (s, e) => OnFileEvent(e.FullPath);
        _watcher.Renamed += (s, e) => OnFileEvent(e.FullPath);
        _watcher.Error += Watcher_error;
    }

    private void Watcher_error(object sender, ErrorEventArgs e)
    {
        throw e.GetException();
    }

    private void OnFileEvent(string filePath)
    {
        try
        {
            MultiplayerCarnageReport report = DeserializeReport(filePath);
            ReportParsed?.Invoke(report);
        }
        catch { }
    }

    private static FileInfo? GetLatestXmlFile(string folderPath)
    {
        var directory = new DirectoryInfo(folderPath);
        var latestFile = directory.GetFiles("mpcarnagereport*.xml")
            .OrderByDescending(f => f.LastWriteTime)
            .FirstOrDefault();

        return latestFile;
    }

    private static MultiplayerCarnageReport DeserializeReport(string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(MultiplayerCarnageReport));
        using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return (MultiplayerCarnageReport)serializer.Deserialize(fs);
    }
}

