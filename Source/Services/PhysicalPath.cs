using Microsoft.Web.Administration;

interface IPhysicalPath
{
    string GetPhysicalPath(Site site);
    string GetPhysicalPathBack(string path, int level = 1);
}

class PhysicalPath : IPhysicalPath
{
    public string GetPhysicalPath(Site site)
    {
        string path = site.Applications[Settings.Root].VirtualDirectories[Settings.Root].PhysicalPath;
        return Environment.ExpandEnvironmentVariables(path);
    }

    public string GetPhysicalPathBack(string path, int level = Settings.LevelMin)
    {
        var strLevel = GetStrBack(path, level);
        string strBack = Path.Combine(strLevel.ToArray());
        return Path.GetFullPath(Path.Combine(path, strBack));
    }

    private IEnumerable<string> GetStrBack(string path, int level)
    {
        yield return path;
        if (level >= Settings.LevelMin)
        {
            for (int i = 0; i < level; i++)
            {
                yield return Settings.BackFolder;
            }
        }
    }
}
