public class Program
{
    public static void Main()
    {
        try
        {
            var fileHandler = new FileHandler();
            var calculator = new DerangementCalculator();

            int input = fileHandler.ReadInput();
            long result = calculator.Calculate(input);
            fileHandler.WriteOutput(result);

            Console.WriteLine($"Calculation complete. Result: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}