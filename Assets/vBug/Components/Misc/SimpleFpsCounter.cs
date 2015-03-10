using UnityEngine;
using System.Collections;


public class SimpleFpsCounter : MonoBehaviour {
	public Rect position = new Rect (50, 100, 200, 50);
    public float interval = 0.5f;

    private float lastTime;
    private float counts = 0;
    private float totalFps = 0f;
    private string fpsLabel;

	void Start () {
        lastTime = Time.realtimeSinceStartup;
        StartCoroutine(UpdateFpsLabel());
	}

    private IEnumerator UpdateFpsLabel() {
        while (true) {
            yield return new WaitForSeconds(interval);
            fpsLabel = "Fps: " + (totalFps / counts);
            counts = 0;
            totalFps = 0;
        }
    }
	
	void Update () {
        counts++;
        float time = Time.realtimeSinceStartup;
        totalFps += 1f / (time - lastTime);
        lastTime = time;
	}

    void OnGUI() {
        GUI.Label(position, fpsLabel + "\n[" + (1f/Time.deltaTime) +"]");
    }

}
