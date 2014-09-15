using UnityEngine;

public class ObjetoInteractuablev2 : MonoBehaviour {

	//Variable para el control de la animacion
	public bool conAnimacion;
	public AnimationClip animacion;
	
	//Variables para el control del sonido
	public bool conSonido;
	public AudioClip sonido;
	
	//Variables para el control del Zoom
	public bool conZoom;
	
	//Posiciones para Zoom
	public Vector3 posicionNueva;
	public Vector3 rotacionNueva;
	public float smooth;
	
	//Personaje con Conversacion
	public bool esNPC = false;
	public string inicioConversacion;
	
	//Objeto que se puede coger
	public bool esCapturable = false;

	//Objeto que se puede Inspeccionar
	public bool esInspeccionable = false;
}
