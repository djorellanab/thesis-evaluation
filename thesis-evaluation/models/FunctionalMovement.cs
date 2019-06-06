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
        public float movementFactor { get; set; }
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
            int index = 0;
            foreach (float step in steps)
            {
                if (((step - movementFactor) <= progress) && (progress <= (step + movementFactor)))
                {
                    return index;
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
