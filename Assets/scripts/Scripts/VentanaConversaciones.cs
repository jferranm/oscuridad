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
	
	private Vector2 posicionBarraScrollConversaciones;
	
	void OnGUI() 
	{ 
		GUI.skin = skinVentana;
		try
		{
			GUILayout.Window(1, new Rect(posicion_x * Screen.width, posicion_y * Screen.height, escala_x * Screen.width, escala_y * Screen.width), ConversacionesWindow,GameCenter.InstanceRef.controladoraGUI.cabeceraInferior);
		}
		catch {}
	}
	
	void ConversacionesWindow(int windowID) 
	{
		posicionBarraScrollConversaciones = GUILayout.BeginScrollView (posicionBarraScrollConversaciones);
			GUILayout.BeginVertical ();
				if (GUILayout.Button(GameCenter.InstanceRef.controladoraGUI.textoBotones[0].TextoPregunta))
				{
					GameCenter.InstanceRef.controladoraGUI.Boton_Pulsado(0);
				}
				
				if (GUILayout.Button(GameCenter.InstanceRef.controladoraGUI.textoBotones[1].TextoPregunta))
				{
					GameCenter.InstanceRef.controladoraGUI.Boton_Pulsado(1);
				}
				
				if (GUILayout.Button(GameCenter.InstanceRef.controladoraGUI.textoBotones[2].TextoPregunta))
				{
					GameCenter.InstanceRef.controladoraGUI.Boton_Pulsado(2);
				}
			GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}
}