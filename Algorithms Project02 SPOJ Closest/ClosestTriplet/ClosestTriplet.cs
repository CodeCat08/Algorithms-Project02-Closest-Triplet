using System;
using System.Collections.Generic;
using System.Drawing;

namespace ClosestTriplet
{
    /// <summary>
    /// The ClosestTriplet class finds the shortest distance between 3 points from
    /// an input of N points.
    /// </summary>
    /// <author>
    /// David Nelson
    /// </author>
    /// <remarks>
    /// Date Created: 10/15/2020
    /// </remarks>
    public static class ClosestTriplet
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        public static void Main()
        {
            Point[] points = loadPoints();

            double d = findClosestThree(points);

            Console.WriteLine($"{d:F}");
        }

        private static double findClosestThree(Point[] points)
        {
            throw new NotImplementedException();
        }

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