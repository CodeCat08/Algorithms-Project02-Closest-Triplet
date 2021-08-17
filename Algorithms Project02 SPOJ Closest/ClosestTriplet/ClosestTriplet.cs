// //----------------------------------------------------------------------------------------------------------------------
// // File name: ClosestTriplet.cs
// // Project name: ClosestTriplet
// // Purpose: This program takes N points as input and returns the closest distance between 3 points of the N points...
// //----------------------------------------------------------------------------------------------------------------------
// // Creation Date: 10/15/2020
// //----------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing; //used for Point data structure
using System.Linq;

namespace ClosestTriplet
{
    /// <summary>
    ///     The ClosestTriplet class finds the shortest distance between 3 points from
    ///     an input of N points.
    /// </summary>
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
            while (true)
            {
                string nLine = Console.ReadLine(); //read the 0th line as a string
                if (nLine == "-1" || string.IsNullOrEmpty(nLine)) //if the line is "-1" or there's nothing to read...
                {
                    return; //exit the while loop
                } //end if (nLine == "-1" || string.IsNullOrEmpty(nLine))

                int.TryParse(nLine, out int n); //parse the 0th line as an integer 'N'

                
                var points = new Point[n]; //initialize a Point array of the 'N' size

                
                for (var i = 0; i < n; i++) //for every coordinate pair following the 'n' line
                {
                    //parse the line as X and Y components
                    string XYString = Console.ReadLine(); //read the XY components line
                    string[] fields = XYString.Split(' '); //split the line into separate components
                    int.TryParse(fields[0], out int x); //read the X component as an integer
                    points[i].X = x;
                    int.TryParse(fields[1], out int y); //read the Y component as an integer
                    points[i].Y = y;
                } //end for(var i = 0; i < n; i++)

                //find the smallest distance and print it to the computer output
                double d = findClosestThree(points); //set the smallest distance between 3 points
                Console.WriteLine(d.ToString("F3")); //print the smallest distance between 3 points
            } //end while(true)
        } //end of main()

        /// <summary>
        ///     Finds the shortest distance between three points of N points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>The shortest distance between three points in the array of points.</returns>
        private static double findClosestThree(Point[] points)
        {
            Array.Sort(points, new PointCompareByX()); //presort points by x-coordinates

            return closestTripletProblem(ref points); //return the smallest distance between 3 points of N points
        } //end findClosestThree(Point[])

        /// <summary>
        ///     Divide and Conquer algorithm to find the closest 3 points of N points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>The shortest distance of 3 points of N points.</returns>
        private static double closestTripletProblem(ref Point[] points)
        {
            if (points.Length < 3) //if there's less than 3 points...
            {
                return double.MaxValue; //return maximum distance as infinity
            } //end if(points.Length < 3)

            if (points.Length == 3) //if there's exactly 3 points
            {
                //calculate the distance between the 3 points
                double threeDist = dist(points[0], points[1]) + dist(points[1], points[2]) + dist(points[0], points[2]);
                return threeDist; //return that distance calculated
            } //end if(points.Length == 3)

            var d = double.MaxValue; //initialize the shortest distance as the maximum value possible

            Point midPoint = points[points.Length / 2]; //get the middle point of all points

            //divide the points into sub-lists left and right of midpoint
            Point[] left = points.Take(points.Length / 2).ToArray(); //store all points before the midpoint
            Point[] right = points.Skip(points.Length / 2).ToArray(); //store all points after the midpoint

            double minLeft = closestTripletProblem(ref left); //Find the shortest distance between 3 points in the left side recursively

            double minRight = closestTripletProblem(ref right); //Find the shortest distance between 3 points in the right side recursively

            d = min(minLeft, minRight); //Use the two sub-solutions and get the minimum of both to get the shortest distance 'd'

            var strip = new List<Point>(); //initialize a list called 'strip' which stores all points which are at most half the 'd'
                                           //distance away from middle line.
            foreach (Point point in points)
            {
                if (Math.Abs(point.X - midPoint.X) < (d / 2.0)) //if the point is less than half 'd' distance from the mid-point...
                {
                    strip.Add(point); //add the point to the strip
                } //end if(Math.Abs(point.X - midPoint.X) < (d / 2.0))
            } //end foreach(Point point in points)

            double dStrip = findShortestInStrip(ref strip, d); //find smallest distance in 'strip'

            return min(d, dStrip); //return closest distance between 3 points
        } //end closestTripletProblem(Point[])

        /// <summary>
        ///     Finds the 3 closest points in the strip.
        /// </summary>
        /// <param name="strip">The strip.</param>
        /// <param name="minLeftRight">The minimum distance of the left and right point arrays.</param>
        /// <returns>The shortest distance between 3 points of the points in the strap.</returns>
        private static double findShortestInStrip(ref List<Point> strip, double minLeftRight)
        {
            
            strip.Sort(new PointCompareByY()); //sort the points in the strip by their Y-components

            //compare each triplet of points to each other
            for (var i = 0; i < strip.Count; i++)
            {
                for (int j = i + 1; j < strip.Count; j++)
                {
                    for (int k = j + 1; k < strip.Count; k++)
                    {
                        //get the distance between 3 points in the 'strip' and store the distance in 'stripShortestDist'
                        double stripShortestDist = dist(strip[i], strip[j]) + dist(strip[j], strip[k]) + dist(strip[i], strip[k]);
                        minLeftRight = min(minLeftRight, stripShortestDist); //if it's shorter than the shortest distance found so far,
                                                                                 //then update the shortest distance to it.
                    } //end for(int k = j + 1; k < strip.Count; k++)
                } //end for(int j = i + 1; j < strip.Count; j++)
            } //end for(var i = 0; i < strip.Count; i++)

            return minLeftRight; //return the shortest distance between 3 points in the 'strip'
        } //end findShortestDistanceInStrip(ref List<Point>, double)

        /// <summary>
        ///     Gets the distance between two specified points.
        /// </summary>
        /// <param name="p1">The point 'p1'.</param>
        /// <param name="p2">The point 'p2'.</param>
        /// <returns>Euclidean distance between three specified points.</returns>
        private static double dist(Point p1, Point p2)
        {
            int x = p1.X - p2.X; //get the difference between the points' X-components
            int y = p1.Y - p2.Y; //get the difference between the points' Y-components
            double distance = Math.Pow(x, 2) + Math.Pow(y, 2); //add the squares together
            return Math.Sqrt(distance); //take the square root of the sum to get the total distance between points
        } //end dist(Point, Point)

        /// <summary>
        ///     Compares value a to value b and returns the smaller
        ///     of the two.
        /// </summary>
        /// <param name="a">The value a.</param>
        /// <param name="b">The value b.</param>
        /// <returns>The smaller of the two specified values</returns>
        private static double min(double a, double b)
        {
            //compare value a to value b
            if (a < b)
            {
                return a; //return a if it's smaller than b
            } //end if(a < b)

            return b; //return b if it's smaller than a
        } //end min(double, double)
    } //end class ClosestTriplet

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
            } //end if(p1.X > p2.X)

            if (p1.X < p2.X)
            {
                return -1;
            } //end if(p1.X < p2.X)

            if (p1.Y > p2.Y)
            {
                return 1;
            } //end if(p1.Y > p2.Y)

            if (p1.Y < p2.Y)
            {
                return -1;
            } //end if(p1.Y < p2.Y)

            return 0;
        } //end Compare(Point, Point)
    } //end class PointCompareByX

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
            } //end if(p1.Y > p2.Y)

            if (p1.Y < p2.Y)
            {
                return -1;
            } //end if(p1.Y < p2.Y)

            if (p1.X > p2.X)
            {
                return 1;
            } //end if(p1.X > p2.X)

            if (p1.X < p2.X)
            {
                return -1;
            } //end if(p1.X < p2.X)

            return 0;
        } //end Compare(Point, Point)
    } //end class PointCompareByY
} //end namespace
