using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thesis_evaluation.models
{
    public class AnalyzerOfMovementFunctional
    {
        private List<List<List<StepFunctionalMovement>>> stepsByMovement;
        private FunctionalMovement functionalMovement;
        private VariablesAnalyzer variablesAnalyzer;
        public ReportAnalyzer report { get; set; }

        public AnalyzerOfMovementFunctional(List<List<List<StepFunctionalMovement>>> _stepsByMovement,
            FunctionalMovement _functionalMovement, int _workTime, int _restTime, int _series)
        { 
            this.variablesAnalyzer = new VariablesAnalyzer() { workTime=_workTime, restTime=_restTime, series = _series};
            this.stepsByMovement = _stepsByMovement;
            this.functionalMovement = _functionalMovement;
            this.report = maping();
        }

        private ReportAnalyzer maping()
        {
            List<EnduranceAnalyzer> datasetEndurance = new List<EnduranceAnalyzer>();
            List<AgilityAnalyzer> dataSetAgilityAnalizer = new List<AgilityAnalyzer>();
            PowerAnalyzer dataPower = new PowerAnalyzer()
            {
                repetitions = int.MinValue,
                time = double.MinValue
            };
            SpeedAnalyzer dataSpeed = new SpeedAnalyzer()
            {
                repetitionsBySerie = 0,
                timeByRepetition = 0
            };
            List<List<object>> datasetIA = new List<List<object>>();

            List<List<double>> datasetAgility = new List<List<double>>();
            List<double> datasetLastStep = new List<double>();
            List<Dictionary<int, List<double>>> datasetStepFlexibility = new List<Dictionary<int, List<double>>>();
            List<int> datasetRepetitions = new List<int>();

            for (int i = 0; i < this.variablesAnalyzer.series; i++)
            {
                EnduranceAnalyzer endurance = new EnduranceAnalyzer()
                {
                    uid = $"d{i}",
                    label = $"Descanso ${i + 1}",
                    showLine = true,
                    data = new List<PointAnalyzer>()
                };
                double inicio = i * this.variablesAnalyzer.workTime;
                double fin = (i * this.variablesAnalyzer.workTime) + this.variablesAnalyzer.restTime;
                endurance.data.Add(new PointAnalyzer() { x = inicio, y = 0 });
                endurance.data.Add(new PointAnalyzer() { x = fin, y = 0 });
                datasetEndurance.Add(endurance);
            }

            for (int i = 0; i < functionalMovement.steps.Count; i++)
            {
                datasetAgility.Add(new List<double>());
                Dictionary<int, List<double>> dataSetFlexibility = new Dictionary<int, List<double>>();
                foreach (int flexibility in functionalMovement.anglesOfMovement)
                {
                    dataSetFlexibility.Add(flexibility, new List<double>());
                }
                datasetStepFlexibility.Add(dataSetFlexibility);
            }

            for (int series = 0; series < stepsByMovement.Count; series++)
            {
                List<List<StepFunctionalMovement>> _serie = stepsByMovement[series];
                EnduranceAnalyzer endurance = new EnduranceAnalyzer()
                {
                    uid = $"s{series}",
                    label = $"Serie {series + 1}",
                    showLine = true,
                    data = new List<PointAnalyzer>()
                };
                int repetitions = 0;
                for (repetitions = 0; repetitions < _serie.Count; repetitions++)
                {
                    List<StepFunctionalMovement> _repetition = _serie[repetitions];
                    int steps = 0;
                    for (steps = 0; steps < _repetition.Count; steps++)
                    {
                        List<object> dataIA = new List<object>();
                        StepFunctionalMovement _step = _repetition[steps];
                        dataIA.Add(_step.step);
                        dataIA.Add(_step.factorMovement);
                        _step.detailsOfStepFunctionalMovement = _step.detailsOfStepFunctionalMovement.OrderBy(x => x.join).ToList();
                        foreach (DetailsOfStepFunctionalMovement detail in _step.detailsOfStepFunctionalMovement)
                        {
                            datasetStepFlexibility[steps][detail.join].Add(detail.angle);
                            dataIA.Add(detail.angle);
                        }
                        datasetAgility[steps].Add(_step.time);
                        datasetIA.Add(dataIA);
                    }
                    double durationRepetition = _repetition[steps - 1].accumulate - _repetition[0].accumulate;
                    datasetLastStep.Add(durationRepetition);
                    endurance.data.Add(new PointAnalyzer()
                    {
                        x = _repetition[steps-1].accumulate,
                        y = repetitions + 1
                    });
                }
                datasetRepetitions.Add(repetitions);
                int repetitionsPower = repetitions;
                double timePower = _serie[repetitions - 1][functionalMovement.steps.Count - 1].accumulate - _serie[0][0].accumulate;
                if (repetitionsPower > dataPower.repetitions)
                {
                    dataPower.updatePower(repetitionsPower, timePower);
                }
                else if((repetitionsPower == dataPower.repetitions) && (timePower < dataPower.time))
                {
                    dataPower.updatePower(repetitionsPower, timePower);
                }
                datasetEndurance.Add(endurance);
            }
            for (int i = 0; i < functionalMovement.steps.Count; i++)
            {
                Dictionary<int, List<double>> dictionary = datasetStepFlexibility[i];
                List<FlexibilityAnalyzer> detailsFlexibility = new List<FlexibilityAnalyzer>();
                foreach (KeyValuePair<int, List<double>> entry in dictionary)
                {
                    double avgFlexibility = entry.Value.Average();
                    string nameAngle = DetailsOfStepFunctionalMovement.getNameJoin(entry.Key);
                    detailsFlexibility.Add(new FlexibilityAnalyzer()
                    {
                        joint = entry.Key,
                        angle = avgFlexibility,
                        name = nameAngle
                    });
                }
                double avgTime = datasetAgility[i].Average();
                dataSetAgilityAnalizer.Add(new AgilityAnalyzer()
                {
                    joints = detailsFlexibility,
                    step = i,
                    time = avgTime
                });
            }
            dataSpeed.timeByRepetition = datasetLastStep.Average();
            dataSpeed.repetitionsBySerie = Convert.ToInt32(datasetRepetitions.Average());
            int duration = this.variablesAnalyzer.series *  (this.variablesAnalyzer.restTime + this.variablesAnalyzer.workTime);
            int totalRepetions = datasetRepetitions.Sum();
            GeneralResults general = new GeneralResults()
            {
                duration = duration,
                repetitions = totalRepetions
            };
            return new ReportAnalyzer()
            {
                variablesAnalyzer = variablesAnalyzer,
                generalResults = general,
                datasIa = datasetIA,
                endurance = datasetEndurance,
                power = dataPower,
                speed = dataSpeed,
                agility = dataSetAgilityAnalizer
            };
        }
    }

    public class PowerAnalyzer
    {
        public int repetitions { get; set; }
        public double time { get; set; }

        public void updatePower(int _repetitions, double time)
        {
            this.repetitions = _repetitions;
            this.time = time;
        }
    }

    public class PointAnalyzer
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class EnduranceAnalyzer
    {
        public string uid { get; set; }
        public string label { get; set; }
        public bool showLine { get; set; }
        public List<PointAnalyzer> data { get; set; }
    }

    public class FlexibilityAnalyzer
    {
        public int joint { get; set; }
        public double angle { get; set; }
        public string name { get; set; }
    }

    public class AgilityAnalyzer
    {
        public List<FlexibilityAnalyzer> joints { get; set; }
        public int step { get; set; }
        public double time { get; set; }
    }

    public class SpeedAnalyzer
    {
        public int repetitionsBySerie { get; set; }
        public double timeByRepetition { get; set; }
    }

    public class VariablesAnalyzer
    {
        public int restTime { get; set; }
        public int workTime { get; set; }
        public int series { get; set; }
    }

    public class GeneralResults
    {
        public int repetitions { get; set; }
        public int duration { get; set; }
    }

    public class ReportAnalyzer
    {
        public VariablesAnalyzer variablesAnalyzer { get; set; }
        public GeneralResults generalResults { get; set; }
        public List<List<object>> datasIa { get; set; }
        public List<EnduranceAnalyzer> endurance { get; set; }
        public PowerAnalyzer power { get; set; }
        public SpeedAnalyzer speed { get; set; }
        public List<AgilityAnalyzer> agility { get; set; }
    }
}
