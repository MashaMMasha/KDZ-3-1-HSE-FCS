using KDZ3_1;
using TripClasses;

internal class Program
{
    public static void Main(string[] args)
    {
        // Сначала только считываем данные, так как без этого у пользователя нет доступа к функционалу программы.
        List<Trips> data;
        List<Trips> newData;
        data = Methods.DataInput();
        do
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Выберите число от 1 до 4:\n" +
                                  "1. Заменить существующие данные.\n" +
                                  "2. Произвести фильтрацию одному из полей.\n" +
                                  "3. Произвести сортировку по одному из полей.\n" +
                                  "4. Вывести данные в консоль или сохранить в файл.");
                int n = Methods.ReadNumber(4);
                // Для каждого пункта из меню есть отделный метод. После сортировок и фильтров пользователю предлагется обновить данные, которые хранит программа.
                switch (n)
                {
                    case 1:
                        data = Methods.DataInput();
                        break;
                    case 2:
                        newData = Methods.Filter(data);
                        data = Methods.UpdateData(data, newData);
                        break;
                    case 3:
                        newData = Methods.Sorting(data);
                        data = Methods.UpdateData(data, newData);
                        break;
                    case 4:
                        Methods.DataOutput(data);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Такого варианта нет!");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("Возникла ошибка при работе программы. Попробуйте еще раз :(" + e.Message);
                Console.BackgroundColor = ConsoleColor.White;
            }

            Console.WriteLine("Нажмите enter чтобы продолжить, esc чтобы завершить работу программы.");
        } while (Console.ReadKey().Key != ConsoleKey.Escape);
    }
}