using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Platform.TestAutomationFramework.UIAutomation
{
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Extensions;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    class FrameHandler : IDisposable
    {
        private readonly IUiAdapter uiAdapter;
        private readonly string logicalFieldName;
        private readonly bool switchedToIFrame;

        public FrameHandler(IUiAdapter uiAdapter, TestRunContext context, string logicalFieldName)
        {
            try
            {
                this.uiAdapter = uiAdapter;
                this.logicalFieldName = logicalFieldName;
                this.switchedToIFrame = TrySwitchToIFrame(context);
            }
            
            catch(Exception ex)
            {
                throw new WorkflowFailedException(
                        string.Format("Tried Switch to IFrame, but the {0} control never appeared.", this.logicalFieldName), ex);
            }
        }

        public void Dispose()
        {
            if (this.switchedToIFrame)
            {
                try
                {
                    //TryReturnToDefaultContext();
                    this.uiAdapter.SwitchToAlert();
                }
                
                catch
                {
                    TryReturnToDefaultContext();
                    //nothing
                }             
            }
        }      
  
        //LN 05/02: Commented Code to Supprt Nested Iframes
        //private bool TrySwitchToIFrame(TestRunContext context)
        //{
        //    var thisControl = context.ControlMap[context.CurrentPage][this.logicalFieldName];
        //    var parentControl = thisControl.ParentControl.IsNotNullOrEmptyOrWhiteSpace()
        //        ? GetParentControl(context, thisControl)
        //        : null;
        //    if (parentControl != null && parentControl.TagName.Equals("IFrame", StringComparison.OrdinalIgnoreCase))
        //    {
        //        this.uiAdapter.SwitchToIFrame(parentControl);
        //        return true;
        //    }

        //    return false;
        //}

        private bool TrySwitchToIFrame(TestRunContext context)
        {
            bool switchFrame=false;
            List<ControlDefinition> parentControls = new List<ControlDefinition>();
            var thisControl = context.ControlMap[context.CurrentPage][this.logicalFieldName];
            var parentControl = thisControl.ParentControl.IsNotNullOrEmptyOrWhiteSpace()
            ? GetParentControl(context, thisControl)
            : null;

            while (parentControl != null)
            {
                parentControls.Add(parentControl);
                var temp = parentControl.ParentControl.IsNotNullOrEmptyOrWhiteSpace()
                ? GetParentControl(context, parentControl)
                : null;
                parentControl = temp;
            }

            for (int i = parentControls.Count-1; i >=0;i-- )
            {
                if (parentControls[i] != null && parentControls[i].TagName.Equals("IFrame", StringComparison.OrdinalIgnoreCase))
                {
                    this.uiAdapter.SwitchToIFrame(parentControls[i]);
                    switchFrame = true;
                }                
            }
            return switchFrame;

                //LN 05/02 : Commeted Code to Support Nested Iframes

                //need to revisit the code
                //var thisControl = context.ControlMap[context.CurrentPage][this.logicalFieldName];
                //var parentControl = thisControl.ParentControl.IsNotNullOrEmptyOrWhiteSpace()
                //    ? GetParentControl(context, thisControl)
                //    : null;
                //var parentofParentControl = parentControl != null ? (parentControl.ParentControl.IsNotNullOrEmptyOrWhiteSpace()
                //    ? GetParentControl(context, parentControl)
                //    : null) : null;
                //var thirdLevelfParentControl = parentofParentControl != null ? (parentofParentControl.ParentControl.IsNotNullOrEmptyOrWhiteSpace()
                //    ? GetParentControl(context, parentofParentControl)
                //    : null) : null;

                //if (thirdLevelfParentControl != null && thirdLevelfParentControl.TagName.Equals("IFrame", StringComparison.OrdinalIgnoreCase))
                //{
                //    if (parentofParentControl != null && parentofParentControl.TagName.Equals("IFrame", StringComparison.OrdinalIgnoreCase))
                //    {
                //        if (parentControl != null && parentControl.TagName.Equals("IFrame", StringComparison.OrdinalIgnoreCase))
                //        {
                //            this.uiAdapter.SwitchToIFrame(thirdLevelfParentControl);
                //            this.uiAdapter.SwitchToIFrame(parentofParentControl);
                //            this.uiAdapter.SwitchToIFrame(parentControl);
                //            //this.uiAdapter.SwitchToIFrame(parentofParentControl, parentControl);
                //            return true;
                //        }
                //    }
                //}
                //else if (parentofParentControl != null && parentofParentControl.TagName.Equals("IFrame", StringComparison.OrdinalIgnoreCase))
                //{
                //    if (parentControl != null && parentControl.TagName.Equals("IFrame", StringComparison.OrdinalIgnoreCase))
                //    {
                //        //this.uiAdapter.SwitchToIFrame(parentofParentControl);
                //        //this.uiAdapter.SwitchToIFrame(parentControl);
                //        this.uiAdapter.SwitchToIFrame(parentofParentControl, parentControl);
                //        return true;
                //    }

                //}
                //else if (parentControl != null && parentControl.TagName.Equals("IFrame", StringComparison.OrdinalIgnoreCase))
                //{
                //    this.uiAdapter.SwitchToIFrame(parentControl);
                //    return true;
                //}
            //return false;
        }

        private static ControlDefinition GetParentControl(TestRunContext context, ControlDefinition thisControl)
        {
            return context.ControlMap[context.CurrentPage].ContainsKey(thisControl.ParentControl)
                ? context.ControlMap[context.CurrentPage][thisControl.ParentControl]
                : null;
        }

        private void TryReturnToDefaultContext()
        {
            this.uiAdapter.SwitchToDefaultContent();
        }
    }
}
