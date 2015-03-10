using System;
using Frankfort.VBug.Internal;
using UnityEngine;

namespace Frankfort.VBug.Editor
{
    public class FpsInfoGraph : BaseGraphContainer
    {
        
        public FpsInfoGraph() : base( 
            new Color32[]
                {
                    new Color(0f, 1f, 0f),
                    new Color(0f, 0f, 0.666f),
                    new Color(0.5f, 0f, 0f)
                },
                new string[] { 
                    "Update",
                    "Av Update",
                    "Fixed",
                })
        {
            this.drawStyle = DrawStyle.addative;
            this.absolutMaxValue = 1024;
            this.notAvailableMessage = "Please enable 'System Info Recording' in vBug-settings.";
        }


        protected override float[] GetSliceGraphData(VerticalActivitySlice slice) {
            if (slice == null || slice.systemInfo == null)
                return null;

            SystemInfoSnapshot snapshot = slice.systemInfo;
            return new float[]
                {
                    snapshot.updateFps,
                    snapshot.averageUpdateFps,
                    snapshot.fixedFps
                };
        }


        protected override string CollectHoverData(VerticalActivitySlice slice){
            string result = "Current update fps: " + slice.systemInfo.updateFps + "\n";
            result += "Average update fps: " + slice.systemInfo.averageUpdateFps + "\n\n";
            result += "Current physics fps: " + slice.systemInfo.fixedFps + "\n";
            return result;
        }

    }
}
