using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_SolarCollectorPerformanceFlatPlate : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SolarCollectorPerformanceFlatPlate();

        private static SolarCollectorPerformanceFlatPlate NewDefaultOpsObj() => new SolarCollectorPerformanceFlatPlate();
        public IB_SolarCollectorPerformanceFlatPlate() : base(NewDefaultOpsObj(new Model()))
        {
        }
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }


    public sealed class IB_SolarCollectorPerformanceFlatPlate_FieldSet
        : IB_FieldSet<IB_SolarCollectorFlatPlateWater_FieldSet, SolarCollectorFlatPlateWater>
    {
        private IB_SolarCollectorPerformanceFlatPlate_FieldSet() { }
    }
}
