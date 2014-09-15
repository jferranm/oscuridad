using UnityEngine;
using System.Collections;
using Oscuridad.Interfaces;
using Oscuridad.Personajes;
using Oscuridad.Enumeraciones;

[System.Serializable]
public class ControladoraJugador 
{
	[HideInInspector]
	public Vector3 posicionInicial;
	[HideInInspector]
	public Quaternion rotacionInicial;
	[HideInInspector]
	public ZoomCamara zoomCamaraRef;
	[HideInInspector]
	public bool estadoCambiado;
	[HideInInspector]
	public ObjetoInteractuablev2 objetoInteractuableRef;
	[HideInInspector]
	public GameObject objetoPulsado;
	
	private estadoJugador estadosJugador;
	private string nombreJugador;

	public ControladoraJugador()
	{
	}

	private static ControladoraJugador instanceRef;
	public static ControladoraJugador InstanceRef()
	{
		if (instanceRef == null)
		{
			instanceRef = new ControladoraJugador();
		}

		return instanceRef;
	}

	public void Update()
	{
		if (estadoCambiado) 
		{
			switch (estadosJugador) 
			{
			case estadoJugador.enEspera:	
				JugadorEnEspera();
				break;
				
			case estadoJugador.enZoomIn:
				GameCenter.InstanceRef.CoroutinaBase(JugadorEnZoomIn());
				break;
				
			case estadoJugador.enZoomOut:
				GameCenter.InstanceRef.CoroutinaBase(JugadorEnZoomOut());
				break;
				
			case estadoJugador.enZoomEspera:
				JugadorEnZoomEspera();
				break;
				
			case estadoJugador.enMenus:
				JugadorEnMenus();
				break;
			}
		}
	}

	public void Cambiar_Estado(estadoJugador nuevoEstado)
	{
		estadosJugador = nuevoEstado;
		estadoCambiado = true;
		GameCenter.InstanceRef.controladoraGUI.estadoCambiado = true;
	}
	
	public estadoJugador Devolver_Estado()
	{
		return estadosJugador;
	}
	
	public void Nombre_Jugador(string nombre)
	{
		nombreJugador = nombre;
	}
	
	public string Devolver_Nombre_Jugador ()
	{
		return nombreJugador;
	}
	
	private void JugadorEnEspera()
	{
		estadoCambiado = false;
	}
	
	IEnumerator JugadorEnZoomIn()
	{
		//Ruta de la camara hacia el objeto seleccionado
		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, objetoInteractuableRef.posicionNueva, Time.deltaTime*objetoInteractuableRef.smooth);
		Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(objetoInteractuableRef.rotacionNueva.x, objetoInteractuableRef.rotacionNueva.y, objetoInteractuableRef.rotacionNueva.z), Time.deltaTime*objetoInteractuableRef.smooth);
		
		if(Camera.main.transform.position.ToString() == objetoInteractuableRef.posicionNueva.ToString())
		{
			Cambiar_Estado(estadoJugador.enZoomEspera);
			estadoCambiado = true;
			yield break;
		}
	}
	
	IEnumerator JugadorEnZoomOut()
	{
		//Ruta del objeto a la posicion inicial de la camara
		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, zoomCamaraRef.posicionInicial, Time.deltaTime*objetoInteractuableRef.smooth);
		Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, zoomCamaraRef.rotacionInicial, Time.deltaTime*objetoInteractuableRef.smooth);
		
		if(Camera.main.transform.position.ToString() == zoomCamaraRef.posicionInicial.ToString())
		{
			Cambiar_Estado(estadoJugador.enEspera);
			estadoCambiado = true;
			objetoInteractuableRef = null;
			yield break;
		}
	}
	
	private void JugadorEnZoomEspera()
	{
		estadoCambiado = false;
	}
	
	private void JugadorEnMenus()
	{
		GameCenter.InstanceRef.controladoraGUI.DesactivarGUI ();
		estadoCambiado = false;
	}
}
