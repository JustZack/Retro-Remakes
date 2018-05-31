using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Snake2
{
    class Board
    {
        Snake s;

        List<Point> mice;

        int width;
        int height;

        public Board() { 
            Console.CursorVisible = false;
            Console.CursorSize = 100;
        }

        public void start() {
            width = Console.WindowWidth;
            height = Console.WindowHeight - 2;
            s = new Snake(width / 2, height / 2);
            mice = new List<Point>(2);
            drawBoard();
            loop();
        }

        private void drawBoard() {
            Console.Clear();

            ConsoleColor[] prev = prevCC();
            setCC(ConsoleColor.White);

            for (int i = 0; i < width; i++) {
                writeCP(i, 0);
                writeCP(i, height - 1);
            }
            for (int i = 0; i < height; i++) {
                writeCP(0, i);
                writeCP(width - 1, i);
            }
            setCC(prev);
        }

        #region Console Utility Methods
        private ConsoleColor[] prevCC() {
            return new ConsoleColor[2]{ Console.ForegroundColor, Console.BackgroundColor };
        }
        private void setCC(ConsoleColor cc) {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = cc;
        }
        private void setCC(ConsoleColor[] cc) {
            Console.ForegroundColor = cc[0];
            Console.BackgroundColor = cc[1];
        }
        private void setCC(ConsoleColor cc_f, ConsoleColor cc_b){
            Console.ForegroundColor = cc_f;
            Console.BackgroundColor = cc_b;
        }
        private void writeCP(int left, int top, char c = '0') {
            Console.CursorLeft = left;
            Console.CursorTop = top;
            Console.Write(c);
        }
        #endregion

        private void message(string message) {
            ConsoleColor[] prev = prevCC();
            setCC(ConsoleColor.Black);

            for (int i = 0; i < width; i++) writeCP(i, height);
            setCC(ConsoleColor.White, ConsoleColor.Black);
            Console.SetCursorPosition(0, height);
            Console.Write(message);
            setCC(prev);
        }

        private void addMouse() {
            Random r = new Random();
            int x = 0, y = 0;
            bool works;
            do {
                works = true;
                x = r.Next(2, width - 3);
                y = r.Next(2, height - 4);
                foreach (Point p in s.snake)
                    if (x == p.x && y == p.y) {
                        works = false;
                    }

            } while (!works);

            ConsoleColor[] prev = prevCC();
            setCC(ConsoleColor.Red);
            mice.Add(new Point(x, y));
            writeCP(mice.Last().x, mice.Last().y);
            setCC(prev);
        }

        private void reset(string s_message) {
            message(s_message + " Your final score was: " + s.length);
            while (Console.ReadKey().Key != ConsoleKey.R) ;
            this.start();
        }

        private void loop() {
            uint prevS = s.length;
            while (true) {
                if (Console.KeyAvailable) {
                    ConsoleKey key = Console.ReadKey(false).Key;
                    Direction dnew = Direction.UP;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            dnew = Direction.UP;
                            break;
                        case ConsoleKey.RightArrow:
                            dnew = Direction.RIGHT;
                            break;
                        case ConsoleKey.DownArrow:
                            dnew = Direction.DOWN;
                            break;
                        case ConsoleKey.LeftArrow:
                            dnew = Direction.LEFT;
                            break;
                        default: break;
                    }

                    if ((dnew == Direction.UP && s.direction == Direction.DOWN)
                    || (dnew == Direction.DOWN && s.direction == Direction.UP)
                    || (dnew == Direction.LEFT && s.direction == Direction.RIGHT)
                    || (dnew == Direction.RIGHT && s.direction == Direction.LEFT));
                    else s.direction = dnew;
                }
                if (prevS != s.length) {
                    prevS = s.length;
                    message("Score: " + s.length);
                }

                ConsoleColor[] prev = prevCC();

                Point tail = s.move();
                if (tail != null){
                    setCC(ConsoleColor.Black);
                    writeCP(tail.x, tail.y);
                }

                setCC(ConsoleColor.Green);
                Point head = s.head;
                writeCP(head.x, head.y);
                setCC(prev);

                for (int i = 0; i < mice.Count; i++)
                    if (head.x == mice[i].x && head.y == mice[i].y){
                        s.addLength(2);
                        mice.RemoveAt(i);
                    }

                if (mice.Count < 3) addMouse();

                if (s.head.x <= 1 || s.head.x >= width - 2
                 || s.head.y <= 1 || s.head.y >= height - 2) reset("You hit the wall!");
                else if (s.selfCollision)                    reset("You ran into your self!");

                Thread.Sleep(50);
            }
        }
    }
}
