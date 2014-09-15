using UnityEngine;

[ExecuteInEditMode]
public class PosicionadorGUIText : MonoBehaviour 
{
	[Range(0, 1)]
	public float posicion_x;
	[Range(0, 1)]
	public float posicion_y;
	[Range(0, 1)]
	public float size_fuente;

	private ParametrosGUIText ParametrosGUIText_ref;

	void Start()
	{
		ParametrosGUIText_ref = this.GetComponent<ParametrosGUIText>();
	}
	
	void Update() 
	{
		this.guiText.fontSize = (int)(size_fuente * Screen.width);
		this.transform.position = new Vector3(0,0, 1);
		this.transform.localScale = Vector3.zero;
		this.guiText.pixelOffset = new Vector2(posicion_x * Screen.width, posicion_y * Screen.height);
		ParametrosGUIText_ref.SetPosicion_X(posicion_x);
		ParametrosGUIText_ref.SetPosicion_Y(posicion_y);
		ParametrosGUIText_ref.SetSizeFuente(size_fuente);
	}
}