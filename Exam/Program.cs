using System;
using System.IO;
using Exam.Models;
using Exam.Services;

namespace Exam
{
    internal static class Program
    {
        public static Shop _shop = new Shop();
        public static string _filename = null;
        public const short _refresh = 1000;

        private static void NewGame()
        {
            _shop.GameTime = new DateTime(2022, 1, 1);
            _shop.Status = ShopStatus.Normal;
            _shop.Budget = 1000;
            _shop.AverageRating = 5;

            _shop.workers.Add(new Worker("Murad", "Musali", 14, "Stands spinner", 400));
            _shop.workers.Add(new Worker("Ramo", "Mustafazade", 15, "Stands spinner", 500));
            _shop.workers.Add(new Worker("Parvin", "Aliyev", 16, "Stands spinner", 300));

            _shop.BuyProducts();

            SaveGame();
        }
        private static bool LoadGame()
        {
            string filename = _filename;
            if (!filename.EndsWith(".bin")) filename = filename.Insert(filename.Length, ".bin");

            try { _shop = SerializeService.BinaryDeserialize(Directory.GetCurrentDirectory() + String.Format(@"\Saves\{0}", filename)) as Shop; }
            catch (Exception ex) { LegacyService.MessageBox(IntPtr.Zero, ex.Message, "File not found", default); return false; }

            if (Directory.Exists(Directory.GetCurrentDirectory() + String.Format(@"\Saves\{0}.json", _filename)))
                _shop.WeekStatistics = SerializeService.JsonDeserialize<Statistics>(Directory.GetCurrentDirectory() + String.Format(@"\Saves\{0}.json", _filename));

            _shop.Status = _shop.Status; // :D

            foreach (var item in _shop.stands.Values)
                foreach (var secondItem in item) secondItem.Status = secondItem.Status;

            _shop.products = new();

            var vegetableType = typeof(Vegetable);
            foreach (var item in vegetableType.Assembly.GetTypes())
                if (item.BaseType?.Name == typeof(Vegetable).Name) _shop.products.Add(item);

            return true;
        }
        private static void SaveGame()
        {
            string filename = _filename;
            if (!filename.EndsWith(".bin")) filename = filename.Insert(filename.Length, ".bin");

            SerializeService.BinarySerialize(Directory.GetCurrentDirectory() + String.Format(@"\Saves\{0}", filename), _shop);
        }

        private static int Menu()
        {
            byte choice = default, column = 2, row = 15;

            Console.SetCursorPosition(row, column++);
            Console.WriteLine(@"     _______.__ .___  ___. __    __  __          ___  .___________. ______  .______");
            Console.SetCursorPosition(row, column++);
            Console.WriteLine(@"    /       |  ||   \/   ||  |  |  ||  |        /   \ |           |/  __  \ |   _  \");
            Console.SetCursorPosition(row, column++);
            Console.WriteLine(@"   |   (----|  ||  \  /  ||  |  |  ||  |       /  ^  \`---|  |----|  |  |  ||  |_)  |");
            Console.SetCursorPosition(row, column++);
            Console.WriteLine(@"    \   \   |  ||  |\/|  ||  |  |  ||  |      /  /_\  \   |  |    |  |  |  ||      /");
            Console.SetCursorPosition(row, column++);
            Console.WriteLine(@".----)   |  |  ||  |  |  ||  `--'  ||  `----./  _____  \  |  |    |  `--'  ||  |\  \----.");
            Console.SetCursorPosition(row, column++);
            Console.WriteLine(@"|_______/   |__||__|  |__| \______/ |_______/__/     \__\ |__|     \______/ | _| `._____|");

            column += 7;
            row += 35;
            choice = column;

            while (true)
            {
                Console.SetCursorPosition(row, column++);
                Console.WriteLine("[1] - New Game - [1]");

                Console.SetCursorPosition(row, column++);
                Console.WriteLine("[2] - Continue - [2]");

                Console.SetCursorPosition(row, column++);
                Console.WriteLine("[3] -   Exit   - [3]");

                Console.SetCursorPosition(row + 21, choice);
                Console.Write('\u221A');

                var key = Console.ReadKey();

                if (key.Key.Equals(ConsoleKey.UpArrow) && choice > column - 3) choice--;
                else if (key.Key.Equals(ConsoleKey.DownArrow) && choice < column - 1) choice++;
                else if (key.Key.Equals(ConsoleKey.Enter)) return choice - column;

                Console.Write("\b \b");
                column -= 3;
            }
        }
        private static void Update()
        {
            _shop.EnqueueCustomers();

            _shop.GameTime = _shop.GameTime.AddHours(1);

            _shop.DequeueCustomers();

            Console.WriteLine("Customers count: {0}", _shop.HourStatistics.CustomersCount);
            Console.WriteLine("Garbage vegetables: {0}", _shop.HourStatistics.GarbageCount);
            Console.WriteLine("Earnings: {0}", _shop.HourStatistics.Earnings);
            Console.WriteLine("Shop rating: {0}", _shop.AverageRating);
            // Console.WriteLine("Vegetables rating: ");
            Console.WriteLine("Shop status: {0}", _shop.Status);
            Console.WriteLine("Budget: {0}", _shop.Budget);
            Console.WriteLine("Buyyed vegetables: \n\n\n", _shop.HourStatistics.PurchasedProductCount);

            foreach (var stand in _shop.stands)
            {
                Console.WriteLine("{0} Count: {1}", stand.Key, stand.Value.Count);
            }

            Console.SetCursorPosition(97, 28);
            Console.WriteLine(_shop.GameTime);
        }
        private static void LoadOptions()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Saves")) 
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Saves");
        }
        private static void Control()
        {
            _shop.WeekStatistics.Add(_shop.HourStatistics);
            _shop.HourStatistics.Reset();

            if (_shop.GameTime.Day == DateTime.DaysInMonth(_shop.GameTime.Year, _shop.GameTime.Month))
            {
                foreach (var item in _shop.workers)
                {
                    if (_shop.Budget < item.Salary) _shop.workers.Remove(item);
                    else _shop.Budget -= item.Salary;
                }
            }
            if (_shop.WeekStatistics.Day == 7)
            {
                if (_shop.Status == ShopStatus.Normal) _shop.BuyProducts();
                else _shop.CheckProducts();

                SerializeService.JsonSerialize(Directory.GetCurrentDirectory() + String.Format(@"\Saves\{0}.json", _filename), _shop.HourStatistics);
                _shop.WeekStatistics.Reset();
            }
            if (_shop.GameTime.Hour == 0)
            {
                _shop.WeekStatistics.Day++;
                SaveGame();
            }
        }

        private static void Main(string[] args)
        {
            restart:

            LoadOptions();
            var choice = Menu() + 4;

            Console.Clear();

            if (choice == 1 | choice == 2)
            {
                string filename = null;

                Console.Write("Include save name: ");

                while (true)
                {
                    filename = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(filename)) 
                        LegacyService.MessageBox(IntPtr.Zero, "The file name cannot be empty!", "İnvalid include!", 0);
                    else break;
                }

                Console.Clear();
                Program._filename = filename;
            }

            if (choice.Equals(1)) NewGame();
            else if (choice.Equals(2))
            {
                if (LoadGame() is false) goto restart;
            }
            else if (choice.Equals(3)) return;
            else Console.WriteLine("You are OK?");

            while (true)
            {
                Control();
                Update();

                System.Threading.Thread.Sleep(_refresh);
                System.Console.Clear();
            }
        }
    }
}
