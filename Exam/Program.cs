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

            // Form 1
            // Console.SetCursorPosition(row, column++);
            // Console.WriteLine(@"   _____ _                 __      __");
            // Console.SetCursorPosition(row, column++);
            // Console.WriteLine(@"  / ___/(_____ ___  __  __/ ____ _/ /_____  _____");
            // Console.SetCursorPosition(row, column++);
            // Console.WriteLine(@"  \__ \/ / __ `__ \/ / / / / __ `/ __/ __ \/ ___/");
            // Console.SetCursorPosition(row, column++);
            // Console.WriteLine(@" ___/ / / / / / / / /_/ / / /_/ / /_/ /_/ / /");
            // Console.SetCursorPosition(row, column++);
            // Console.WriteLine(@"/____/_/_/ /_/ /_/\__,_/_/\__,_/\__/\____/_/");

            // Form 2
            // Console.SetCursorPosition(row, column++);
            // Console.WriteLine(@" ____                            ___            __");
            // Console.SetCursorPosition(row, column++);
            // Console.WriteLine(@"/\  _`\   __                    /\_ \          /\ \__");
            // Console.SetCursorPosition(row, column++);
            // Console.WriteLine(@"\ \,\L\_\/\_\    ___ ___   __  _\//\ \      __ \ \ ,_\   ___   _ __");
            // Console.SetCursorPosition(row, column++);
            // Console.WriteLine(@" \/_\__ \\/\ \ /' __` __`\/\ \/\ \\ \ \   /'__`\\ \ \/  / __`\/\`'__\");
            // Console.SetCursorPosition(row, column++);
            // Console.WriteLine(@"   /\ \L\ \ \ \/\ \/\ \/\ \ \ \_\ \\_\ \_/\ \L\.\\ \ \_/\ \L\ \ \ \/");
            // Console.SetCursorPosition(row, column++);
            // Console.WriteLine(@"   \ `\____\ \_\ \_\ \_\ \_\ \____//\____\ \__/.\_\ \__\ \____/\ \_\");
            // Console.SetCursorPosition(row, column++);
            // Console.WriteLine(@"    \/_____/\/_/\/_/\/_/\/_/\/___/ \/____/\/__/\/_/\/__/\/___/  \/_/");

            // Form 3
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
            if (!filename.EndsWith(".bin")) filename = filename.Insert(filename.Length, ".bin");
            BinarySerializer.Serialize(filename.ToString(), MyShop);

        }
        static bool loadGame(string filename)
        {
            try
            {
                if (!filename.EndsWith(".bin")) filename = filename.Insert(filename.Length, ".bin");
                MyShop = BinarySerializer.Deserialize(filename) as Shop;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return false;
            }
        }

        static void Update()
        {
            time = time.AddHours(1);

            Console.SetCursorPosition(97, 30);
            Console.WriteLine(time);
        }


        static void Main(string[] args)
        {
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

            MyShop.print();
            Console.ReadKey();

            while (true)
            {
                Update();

                System.Threading.Thread.Sleep(Refresh);
                System.Console.Clear();
            }
        }
    }
}
