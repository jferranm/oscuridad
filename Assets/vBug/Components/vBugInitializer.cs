using UnityEngine;
using Frankfort.VBug;
using System.Collections;

/// <summary>
/// This is an example class of a simple vBug-implementation. You can create your own based on the logic found in this class.
/// Its not obligated to exposed and set the 'vBugGlobalSettings', sins by default they are set to optimal mobile settings
/// </summary>
[AddComponentMenu("vBug/Initializer")]
public class vBugInitializer : MonoBehaviour {
    public int startDelay = 0;
    public bool dontDestroyOnLoad = true;
    public vBugGlobalSettings settings;

    private bool isUnique = false;

	// Use this for initialization
	IEnumerator Start () {
        if (GameObject.FindObjectsOfType<vBugInitializer>().Length >= 2) { //There is already another initializer running...
            Destroy(this.gameObject); //goodbye :(
            yield break;
        } else {
            isUnique = true;
        }

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(this.gameObject);

        for (int i = 0; i < startDelay; i++ )
            yield return null;

        vBug.settings = this.settings;
        vBug.StartRecording();
	}
	
    void OnDestroy(){
        if (isUnique && vBug.isRunning)
            vBug.StopRecording();
    }
}
