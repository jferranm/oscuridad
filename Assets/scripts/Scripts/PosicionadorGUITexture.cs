using UnityEngine;

[ExecuteInEditMode]
public class PosicionadorGUITexture : MonoBehaviour 
{
	[Range(0, 1)]
	public float posicion_x;
	[Range(0, 1)]
	public float posicion_y;
	[Range(0, 1)]
	public float escala_x;
	[Range(0, 1)]
	public float escala_y;
	[Range(0,1)]
	public float posicion_z;

	private ParametrosGUITexture ParametrosGUITexture_ref;

	void Start()
	{
		ParametrosGUITexture_ref = this.GetComponent<ParametrosGUITexture>();
	}

	void Update() 
	{
		this.transform.position = new Vector3(0,0,posicion_z);
		this.transform.localScale = Vector3.zero;
		this.guiTexture.pixelInset = new Rect(posicion_x * Screen.width, posicion_y * Screen.height, escala_x * Screen.width, escala_y * Screen.width);
		ParametrosGUITexture_ref.SetPosicion_X(posicion_x);
		ParametrosGUITexture_ref.SetPosicion_Y(posicion_y);
		ParametrosGUITexture_ref.SetEscala_X (escala_x);
		ParametrosGUITexture_ref.SetEscala_Y (escala_y);
		ParametrosGUITexture_ref.SetProfundidad (posicion_z);
	}
}