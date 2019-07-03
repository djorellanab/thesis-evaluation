using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace thesis_evaluation
{
    /// <summary>
    /// Clase que enlista todos los angulos de movimientos
    /// </summary>
    public class KinectAngleBody
    {
        public KinectAngleBody()
        {
        }

        public static double getAngleBody(Point origenPoint, Point p1, Point p2)
        {

            List<double> _origenPoint = new List<double> { origenPoint.X, origenPoint.Y };
            List<double> _p1 = new List<double> { p1.X, p1.Y };
            List<double> _p2 = new List<double> { p2.X, p2.Y };

            _p1 = KinectAngleBody.GetTraslationPoint(_origenPoint, _p1);
            _p2 = KinectAngleBody.GetTraslationPoint(_origenPoint, _p2);

            return KinectAngleBody.getVectorAngle(_p1, _p2);
        }

        public static double getVectorAngle(List<double> p1, List<double> p2)
        {
            if (p1 == null || p2 == null)
            {
                throw new Exception("all points have to exist");
            }
            if (p1.Count != p2.Count)
            {
                throw new Exception("All points have to same space");
            }
            double dotProductPoint = KinectAngleBody.GetDotProduct(new List<List<double>>() { p1, p2 });
            double productMagnitude = KinectAngleBody.GetMagnitude(p1, new List<double> { 0, 0 });
            productMagnitude *= KinectAngleBody.GetMagnitude(p2, new List<double> { 0, 0 });

            dotProductPoint /= productMagnitude;

            double radians = Math.Acos(dotProductPoint);
            return (radians * 180) / Math.PI;
        }

        public static List<double> GetTraslationPoint(List<double> origenPoint, List<double> refPoint)
        {
            if (origenPoint == null || refPoint == null)
            {
                throw new Exception("All points have to exist");
            }
            if (origenPoint.Count != refPoint.Count)
            {
                throw new Exception("All points have to same space");
            }
            for (int i = 0; i < origenPoint.Count; i++)
            {
                refPoint[i] -= origenPoint[i];
            }
            return refPoint;
        }

        public static double GetMagnitude(List<double> point, List<double> point2)
        {
            if (point == null || point2 == null)
            {
                throw new Exception("Point has to exist");
            }
            if (point.Count != point2.Count)
            {
                throw new Exception("All points have to same space");
            }
            double total = 0;
            for (int i = 0; i < point.Count; i++)
            {
                double dif = point[i] - point2[i];
                total += Math.Pow(dif, 2);
            }
            return Math.Sqrt(total);
        }
        public static double GetDotProduct(List<List<double>> points)
        {
            if (points == null)
            {
                throw new Exception("its doenst have points");
            }
            if (points.Count < 2)
            {
                throw new Exception("its necessary two or more points");
            }
            int space = points[0].Count;
            foreach (List<double> point in points)
            {
                if (point.Count != space)
                {
                    throw new Exception("All points have to same space");
                }
            }
            List<double> totals = new List<double>();
            for (int i = 0; i < space; i++)
            {
                totals.Add(1);
            }
            foreach (List<double> point in points)
            {
                for (int i = 0; i < space; i++)
                {
                    totals[i] *= point[i];
                }
            }
            double final = 0;
            foreach (double total in totals)
            {
                final += total;
            }
            return final;
        }
    }
}
