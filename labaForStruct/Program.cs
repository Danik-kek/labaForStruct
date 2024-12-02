using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public struct Toy : IComparable<Toy>
{
    public string Name;
    public int Price;
    public int AgeMin;
    public int AgeMax;

    public int CompareTo(Toy other)
    {
        return this.Price.CompareTo(other.Price);
    }

    public override string ToString()
    {
        return $"{Name};{Price};{AgeMin};{AgeMax}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        string inputFileName = "input.txt";
        string outputFileName = "output.txt";

        // Чтение данных из файла
        Toy[] toys = ReadFile(inputFileName);
        if (toys.Length == 0)
        {
            Console.WriteLine("Нет доступных игрушек для обработки.");
            return;
        }

        // Запрос возрастных ограничений у пользователя
        Console.Write("Введите минимальный возраст: ");
        int minAge = int.Parse(Console.ReadLine());

        Console.Write("Введите максимальный возраст: ");
        int maxAge = int.Parse(Console.ReadLine());

        // Фильтрация игрушек по возрастным ограничениям
        var filteredToys = toys.Where(t => (minAge >= t.AgeMin && minAge <= t.AgeMax) ||
                                            (maxAge >= t.AgeMin && maxAge <= t.AgeMax)).ToList();

        // Сортировка игрушек по стоимости
        filteredToys.Sort(); // Здесь это будет работать, поскольку List<Toy> реализует IComparable

        // Запись результатов в файл
        WriteFile(outputFileName, filteredToys);

        // Вывод результатов в консоль
        Display(filteredToys);
    }

    public static Toy[] ReadFile(string fileName)
    {
        try
        {
            string[] lines = File.ReadAllLines(fileName);
            List<Toy> toys = new List<Toy>();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue; // Пропускаем пустые строки

                string[] toyFields = line.Split(';');
                if (toyFields.Length != 4) continue; // Проверка на количество полей

                try
                {
                    Toy toy = new Toy
                    {
                        Name = toyFields[0],
                        Price = int.Parse(toyFields[1]),
                        AgeMin = int.Parse(toyFields[2]),
                        AgeMax = int.Parse(toyFields[3])
                    };
                    toys.Add(toy);
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Ошибка формата в строке: {line}");
                }
            }
            return toys.ToArray();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            return Array.Empty<Toy>(); // Возвращаем пустой массив при ошибке
        }
    }

    public static void WriteFile(string fileName, List<Toy> toys)
    {
        try
        {
            File.WriteAllLines(fileName, toys.Select(t => t.ToString()));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при записи в файл: {ex.Message}");
        }
    }

    public static void Display(List<Toy> toys)
    {
        Console.WriteLine("Отфильтрованные и отсортированные игрушки:");
        foreach (var toy in toys)
        {
            Console.WriteLine(toy);
        }
    }

}
