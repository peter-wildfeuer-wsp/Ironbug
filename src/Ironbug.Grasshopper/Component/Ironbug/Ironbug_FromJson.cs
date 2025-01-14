﻿using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_FromJson : Ironbug_Component
    {
        public Ironbug_FromJson()
          : base("IB_FromJson", "FromJson",
              "Use this component to measure variables like temperature, flow rate, etc, in the loop.\nPlace this between loopObjects.",
              "Ironbug", "HVAC")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary | GH_Exposure.hidden;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Json", "Json", "A HVAC system from Ironbug_HVACSystem", GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Hvac", "Hvac", "TODO....", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string json = null;
            DA.GetData(0, ref json);

            HVAC.IB_HVACSystem sys = HVAC.IB_HVACSystem.FromJson(json);
            DA.SetData(0, sys);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("90AB41C5-F652-4B17-8B02-BA8CF8739821");

    }

   
}