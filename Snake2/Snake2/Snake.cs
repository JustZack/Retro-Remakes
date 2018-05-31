using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake2
{
    class Snake
    {

        public uint length { get; private set; }
        public uint maxlength { get; private set; }
        public bool isGrowing { get; private set; }

        public Direction direction { get; set; }

        public Queue<Point> snake;
        public Point head;

        public bool selfCollision { get; private set; }

        public Snake(int startx, int starty, uint startlength = 5, Direction starting_direction = Direction.LEFT) {
            length = 0;
            maxlength = startlength;
            direction = starting_direction;
            snake = new Queue<Point>(10);
            snake.Enqueue(new Point(startx, starty));
            head = snake.First();
        }

        public Point move() {
            if (length < maxlength) isGrowing = true;
            else isGrowing = false;

            Point pnew = new Point(snake.Last().x, snake.Last().y);

            switch (direction) {
                case Direction.UP:
                    pnew.y--;
                    break;
                case Direction.RIGHT:
                    pnew.x++;
                    break;
                case Direction.DOWN:
                    pnew.y++;
                    break;
                case Direction.LEFT:
                    pnew.x--;
                    break;
            }

            foreach (Point p in snake)
                if (p.x == pnew.x && p.y == pnew.y)
                    selfCollision = true;
            snake.Enqueue(pnew);
            head = pnew;
            if (isGrowing) {
                length++;
                return null;
            } else return snake.Dequeue();
        }

        public void addLength(uint amount) {
            maxlength += amount;
        }
    }

    public class Point
    {
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x { get; set; }
        public int y { get; set; }
    }

    public enum Direction { UP, RIGHT, LEFT, DOWN }
}
