using UnityEngine;
using System.Collections;

public class OpcionesCanvasMenuOpciones : MonoBehaviour 
{
    void Start () 
	{
		this.gameObject.SetActive (false);
	}

	void Update()
	{
		if (this.gameObject.activeSelf) 
		{
			if (Input.GetKeyDown (KeyCode.Escape)) 
				this.gameObject.SetActive(!this.gameObject.activeSelf);
		}
	}
}
