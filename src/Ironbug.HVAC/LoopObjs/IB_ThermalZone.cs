﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public class IB_ThermalZone : IB_HVACObject, IIB_AirLoopObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ThermalZone();
        private static ThermalZone InitMethod(Model model) => new ThermalZone(model);

        public IB_AirTerminal AirTerminal { get; private set; } = new IB_AirTerminalSingleDuctUncontrolled();
        public List<IIB_ZoneEquipment> ZoneEquipments { get; private set; } = new List<IIB_ZoneEquipment>();
        private IB_SizingZone IB_SizingZone { get; set; } = new IB_SizingZone();

        

        
        public IB_ThermalZone():base(InitMethod(new Model()))
        {
            
        }
        public IB_ThermalZone(string HBZoneName) : base(InitMethod(new Model()))
        {
            base.SetAttribute(IB_ThermalZone_DataFieldSet.Value.Name, HBZoneName);

        }


        public ModelObject GetModelObject()
        {
            return base.GhostOSObject;
        }

        /// <summary>
        /// Do not use this 
        /// Use this method to add a new SizingZone to this ThermalZone;
        /// Or construct the IB_SizingZone by IB_SizingZone(IB_ThermalZone);
        /// </summary>
        /// <param name="NewSizingZone"></param>
        public void SetSizingZone(IB_SizingZone NewSizingZone)
        {
            this.IB_SizingZone = NewSizingZone;
        }

        
        public void SetAirTerminal(IB_AirTerminal AirTerminal)
        {
            this.AirTerminal = AirTerminal;
        }

        public void AddZoneEquipment(IB_ZoneEquipment Equipment)
        {
            this.ZoneEquipments.Add(Equipment);
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            //check the model if there's a same named thermal zone
            //if yes, then return it
            //if no, then create a new one
            
            var optionalNames = this.CustomAttributes.Where(_ => _.Key == "setName");
            var optionalZone = new OptionalThermalZone();

            if (optionalNames.Any())
            {
                var zoneName = optionalNames.First().Value.ToString();
                optionalZone = model.getThermalZoneByName(zoneName);
            }

            ThermalZone newZone = null;

            if (optionalZone.is_initialized())
            {
                //get the thermal zone that is generated by HB, 
                //and set the custom attributes.
                newZone = optionalZone.get();
                newZone.SetCustomAttributes(this.CustomAttributes);
                
            }
            else
            {
                newZone = (ThermalZone)base.OnInitOpsObj(InitMethod, model);
            }
            
            
            //add child to newZone
            this.IB_SizingZone.ToOS(newZone);

            foreach (var item in this.ZoneEquipments)
            {
                var eqp = (ZoneHVACComponent)item.ToOS(model);
                eqp.addToThermalZone(newZone);
                //newZone.addEquipment(eqp);
            }

            //AirTerminal has been added with zone when the zone was added to the loop
            //var newTerminal = this.AirTerminal.ToOS(model);
            //newZone.addEquipment(newTerminal);
            
            return newZone;
        }

        public override IB_ModelObject Duplicate()
        {
            
            //Duplicate self;
            var newObj = base.DuplicateIBObj(() => new IB_ThermalZone());

            //Duplicate child member; //add new child member to new object;
            newObj.SetAirTerminal((IB_AirTerminal)this.AirTerminal.Duplicate());
            newObj.SetSizingZone((IB_SizingZone)this.IB_SizingZone.Duplicate());


            foreach (var item in this.ZoneEquipments)
            {
                //Duplicate child member; 
                var newItem = (IB_ZoneEquipment)item.Duplicate();

                //add new child member to new object;
                newObj.ZoneEquipments.Add(newItem);
            }

            
            return newObj;
        }
        

        public override bool AddToNode(Node node)
        {
            throw new NotImplementedException();
        }
        
    }

    public sealed class IB_ThermalZone_DataFieldSet 
        : IB_DataFieldSet<IB_ThermalZone_DataFieldSet, ThermalZone>
    {
        
        private IB_ThermalZone_DataFieldSet() {}

        public IB_DataField Name { get; }
            = new IB_BasicDataField("Name", "Name")
            {
                DetailedDescription = "A unique identifying name for each coil."
            };


        public IB_DataField Multiplier { get; }
            = new IB_BasicDataField("Multiplier", "Multiplier")
            {
                DetailedDescription = "Zone Multiplier is designed as a “multiplier” for floor area, zone loads, and energy consumed by internal gains. "+
                "It takes the calculated load for the zone and multiplies it, sending the multiplied load to the attached HVAC system. "+
                "The HVAC system size is specified to meet the entire multiplied zone load and will report the amount of the load met in the Zone Air System Sensible Heating or Cooling Energy/Rate output variable. "+
                "Autosizing automatically accounts for multipliers. Metered energy consumption by internal gains objects such as Lights or Electric Equipment will be multiplied. "+
                "The default is 1."
            };

        public IB_DataField ZoneInsideConvectionAlgorithm { get; }
            = new IB_BasicDataField("ZoneInsideConvectionAlgorithm", "InConvection");


        public IB_DataField ZoneOutsideConvectionAlgorithm { get; }
            = new IB_BasicDataField("ZoneOutsideConvectionAlgorithm", "OutConvection");
        


    }
}