﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Ironbug.Grasshopper.Properties;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingWater : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_CoilHeatingWater class.
        /// </summary>
        public Ironbug_CoilHeatingWater()
          : base("Ironbug_CoilHeatingWater", "Nickname",
              "Description",
              "Ironbug", "01:LoopComponents",
              typeof(HVAC.IB_CoilHeatingWater_DataFieldSet))
        {
        }
        

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Hot water supply", "_supply", "hot water supply source from hot water plant loop.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Parameters for Coil:Heating:Water", "params_", "Detail settings for this Coil. Use Ironbug_ObjParams to set this.", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
            //AddParams();
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingWater", "CoilHW", "connect to airloop's supply side", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilHeatingWater();
            
            //CollectSettingData(ref coil);

            var settingParams = new Dictionary<HVAC.IB_DataField, object>();
            DA.GetData(1, ref settingParams);

            obj.SetAttributes(settingParams);


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
                return Resources.CoilHW;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("4f849460-bb38-441c-9387-95c5be5830e7"); }
        }
        
    }
    
}