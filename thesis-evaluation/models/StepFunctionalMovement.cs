using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thesis_evaluation.models
{
    public class StepFunctionalMovement
    {
        public string functionalMovement { get; set; }
        public int step { get; set; }
        public double time { get; set; }
        public double accumulate { get; set; }
        public bool clasification { get; set; }
        public List<DetailsOfStepFunctionalMovement> detailsOfStepFunctionalMovement { get; set; }
        public string pathImage { get; set; }
        public bool status { get; set; }
        public float factorMovement { get; set; }



        public static StepFunctionalMovement createStep(List<DetailsOfStepFunctionalMovement> details, int _step,
            string _functionalMovement, double _time, double _accumulate, float _factorMovement)
        {
            return new StepFunctionalMovement()
            {
                clasification = false,
                detailsOfStepFunctionalMovement = details,
                functionalMovement = _functionalMovement,
                status = true,
                step = _step,
                time = _time,
                accumulate = _accumulate,
                factorMovement = _factorMovement
            };
        }

    }
}
