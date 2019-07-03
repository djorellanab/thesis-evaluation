using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thesis_evaluation.common;
using thesis_evaluation.models;
using WinForms = System.Windows.Forms;

namespace thesis_evaluation.views
{
    public sealed class HeaderView : BindableBase
    {

        public bool isGetData = false;
        public Trainer Trainer = null;

        public bool btnOpenFileGBDIsEnable = true;
        public bool BtnOpenFileGBDIsEnable
        {
            get
            {
                return this.btnOpenFileGBDIsEnable;
            }
            private set
            {
                this.SetProperty(ref this.btnOpenFileGBDIsEnable, value);
            }
        }
        public string btnOpenFileGBDText = "No se ha seleccionado archivo Base de datos de gesturas";
        public string BtnOpenFileGBDText
        {
            get
            {
                return this.btnOpenFileGBDText;
            }
            private set
            {
                this.SetProperty(ref this.btnOpenFileGBDText, value);
            }
        }

        public bool btnOpenFileJSONIsEnable = true;
        public bool BtnOpenFileJSONIsEnable
        {
            get
            {
                return this.btnOpenFileJSONIsEnable;
            }
            private set
            {
                this.SetProperty(ref this.btnOpenFileJSONIsEnable, value);
            }
        }
        public string btnOpenFileJSONText = "No se ha seleccionado archivo metadata";
        public string BtnOpenFileJSONText
        {
            get
            {
                return this.btnOpenFileJSONText;
            }
            private set
            {
                this.SetProperty(ref this.btnOpenFileJSONText, value);
            }
        }

        public bool btnOpenCarpetIsEnable = true;
        public bool BtnOpenCarpetIsEnable
        {
            get
            {
                return this.btnOpenCarpetIsEnable;
            }
            private set
            {
                this.SetProperty(ref this.btnOpenCarpetIsEnable, value);
            }
        }
        public string btnOpenCarpetText = "No se ha seleccionado la carpeta de entrenamiento";
        public string BtnOpenCarpetText
        {
            get
            {
                return this.btnOpenCarpetText;
            }
            private set
            {
                this.SetProperty(ref this.btnOpenCarpetText, value);
            }
        }

        public bool btnCalibrarIsEnable = false;
        public bool BtnCalibrarIsEnable
        {
            get
            {
                return this.btnCalibrarIsEnable;
            }
            private set
            {
                this.SetProperty(ref this.btnCalibrarIsEnable, value);
            }
        }

        public bool btnPararIsEnable = false;
        public bool BtnPararIsEnable
        {
            get
            {
                return this.btnPararIsEnable;
            }
            private set
            {
                this.SetProperty(ref this.btnPararIsEnable, value);
            }
        }

        //aca
        public bool btnPlayTomaDeDatosIsEnable = true;
        public bool BtnPlayTomaDeDatosIsEnable
        {
            get
            {
                return this.btnPlayTomaDeDatosIsEnable;
            }
            private set
            {
                this.SetProperty(ref this.btnPlayTomaDeDatosIsEnable, value);
            }
        }


        public string lbAlturaKinectText = "No se tiene informacion de altura";
        public string LbAlturaKinectText
        {
            get
            {
                return this.lbAlturaKinectText;
            }
            private set
            {
                this.SetProperty(ref this.lbAlturaKinectText, value);
            }
        }

        public string lbProfundidadText = "No se tiene informacion de profundidad";
        public string LbProfundidadText
        {
            get
            {
                return this.lbProfundidadText;
            }
            private set
            {
                this.SetProperty(ref this.lbProfundidadText, value);
            }
        }

        private string lbCalibracion = "Calibracion de articulacion";
        public string LbCalibracion
        {
            get { return lbCalibracion; }
            private set
            {
                this.SetProperty(ref this.lbCalibracion, value);
            }
        }

        private bool tbSeriesIsEnable = true;
        public bool TbSeriesIsEnable
        {
            get { return tbSeriesIsEnable; }
            private set
            {
                this.SetProperty(ref this.tbSeriesIsEnable, value);
            }
        }

        private bool tbDescansoIsEnable = true;
        public bool TbDescansoIsEnable
        {
            get { return tbDescansoIsEnable; }
            private set
            {
                this.SetProperty(ref this.tbDescansoIsEnable, value);
            }
        }

        private bool tbTrabajoIsEnable = true;
        public bool TbTrabajoIsEnable
        {
            get { return tbTrabajoIsEnable; }
            private set
            {
                this.SetProperty(ref this.tbTrabajoIsEnable, value);
            }
        }
        public HeaderView()
        {
            this.Trainer = new Trainer();
        }

