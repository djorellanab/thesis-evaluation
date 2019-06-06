using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thesis_evaluation.models
{
    public class ResultAI
    {
        public double x { get; set; }
        public double y { get; set; }
        public double angle { get; set; }
        public double accumulateTime { get; set; }
        public double time { get; set; }
        public int joint { get; set; }
        public int step { get; set; }
        public int repetition { get; set; }
        public int serie { get; set; }
    }
}
