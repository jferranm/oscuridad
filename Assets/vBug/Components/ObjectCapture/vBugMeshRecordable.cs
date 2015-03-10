using UnityEngine;
using Frankfort.VBug;

[AddComponentMenu("vBug/MeshRecordable")]
public class vBugMeshRecordable: MonoBehaviour{
    /// <summary>
    /// Pick 'smartMesh' if you are using bones or if its just an animated model without any mesh-deformations.
    /// Pick 'fullMesh' if any of the meshes deforms every frame.
    /// </summary>
    public MeshCaptureMethod captureMethod;
    public Color color = Color.cyan;
}
