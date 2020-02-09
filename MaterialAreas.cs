using System;
using System.IO;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects.Tables;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.DocObjects;

namespace Scripted_Utilities
{
    public class MaterialAreas : Command
    {
        public MaterialAreas()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static MaterialAreas Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "MaterialAreas"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoApp.WriteLine("The {0} command will run now", EnglishName);

            MaterialTable materialTable = doc.Materials;
            List<double> totalAreas = new List<double>();
            double cumulativeArea = 0;

            foreach(Material m in materialTable)
            {
                doc.Objects.UnselectAll();
                string command = string.Format("_-SelMaterialName {0}", m.Name);
                RhinoApp.RunScript(command, true);
                IEnumerable<RhinoObject> selectedObjects = doc.Objects.GetSelectedObjects(false, false);
                double totalArea = 0;
                foreach(RhinoObject obj in selectedObjects)
                {
                    if(obj.ObjectType == ObjectType.Brep)
                    {
                        Brep brepGeom = (Brep) obj.Geometry;
                        double area = AreaMassProperties.Compute(brepGeom).Area;
                        totalArea += area;
                    }
                }
                cumulativeArea += totalArea;
                totalAreas.Add(totalArea);
            }

            List<double> areaPercent = new List<double>();
            totalAreas.ForEach(area => areaPercent.Add(area / cumulativeArea));

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "materialAreas.txt");

            using (StreamWriter sw = File.CreateText(path))
            {
                for(int i=0; i<areaPercent.Count; i++)
                {
                    string line = string.Format("Material: {0}, Area: {1}", materialTable[i], areaPercent[i]);
                }
            }


            doc.Views.Redraw();
            RhinoApp.WriteLine("The curves were rebuilt");

            return Result.Success;
        }
    }
}
