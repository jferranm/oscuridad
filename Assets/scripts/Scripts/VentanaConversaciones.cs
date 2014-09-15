using UnityEngine;

[ExecuteInEditMode]
public class VentanaConversaciones: MonoBehaviour 
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

	public GUISkin skinVentana;
	
	void OnGUI() 
	{ 
		GUI.skin = skinVentana;
		GUILayout.Window(1, new Rect(posicion_x * Screen.width, posicion_y * Screen.height, escala_x * Screen.width, escala_y * Screen.width),Conversaciones,"Ventana Conversaciones");
	}
	
	void Conversaciones(int windowID) 
	{
	}
}