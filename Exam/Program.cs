using System;
using Exam.Models;
using Exam.Services;

namespace Exam
{
    public static class Program
    {
        static DateTime time = new DateTime(2022, 1, 1);

        static Shop MyShop = new Shop();

        const short Refresh = 1000;

        static void LoadOptions()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }
        static int Menu()
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

        static void NewGame(string filename)
        {
            MyShop.Budget = 5000;

            MyShop.AddWorker("Murad", "Musali", 14, "Stands spinner", 400);
            MyShop.AddWorker("Ramo", "Mustafazade", 15, "Stands spinner", 500);
            MyShop.AddWorker("Parvin", "Aliyev", 16, "Stands spinner", 300);

            if (!filename.EndsWith(".bin")) filename = filename.Insert(filename.Length, ".bin");
            SerializeService.Serialize(filename.ToString(), MyShop);

        }
        static void loadGame(string filename)
        {
            try
            {
                if (!filename.EndsWith(".bin")) filename = filename.Insert(filename.Length, ".bin");
                MyShop = SerializeService.Deserialize(filename) as Shop;
            }
            catch (Exception ex)
            {
                LegacyService.MessageBox(IntPtr.Zero, ex.Message, "File not found", 0);
            }
        }

        static void Update()
        {
            MyShop.EnqueueCustomers();

            time = time.AddHours(1);

            MyShop.DequeueCustomers();

            Console.SetCursorPosition(97, 30);
            Console.WriteLine(time);
        }


        static void Main(string[] args)
        {
            MyShop.CalcProducts();
            LoadOptions();
            var choice = Menu() + 4;

            Console.Clear();


            if (choice.Equals(1))
            {
                NewGame("dubai");
                LegacyService.MessageBox(IntPtr.Zero, "Dubia", "ad", 0);
            }
            else if (choice.Equals(2))
            {
                loadGame("dubai");
                LegacyService.MessageBox(IntPtr.Zero, "Dubia", "ad", 0);
            }
            else if (choice.Equals(3)) return;
            else Console.WriteLine("You are OK?");

            while (true)
            {
                Update();

                System.Threading.Thread.Sleep(Refresh);
                System.Console.Clear();
            }
        }
    }
}
