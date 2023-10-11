namespace TheEsportsReplayManager.Data;

public static class TERMConstants
{
    public const string DatabaseFilename = "TheEsportsReplayManager.db3";

    public static string DatabasePath => Path.Combine(FileSystem.Current.AppDataDirectory, DatabaseFilename);

    public const SQLite.SQLiteOpenFlags SQLiteFlags =
        SQLite.SQLiteOpenFlags.ReadWrite |
        SQLite.SQLiteOpenFlags.Create;
}
