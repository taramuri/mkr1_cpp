public class FileHandler
{
    private readonly string projectDirectory;
    private const string INPUT_FILENAME = "INPUT.txt";
    private const string OUTPUT_FILENAME = "OUTPUT.txt";
    private const string PROJECT_FOLDER = "mkr1";

    public FileHandler()
    {
        projectDirectory = FindProjectDirectory();
    }

    public int ReadInput()
    {
        string inputPath = Path.Combine(projectDirectory, PROJECT_FOLDER, INPUT_FILENAME);
        if (!File.Exists(inputPath))
        {
            throw new FileNotFoundException($"Input file not found at: {inputPath}");
        }
        string input = File.ReadAllText(inputPath);
        return int.Parse(input);
    }

    public void WriteOutput(long result)
    {
        string outputPath = Path.Combine(projectDirectory, PROJECT_FOLDER, OUTPUT_FILENAME);
        File.WriteAllText(outputPath, result.ToString());
        Console.WriteLine($"Output written to: {outputPath}");
    }

    private string FindProjectDirectory()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        while (!Directory.Exists(Path.Combine(currentDirectory, PROJECT_FOLDER)))
        {
            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
            if (currentDirectory == null)
            {
                throw new DirectoryNotFoundException($"Could not find the '{PROJECT_FOLDER}' directory.");
            }
        }
        return currentDirectory;
    }
}