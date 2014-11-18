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

	//---- Opciones de Jugador
	[HideInInspector]
	public string textoBoton1 = "";
	[HideInInspector]
	public string textoBoton2 = "";
	[HideInInspector]
	public string textoBoton3 = "";
	[HideInInspector]
	public int numeroPregunta1;
	[HideInInspector]
	public int numeroPregunta2;
	[HideInInspector]
	public int numeroPregunta3;
	[HideInInspector]
	public string textoPregunta;
	//------

	private Vector2 posicionBarraScrollConversaciones;
	
	void OnGUI() 
	{ 
		GUI.skin = skinVentana;
		GUILayout.Window(1, new Rect(posicion_x * Screen.width, posicion_y * Screen.height, escala_x * Screen.width, escala_y * Screen.width), ConversacionesWindow,"Ventana Conversaciones");
	}
	
	void ConversacionesWindow(int windowID) 
	{
		posicionBarraScrollConversaciones = GUILayout.BeginScrollView (posicionBarraScrollConversaciones);
			GUILayout.BeginVertical ();
				if (GUILayout.Button(textoBoton1))
				{
					//Insertar_Label_Tabla(true, textoBoton1, Color.green);
					//posicionBarraScrollConversaciones.y = Mathf.Infinity;
					//Limpiar_Datos();
					//Iniciar_Conversacion(numeroPregunta1.ToString(), GameCenter.InstanceRef.controladoraJugador.objetoPulsado.tag.ToString ());
				}
				
				if (GUILayout.Button(textoBoton2))
				{
					//Insertar_Label_Tabla(true, textoBoton2, Color.green);
					//posicionBarraScrollConversaciones.y = Mathf.Infinity;
					//Limpiar_Datos();
					//Iniciar_Conversacion(numeroPregunta2.ToString(), GameCenter.InstanceRef.controladoraJugador.objetoPulsado.tag.ToString ());
				}
				
				if (GUILayout.Button(textoBoton3))
				{
					//Insertar_Label_Tabla(true, textoBoton3, Color.green);
					//posicionBarraScrollConversaciones.y = Mathf.Infinity;
					//Limpiar_Datos();
					//Iniciar_Conversacion(numeroPregunta3.ToString(), GameCenter.InstanceRef.controladoraJugador.objetoPulsado.tag.ToString ());
				}
			GUILayout.EndVertical();
		GUILayout.EndScrollView();

	}

	public void Limpiar_Datos()
	{
		textoPregunta = "";
		textoBoton1 = "";
		textoBoton2 = "";
		textoBoton3 = "";
	}
}