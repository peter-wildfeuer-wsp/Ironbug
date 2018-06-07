﻿using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveQuadratic : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveQuadratic();
        private static CurveQuadratic InitMethod(Model model)
            => new CurveQuadratic(model);
        

        public IB_CurveQuadratic():base(InitMethod(new Model()))
        {
        }
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CurveQuadratic().get();
        }
    }

    public sealed class IB_CurveQuadratic_DataFieldSet
        : IB_FieldSet<IB_CurveQuadratic_DataFieldSet, CurveQuadratic>
    {
        private IB_CurveQuadratic_DataFieldSet() { }
    }
}
