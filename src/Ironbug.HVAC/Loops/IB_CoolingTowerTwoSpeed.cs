﻿using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoolingTowerTwoSpeed : IB_HVACObject, IIB_PlantLoopObjects
    {
        private static CoolingTowerTwoSpeed InitMethod(Model model) => new CoolingTowerTwoSpeed(model);
        public IB_CoolingTowerTwoSpeed() : base(InitMethod(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoolingTowerTwoSpeed)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_CoolingTowerTwoSpeed());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_CoolingTowerTwoSpeed().get();
        }
    }

    public sealed class IB_CoolingTowerTwoSpeed_DataFields
        : IB_DataFieldSet<IB_CoolingTowerTwoSpeed_DataFields, CoolingTowerTwoSpeed>
    {

        private IB_CoolingTowerTwoSpeed_DataFields() { }

        public IB_DataField HighSpeedNominalCapacity { get; }
            = new IB_BasicDataField("HighSpeedNominalCapacity", "HCapacity");

        public IB_DataField LowSpeedNominalCapacity { get; }
            = new IB_BasicDataField("LowSpeedNominalCapacity", "LCapacity");


    }
}