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
		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, vectorAuxiliarPosicion, Time.deltaTime*smoothAuxiliar);
		Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(vectorAuxiliarRotacion.x, vectorAuxiliarRotacion.y, vectorAuxiliarRotacion.z), Time.deltaTime*smoothAuxiliar);
		
		if(Camera.main.transform.position.ToString() == vectorAuxiliarPosicion.ToString())
		{
			Cambiar_Estado(EstadosJugador.enZoomEspera);
			estadoCambiado = true;
			yield break;
		}
	}
	
	IEnumerator JugadorEnZoomOut()
	{
		float smoothAuxiliar;
		
		if (GameCenter.InstanceRef.controladoraJuego.objetoPulsado == null) 
		//Es un Personaje
			smoothAuxiliar = GameCenter.InstanceRef.controladoraJuego.personajePulsado.Smooth;
		else
		//Es un objeto
			smoothAuxiliar = GameCenter.InstanceRef.controladoraJuego.objetoPulsado.Smooth;

		//Ruta del objeto a la posicion inicial de la camara
		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, zoomCamaraRef.posicionInicial, Time.deltaTime*smoothAuxiliar);
		Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, zoomCamaraRef.rotacionInicial, Time.deltaTime*smoothAuxiliar);
		
		if(Camera.main.transform.position.ToString() == zoomCamaraRef.posicionInicial.ToString())
		{
			Cambiar_Estado(EstadosJugador.enEspera);
			estadoCambiado = true;
			objetoInteractuableRef = null;
			GameCenter.InstanceRef.controladoraJuego.objetoPulsado = null;
			GameCenter.InstanceRef.controladoraJuego.personajePulsado = null;
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
