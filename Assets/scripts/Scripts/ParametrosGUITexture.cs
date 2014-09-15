using UnityEngine;

public class ParametrosGUITexture : MonoBehaviour 
{
	public float posicion_x;
	public float posicion_y;
	public float escala_x;
	public float escala_y;
	public float posicion_z;

	void Awake()
	{
	 	SituarGUITexture();
	    this.GetComponent<PosicionadorGUITexture>().enabled = false;
	}

	void Update()
	{
		SituarGUITexture();
	}

	public void SituarGUITexture()
	{
	    this.transform.position = new Vector3(0,0,posicion_z);
	    this.transform.localScale = Vector3.zero;
		this.guiTexture.pixelInset = new Rect (posicion_x * Screen.width, posicion_y * Screen.height, escala_x * Screen.width, escala_y * Screen.width);
	}

	public void SetPosicion_X (float p)
	{
		posicion_x = p;
	}

	public void SetPosicion_Y (float p)
	{
		posicion_y = p;
	}

	public void SetEscala_X (float e)
	{
		escala_x = e;
	}

	public void SetEscala_Y (float e)
	{
		escala_y = e;
	}

	public void SetProfundidad (float z)
	{
		posicion_z = z;
	}
}