using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using thesis_evaluation.models;

namespace thesis_evaluation
{
    public sealed class GestureDetector : IDisposable
    {
        #region atributos

        private readonly int MINDETECTOR = 0;
        private readonly int MAXDETECTOR = 1;

        /// <summary>
        /// obtiene los frames de gesturas
        /// </summary>
        private VisualGestureBuilderFrameSource vgbFrameSource = null;

        /// <summary>
        ///  Obtiene el identificador del cuerpo que se encuentra detectando actualmente
        /// </summary>
        public ulong TrackingId
        {
            get
            {
                return this.vgbFrameSource.TrackingId;
            }

            set
            {
                if (this.vgbFrameSource.TrackingId != value)
                {
                    this.vgbFrameSource.TrackingId = value;
                }
            }
        }

        /// <summary>
        ///  Verifica si la deteccion se encuentra pausado
        /// </summary>
        public bool IsPaused
        {
            get
            {
                return this.vgbFrameReader.IsPaused;
            }

            set
            {
                if (this.vgbFrameReader.IsPaused != value)
                {
                    this.vgbFrameReader.IsPaused = value;
                }
            }
        }

        /// <summary>
        /// Obtiene la lecturas de gesturas
        /// </summary>
        private VisualGestureBuilderFrameReader vgbFrameReader = null;

        /// <summary>
        /// Almacena los resultados respectivos de la gestura para mostrarlo en la UI
        /// </summary>
        public GestureResultView GestureResultView { get; private set; }

        #endregion

        #region constructor
        /// <summary>
        /// Constructor que instancias los atributos correspondientes
        /// </summary>
        /// <param name="kinectSensor"></param>
        /// <param name="gestureResultView"></param>
        public GestureDetector(KinectSensor kinectSensor, GestureResultView gestureResultView, string pathGBD)
        {
            // Verifica si el kinect esta conectado
            if (kinectSensor == null)
            {
                throw new ArgumentNullException("kinectSensor");
            }

            // Verifica si existe el objeto que almacenara los resultados
            if (gestureResultView == null)
            {
                throw new ArgumentNullException("gestureResultView");
            }

            // Le asigna los valores correspondiente a los atributos globales
            this.GestureResultView = gestureResultView;

            // Crea el objeto para asociar los recursos, respecto al cuerpo que se esta analizando
            this.vgbFrameSource = new VisualGestureBuilderFrameSource(kinectSensor, 0);

            // Obtiene los valores de lecturas (En caso que no se tenga la lectura se pausa)
            this.vgbFrameReader = this.vgbFrameSource.OpenReader();
            if (this.vgbFrameReader != null)
            {
                this.vgbFrameReader.IsPaused = true;
            }
            // Obtiene toda la informacion de la base de datos
            using (var database = new VisualGestureBuilderDatabase(pathGBD))
            {
                this.vgbFrameSource.AddGestures(database.AvailableGestures);
            }

        }
        #endregion


        #region metodos

        /// <summary>
        /// Metodo para limpiar las lecturas correspondientes
        /// </summary>
        public void Dispose()
        {
            if (this.vgbFrameReader != null)
            {
                this.vgbFrameReader.Dispose();
                this.vgbFrameReader = null;
            }

            if (this.vgbFrameSource != null)
            {
                this.vgbFrameSource.Dispose();
                this.vgbFrameSource = null;
            }
        }

        /// <summary>
        /// Actualiza la ultima lectura del sensor de gesturas
        /// </summary>
        public void UpdateGestureData()
        {
            // Obtiene el respectivo frame a analizar
            using (var frame = this.vgbFrameReader.CalculateAndAcquireLatestFrame())
            {
                // Verifica si el frame existe
                if (frame != null)
                {
                    // Obtiene los resultados del frame de valores coninuo
                    var continuousResults = frame.ContinuousGestureResults;
                    // Obtiene los resulados del proceso
                    float progress = this.GestureResultView.Progress;
                    // Recorre todas las gestura analizar
                    foreach (var gesture in this.vgbFrameSource.Gestures)
                    {
                        // Verifica que tenga el paquete resultado tenga valiables continuas 
                        if (continuousResults != null)
                        {
                            // Verifica los atributos a analizar
                            if (gesture.GestureType == GestureType.Continuous)
                            {
                                // Obtengo los valores continuos
                                ContinuousGestureResult result = null;
                                continuousResults.TryGetValue(gesture, out result);
                                // Verifica si tienes resultados
                                if (result != null)
                                {
                                    progress = result.Progress;
                                }
                            }
                        }
                        // Obtiene resultado dentro del parametro continuo
                        if (progress < MINDETECTOR)
                        {
                            progress = MINDETECTOR;
                        }
                        else if (progress > MAXDETECTOR)
                        {
                            progress = MAXDETECTOR;
                        }
                        // Actualiza los resultados
                        this.GestureResultView.UpdateGestureResult(true, progress);
                    }
                }
            }
        }
        #endregion
    }
}
