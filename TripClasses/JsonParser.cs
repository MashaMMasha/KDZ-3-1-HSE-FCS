using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TripClasses
{
    public static class JsonParser
    {
        // Метод, который считывает данные из консоли и возвращает строку в формате json.
        public static string GetFromConsole()
        {
            string jsonData = "";
            string newLine = "";
            var inputStream = Console.OpenStandardInput(); // Используем стаднартный поток для того чтобы считать данные.
            using (StreamReader fileReader = new StreamReader(inputStream))
            {
                while (newLine != null)
                {
                    newLine = fileReader.ReadLine();
                    jsonData += newLine + "\n"; // Записываем все данные с консоли в одну строку.
                }
            }
            return jsonData;
        }
        
        // Метод который перенаправляет стандартный поток в файл, считывает оттуда данные и возвращает их.
        // в виде строки формата json.
        public static string GetFromFile(string filePath)
        {
            string jsonData = "", newLine = "";
            var standardInput = new StreamReader(Console.OpenStandardInput()); // Сохраняем стандартный поток чтобы вернуться к работе с консолью после работы с файлом.
            using (StreamReader fileReader = new StreamReader(filePath))
            {
                Console.SetIn(fileReader);
                do
                {
                    newLine = fileReader.ReadLine();
                    jsonData += newLine + "\n";
                } while (newLine != null);
                Console.SetIn(standardInput); // Возвращаем стандартный поток на тот который мы слхранили до работы с файлом.
            }
            return jsonData;
        }
        
        // Дополнительный метод который проверяет на соответсвие формату данные внутри массива объекта файла.
        public static string[] ExtractArrayValues(string arrayString)
        {
            // Метод возврашает массив данных, которые проверет на формат. Это должна быть строка в начале и конце которой стоят кавычки.
            return Regex.Matches(arrayString, "\"([^\"]+)\"")
                .Cast<Match>()
                .Select(match => match.Groups[1].Value)
                .ToArray();
        }
        
        // Основной метод для парсинга файла. Возвращает список объектов Trips.
        public static List<Trips> ReadJson(int mode, string filePath)
        {
            Trips trip;
            List<Trips> trips = new List<Trips>();
            string tripId, destination, startDate, endDate, accommodation, activitiesMatch,travelersMatch, jsonData = "";
            string[] activities, travelers;

            // Получаем строку из файла или консоли в зависимости от выбора пользователя.
            if (mode == 1)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Введите данные в формате json. " +
                                  "Для завершения воода нажмите ctrl+z если у вас windows и ctrl+d если вы работаете на macOS.");
                jsonData = GetFromConsole();
            }
            else
            {
                jsonData = GetFromFile(filePath);
            }
            // Используя регулярное выражение, вычленем из полученной строки необходимую для полей каждого объекта информацию.
            // Паттер проверяет данные на соответсвие формату файла (строки, массивы, числа, даты)
            string pattern = "\"trip_id\":\\s*(\\d+),\\s*\"destination\":\\s*\"([^\"]+)\",\\s*\"start_date" +
                             "\":\\s*\"(\\d{4}-\\d{2}-\\d{2})\",\\s*\"end_date\":\\s*\"(\\d{4}-\\d{2}-\\d{2})\"," +
                             "\\s*\"travelers\":\\s*\\[\\s*((?:\"[^\"]+\",\\s*)*\"[^\"]+\")?\\s*\\]," +
                             "\\s*\"accommodation\":\\s*\"([^\"]+)\",\\s*\"activities\":\\s*\\[\\s*((?:\"[^\"]+\",\\s*)*\"[^\"]+\")?\\s*\\]";
            
            MatchCollection matches = Regex.Matches(jsonData, pattern);
            // Создаем список объектов нашего класса заполняя необходимые поля данными, которые соответсвуют формату исходного файла.
            foreach (Match match in matches)
            {
                tripId = match.Groups[1].Value;
                destination = match.Groups[2].Value;
                startDate = match.Groups[3].Value;
                endDate = match.Groups[4].Value;
                travelersMatch = match.Groups[5].Value;
                travelers = ExtractArrayValues(travelersMatch);
                accommodation = match.Groups[6].Value;
                activitiesMatch = match.Groups[7].Value;
                activities = ExtractArrayValues(activitiesMatch);

                trip = new Trips(tripId, destination, startDate, endDate, travelers.ToList(), accommodation,
                    activities.ToList());
                trips.Add(trip);
            }

            return trips;
        }
        
        // Метод для записи строки нужного формата в файл, который пользователь передает по пути.
        public static void WriteToFile(string jsonData, string filePath)
        {
            File.WriteAllText(filePath, string.Empty);
            var standardOutput = new StreamWriter(Console.OpenStandardOutput()); // Сохраняем стандартный поток вывода через консоль, чтобы вернуться к нему после работы с файлом.
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                Console.SetOut(sw); // Перенаправляем поток в файл.
                Console.WriteLine(jsonData); // Записываем в файл данные.
                Console.SetOut(standardOutput); // Возвращаем консоль как стандартный поток вывода.
            }
        }
        public static void WriteToConsole(string data)
        {
            var outputStream = Console.OpenStandardOutput(); // Используем стаднартный поток для вывода данных.
            using (StreamWriter fileWriter = new StreamWriter(outputStream))
            {
                fileWriter.WriteLine(data);
            }
        }

        // Метод для записи данных в файл или консоль. Вызывает нужный метод, в зависимости от выбора пользователя.
        public static void WriteJson(string data, string filePath, int mode)
        {
            switch (mode)
            {
                case 1:
                    WriteToConsole(data);
                    break;
                case 2:
                    WriteToFile(data, filePath);
                    break;
            }
        }
    }
}