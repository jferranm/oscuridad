using UnityEngine;
using System.Collections;

public class FondoPantalla : MonoBehaviour 
{
	void OnEnable()
	{
		this.transform.position = Vector3.zero;
		this.transform.localScale = Vector3.zero;
		this.guiTexture.pixelInset = new Rect(0,0, Screen.width, Screen.height);
	}
}
