namespace Task2_3.Models;

internal class Note
{
    public Note()
    {
        Filename = $"{Path.GetRandomFileName()}.notes.txt";
        Date = DateTime.Now;
        Text = "";
    }

    public string Filename { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }

    public void Save()
    {
        var filename = Path.Combine(FileSystem.AppDataDirectory, Filename);
        File.WriteAllText(filename, Text);
    }

    public void Delete()
    {
        File.Delete(Path.Combine(FileSystem.AppDataDirectory, Filename));
    }

    public static Note Load(string filename)
    {
        filename = Path.Combine(FileSystem.AppDataDirectory, filename);

        if (!File.Exists(filename))
            throw new FileNotFoundException("Unable to find file on local storage.", filename);

        return
            new Note
            {
                Filename = Path.GetFileName(filename),
                Text = File.ReadAllText(filename),
                Date = File.GetLastWriteTime(filename)
            };
    }

    public static IEnumerable<Note> LoadAll()
    {
        // Get the folder where the notes are stored.
        var appDataPath = FileSystem.AppDataDirectory;

        // Use Linq extensions to load the *.notes.txt files.
        return Directory

            // Select the file names from the directory
            .EnumerateFiles(appDataPath, "*.notes.txt")

            // Each file name is used to load a note
            .Select(filename => Load(Path.GetFileName(filename)))

            // With the final collection of notes, order them by date
            .OrderByDescending(note => note.Date);
    }
}