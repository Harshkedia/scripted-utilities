using System;
using System.IO;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.ApplicationSettings;
using Rhino.Input;
using System.Threading.Tasks;

namespace Scripted_Utilities
{
    public class DisableAutosave : Command
    {
        public DisableAutosave()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static DisableAutosave Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "DisableAutosave"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoApp.WriteLine("The {0} command will run now", EnglishName);

            int after = 10;
            RhinoGet.GetInteger("Disable Autosave for: (minutes):", true, ref after);

            FileSettings.AutoSaveEnabled = false;

            RhinoApp.WriteLine("Disabling Autosave for {0} minutes", after);

            enableAutosave(after);

            return Result.Success;
            
    
        }

        private async void enableAutosave(int after)
        {
            await Task.Delay(after * 60000);
            RhinoApp.WriteLine("Re-Enabling Autosave Now", after);
            FileSettings.AutoSaveEnabled = true;
        }
    }
}
