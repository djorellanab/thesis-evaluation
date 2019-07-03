using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thesis_evaluation.models
{
    public class FunctionalMovement
    {
        public string _ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<decimal> steps { get; set; }
        public List<int> anglesOfMovement { get; set; }
        public decimal movementFactor { get; set; }
        public decimal height { get; set; }
        public float depthMin { get; set; }
        public float depthMax { get; set; }
        public string time_stamp { get; set; }
        public string time_stamp_hour { get; set; }
        public bool state { get; set; }
        public string file { get; set; }
        public List<string> stepsFunctionalMovement { get; set; }
        public int focusJoin { get; set; }

        public int getStep(float progress)
        {
            if (progress < 0)
            {
                return -1;
            }
            int index = 0;
            decimal dis = steps[1] * movementFactor;
            foreach (decimal step in steps)
            {
                if (index == 0)
                {
                    decimal max = dis + step;
                    if (((float)step <= progress) && (progress <= (float)max))
                    {
                        return index;
                    }
                }
                else if (index == (steps.Count - 1))
                {
                    decimal min = step - dis;
                    if (((float)min <= progress) && (progress <= (float)step))
                    {
                        return index;
                    }
                }
                else
                {
                    decimal avg = dis / 2;
                    decimal min = step - avg;
                    decimal max = step + avg;
                    if (((float)min <= progress) && (progress <= (float)max))
                    {
                        return index;
                    }
                }
                index++;
            }
            return -1;
        }

        public bool isCalibrate(float depth)
        {
            return (this.depthMin <= depth) && (depth <= this.depthMax);
        }
    }
}
