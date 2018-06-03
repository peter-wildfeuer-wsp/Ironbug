﻿using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingWaterBaseboard : Ironbug_HVACComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_CoilHeatingWater class.
        /// </summary>
        public Ironbug_CoilHeatingWaterBaseboard()
          : base("Ironbug_CoilHeatingWaterBaseboard", "CoilHW_Bd",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilHeatingWaterBaseboard_DataFieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingWaterBaseboard", "CoilHW_Bd", "connect to baseboard", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilHeatingWaterBaseboard();
            obj.PuppetEventHandler += PuppetStateChanged;

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.CoilHWBaseboard;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("E80D4D7A-F34C-4217-A87A-B34E3372F79F"); }
        }
        
    }
    
}