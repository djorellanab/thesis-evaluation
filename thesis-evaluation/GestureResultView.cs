using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using thesis_evaluation.common;
using thesis_evaluation.models;

namespace thesis_evaluation
{
    /// <summary>
    /// Objeto que almacena los valores correspondientes y asi mismo notifica su acgtualizacion
    /// </summary>
    public sealed class GestureResultView : BindableBase
    {
        #region  Atributos
        private readonly double tickTime = 0.017;
        private int countStep = 0;
        public List<List<List<StepFunctionalMovement>>> stepsByMovement = null;
        private int repetitions = 0;
        public int Repetitions
        {
            get
            {
                return this.repetitions;
            }

            private set
            {
                this.SetProperty(ref this.repetitions, value);
            }
        }
        /// <summary>
        /// Valor continuo que verifica el progreso de levantamiento de mano
        /// </summary>
        private float progress = 0.0f;

        /// <summary> 
        /// Valor de UI
        /// </summary>
        public float Progress
        {
            get
            {
                return this.progress;
            }

            private set
            {
                this.SetProperty(ref this.progress, value);
            }
        }


        /// <summary>
        /// Verifica si el cuerpo actual tiene seguimiento
        /// </summary>
        private bool isTracked = false;

        /// <summary>
        /// Valor que indica si el cuerpo  asociado al detector tiene seguimiento
        /// </summary>
        public bool IsTracked
        {
            get
            {
                return this.isTracked;
            }

            private set
            {
                this.SetProperty(ref this.isTracked, value);
            }
        }

        private int indexStep = 0;
        public int IndexStep
        {
            get
            {
                return this.indexStep;
            }

            private set
            {
                this.SetProperty(ref this.indexStep, value);
            }
        }

        public bool isNewFunctionalMovement = false;
        public bool isWorkFunctionalMovement = false;

        private string stateMF ="INACTIVO";
        public string StateMF
        {
            get
            {
                return this.stateMF;
            }

            private set
            {
                this.SetProperty(ref this.stateMF, value);
            }
        }

        private int timeMF = 0;
        public int TimeMF
        {
            get
            {
                return this.timeMF;
            }

            private set
            {
                this.SetProperty(ref this.timeMF, value);
            }
        }

        private double timeTotalMF = 0;
        public double TimeTotalMF
        {
            get
            {
                return this.timeTotalMF;
            }

            private set
            {
                this.SetProperty(ref this.timeTotalMF, value);
            }
        }

        private int serieMF = 0;
        public int SerieMF
        {
            get
            {
                return this.serieMF;
            }

            private set
            {
                this.SetProperty(ref this.serieMF, value);
            }
        }

        public int descanso = 0;
        public int trabajo = 0;
        public int series = 0;

        public int offSet = 0;

        public double timeStep = 0;
        public List<ResultAI> results;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor que inicializa todos los componentes
        /// </summary>
        /// <param name="isTracked">Variable de seguimiento</param>
        /// <param name="progress">Valor de proceso</param>
        public GestureResultView(bool isTracked, float _progress, int _countStep)
        {
            this.progress = _progress;
            this.IsTracked = isTracked;
            this.IndexStep = 0;
            this.stepsByMovement = new List<List<List<StepFunctionalMovement>>>();
            this.repetitions = 0;
            this.results = new List<ResultAI>();
            this.countStep = _countStep;
        }

        public void updateRutine(int _series, int _descanso, int _trabajo)
        {
            this.series = _series;
            this.descanso = _descanso;
            this.trabajo = _trabajo;
        }

        /// <summary>
        /// Actualiza los valores desplegado en la interfaza de usuario
        /// </summary>
        /// <param name="isBodyTrackingIdValid">Verifica que el cuerpo tenga seguimiento</param>
        /// <param name="progress">El valor del progreso continuo</param>
        public void UpdateGestureResult(bool isBodyTrackingIdValid, float progress)
        {
            // Actualiza seguimiento
            this.IsTracked = isBodyTrackingIdValid;

            // Si no hay seguimiento, se asigna valores por default
            if (!this.isTracked)
            {
                this.Progress = -1.0f;
            }
            else // Si hay, Se asigna los valores pasado
            {
                this.Progress = progress;
            }
        }

        private bool isCorrectMF()
        {
            foreach (StepFunctionalMovement item in this.stepsByMovement[this.serieMF - 1][this.repetitions])
            {
                if (item == null) { return false; }
            }
            return true;
        }
        public void addStepDetail(StepFunctionalMovement step)
        {
            this.stepsByMovement[this.serieMF-1][this.repetitions][step.step] = step;

            if (step.step == (this.stepsByMovement.Count - 1) && isCorrectMF())
            {
                isNewFunctionalMovement = true;
                this.createStepsDetail();
                this.Repetitions++;
            }
        }

