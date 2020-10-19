// //----------------------------------------------------------------------------------------------------------------------
// // File name: ClosestTriplet.cs
// // Project name: ClosestTriplet
// // Purpose: This program takes N points as input and returns the closest distance between 3 points of the N points..
// //----------------------------------------------------------------------------------------------------------------------
// // Programmer name: David Nelson (nelsondk@etsu.edu)
// // Course Name: CSCI 3230 Algorithms
// // Course Section: 901
// // Creation Date: 10/15/2020
// //----------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ClosestTriplet
{
    /// <summary>
    ///     The ClosestTriplet class finds the shortest distance between 3 points from
    ///     an input of N points.
    /// </summary>
    /// <author>
    ///     David Nelson
    /// </author>
    /// <remarks>
    ///     Date Created: 10/15/2020
    /// </remarks>
    public static class ClosestTriplet
    {
        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        public static void Main()
        {
            Point[] points = loadPoints();

            double d = findClosestThree(points);

            Console.WriteLine($"{d:F3}");
        }

        /// <summary>
        ///     Finds the shortest distance between three points of N points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>The shortest distance between three points in the array of points.</returns>
        private static double findClosestThree(Point[] points)
        {
            //presort points by x-coordinates
            Array.Sort(points, new PointCompareByX());

            return closestTripletProblem(ref points);
        }

        /// <summary>
        ///     Divide and Conquer algorithm to find the closest 3 points of N points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>The shortest distance of 3 points of N points.</returns>
        private static double closestTripletProblem(ref Point[] points)
        {
            if (points.Length < 3)
            {
                return double.MaxValue;
            }

            if (points.Length == 3)
            {
                double threeDist = dist(points[0], points[1]) + dist(points[1], points[2]) + dist(points[0], points[2]);
                return threeDist;
            }

            var d = double.MaxValue;

            //get the middle point
            Point midPoint = points[points.Length / 2];

            //divide the points into sub-lists left and right of midpoint
            Point[] left = points.Take(points.Length / 2).ToArray();
            Point[] right = points.Skip(points.Length / 2).ToArray();

            //Find closest sortedByY in left recursively
            double minLeft = closestTripletProblem(ref left);

            //Find closest sortedByY in right recursively
            double minRight = closestTripletProblem(ref right);

            //Use two sub-solutions and get minimum of both to get
            //shortest distance 'd'
            d = min(minLeft, minRight);

            //create an array called 'strip' which stores all points
            //which are at most 'd' distance away from middle line
            var strip = new List<Point>();
            foreach (Point point in points)
            {
                if (Math.Abs(point.X - midPoint.X) < d)
                {
                    strip.Add(point);
                }
            }

            //find smallest distance in 'strip'
            double dStrip = findShortestInStrip(ref strip, d);

            //return closest distance
            return min(d, dStrip);
        }

        /// <summary>
        /// Finds the 3 closest points in the strip.
        /// </summary>
        /// <param name="strip">The strip.</param>
        /// <param name="minLeftRight">The minimum distance of the left and right point arrays.</param>
        /// <returns>The shortest distance between 3 points of the points in the strap.</returns>
        private static double findShortestInStrip(ref List<Point> strip, double minLeftRight)
        {
            strip.Sort(new PointCompareByY());

            for (var i = 0; i < strip.Count; i++)
            {
                for (int j = i + 1; j < strip.Count; j++)
                {
                    for (int k = j + 1; k < strip.Count; k++)
                    {
                        double stripShortestDist = dist(strip[i], strip[j]) + dist(strip[j], strip[k]) + dist(strip[i], strip[k]);
                        minLeftRight = min(minLeftRight, stripShortestDist);
                    }
                }
            }

            return minLeftRight;
        }

        /// <summary>
        ///     Gets the distance between two specified points.
        /// </summary>
        /// <param name="p1">The point 'p1'.</param>
        /// <param name="p2">The point 'p2'.</param>
        /// <returns>Euclidean distance between three specified points.</returns>
        private static double dist(Point p1, Point p2)
        {
            int x = p1.X - p2.X;
            int y = p1.Y - p2.Y;
            double distance = Math.Pow(x, 2) + Math.Pow(y, 2);
            return Math.Sqrt(distance);
        }

        /// <summary>
        ///     Compares value a to value b and returns the smaller
        ///     of the two.
        /// </summary>
        /// <param name="a">The value a.</param>
        /// <param name="b">The value b.</param>
        /// <returns>The smaller of the two specified values</returns>
        private static double min(double a, double b)
        {
            if (a < b)
            {
                return a;
            }

            return b;
        }

        /// <summary>
        ///     Loads the points list into an array and returns the array of points.
        /// </summary>
        /// <returns>an array of points</returns>
        /// <exception cref="ArgumentNullException">
        ///     1st line of input can't be null
        ///     or
        ///     Point {i + 1} can't be null
        /// </exception>
        /// <exception cref="FormatException">Value \"{nString}\" could not be converted to an integer</exception>
        private static Point[] loadPoints()
        {
            //read the number of points to be input
            string nString = Console.ReadLine() ?? throw new ArgumentNullException("1st line of input can't be null");
            bool isValidN = int.TryParse(nString, out int n);
            if (!isValidN)
            {
                throw new FormatException($"Value \"{nString}\" could not be converted to an integer");
            }

            var points = new Point[n]; //initialize array of 'n' points

            //assign each point in 'points' with an X and Y value
            for (var i = 0; i < n; i++)
            {
                string pointString = Console.ReadLine() ?? throw new ArgumentNullException($"Point {i + 1} can't be null");
                string[] values = pointString.Split(' ');
                int.TryParse(values[0], out int tempX);
                int.TryParse(values[1], out int tempY);
                points[i].X = tempX;
                points[i].Y = tempY;
            }

            return points; //return the points
        }
    }

    /// <summary>
    ///     The PointCompareByY class compares two points by their X values.
    /// </summary>
    /// <seealso cref="Point" />
    /// <author>
    ///     Professor Christopher Wallace
    ///     Used with permission
    /// </author>
    /// <remarks>
    ///     Date Created: 10/15/2020
    /// </remarks>
    internal class PointCompareByX : IComparer<Point>
    {
        /// <summary>
        ///     Compares the specified point 'p1' to point 'p2'.
        /// </summary>
        /// <param name="p1">The point 'p1'.</param>
        /// <param name="p2">The point 'p2'.</param>
        /// <returns>1 if 'p1' is larger than 'p2'; -1 if 'p1' is smaller than 'p2'.</returns>
        public int Compare(Point p1, Point p2)
        {
            if (p1.X > p2.X)
            {
                return 1;
            }

            if (p1.X < p2.X)
            {
                return -1;
            }

            if (p1.Y > p2.Y)
            {
                return 1;
            }

            if (p1.Y < p2.Y)
            {
                return -1;
            }

            return 0;
        }
    }

    /// <summary>
    ///     The PointCompareByY class compares two points by their Y values.
    /// </summary>
    /// <seealso cref="Point" />
    /// <author>
    ///     Professor Christopher Wallace
    ///     Used with permission
    /// </author>
    /// <remarks>
    ///     Date Created: 10/15/2020
    /// </remarks>
    internal class PointCompareByY : IComparer<Point>
    {
        /// <summary>
        ///     Compares the specified point 'p1' to point 'p2'.
        /// </summary>
        /// <param name="p1">The point 'p1'.</param>
        /// <param name="p2">The point 'p2'.</param>
        /// <returns>1 if p1 greater than p2; -1 if p1 less than p2 </returns>
        public int Compare(Point p1, Point p2)
        {
            if (p1.Y > p2.Y)
            {
                return 1;
            }

            if (p1.Y < p2.Y)
            {
                return -1;
            }

            if (p1.X > p2.X)
            {
                return 1;
            }

            if (p1.X < p2.X)
            {
                return -1;
            }

            return 0;
        }
    }
}