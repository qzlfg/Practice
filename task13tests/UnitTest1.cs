using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public class StudentSerializationTests
{
    private JsonSerializerOptions GetSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };
        options.Converters.Add(new CustomDateConverter());
        return options;
    }

    [Fact]
    public void SerializationAndDeserialization_ShouldPreserveDataIntegrity()
    {
        var options = GetSerializerOptions();
        var originalStudent = new Student
        {
            FirstName = "Тест",
            LastName = "Тестов",
            BirthDate = new DateTime(2004, 1, 1),
            Grades = new List<Subject>
            {
                new Subject { Name = "Физика", Grade = 4 },
                new Subject { Name = "Информатика", Grade = 5 }
            }
        };

        string jsonString = JsonSerializer.Serialize(originalStudent, options);
        Student? deserializedStudent = JsonSerializer.Deserialize<Student>(jsonString, options);

        Assert.NotNull(deserializedStudent);
        Assert.Equal(originalStudent.FirstName, deserializedStudent.FirstName);
        Assert.Equal(originalStudent.LastName, deserializedStudent.LastName);
        Assert.Equal(originalStudent.BirthDate, deserializedStudent.BirthDate);
        
        Assert.NotNull(deserializedStudent.Grades);
        Assert.Equal(originalStudent.Grades.Count, deserializedStudent.Grades.Count);
        Assert.Equal(originalStudent.Grades[0].Name, deserializedStudent.Grades[0].Name);
        Assert.Equal(originalStudent.Grades[0].Grade, deserializedStudent.Grades[0].Grade);
    }

    [Fact]
    public void FileOperations_ShouldSuccessfullySaveAndLoadJsonFile()
    {
        var options = GetSerializerOptions();
        string tempFilePath = Path.Combine(Path.GetTempPath(), $"student_test_{Guid.NewGuid()}.json");

        var student = new Student
        {
            FirstName = "Анна",
            LastName = "Тестова",
            BirthDate = new DateTime(2003, 8, 15),
            Grades = new List<Subject>()
        };

        try
        {
            string jsonToWrite = JsonSerializer.Serialize(student, options);
            File.WriteAllText(tempFilePath, jsonToWrite);

            Assert.True(File.Exists(tempFilePath));

            string jsonFromFile = File.ReadAllText(tempFilePath);
            Student? loadedStudent = JsonSerializer.Deserialize<Student>(jsonFromFile, options);

            Assert.NotNull(loadedStudent);
            Assert.Equal("Анна", loadedStudent.FirstName);
            Assert.Equal("Тестова", loadedStudent.LastName);
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    [Fact]
    public void Main_WhenExecuted_ShouldSaveFileReadItAndPassValidation()
    {
        string expectedFile = "student.json";
        
        if (File.Exists(expectedFile))
        {
            File.Delete(expectedFile);
        }

        using var stringWriter = new StringWriter();
        var originalOutput = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            Program.Main();

            string actualOutput = stringWriter.ToString();

            Assert.True(File.Exists(expectedFile), "Программа должна была создать файл student.json на диске.");

            Assert.Contains("успешно сохранен", actualOutput);
            Assert.Contains("Объект успешно восстановлен в оперативной памяти", actualOutput);
            Assert.Contains("полностью валидны", actualOutput);


            Assert.DoesNotContain("Ошибка", actualOutput);
            Assert.DoesNotContain("Валидация провалена", actualOutput);
        }
        finally
        {
            Console.SetOut(originalOutput);

            if (File.Exists(expectedFile))
            {
                File.Delete(expectedFile);
            }
        }
    }
}