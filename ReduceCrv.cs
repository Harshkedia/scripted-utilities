using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.DocObjects;

namespace Scripted_Utilities
{
    public class ReduceCrv : Command
    {
        public ReduceCrv()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static ReduceCrv Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "ReduceCrv"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoApp.WriteLine("The {0} command will run now", EnglishName);

            ObjectType filter = ObjectType.Curve;
            int percent = 50;
            int degree = 3;

            RhinoGet.GetInteger("Enter the percentage you want to reduce the curves by (1-100)", true, ref percent);
            double reduce = ((double)percent) / 100;

            RhinoGet.GetInteger("Enter the new curve degree", true, ref degree);

            GetObject go = new GetObject();
            go.SetCommandPrompt("Select curves you want to reduce");
            go.GeometryFilter = filter;
            go.GroupSelect = true;

            GetResult get = go.GetMultiple(1, 0);

            for(int i=0; i<go.ObjectCount; i++)
            {
                CurveObject curObj = (CurveObject) go.Object(i).Object();
                Curve curCurve = curObj.CurveGeometry;
                NurbsCurve curNurbsCurve = curCurve.ToNurbsCurve();
                int curControlPoints = curNurbsCurve.Points.Count;
                int newPoints = Convert.ToInt32(curControlPoints * reduce);
                Curve rebuiltCurve = curCurve.Rebuild(newPoints, degree, false);
                doc.Objects.Add(rebuiltCurve, curObj.Attributes);
                doc.Objects.Delete(curObj);
            }

            doc.Views.Redraw();
            RhinoApp.WriteLine("The curves were rebuilt");

            return Result.Success;
        }
    }
}
