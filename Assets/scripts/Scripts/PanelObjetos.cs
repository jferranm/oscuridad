using UnityEngine;
using System.Collections;
using Oscuridad.Estados;
using Oscuridad.Clases;

public class PanelObjetos : MonoBehaviour 
{
	public Texture2D botonVolver;
	public Texture2D botonCoger;
	public Texture2D botonCogerInactivo;
	public Texture2D botonHablar;
	public Texture2D botonHablarInactivo;
	public Texture2D botonInspeccionar;
	public Texture2D botonInspeccionarInactivo;

	private PanelVentana panelVentanaRef;

	void Awake()
	{
		DontDestroyOnLoad(this);
		this.gameObject.SetActive(false);
	}

	void Start()
	{
		panelVentanaRef = GameObject.Find("PanelGuiVentana").GetComponent<PanelVentana> ();
	}

	void OnEnable()
	{
		if (GameCenter.InstanceRef.controladoraJugador.objetoInteractuableRef != null) 
		{
			//Campturamos el script del objeto
			Objetos nuevoObjetoInteractuable = GameCenter.InstanceRef.controladoraObjetos.Buscar_Objeto(GameCenter.InstanceRef.controladoraJugador.objetoInteractuableRef.tag.ToString());

			//Ponemos la textura al boton coger, activo o inactivo
			if (nuevoObjetoInteractuable.cogido) 
				Activar_Desactivar_Textura("BotonCoger", botonCogerInactivo);
			else
				Activar_Desactivar_Textura("BotonCoger", botonCoger);
			
			//Ponemos la textura al boton hablar, activo o inactivo
			if (GameCenter.InstanceRef.controladoraJugador.objetoInteractuableRef.esNPC)
				Activar_Desactivar_Textura("BotonHablar", botonHablar);
			else
				Activar_Desactivar_Textura("BotonHablar", botonHablarInactivo);
						
			//Ponemos la textura al boton inspeccionar, activo o inactivo
			if (nuevoObjetoInteractuable.inspeccionado) 
				Activar_Desactivar_Textura("BotonInspeccionar", botonInspeccionarInactivo);
			else
				Activar_Desactivar_Textura("BotonInspeccionar", botonInspeccionar);
		}
	}

	public void Activar_Desactivar_Textura(string objetoActivar, Texture2D texturaSeleccionada)
	{
		foreach (Transform objetoHijo in this.transform) 
		{
			if(objetoHijo.name == objetoActivar)
			{
				objetoHijo.guiTexture.texture = texturaSeleccionada;
			}
		}
	}
}
