using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thesis_evaluation.models
{
    public class DetailsOfStepFunctionalMovement
    {
        public int join { get; set; }
        public double angle { get; set; }
        public double x { get; set; }
        public double y { get; set; }

        public DetailsOfStepFunctionalMovement clone()
        {
            return new DetailsOfStepFunctionalMovement()
            {
                join = this.join,
                angle = this.angle,
                x = this.x,
                y = this.y
            };
        }

        public static List<JointType> translateAngles(int angle)
        {
            switch (angle)
            {
                case 2: return new List<JointType>() { JointType.Neck, JointType.Head, JointType.SpineShoulder };
                case 4: return new List<JointType>() { JointType.ShoulderLeft, JointType.SpineShoulder, JointType.ElbowLeft };
                case 8: return new List<JointType>() { JointType.ShoulderRight, JointType.SpineShoulder, JointType.ElbowRight };
                case 5: return new List<JointType>() { JointType.ElbowLeft, JointType.ShoulderLeft, JointType.WristLeft };
                case 9: return new List<JointType>() { JointType.ElbowRight, JointType.ShoulderRight, JointType.WristRight };
                case 6: return new List<JointType>() { JointType.WristLeft, JointType.ElbowLeft, JointType.HandLeft };
                case 10: return new List<JointType>() { JointType.WristRight, JointType.ElbowRight, JointType.HandRight };
                case 1: return new List<JointType>() { JointType.SpineMid, JointType.SpineShoulder, JointType.SpineBase };
                case 12: return new List<JointType>() { JointType.HipLeft, JointType.SpineBase, JointType.KneeLeft };
                case 16: return new List<JointType>() { JointType.HipRight, JointType.SpineBase, JointType.KneeRight };
                case 13: return new List<JointType>() { JointType.KneeLeft, JointType.HipLeft, JointType.AnkleLeft };
                case 17: return new List<JointType>() { JointType.KneeRight, JointType.HipRight, JointType.AnkleRight };
                case 14: return new List<JointType>() { JointType.AnkleLeft, JointType.KneeLeft, JointType.FootLeft };
                case 18: return new List<JointType>() { JointType.AnkleRight, JointType.KneeRight, JointType.FootRight };
                default: return new List<JointType>();
            }
        }

        public static string getNameJoin(int join)
        {
            switch (join)
            {
                case 0: return "Centro de cadera";
                case 1: return "Centro de la espalda";
                case 2: return "Cuello";
                case 3: return "Cabeza";
                case 4: return "Hombro izquierdo";
                case 5: return "Codo izquierdo";
                case 6: return "Muñeca izquierda";
                case 7: return "Mano izquierda";
                case 8: return "Hombro derecho";
                case 9: return "Codo derecho";
                case 10: return "Muñeca derecha";
                case 11: return "Mano derecha";
                case 12: return "Cadera izquierda";
                case 13: return "Rodilla izquierda";
                case 14: return "Tobillo izquierdo";
                case 15: return "Pie izquierdo";
                case 16: return "Cadera derecha";
                case 17: return "Rodilla derecha";
                case 18: return "Tobillo derecho";
                case 19: return "Pie derecho";
                case 20: return "Centro de hombros";
                default: return "Sin articulacion";
            }
        }
    }
}