        private void createStepsDetail()
        {
            List<StepFunctionalMovement> _steps = new List<StepFunctionalMovement>();
            for (int i = 0; i < this.countStep; i++)
            {
                _steps.Add(null);
            }
            this.stepsByMovement[this.serieMF - 1].Add(_steps);
        }

        public void getAngle(List<int> _angles, int i)
        {
            List<List<StepFunctionalMovement>> _series = this.stepsByMovement[i];
            int _repetition = 0;
            foreach (List<StepFunctionalMovement> _repetitions in _series)
            {
                _repetition++;
                List<ResultAI> rais = new List<ResultAI>();
                bool isOut = false;
                foreach (StepFunctionalMovement step in _repetitions)
                {
                    if (step != null)
                    {
                        foreach (int _angle in _angles)
                        {
                            List<JointType> joints = DetailsOfStepFunctionalMovement.translateAngles(_angle);
                            List<DetailsOfStepFunctionalMovement> vectorialPoints = step.detailsOfStepFunctionalMovement.FindAll(x => joints.Contains((JointType)x.join));
                            if (vectorialPoints.Count != 3) { isOut = true; return; }
                            DetailsOfStepFunctionalMovement origen = vectorialPoints.Find(x => (int)x.angle == (int)joints[0]);
                            if (origen != null) { isOut = true; return; }
                            vectorialPoints.RemoveAll(x => (int)x.angle == (int)joints[0]);
                            if (vectorialPoints.Count != 2) { isOut = true;  continue; }
                            else if (vectorialPoints[0] == null && vectorialPoints[1] == null) { isOut = true; continue; }
                            Point po = new Point(origen.x, origen.y);
                            Point p1 = new Point(vectorialPoints[0].x, vectorialPoints[0].y);
                            Point p2 = new Point(vectorialPoints[1].x, vectorialPoints[1].y);
                            origen.angle = KinectAngleBody.getAngleBody(po, p1, p2);

                            rais.Add(new ResultAI()
                            {
                                accumulateTime = step.accumulate,
                                angle = origen.angle,
                                joint = origen.join,
                                repetition = _repetition,
                                serie =  i,
                                step = step.step,
                                time = step.time,
                                x = origen.x,
                                y = origen.y
                            });
                        }
                        if (isOut)
                        {
                            return; 
                        }
                    }
                    else { isOut = true; return; }
                }
                if (!isOut)
                {
                    foreach (ResultAI rai in rais)
                    {
                        this.results.Add(rai);
                    }
                }
            }
        }

        public void checkNewMovementFunctional()
        {
            if (this.isNewFunctionalMovement)
            {
                this.isNewFunctionalMovement = (this.indexStep == 0);
            }
        }

        public bool isTakeDataOfFunctionalMovement()
        {
            return this.stepsByMovement[this.repetitions][this.indexStep] != null;
        }

        public void updateStep(int step)
        {
            if (step > -1)
            {
                this.IndexStep = step;
            }
        }

        public void updateTimeTotal()
        {
            this.TimeTotalMF += tickTime;
            this.timeStep += tickTime;
            if (!isWorkFunctionalMovement)
            {
                this.StateMF = "DESCANSO";
                int modTime = (Convert.ToInt32(this.TimeTotalMF) + offSet) % descanso;
                this.TimeMF = descanso - modTime;
            }
            else
            {
                this.StateMF = "TRABAJO";
                int modTime = (Convert.ToInt32(this.TimeTotalMF) + offSet) % trabajo;
                this.TimeMF = trabajo - modTime;
            }

            if ((Convert.ToDouble(descanso) <= timeStep) && !isWorkFunctionalMovement)
            {
                offSet += descanso;
                timeStep = 0;
                isWorkFunctionalMovement = !isWorkFunctionalMovement;
                addSerie();
            }
            else if ( (Convert.ToDouble(trabajo) <= timeStep) && isWorkFunctionalMovement)
            {
                if (this.serieMF != series)
                {
                    offSet += trabajo;
                    timeStep = 0;
                    isWorkFunctionalMovement = !isWorkFunctionalMovement;
                }
                else
                {
                    this.StateMF = "FIN";
                }
            }
        }

        public bool isGetDataForRutine()
        {
            return (this.serieMF < this.series);
        }

        public bool isFinish()
        {
            return this.stateMF == "FIN";
        }
        private void addSerie()
        {
            this.SerieMF++;
            this.stepsByMovement.Add(new List<List<StepFunctionalMovement>>());
            this.Repetitions = 0;
            createStepsDetail();
        }

        public void updateState(string state)
        {
            this.StateMF = state;
        }

        public bool isPrintData()
        {
            return this.stateMF == "CAPTURANDO";
        }

        #endregion
    }
}
