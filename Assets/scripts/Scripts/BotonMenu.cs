using UnityEngine;
using System.Collections;
using Oscuridad.Personajes;
using Oscuridad.Clases;

public class BotonMenu : MonoBehaviour 
{
	private GameObject controlMenuRef;

	void OnEnable()
	{
		controlMenuRef = GameObject.Find("ControlMenu");
	}

	void OnMouseOver()
	{
		this.guiText.fontStyle = FontStyle.Bold;
	}

	void OnMouseExit() 
	{
		this.guiText.fontStyle = FontStyle.Normal;
	}

	void OnMouseDown() 
	{
		GameCenter.InstanceRef.audio.PlayOneShot(GameCenter.InstanceRef.controladoraSonidos.sonidoFalloPulsar);

		if(this.guiText.tag.ToString().Contains("BotonCambio"))
			controlMenuRef.SetActive(false);

		Lanzar_Pantalla(this.guiText.gameObject.name.ToString());
	}

	private void Lanzar_Pantalla(string boton)
	{
		switch(boton)
		{
			case "Comenzar": 
				GameCenter.InstanceRef.controladoraEscenas.IrEscena1();
				break;

			case "Continuar":
				//TODO: empezar por la ultima pantalla
				GameCenter.InstanceRef.controladoraEscenas.IrEscena10();
				break;

			case "Opciones":
				GameCenter.InstanceRef.controladoraEscenas.IrEscena2();
				break;

			case "Salir":
				Application.Quit();	
				break;

			case "MariaGibbs":
				//TODO: ir a mapa
				GameCenter.InstanceRef.controladoraJugador.Nombre_Jugador("Maria Gibbs");
				Seleccion_Personaje();
				GameCenter.InstanceRef.controladoraEscenas.IrEscena10();
				break;

			case "WarrenBedford":
				//TODO: ir a mapa
				GameCenter.InstanceRef.controladoraJugador.Nombre_Jugador("Warren Bedford");
				Seleccion_Personaje();
				GameCenter.InstanceRef.controladoraEscenas.IrEscena10();
				break;

			case "RobertDuncan":
				//TODO: ir a mapa
				GameCenter.InstanceRef.controladoraJugador.Nombre_Jugador("Robert Duncan");
				Seleccion_Personaje();
				GameCenter.InstanceRef.controladoraEscenas.IrEscena10();
				break;

			case "Atras":
				GameCenter.InstanceRef.controladoraEscenas.IrMenuPrincipal();
				break;

			case "PartidaNueva":
				ControlXMLGlobal nuevoControl = new ControlXMLGlobal();
				nuevoControl.Borrar_Configuracion();
				GameCenter.InstanceRef.controladoraEscenas.IrEscena1();
				break;

			case "Musica":
				if(this.guiText.text.Contains("ON"))
			   	{
			    	//TODO: lanzar apagar musica
					this.guiText.text.Replace("ON", "OFF");
				}
				else
				{
					//TODO: lanzar encender musica
					this.guiText.text.Replace("OFF", "ON");
				}
				break;

			case "Sonido":
				if(this.guiText.text.Contains("ON"))
				{
					//TODO: lanzar apagar musica
					this.guiText.text.Replace("ON", "OFF");
				}
				else
				{
					//TODO: lanzar encender musica
					this.guiText.text.Replace("OFF", "ON");
				}
				break;	
		}
	}

	public void Seleccion_Personaje()
	{
		Utils util = new Utils();

		switch (GameCenter.InstanceRef.controladoraJugador.Devolver_Nombre_Jugador()) 
		{
			case "Maria Gibbs":	
				util.crearHojaPersonaje(Personaje.MarlaGibbs);
				break;
				
			case "Warren Bedford": 	
				util.crearHojaPersonaje(Personaje.WarrenBedford);
				break;
				
			case "Robert Duncan":	
				util.crearHojaPersonaje(Personaje.RobertDuncan);
				break;
		}

		//TODO: inicializar el personaje seleccionado
		util.crearHojaPersonaje(Personaje.MarlaGibbs);
		util.Inicializar_XML();
	}
}
