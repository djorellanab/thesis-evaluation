using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using thesis_evaluation.models;

namespace thesis_evaluation
{
    public class Trainer
    {
        public FunctionalMovement functionalMovement { get; set; }

        public bool hasGBD { get; set; }
        public bool hasMetadata { get; set; }
        public bool hasFolder { get; set; }

        public string GBD { get; set; }
        public string metadata { get; set; }
        public string folder { get; set; }

        public int series { get; set; }
        public int descanso { get; set; }
        public int trabajo { get; set; }

        public Trainer()
        {
            dispose();
        }

        public void dispose()
        {
            this.hasGBD = false;
            this.hasMetadata = false;
            this.hasFolder = false;

            this.GBD = "";
            this.metadata = "";
            this.folder = "";

            this.functionalMovement = null;
        }

        public void getGBD(string _GBD)
        {
            this.hasGBD = !string.IsNullOrEmpty(_GBD);
            if (!this.hasGBD)
            {
                throw new Exception("Es necesario obtener path GBD ");
            }
            this.GBD = _GBD;
        }

        public void getMetadata(string _metadata)
        {
            this.hasMetadata = !string.IsNullOrEmpty(_metadata);
            if (!this.hasMetadata)
            {
                throw new Exception("Es necesario obtener path del metadata ");
            }
            this.metadata = _metadata;
        }

        public void getCarpet(string _folder)
        {
            this.hasFolder = !string.IsNullOrEmpty(_folder);
            if (!this.hasFolder)
            {
                throw new Exception("Es necesario obtener path de la carpeta ");
            }
            this.folder = _folder;
        }

        private void getFunctionalMovement()
        {
            string json = File.ReadAllText(this.metadata);
            this.functionalMovement = JsonConvert.DeserializeObject<FunctionalMovement>(json);
            createSaveFolder();
        }

        private void createSaveFolder()
        {
            DateTime date = DateTime.Now;
            string[] names = this.functionalMovement.name.Trim().Split(' ');
            string folderName = "/" + string.Concat(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            foreach (string name in names)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    folderName = string.Join("-", folderName, name);
                }
            }
            this.folder += folderName;
            if (!Directory.Exists(this.folder))
            {
                Directory.CreateDirectory(this.folder);
            }
        }

        public bool getTraining()
        {
            bool training = this.hasFolder && this.hasGBD && this.hasMetadata;
            if (training)
            {
                getFunctionalMovement();
            }
            return training;
        }
    }
}
