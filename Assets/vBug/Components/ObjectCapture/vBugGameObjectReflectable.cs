using UnityEngine;
using Frankfort.VBug;

[AddComponentMenu("vBug/GameObjectReflectable")]
public class vBugGameObjectReflectable : MonoBehaviour{
    /// <summary>
    /// This might throw errors as soon as vBug tries to reflect properties that are not allowed to be scannen by Unity.
    /// This happens for example when you try to reflect the Unity 'Animator' component.
    /// </summary>
    public bool allowNativeUnityComponents = false;
}
