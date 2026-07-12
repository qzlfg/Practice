using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;


public class Program
{
    public static void Main()
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };

        options.Converters.Add(new CustomDateConverter());



        string filePath = "student.json";


        var originalStudent = new Student
        {
            FirstName = "Alex",
            LastName = "Black",
            BirthDate = new DateTime(2005, 10, 15),
            Grades = new List<Subject>
            {
                new Subject { Name = "Programming", Grade = 5 },
                new Subject { Name = "PE", Grade = 5 }
            }
        };

        try
        {
            string jsonString = JsonSerializer.Serialize(originalStudent, options);
            
            File.WriteAllText(filePath, jsonString);
            
            Console.WriteLine($"Файл {filePath} успешно сохранен. Содержимое файла:");
            Console.WriteLine(jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
            return;
        }


        Student? deserializedStudent = null;

        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Ошибка: файл не найден на диске!");
                return;
            }

            string jsonFromFile = File.ReadAllText(filePath);
            
            deserializedStudent = JsonSerializer.Deserialize<Student>(jsonFromFile, options);
            
            Console.WriteLine("Объект успешно восстановлен в оперативной памяти.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении/десериализации: {ex.Message}");
            return;
        }

        if (deserializedStudent == null)
        {
            Console.WriteLine("Объект пуст (null) после десериализации.");
            return;
        }

        bool isDataCorrect = true;

        if (string.IsNullOrWhiteSpace(deserializedStudent.FirstName) || string.IsNullOrWhiteSpace(deserializedStudent.LastName))
        {
            Console.WriteLine("Имя или Фамилия студента пусты или некорректны!");
            isDataCorrect = false;
        }

        if (deserializedStudent.BirthDate > DateTime.Now || deserializedStudent.BirthDate.Year < 1900)
        {
            Console.WriteLine("Указана нереальная дата рождения!");
            isDataCorrect = false;
        }


        if (deserializedStudent.Grades == null || deserializedStudent.Grades.Count == 0)
        {
            Console.WriteLine("У студента нет ни одного предмета в списке.");
        }
        else
        {
            foreach (var subject in deserializedStudent.Grades)
            {
                if (string.IsNullOrWhiteSpace(subject.Name))
                {
                    Console.WriteLine("Обнаружен предмет без названия!");
                    isDataCorrect = false;
                }

                if (subject.Grade < 2 || subject.Grade > 5)
                {
                    Console.WriteLine($"Некорректная оценка '{subject.Grade}' по предмету {subject.Name}!");
                    isDataCorrect = false;
                }
            }
        }

        if (isDataCorrect)
        {
            Console.WriteLine($"Данные студента {deserializedStudent.FirstName} {deserializedStudent.LastName} полностью валидны");
        }
        else
        {
            Console.WriteLine("Валидация провалена. Структура данных нарушена.");
        }
    }
}