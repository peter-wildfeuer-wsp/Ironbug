using System;
using Grasshopper.Kernel;
namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_SolarCollectorPerformanceFlatPlate : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_SolarCollectorPerformanceFlatPlate()
          : base("IB_SolarCollectorCollectorPerformanceFlatPlate", "SolarCollectorCollectorPerformanceFlatPlate",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_SolarCollectorPerformanceFlatPlate_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SolarCollectorPerformanceFlatPlate", "SolarCollectorPerformanceFlatPlate", "Connect to hot water loop's supply side.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SolarCollectorPerformanceFlatPlate();
            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }
        
        protected override System.Drawing.Bitmap Icon => Properties.Resources.WaterHeaterMix;
        
        public override Guid ComponentGuid => new Guid("ABDBE4EE-CCB7-4DB8-B7DF-171F800331E5");

    }
}