using System.Collections.Concurrent;
using System.Text;
using TripClasses;

namespace KDZ3_1;

public class Methods
{
    // Метод получает имя существующего файла, для последующей работы с ним.
    public static string GetFilePath()
    {
        string filePath = "";
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Введите путь до файла.");
        filePath = Console.ReadLine();
        // Проверяем что файл существует.
        while (!File.Exists(filePath))
        {
            Console.WriteLine("Похоже такого файла не существует. Попробуйте ввести путь еще раз: ");
            filePath = Console.ReadLine();
        }
        return filePath;
    }

    // Метод вызывается после сортировок и фильтраций и предлагает пользователю обновить данные, которые хранит программа.
    public static List<Trips> UpdateData(List<Trips> data, List<Trips> newData)
    {
        // Если метод возвращает пустые данные то смысла их сохранять нет.
        if (newData != null)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Вы хотите обновить данные (продолжить работу с новыми)? \n" +
                              "1. Да \n" +
                              "2. Нет");
            int choice = ReadNumber(2);
            switch (choice)
            {
                case 1:
                    return newData;
                case 2:
                    return data;
                default:
                    return data;
            }
        }
        else
        {
            // Если список данных пуст возвращаем данные которые уже хранятся в программе и ни на что их не заменяем.
            return data;
        }
    }
    
    // Метод получает на вход список объектов класса и возвращает строку формата json.
    public static string ConvertToJson(List<Trips> trips)
    {
        StringBuilder jsonString = new StringBuilder();
        // Восстанавливаем структуру исходного файла
        jsonString.AppendLine("[");
        for (int t = 0; t < trips.Count; t++)
        {
            Trips trip = trips[t];
            jsonString.AppendLine("  {");
            jsonString.AppendLine($"    \"trip_id\": {trip.TripId},");
            jsonString.AppendLine($"    \"destination\": \"{trip.Destination}\",");
            jsonString.AppendLine($"    \"start_date\": \"{trip.StartDate}\",");
            jsonString.AppendLine($"    \"end_date\": \"{trip.EndDate}\",");
            jsonString.AppendLine($"    \"travelers\": [");
            for (int i = 0; i < trip.Travelers.Count; i++)
            {
                if (i != trip.Travelers.Count - 1)
                {
                    jsonString.AppendLine($"      \"{trip.Travelers[i]}\",");
                }
                else
                {
                    jsonString.AppendLine($"      \"{trip.Travelers[i]}\"");
                }
                
            }
            jsonString.AppendLine("    ],");
            jsonString.AppendLine($"    \"accommodation\": \"{trip.Accommodation}\",");
            jsonString.AppendLine($"    \"activities\": [");
            for (int i = 0; i < trip.Activities.Count; i++)
            {
                if (i != trip.Activities.Count - 1)
                {
                    jsonString.AppendLine($"      \"{trip.Activities[i]}\",");
                }
                else
                {
                    jsonString.AppendLine($"      \"{trip.Activities[i]}\"");
                }
                
            }
            jsonString.AppendLine("    ]");
            if (t != trips.Count - 1)
            {
                jsonString.AppendLine("  },");
            }
            else
            {
                jsonString.AppendLine("  }");
            }
        }
        jsonString.AppendLine("]");
        // Возвращаем данные в формате строки как в исходном файле.
        return jsonString.ToString();
    }
    
    // Метод для считывания у пользовталея числа.
    public static int ReadNumber(int k)
    {
        int n;
        Console.WriteLine($"Введите число от 1 до {k}");
        // Проверем что то, что пользователь вводит в консоль - число, в необходимых пределах.
        while (!int.TryParse(Console.ReadLine(), out n) || n > k || n < 1)
        {
            Console.WriteLine($"Вы ввели неверные данные!!! Введите число от 1 до {k}");
        }

        return n;
    }
    
    // Метод запрашивает у пользователя данные о том откуда он хочет считать данные.
    public static List<Trips> DataInput()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Как вы ходите передать данные? Выберете нужный вариант. \n" +
                          "1. Считать из консоли\n" +
                          "2. Считать из файла");
        int choice = ReadNumber(2);
        return JsonParser.ReadJson(choice, choice == 2 ? GetFilePath(): "");
    }

    // Добавила дополнительно для проверки полей в сортировках и фильтрах.
    public enum TripsField
    {
        TripId = 1,
        Destination,
        StartDate,
        EndDate,
        Travelers,
        Accommodation,
        Activities,
        None
    }

    // Метод который преобразует данные в строку для вывода в консоль (в красивом виде)
    public static void PrintData(List<Trips> data, TripsField field, string printParam)
    {
        Console.WriteLine(data.Count);
        foreach (Trips trip in data)
        {
            // Добавляем цвет чтобы было видно где выводятся данные, а где было общение с пользователем.
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"TripId = {trip.TripId}");
            Console.WriteLine($"Destination = {trip.Destination}");
            Console.WriteLine($"StartDate = {trip.StartDate}");
            Console.WriteLine($"EndDate = {trip.EndDate}");
            Console.WriteLine("Travelers: ");
            foreach (string travaler in trip.Travelers)
            {
                Console.WriteLine($"    - {travaler}");
            }
            Console.WriteLine($"Accommodation = {trip.Accommodation}");
            Console.WriteLine("Activities: ");
            foreach (string activity in trip.Activities)
            {
                Console.WriteLine($"    - {activity}");
            }
            if (field != TripsField.None)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                // Выводим после каждого объекта информацию о том что было произведено (фильтрация/сортировка и по какому полю).
                Console.WriteLine($"----------{printParam} by {field}----------");
            }
            // Добавляем цветной разделитель между объектами при выводе для повыщения уровня читаемости.
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('=', Console.WindowWidth - 1));
        }
    }
    
    // Метод фильтрации. Выводит данные поля которых содержат то что ввел пользователь (напрмер, если введено 1 то учтутся 11 12... 100).
    public static List<Trips> Filter(List<Trips> tripsData)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("По какому полю вы хотите произвести фильтрацию? Выберете нужный вариант. \n" +
                          "1. trip_id \n" +
                          "2. destination \n" +
                          "3. start_date \n" +
                          "4. end_date \n" +
                          "5. travelers \n" +
                          "6. accommodation \n" +
                          "7. activities");
        TripsField filterField = (TripsField)ReadNumber(7);
        Console.WriteLine("Введите значение по которому будет производиться фильтрация:");
        string filterValue = Console.ReadLine();
        List<Trips> filtredData = new List<Trips>();
        switch (filterField)
        {
            // Отдельно пишем фильтрацию для каждого поля и сразу же вызываем вывод информации, для того чтобы пользователь видел результат работы программы.
            case TripsField.TripId:
                Console.WriteLine(tripsData.Count);
                filtredData = tripsData.Where(v => v.TripId.Contains(filterValue)).ToList();
                if (filtredData.Count != 0 && filtredData != null)
                {
                    PrintData(filtredData, filterField, "Filtred");
                    return filtredData;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Такх значений нет :(");
                    return null;
                }
            case TripsField.Destination:
                Console.WriteLine(tripsData.Count);
                filtredData = tripsData.Where(v => v.TripId.Contains(filterValue)).ToList();
                if (filtredData.Count != 0 && filtredData != null)
                {
                    PrintData(filtredData, filterField, "Filtred");
                    return filtredData;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Такх значений нет :(");
                    return null;
                }
            case TripsField.StartDate:
                Console.WriteLine(tripsData.Count);
                filtredData = tripsData.Where(v => v.TripId.Contains(filterValue)).ToList();
                if (filtredData.Count != 0 && filtredData != null)
                {
                    PrintData(filtredData, filterField, "Filtred");
                    return filtredData;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Такх значений нет :(");
                    return null;
                }
            case TripsField.EndDate:
                Console.WriteLine(tripsData.Count);
                filtredData = tripsData.Where(v => v.TripId.Contains(filterValue)).ToList();
                if (filtredData.Count != 0 && filtredData != null)
                {
                    PrintData(filtredData, filterField, "Filtred");
                    return filtredData;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Такх значений нет :(");
                    return null;
                }
            case TripsField.Travelers:
                filtredData = tripsData.Where(v => v.Travelers.Any(t => t.Contains(filterValue))).ToList();
                if (filtredData.Count != 0 && filtredData != null)
                {
                    PrintData(filtredData, filterField, "Filtred");
                    return filtredData;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Такх значений нет :(");
                    return null;
                }
            case TripsField.Accommodation:
                Console.WriteLine(tripsData.Count);
                filtredData = tripsData.Where(v => v.TripId.Contains(filterValue)).ToList();
                if (filtredData.Count != 0 && filtredData != null)
                {
                    PrintData(filtredData, filterField, "Filtred");
                    return filtredData;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Такх значений нет :(");
                    return null;
                }
            case TripsField.Activities:
                filtredData = tripsData.Where(v => v.Travelers.Any(t => t.Contains(filterValue))).ToList();
                if (filtredData.Count != 0 && filtredData != null)
                {
                    PrintData(filtredData, filterField, "Filtred");
                    return filtredData;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Такх значений нет :(");
                    return null;
                }
        }

        return new List<Trips>();
    }

    // Метод для общения с пользователем при выборе сортировки в меню.
    public static List<Trips> Sorting(List<Trips> tripsData)
    {
        List<Trips> sortedTrips = new List<Trips>();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("По какому полю вы хотите произвести сортировку? Выберете нужный вариант. \n" +
                          "1. trip_id \n" +
                          "2. destination \n" +
                          "3. start_date \n" +
                          "4. end_date \n" +
                          "5. travelers \n" +
                          "6. accommodation \n" +
                          "7. activities");
        TripsField sortField = (TripsField)ReadNumber(7);
        Console.WriteLine("Как вы хотите произвести сортировку? Выберете нужный вариант. \n " +
                          "1. По возрастанию \n " +
                          "2. По убыванию");
        int sortingParametr = ReadNumber(2);
        // Вызываем нужный метод для сортировки в зависимости от решения пользователя.
        switch (sortingParametr)
        {
            case 1:
                return SortingByAscending(tripsData, sortField);
            case 2:
                return SortingByDescending(tripsData, sortField);
        }
        return new List<Trips>();
    }
    
    // Метод для сортировки списка объектов класса по возрастанию.
    public static List<Trips> SortingByAscending(List<Trips> tripsData, TripsField sortingField)
    {
        List<Trips> sortedData = new List<Trips>();
        switch (sortingField)
        {
            case TripsField.TripId:
                // Переводим поле к типу числа, чтобы сортировка работала корректно.
                sortedData = tripsData.OrderBy(v => int.Parse(v.TripId)).ToList();
                PrintData(sortedData, sortingField, "Sorted");
                return sortedData;с
            case TripsField.Destination:
                sortedData = tripsData.OrderBy(v => v.Destination).ToList();
                PrintData(sortedData, sortingField, "Sorted");
                return sortedData;
            case TripsField.StartDate:
                sortedData = tripsData.OrderBy(v => v.StartDate).ToList();
                PrintData(sortedData, sortingField, "Sorted");
                return sortedData;
            case TripsField.EndDate:
                sortedData = tripsData.OrderBy(v => v.EndDate).ToList();
                PrintData(sortedData, sortingField, "Sorted");
                return sortedData;
            case TripsField.Travelers:
                sortedData = tripsData.OrderBy(v => string.Join(", ", v.Travelers)).ToList();
                PrintData(sortedData, sortingField, "Sorted");
                return sortedData;
            case TripsField.Accommodation:
                sortedData = tripsData.OrderBy(v => v.Accommodation).ToList();
                PrintData(sortedData, sortingField, "Sorted");
                return sortedData;
            case TripsField.Activities:
                sortedData = tripsData.OrderBy(v => string.Join(", ", v.Activities)).ToList();
                PrintData(sortedData, sortingField, "Sorted");
                return sortedData;
            default:
                Console.WriteLine("Такого поля нет");
                return tripsData;
        }
    }

    // Метод для сортировки по убыванию. В задании не было ничего про него сказано, но добавила для расширения функционала :)
    public static List<Trips> SortingByDescending(List<Trips> tripsData, TripsField sortingField)
    {
        List<Trips> sortedData = new List<Trips>();
        switch (sortingField)
        {
            case TripsField.TripId:
                sortedData = tripsData.OrderByDescending(v => int.Parse(v.TripId)).ToList();
                PrintData(sortedData, sortingField, "Sorted");
                return sortedData;
            case TripsField.Destination:
                sortedData = tripsData.OrderByDescending(v => v.Destination).ToList();
                PrintData(sortedData, sortingField, "Sorted");
                return sortedData;
            case TripsField.StartDate:
                sortedData = tripsData.OrderByDescending(v => v.StartDate).ToList();
                PrintData(sortedData, sortingField, "Sorted");
                return sortedData;
            case TripsField.EndDate:
                sortedData = tripsData.OrderByDescending(v => v.EndDate).ToList();
                PrintData(sortedData, sortingField, "Sorted");
                return sortedData;
            case TripsField.Travelers:
                sortedData = tripsData.OrderByDescending(v => string.Join(", ", v.Travelers)).ToList();
                PrintData(sortedData, sortingField, "Sorted");
                return sortedData;
            case TripsField.Accommodation:
                sortedData = tripsData.OrderByDescending(v => v.Accommodation).ToList();
                PrintData(sortedData, sortingField, "Sorted");
                return sortedData;
            case TripsField.Activities:
                sortedData = tripsData.OrderByDescending(v => string.Join(", ", v.Activities)).ToList();
                PrintData(sortedData, sortingField, "Sorted");
                return sortedData;
            default:
                Console.WriteLine("Такого поля нет");
                return tripsData;
        }
    }

    // Метод который вызывается когда пользователь хочет вывести или сохранить данные.
    public static void DataOutput(List<Trips> tripsData)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Что вы хотите сдлеать?\n" +
                          "1. Вывести данные в консоль. \n" +
                          "2. Заменить данные в существующем файле. \n" +
                          "3. Создать новый файл.");
        int choice = ReadNumber(3);
        // Узнаем у пользователя как именно он хочет сохранить данные и в зависимости от этого вызываем соответствующий метод.
        switch (choice)
        {
            case 1:
                JsonParser.WriteJson(ConvertToJson(tripsData), "", 1);
                break;
            case 2:
                try
                {
                    JsonParser.WriteJson(ConvertToJson(tripsData), GetFilePath(), 2);
                }
                // Ловим все возможные ошибки.
                catch (ArgumentException ex)
                {
                    // Передаем ошибку с сообщением в основную программу
                    throw new ArgumentException("Введен некорректный путь к файлу. Повторите попытку.");
                }
                catch (DirectoryNotFoundException ex)
                {
                    // Передаем ошибку с сообщением в основную программу
                    throw new DirectoryNotFoundException(
                        "Директория с необходимым файлом не найдена. Повторите попытку.");
                }
                catch (IOException ex)
                {
                    //Передаем ошибку с сообщением в основную программу
                    throw new IOException("Ошибка при открытии файла. Повторите попытку.");
                }
                catch (Exception e)
                {
                    // Передаем ошибку с сообщением в основную программу
                    throw new Exception("Ошибка. Повторите попытку.");
                }

                break;
            case 3:
                Console.WriteLine("Укажите абсолютный путь для файла, который вы хотите создать. В том числе имя_файла.json");
                string newFilePath = Console.ReadLine();
                while (newFilePath.Length < 6 || newFilePath.Substring(newFilePath.Length - 5, 5) != ".json" ||
                    File.Exists(newFilePath))
                {
                    Console.WriteLine("Вы ввели некорректные данные. Укажите абсолютный путь для файла, который вы хотите создать. В том числе имя_файла.json");
                    newFilePath = Console.ReadLine();
                }
                try
                {
                    if (string.IsNullOrWhiteSpace(newFilePath))
                    {
                        throw new ArgumentException("Некорректный путь к файлу");
                    }

                    // Проверяем, существует ли файл по указанному пути.
                    if (!File.Exists(newFilePath))
                    {
                        JsonParser.WriteJson(ConvertToJson(tripsData), newFilePath, 2);
                    }
                    else
                    {
                        Console.WriteLine("Такой файл уже существует! Попробуйте еще раз.");
                    }
                }
                // Ловим все возможные ошибки.
                catch (ArgumentException ex)
                {
                    // Передаем ошибку с сообщением в основную программу
                    throw new ArgumentException("Введен некорректный путь к файлу. Повторите попытку.");
                }
                catch (DirectoryNotFoundException ex)
                {
                    // Передаем ошибку с сообщением в основную программу
                    throw new DirectoryNotFoundException("Директория с необходимым файлом не найдена. Повторите попытку.");
                }
                catch (IOException ex)
                {
                    //Передаем ошибку с сообщением в основную программу
                    throw new IOException("Ошибка при открытии файла. Повторите попытку.");
                }
                catch (Exception e)
                {
                    // Передаем ошибку с сообщением в основную программу
                    throw new Exception("Ошибка. Повторите попытку.");
                }

                break;
        }
    }
}