        private void enableAllButtonFiles()
        {
            this.BtnOpenCarpetIsEnable = true;
            this.BtnOpenFileGBDIsEnable = true;
            this.BtnOpenFileJSONIsEnable = true;
        }

        private void disableAllButtonFiles()
        {
            this.BtnOpenCarpetIsEnable = false;
            this.BtnOpenFileGBDIsEnable = false;
            this.BtnOpenFileJSONIsEnable = false;
        }

        private void readMetadata()
        {
            try
            {
                bool read = this.Trainer.getTraining();
                if (read)
                {
                    this.disableAllButtonFiles();
                    string nameCal = DetailsOfStepFunctionalMovement.getNameJoin(this.Trainer.functionalMovement.focusJoin);
                    this.LbCalibracion = $"Calibracion de ${nameCal}";
                    this.LbAlturaKinectText = $"La altura del Kinect es de {this.Trainer.functionalMovement.height} mts";
                }
                this.BtnCalibrarIsEnable = read;
            }
            catch (Exception)
            {
                this.BtnCalibrarIsEnable = false;
                this.enableAllButtonFiles();
                WinForms.MessageBox.Show("Hubo un error en la carpeta de metadata");
            }
        }

        private void disableAllButtonGetData()
        {
            // aca
            this.BtnPlayTomaDeDatosIsEnable = true;
        }


        private void enableAllButtonGetData()
        {
            this.BtnPlayTomaDeDatosIsEnable = true;
        }
        public void updateDepth(float depth)
        {
            this.LbProfundidadText = $"rango de profundida: {this.Trainer.functionalMovement.depthMin.ToString("0.00")} <= {depth.ToString("0.00")} <= {this.Trainer.functionalMovement.depthMax.ToString("0.00")} mts";
            if (this.Trainer.functionalMovement.isCalibrate(depth))
            {
                enableAllButtonGetData();
            }
            else
            {
                disableAllButtonGetData();
            }
        }


        public void btnOpenFileGBD_Click()
        {
            WinForms.OpenFileDialog openFileDialog = new WinForms.OpenFileDialog();
            openFileDialog.Filter = "Gesture base data (*.gbd)|*.gbd";
            if (openFileDialog.ShowDialog() == WinForms.DialogResult.OK)
            {
                this.BtnOpenFileGBDText = openFileDialog.FileName;
                this.Trainer.getGBD(openFileDialog.FileName);
                readMetadata();
            }
        }

        public void btnOpenFileJSON_Click()
        {
            WinForms.OpenFileDialog openFileDialog = new WinForms.OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == WinForms.DialogResult.OK)
            {
                this.BtnOpenFileJSONText = openFileDialog.FileName;
                this.Trainer.getMetadata(openFileDialog.FileName);
                readMetadata();
            }
        }

        public void btnOpenFileCarpet_Click()
        {
            using (var fbd = new WinForms.FolderBrowserDialog())
            {
                WinForms.DialogResult result = fbd.ShowDialog();

                if (result == WinForms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    BtnOpenCarpetText = fbd.SelectedPath;
                    this.Trainer.getCarpet(fbd.SelectedPath);
                    readMetadata();
                }
            }
        }

        public void btnCalibrar_Click()
        {
            this.BtnCalibrarIsEnable = false;
            this.BtnPararIsEnable = true;
        }

        private void enableAllParameters()
        {
            this.TbDescansoIsEnable = true;
            this.TbSeriesIsEnable = true;
            this.TbTrabajoIsEnable = true;
        }

        private void disableAllParameters()
        {
            this.TbDescansoIsEnable = false;
            this.TbSeriesIsEnable = false;
            this.TbTrabajoIsEnable = false;
        }

        public void btnPlayTomaDeDatos_Click(int series, int descanso, int trabajo)
        {
            disableAllParameters();
            this.Trainer.series = series;
            this.Trainer.descanso = descanso;
            this.Trainer.trabajo = trabajo;
            this.BtnPararIsEnable = false;
            // aca
            this.BtnPlayTomaDeDatosIsEnable = true;
            this.isGetData = true;
        }

        public void disableAllButtonsActions()
        {
            this.isGetData = false;
            this.BtnPararIsEnable = false;
            // aca
            this.BtnPlayTomaDeDatosIsEnable = true;
            this.BtnCalibrarIsEnable = false;
        }

        public static HeaderView clear()
        {
            return new HeaderView();
        }

    }
}
