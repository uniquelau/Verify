static class FileCache
{
    static  ConcurrentDictionary<string, string> cache = new();

    public static Task<string> Get(string path)
    {
        if (cache.TryGetValue(path, out var value))
        {
            Debug.WriteLine("hit");
            return Task.FromResult(value);
        }

        Debug.WriteLine("mis");
        cache[path] = "";
        return Read(path);
    }

    public static async Task Init(string directory)
    {
        foreach (var file in Directory.EnumerateFiles(directory, "*.verified.txt", SearchOption.AllDirectories))
        {
            if (!cache.ContainsKey(file))
            {
                cache.TryAdd(file, await Read(file));
            }
        }
    }

    static async Task<string> Read(string file)
    {
        var verifiedBuilder = await FileHelpers.ReadStringBuilder(file);
        return verifiedBuilder.ToString();
    }
}