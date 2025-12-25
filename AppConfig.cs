public static class AppConfig
{
    private static readonly string _appDataPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RailwayApp");
    public static string AppDataPath => _appDataPath;

    public static string DatabasePath => Path.Combine(_appDataPath, "railway.db");
    public static bool UseDatabase { get; set; } = false;

    public static void EnsureDatabaseDirectoryExists()
    {
        if (!Directory.Exists(_appDataPath))
        {
            Directory.CreateDirectory(_appDataPath);
        }
    }
}