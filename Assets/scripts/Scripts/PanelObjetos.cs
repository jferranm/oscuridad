using UnityEngine;
using System.Collections;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;

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
		if (GameCenter.InstanceRef.controladoraJuego.objetoPulsado == null) 
		{
			//Es un Personaje y Desactivamos las opciones de coger e inspeccionar
			Desactivar_Opcion("BotonCoger");
			Desactivar_Opcion("BotonInspeccionar");
		}
		else
		{
			//Es un Objeto
			foreach (opcionObjeto opcion in GameCenter.InstanceRef.controladoraJuego.objetoPulsado.ObjetoOpciones) 
			{
				// Si no tiene ninguna opcion de objeto lo desconectamos todo
				string prueba = opcion.ToString();
				if (opcion.Equals(opcionObjeto.Ninguna))
				{
					//Si no tiene ninguna opcion ese objeto desactivamos todas las opciones
					Desactivar_Opcion("BotonCoger");
					Desactivar_Opcion("BotonHablar");
					Desactivar_Opcion("BotonInspeccionar");
					break;
				}
				
				//Ponemos la textura al boton coger, activo o inactivo
				if (opcion.Equals(opcionObjeto.Coger))
					Activar_Opcion("BotonCoger");
				else 
					Desactivar_Opcion("BotonCoger");

				//Ponemos la textura al boton hablar, activo o inactivo
				if (opcion.Equals(opcionObjeto.Hablar))
					Activar_Opcion("BotonHablar");
				else
					Desactivar_Opcion("BotonHablar");

				//Ponemos la textura al boton inspeccionar, activo o inactivo
				if (opcion.Equals(opcionObjeto.Observar))
				{
					if(GameCenter.InstanceRef.controladoraJuego.objetoPulsado.ObjetoInspeccionado)
						Desactivar_Opcion("BotonInspeccionar");
					else
						Activar_Opcion("BotonInspeccionar");
				}
				else
					Desactivar_Opcion("BotonInspeccionar");
			}
		}
	}

	public void Activar_Opcion(string objetoActivar)
	{
		foreach (Transform objetoHijo in this.transform) 
		{
			if(objetoHijo.name == objetoActivar)
			{
				switch(objetoActivar)
				{
					case "BotonCoger": 
						objetoHijo.guiTexture.texture = botonCoger;
						break;
					case "BotonHablar": 
						objetoHijo.guiTexture.texture = botonHablar;
						break;
					case "BotonInspeccionar": 
						objetoHijo.guiTexture.texture = botonInspeccionar;
						break;
				}
			}
		}
	}

	public void Desactivar_Opcion(string objetoDesactivar)
	{
		foreach (Transform objetoHijo in this.transform) 
		{
			if(objetoHijo.name == objetoDesactivar)
			{
				switch(objetoDesactivar)
				{
				case "BotonCoger": 
					objetoHijo.guiTexture.texture = botonCogerInactivo;
					break;
				case "BotonHablar": 
					objetoHijo.guiTexture.texture = botonHablarInactivo;
					break;
				case "BotonInspeccionar": 
					objetoHijo.guiTexture.texture = botonInspeccionarInactivo;
					break;
				}
			}
		}
	}
}
