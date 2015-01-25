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

	private EstadosJugador estadoJugador;
	[HideInInspector]
	public EstadosJugador EstadoJugador
	{
		get { return estadoJugador; }
		set
		{
			estadoJugador = value;
			CambioEnEstado();
			GameCenter.InstanceRef.controladoraGUI.CambioEnEstado();
		}
	}
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
			switch (EstadoJugador) 
			{
				case EstadosJugador.enZoomIn:
					JugadorEnZoomIn();
					break;
					
				case EstadosJugador.enZoomOut:
					JugadorEnZoomOut();
					break;
			}
		}
	}

	private void CambioEnEstado()
	{
		if (EstadoJugador.Equals (EstadosJugador.enZoomIn) || EstadoJugador.Equals (EstadosJugador.enZoomOut)) 
		{
			estadoCambiado = true;
		}
	}

	public void Nombre_Jugador(string nombre)
	{
		nombreJugador = nombre;
	}
	
	public string Devolver_Nombre_Jugador ()
	{
		return nombreJugador;
	}
	
	private void JugadorEnZoomIn()
	{
		Vector3 vectorAuxiliarPosicion = new Vector3 ();
		Vector3 vectorAuxiliarRotacion = new Vector3 ();
		float smoothAuxiliar;

		if (GameCenter.InstanceRef.controladoraJuego.objetoPulsado == null) 
		//Es un Personaje
		{
			vectorAuxiliarPosicion = GameCenter.InstanceRef.controladoraJuego.personajePulsado.PosicionNueva;
			vectorAuxiliarRotacion = GameCenter.InstanceRef.controladoraJuego.personajePulsado.RotacionNueva;
			smoothAuxiliar = GameCenter.InstanceRef.controladoraJuego.personajePulsado.Smooth;
		}
		else
		//Es un objeto
		{
			vectorAuxiliarPosicion = GameCenter.InstanceRef.controladoraJuego.objetoPulsado.PosicionNueva;
			vectorAuxiliarRotacion = GameCenter.InstanceRef.controladoraJuego.objetoPulsado.RotacionNueva;
			smoothAuxiliar = GameCenter.InstanceRef.controladoraJuego.objetoPulsado.Smooth;
		}

		//Ruta de la camara hacia el objeto seleccionado
		GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.position = Vector3.Lerp(GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.position, vectorAuxiliarPosicion, Time.deltaTime*smoothAuxiliar);
		GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.rotation = Quaternion.Lerp(GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.rotation, Quaternion.Euler(vectorAuxiliarRotacion.x, vectorAuxiliarRotacion.y, vectorAuxiliarRotacion.z), Time.deltaTime*smoothAuxiliar);

		Debug.Log ("Posicion Objeto:" + GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.position);
		Debug.Log ("Posicion Camara:" + vectorAuxiliarPosicion);
		if (GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.position.ToString().Equals(vectorAuxiliarPosicion.ToString())) 
		{
			EstadoJugador = EstadosJugador.enZoomEspera;
			estadoCambiado = false;
		} 
	}

	private void JugadorEnZoomOut()
	{
		float smoothAuxiliar;
		
		if (GameCenter.InstanceRef.controladoraJuego.objetoPulsado == null) 
		//Es un Personaje
			smoothAuxiliar = GameCenter.InstanceRef.controladoraJuego.personajePulsado.Smooth;
		else
		//Es un objeto
			smoothAuxiliar = GameCenter.InstanceRef.controladoraJuego.objetoPulsado.Smooth;

		//Ruta del objeto a la posicion inicial de la camara
		GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.position = Vector3.Lerp(GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.position, zoomCamaraRef.posicionInicial, Time.deltaTime*smoothAuxiliar);
		GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.rotation = Quaternion.Lerp(GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.rotation, zoomCamaraRef.rotacionInicial, Time.deltaTime*smoothAuxiliar);
		
		if(GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.position.ToString() == zoomCamaraRef.posicionInicial.ToString())
		{
			EstadoJugador = EstadosJugador.enEspera;
			estadoCambiado = false;
			objetoInteractuableRef = null;
			GameCenter.InstanceRef.controladoraJuego.objetoPulsado = null;
			GameCenter.InstanceRef.controladoraJuego.personajePulsado = null;
		}
	}
}
