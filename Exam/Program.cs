using System;
using System.IO;
using Exam.Models;
using Exam.Services;

namespace Exam
{
    internal static class Program
    {
        public static Shop shop = new Shop();
        public static string filename = null;

        /// <summary>
        /// if you change the value of hour, then the products will spoil faster and the likelihood of an epidemic increases
        /// </summary>
        public const short hour = 5000;
        public const short _refresh = hour;

        private static void NewGame()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + String.Format(@"\Saves\{0}.json", filename)))
                File.Delete(Directory.GetCurrentDirectory() + String.Format(@"\Saves\{0}.json", filename));

            shop.GameTime = new DateTime(2022, 1, 1, 10, 0, 0);
            shop.Status = ShopStatus.Normal;
            shop.Budget = 1000;

            shop.workers.Add(new Worker("Murad", "Musali", 14, "Stands spinner", 100));
            shop.workers.Add(new Worker("Ramo", "Mustafazade", 15, "Stands spinner", 200));

            shop.BuyProducts();

            SaveGame();
        }
        private static bool LoadGame()
        {
            try { shop = SerializeService.BinaryDeserialize(Directory.GetCurrentDirectory() + String.Format(@"\Saves\{0}.bin", filename)) as Shop; }
            catch (Exception ex) { LegacyService.MessageBox(IntPtr.Zero, ex.Message, "File not found!", default); return false; }

            if (Directory.Exists(Directory.GetCurrentDirectory() + String.Format(@"\Saves\{0}.json", filename)))
                shop.WeekStatistics = SerializeService.JsonDeserialize<Statistics>(Directory.GetCurrentDirectory() + String.Format(@"\Saves\{0}.json", filename));

            shop.Status = shop.Status; // :D

            foreach (var item in shop.stands.Values)
                foreach (var secondItem in item) secondItem.Status = secondItem.Status;

            shop.products = new();
            shop.customers = new();

            var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            var baseTypeName = typeof(Vegetable).Name;

            foreach (var item in types) if (item.BaseType?.Name == baseTypeName) shop.products.Add(item);


            return true;
        }
        private static void SaveGame()
        {
            SerializeService.BinarySerialize(Directory.GetCurrentDirectory() + String.Format(@"\Saves\{0}.bin", filename), shop);
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
            shop.GameTime = shop.GameTime.AddHours(1);
            shop.DequeueCustomers();

            {
                int index = default;

                Console.CursorLeft = 25;
                Console.WriteLine("--- Products Info ---\n");
                foreach (var stand in shop.stands)
                {
                    var price = Convert.ToDouble(shop.products.Find(a => a.Name.Equals(stand.Key)).GetField("price").GetValue(null));

                    Console.WriteLine(String.Format("{0}) Name: {1, -12} Buy Price: {2:0.00}\t Sell Price: {3:0.00}\t Count: {4, -10}"
                        , ++index, stand.Key, price, price * 1.7, stand.Value.Count));
                }
                Console.WriteLine();
            }

            Console.CursorLeft = 25;
            Console.WriteLine("---  Hour Raport  ---\n");
            Console.WriteLine(shop.HourStatistics);

            if (shop.WeekStatistics.day >= 7 & shop.GameTime.Hour == 1)
            {
                Console.CursorLeft = 25;
                Console.WriteLine("---  Week Raport  ---\n");
                Console.WriteLine(shop.WeekStatistics);
                shop.WeekStatistics.Reset();
            }

            Console.SetCursorPosition(3, 28);
            Console.WriteLine("Shop: {0}", ( shop.GameTime.Hour < 9) ? "Closed" : "Open");

            Console.SetCursorPosition(18, 28);
            Console.Write("Status: {0}", shop.Status);

            Console.SetCursorPosition(38, 28);
            Console.Write("Rating(3-5): {0}.0", shop.AverageRating);

            Console.SetCursorPosition(58, 28);
            Console.Write("Budget: {0} $", shop.Budget);

            Console.SetCursorPosition(97, 28);
            Console.Write(shop.GameTime);
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
            shop.WeekStatistics.Add(shop.HourStatistics);

            if (shop.GameTime.Day == DateTime.DaysInMonth(shop.GameTime.Year, shop.GameTime.Month) & shop.GameTime.Hour == 0)
            {
                foreach (var item in shop.workers)
                {
                    if (shop.Budget < item.Salary & shop.workers.Count > 1) shop.workers.Remove(item);
                    else shop.Budget -= item.Salary;
                }
            }
            if (shop.GameTime.Hour == 0)
            {
                shop.WeekStatistics.day++;

                if (shop.WeekStatistics.day >= 7)
                {
                    if (shop.Status == ShopStatus.Normal) shop.BuyProducts();
                    else shop.CheckProducts();

                    SerializeService.JsonSerialize(Directory.GetCurrentDirectory() + String.Format(@"\Saves\{0}.json", filename), shop.WeekStatistics);
                }

                SaveGame();
            }

            shop.HourStatistics.Reset();
            if (shop.GameTime.Hour > 7) shop.EnqueueCustomers();
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
                Program.filename = filename;
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

                System.Threading.Thread.Sleep(Program._refresh);
                System.Console.Clear();
            }
        }
    }
}
