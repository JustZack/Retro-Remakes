using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math2D
{

    /// <summary>
    /// Class which contains the cartesian (x,y) coordinates for a point.
    /// </summary>
    public class Point {

        /// <summary>
        /// The 'x' position of the point.
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// The 'y' position of the point.
        /// </summary>
        public double Y { get; set; }

        /// <param name="x">The 'x' position of the point.</param>
        /// <param name="y">The 'y' position of the point.</param>
        public Point(double x = 0, double y = 0) {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Moves the point by the specified x and specified y amount. 
        /// </summary>
        /// <param name="x">The amount to move along the x axis</param>
        /// <param name="y">The amount to move along the y axis</param>
        public void transform(int x, int y) {
            this.X += x;
            this.Y += y;
        }

        override public string ToString() {
            return X + ", " + Y;
        }

        public static Point operator -(Point p1, Point p2) {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }
        public static Point operator +(Point p1, Point p2) {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

    }


    /// <summary>
    /// Class which contains a line based on cartesian (x,y) coerdinates.
    /// </summary>
    public class Line {
        private Point p1;
        public Point P1 {
            get {
                return p1;
            }
            set {
                this.p1 = value;
                invalidated(false);
            }
        }

        private Point p2;
        public Point P2 {
            get {
                return p2;
            }
            set {
                this.p2 = value;
                invalidated(false);
            }
        }

        
        /// <summary>
        /// The Distance from P1.X to P2.X
        /// </summary>
        public double DeltaX { get; private set; }
        /// <summary>
        /// The Distance from P1.Y to P2.Y
        /// </summary>
        public double DeltaY { get; private set; }

        private double degree;
        /// <summary>
        /// The rotation of the amount in degrees, using P1 as the origin.
        /// Upon setting, P2 will be adjusted accordingly.
        /// </summary>
        public double Degree {
            get {
                return this.degree;
            }
            set {
                this.degree = value;
                invalidated(true);
            }
        }

        private double length;
        /// <summary>
        /// The length of the line.
        /// Upon setting, P2 will be adjusted to fit this requirement.
        /// </summary>
        public double Length {
            get {
                return length;
            }
            set {
                length = value;
                invalidated(true);
            }

        }

        /// <summary>
        /// The slope of the line, independent of P2.X coming before P1.X on the numer Line.
        /// I.E. If Points are entered out of order the slope will always be correct.
        /// </summary>
        public double Slope { get; private set; }
        private bool slopeInfinite;

        private void init(int x1 = 0, int x2 = 0, int y1 = 0, int y2 = 0) {
            p1 = new Point(x1, y1);
            p1 = new Point(x2, y2);
        }
        public Line(Point p1, Point p2) {
            this.p1 = p1;
            this.p2 = p2;
            calcAll(false);
        }
        public Line(Point p, double length, double degree) {
            this.p1 = p;
            this.length = length;
            this.degree = degree;
            calcAll(true);
        }
        public Line(int x1 = 0, int x2 = 0, int y1 = 0, int y2 = 0) {
            init(x1, x2, y1, y2);
            calcAll(false);
        }
        public Line(double length, double degree, int x = 0, int y = 0) {
            this.degree = degree;
            this.length = length;
            init(x, y);
            calcAll(true);
        }


        /// <summary>
        /// Moves the Line (Both points) by the specified x and specified y amount. 
        /// </summary>
        /// <param name="x">The amount to move along the x axis</param>
        /// <param name="y">The amount to move along the y axis</param>
        public void transform(int x, int y) {
            this.P1.transform(x, y);
            this.P2.transform(x, y);
        }

        public override string ToString() {
            return "Line: \t(" + p1.ToString() + ") --> (" + p2.ToString() + ")\n"
                 + "\tLength: " + this.length + "\n"
                 + "\tDegree: " + this.degree + "\n"
                 + "\tSlope: " + (this.slopeInfinite ? "Inf" : this.Slope + "") + "\n";
        }

        private void calcAll(bool with_degree_or_point) {
            if (with_degree_or_point) {
                p2 = new Point();
                double conversionR = (Math.PI / 180);
                p2.X = (Math.Sin(this.degree * conversionR) * this.length) + p1.X;
                p2.Y = (Math.Cos(this.degree * conversionR) * this.length) + p1.Y;
                calcDelta_Slope();
            } else {
                calcDelta_Slope();
                double conversionD = (180 / Math.PI);
                this.degree = Math.Atan2(this.DeltaY, this.DeltaX) * conversionD;
                this.length = Math.Sqrt(Math.Pow(this.DeltaX, 2) + Math.Pow(this.DeltaY, 2));
            }
        }
        private void calcDelta_Slope() {
            this.slopeInfinite = false;
            this.DeltaX = this.p2.X - this.p1.X;
            this.DeltaY = this.p2.Y - this.p1.Y;

            if (this.p1.X < this.p2.X) {          //Normal
                this.Slope = this.DeltaY / this.DeltaX;
            } else if (this.p2.X < this.p1.X) {   //Reverse
                this.Slope = this.DeltaY / (this.p1.X - this.p2.X);
            } else if (this.p1.X == this.p2.X) {  //Equal
                this.Slope = 0;
                this.slopeInfinite = true;
            }
        }
        private void invalidated(bool with_degree_or_point) {
            calcAll(with_degree_or_point);
        }
    }

    /// <summary>
    /// Class which contains a polygon based on cartesian (x,y) coerdinates.
    /// </summary>
    public class Polygon {
        private Line[] lines;
        /// <summary>
        /// All of the lines in the Polygon.
        /// Upon setting, Points will be reevaluated.
        /// </summary>
        public Line[] Lines {
            get {
                return lines;
            }
            set {
                this.lines = value;
                this.points = new Point[lines.Length];
                setPoints();
            }
        }
        private Point[] points;
        /// <summary>
        /// All of the Points in the Polygon.
        /// Upon setting, Lines will be reevaluated.
        /// </summary>
        public Point[] Points {
            get {
                return points;
            }
            set {
                points = value;
                lines = new Line[this.points.Length];
                setLines();
            }
        }

        private void setLines() {
            refrencePoints = true;
            int lineCounter = 0;
            for (int i = 0; i < points.Count(); i++) {
                if (i <= points.Count() - 2) { //Main case
                    lines[lineCounter++] = new Line(points[i], points[i + 1]);
                } else if (i == points.Count() - 1) { //Once at end of loop.
                    lines[lineCounter] = new Line(points[i], points[0]);
                }
            }
        }
        private void setPoints() {
            refrencePoints = false;
            for (int i = 0; i < lines.Count(); i++)
                points[i] = lines[i].P1;
        }

        private bool refrencePoints;
        public Polygon() {
            lines = new Line[0];
            points = new Point[0];
            refrencePoints = true;
        }
        public Polygon(params Point[] points) {
            if (points.Length < 3) throw new Exception("Polygons must have three points minimun.");
            this.Points = points;
        }
        public Polygon(params Line[] lines) {
            if (lines.Length < 3) throw new Exception("Polygons must have three points minimun.");
            this.Lines = lines;
        }

        /// <summary>
        /// Moves the Polygon (Lines and Points) by the specified x and specified y amount. 
        /// </summary>
        /// <param name="x">The amount to move along the x axis</param>
        /// <param name="y">The amount to move along the y axis</param>
        public void transform(int x, int y) {
            if (refrencePoints) foreach (Point p in points) p.transform(x, y);
            else                foreach (Line l in lines) l.transform(x, y);
        }

        public override string ToString() {
            string s = "";

            s += "Points: ";
            foreach (Point p in this.points) s += "(" + p.ToString() + ") --> ";
            s = s.Remove(s.Length - 4);
            s += "\n\n";
            s += "Lines: \n";
            foreach (Line l in this.lines) s += "{" + l.ToString() + "},\n";
            s = s.Remove(s.Length - 2);
            s += '\n';

            return s;
        }
    }


    /// <summary>
    /// Builds a three sided polygon.
    /// Child class to make generating triangles easier.
    /// </summary>
    public class Triangle : Polygon {
        public Triangle(Point p1, Point p2, Point p3) : base(p1, p2, p3) { }
        public Triangle(Line l1, Line l2, Line l3) : base(l1, l2, l3) { }
        public Triangle(int x1, int y1, int x2, int y2, int x3, int y3) : base(new Point(x1, y1), new Point(x2, y2), new Point(x3, y3)) { }
    }

    /// <summary>
    /// Builds a four sided polygon.
    /// Child class to make generating rectagles (or squares) easier.
    /// </summary>
    public class Rectangle : Polygon {
        public Rectangle(Point p1, Point p2, Point p3, Point p4) : base(p1, p2, p3, p4) { }
        public Rectangle(Line l1, Line l2, Line l3, Line l4) : base(l1, l2, l3, l4) { }
        public Rectangle(int x1, int y1, int x2, int y2,
                         int x3, int y3, int x4, int y4) : base(new Point(x1, y1),
                                                                        new Point(x2, y2),
                                                                        new Point(x3, y3),
                                                                        new Point(x4, y4)) { }
    }
}
