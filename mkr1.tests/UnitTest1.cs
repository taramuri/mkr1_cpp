using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace mkr1.tests
{
    public class Tests
    {
        public class DerangementCalculatorTests
        {
            private readonly ITestOutputHelper output;
            private readonly DerangementCalculator calculator;

            public DerangementCalculatorTests(ITestOutputHelper output)
            {
                this.output = output;
                this.calculator = new DerangementCalculator();
            }

            public static IEnumerable<object[]> DerangementTestData()
            {
                yield return new object[] { 0, 1 };
                yield return new object[] { 1, 0 };
                yield return new object[] { 2, 1 };
                yield return new object[] { 5, 44 };
                yield return new object[] { 20, 895014631192902121 };
            }

            [Xunit.Theory]
            [MemberData(nameof(DerangementTestData))]
            public void Calculate_ValidInput_ReturnsExpectedResult(int input, long expected)
            {
                // Act
                long result = calculator.Calculate(input);

                // Assert
                Xunit.Assert.Equal(expected, result);
                output.WriteLine($"Input: {input}, Result: {result}");
            }

            [Fact]
            public void Calculate_NegativeInput_ThrowsArgumentException()
            {
                // Arrange
                int input = -1;

                // Act & Assert
                Xunit.Assert.Throws<ArgumentException>(() => calculator.Calculate(input));
            }
        }

        public class FileHandlerTests
        {
            private readonly ITestOutputHelper output;
            private string tempDir;
            private string projectDir;
            private FileHandler fileHandler;
            private string originalDirectory;

            public FileHandlerTests(ITestOutputHelper output)
            {
                this.output = output;
                SetupTestEnvironment();
            }

            private void SetupTestEnvironment()
            {
                originalDirectory = Directory.GetCurrentDirectory();
                tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                projectDir = Path.Combine(tempDir, "cpp_mkr1");
                Directory.CreateDirectory(projectDir);
            }

            [Fact]
            public void ReadInput_ValidFile_ReturnsExpectedNumber()
            {
                try
                {
                    // Arrange
                    string inputPath = Path.Combine(projectDir, "INPUT.txt");
                    File.WriteAllText(inputPath, "5");
                    Directory.SetCurrentDirectory(tempDir);
                    fileHandler = new FileHandler();

                    // Act
                    int result = fileHandler.ReadInput();

                    // Assert
                    Xunit.Assert.Equal(5, result);
                }
                finally
                {
                    CleanupTestEnvironment();
                }
            }

            [Fact]
            public void WriteOutput_ValidResult_CreatesOutputFile()
            {
                try
                {
                    // Arrange
                    Directory.SetCurrentDirectory(tempDir);
                    fileHandler = new FileHandler();
                    long resultToWrite = 44;

                    // Act
                    fileHandler.WriteOutput(resultToWrite);

                    // Assert
                    string outputPath = Path.Combine(projectDir, "OUTPUT.txt");
                    Xunit.Assert.True(File.Exists(outputPath));
                    string writtenContent = File.ReadAllText(outputPath);
                    Xunit.Assert.Equal("44", writtenContent);
                }
                finally
                {
                    CleanupTestEnvironment();
                }
            }

            [Fact]
            public void ReadInput_FileNotFound_ThrowsFileNotFoundException()
            {
                try
                {
                    // Arrange
                    Directory.SetCurrentDirectory(tempDir);
                    fileHandler = new FileHandler();

                    // Act & Assert
                    Xunit.Assert.Throws<FileNotFoundException>(() => fileHandler.ReadInput());
                }
                finally
                {
                    CleanupTestEnvironment();
                }
            }

            private void CleanupTestEnvironment()
            {
                Directory.SetCurrentDirectory(originalDirectory);
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
                output.WriteLine($"Cleaned up test environment: {tempDir}");
            }
        }

        public class IntegrationTests
        {
            private readonly ITestOutputHelper output;
            private string tempDir;
            private string projectDir;
            private string originalDirectory;

            public IntegrationTests(ITestOutputHelper output)
            {
                this.output = output;
                SetupTestEnvironment();
            }

            private void SetupTestEnvironment()
            {
                originalDirectory = Directory.GetCurrentDirectory();
                tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                projectDir = Path.Combine(tempDir, "cpp_mkr1");
                Directory.CreateDirectory(projectDir);
            }

            [Fact]
            public void CompleteWorkflow_ValidInput_ProducesExpectedOutput()
            {
                try
                {
                    // Arrange
                    string inputPath = Path.Combine(projectDir, "INPUT.txt");
                    File.WriteAllText(inputPath, "5");
                    Directory.SetCurrentDirectory(tempDir);

                    // Act
                    Program.Main();

                    // Assert
                    string outputPath = Path.Combine(projectDir, "OUTPUT.txt");
                    Xunit.Assert.True(File.Exists(outputPath));
                    string result = File.ReadAllText(outputPath);
                    Xunit.Assert.Equal("44", result);
                }
                finally
                {
                    CleanupTestEnvironment();
                }
            }

            private void CleanupTestEnvironment()
            {
                Directory.SetCurrentDirectory(originalDirectory);
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
                output.WriteLine($"Cleaned up test environment: {tempDir}");
            }
        }
    }
}