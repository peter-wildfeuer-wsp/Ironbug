﻿using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingWater: IB_CoilBasic, IIB_ShareableObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingWater();

        private static CoilHeatingWater InitMethod(Model model) => new CoilHeatingWater(model);
        
        public IB_CoilHeatingWater() : base(InitMethod(new Model()))
        {
        }
        
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilHeatingWater)this.InitOpsObj(model)).addToNode(node);
            
        }


        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoilHeatingWater().get();
        }


    }

    public sealed class IB_CoilHeatingWater_DataFieldSet
        : IB_DataFieldSet<IB_CoilHeatingWater_DataFieldSet, CoilHeatingWater>
    {
        private IB_CoilHeatingWater_DataFieldSet() {}


        //https://bigladdersoftware.com/epx/docs/8-0/input-output-reference/page-042.html#coilheatingwater
        public IB_DataField Name { get; }
            = new IB_BasicDataField("Name", "Name")
            {
                DetailedDescription = "A unique identifying name for each coil."
            };
        

        public IB_DataField RatedInletWaterTemperature { get; }
            = new IB_BasicDataField("RatedInletWaterTemperature", "InWaterTemp")
            {
                DetailedDescription = "The inlet water temperature (degrees C) corresponding to the rated heating capacity. " +
                "The default is 82.2 degrees C (180 degrees F)."
            };

        public IB_DataField RatedInletAirTemperature { get; }
            = new IB_ProDataField("RatedInletAirTemperature", "InAirTemp");
            

        public IB_DataField RatedOutletWaterTemperature { get; }
            = new IB_BasicDataField("RatedOutletWaterTemperature", "OutWaterTemp");

        public IB_DataField RatedOutletAirTemperature { get; }
            = new IB_ProDataField("RatedOutletAirTemperature", "OutAirTemp");

        public IB_DataField UFactorTimesAreaValue { get; }
            = new IB_ProDataField("UFactorTimesAreaValue", "UFactor");

        public IB_DataField MaximumWaterFlowRate { get; }
            = new IB_ProDataField("MaximumWaterFlowRate", "MaxFlow")
            {
                DetailedDescription = "The maximum possible water flow rate (m3/sec) through the coil. " +
                "This field is used when Coil Performance Input Method = UFactorTimesAreaAndDesignWaterFlowRate. " +
                "This field is autosizable.",

            };

        public IB_DataField RatedRatioForAirAndWaterConvection { get; }
            = new IB_ProDataField("RatedRatioForAirAndWaterConvection", "AirWaterRatio");
        
    }
    
    

}