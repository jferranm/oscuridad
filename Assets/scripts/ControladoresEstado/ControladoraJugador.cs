using UnityEngine;
using System.Collections;
using Oscuridad.Interfaces;
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
	
	private EstadosJugador estadosJugador;
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
			case EstadosJugador.enEspera:	
				JugadorEnEspera();
				break;
				
			case EstadosJugador.enZoomIn:
				GameCenter.InstanceRef.CoroutinaBase(JugadorEnZoomIn());
				break;
				
			case EstadosJugador.enZoomOut:
				GameCenter.InstanceRef.CoroutinaBase(JugadorEnZoomOut());
				break;
				
			case EstadosJugador.enZoomEspera:
				JugadorEnZoomEspera();
				break;
				
			case EstadosJugador.enMenus:
				JugadorEnMenus();
				break;
			}
		}
	}

	public void Cambiar_Estado(EstadosJugador nuevoEstado)
	{
		estadosJugador = nuevoEstado;
		estadoCambiado = true;
		GameCenter.InstanceRef.controladoraGUI.estadoCambiado = true;
	}
	
	public EstadosJugador Devolver_Estado()
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
		//Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, objetoInteractuableRef.posicionNueva, Time.deltaTime*objetoInteractuableRef.smooth);
		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, GameCenter.InstanceRef.controladoraJuego.objetoPulsado.PosicionNueva, Time.deltaTime*GameCenter.InstanceRef.controladoraJuego.objetoPulsado.Smooth);
		Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(GameCenter.InstanceRef.controladoraJuego.objetoPulsado.RotacionNueva.x, GameCenter.InstanceRef.controladoraJuego.objetoPulsado.RotacionNueva.y, GameCenter.InstanceRef.controladoraJuego.objetoPulsado.RotacionNueva.z), Time.deltaTime*GameCenter.InstanceRef.controladoraJuego.objetoPulsado.Smooth);
		
		if(Camera.main.transform.position.ToString() == GameCenter.InstanceRef.controladoraJuego.objetoPulsado.PosicionNueva.ToString())
		{
			Cambiar_Estado(EstadosJugador.enZoomEspera);
			estadoCambiado = true;
			yield break;
		}
	}
	
	IEnumerator JugadorEnZoomOut()
	{
		//Ruta del objeto a la posicion inicial de la camara
		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, zoomCamaraRef.posicionInicial, Time.deltaTime*GameCenter.InstanceRef.controladoraJuego.objetoPulsado.Smooth);
		Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, zoomCamaraRef.rotacionInicial, Time.deltaTime*GameCenter.InstanceRef.controladoraJuego.objetoPulsado.Smooth);
		
		if(Camera.main.transform.position.ToString() == zoomCamaraRef.posicionInicial.ToString())
		{
			Cambiar_Estado(EstadosJugador.enEspera);
			estadoCambiado = true;
			objetoInteractuableRef = null;
			GameCenter.InstanceRef.controladoraJuego.objetoPulsado = null;
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
