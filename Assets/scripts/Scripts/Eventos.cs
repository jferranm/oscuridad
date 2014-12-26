using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using Oscuridad.Enumeraciones;

public class Eventos : MonoBehaviour 
{
	public void BotonComenzar()
	{
		string archivoJugador = Path.Combine (GameCenter.InstanceRef.USERPATH, "Jugador.xml");

		if (File.Exists (archivoJugador)) 
		{
			//TODO: empezar por la ultima pantalla
			GameCenter.InstanceRef.controladoraJuego.CargarJugador ();
			GameCenter.InstanceRef.controladoraEscenas.IrEscena10 ();
		}
		else
			GameCenter.InstanceRef.controladoraEscenas.IrEscena1();
	}

	public void BotonPersonaje(string nombrePersonaje)
	{
		switch (nombrePersonaje) 
		{
			case "Maria":
				GameCenter.InstanceRef.controladoraJuego.Inicializar_Partida(Personaje.MarlaGibbs);
				break;

			case "Robert":
				GameCenter.InstanceRef.controladoraJuego.Inicializar_Partida(Personaje.RobertDuncan);
				break;

			case "Warren":
				GameCenter.InstanceRef.controladoraJuego.Inicializar_Partida(Personaje.WarrenBedford);
				break;
		}

		GameCenter.InstanceRef.controladoraEscenas.IrEscena10 ();
	}

	public void BotonAtras(string nombreEscena)
	{
		switch (nombreEscena) 
		{
			case "Escena1":
				GameCenter.InstanceRef.controladoraEscenas.IrMenuPrincipal();
				break;
		}
	}

	public void BotonDireccion(GameObject botonDireccion)
	{
		Color rojo = new Color (255,0,0);
		Image imagenBoton = botonDireccion.GetComponent<Image> ();

		Escenas escenaSeleccionada = Escenas.ninguna;
		
		if (!imagenBoton.color.Equals (rojo)) 
		{
			if(botonDireccion.name.Contains("Arriba"))
			   escenaSeleccionada = GameCenter.InstanceRef.controladoraJuego.escenaActual.EscenaNorte;
			  
			if(botonDireccion.name.Contains("Abajo"))
			   escenaSeleccionada = GameCenter.InstanceRef.controladoraJuego.escenaActual.EscenaSur;

			if(botonDireccion.name.Contains("Izquierda"))
				escenaSeleccionada = GameCenter.InstanceRef.controladoraJuego.escenaActual.EscenaEste;

			if(botonDireccion.name.Contains("Derecha"))
				escenaSeleccionada = GameCenter.InstanceRef.controladoraJuego.escenaActual.EscenaOeste;

			GameCenter.InstanceRef.controladoraGUI.DesactivarGUI ();
			GameCenter.InstanceRef.controladoraJuego.Guardar_Escena ((Escenas)Enum.Parse (typeof(Escenas), Application.loadedLevelName));
			GameCenter.InstanceRef.controladoraEscenas.CambiarSceneSegunEnum(escenaSeleccionada);
		}
	}

	public void BotonObjeto(GameObject botonObjeto)
	{
		Color rojo = new Color (255,0,0);
		Image imagenBoton = botonObjeto.GetComponent<Image> ();

		if (!imagenBoton.color.Equals (rojo)) 
		{
			if(botonObjeto.name.Contains("Volver"))
				GameCenter.InstanceRef.controladoraJugador.EstadoJugador = EstadosJugador.enZoomOut;

			if(botonObjeto.name.Contains("Coger"))
				Coger_Objeto();

			if(botonObjeto.name.Contains("Inspeccionar"))
				Inspeccionar_Objeto();

			if(botonObjeto.name.Contains("Hablar"))
				Hablar_Personaje();
		}
	}

	private void Coger_Objeto()
	{
		//Desactivamos el objeto
		GameObject.FindGameObjectWithTag(GameCenter.InstanceRef.controladoraJuego.objetoPulsado.Nombre).SetActive(false);
		
		//Deshabilitamos los botones
		GameCenter.InstanceRef.controladoraGUI.panelObjetos.GetComponent<PanelObjetosOpciones> ().Desactivar ("Coger");
		GameCenter.InstanceRef.controladoraGUI.panelObjetos.GetComponent<PanelObjetosOpciones> ().Desactivar ("Hablar");
		GameCenter.InstanceRef.controladoraGUI.panelObjetos.GetComponent<PanelObjetosOpciones> ().Desactivar ("Inspeccionar");

		//Insertar objeto en el inventario del jugador
		GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddInventario ((Objetos)Enum.Parse(typeof(Objetos), GameCenter.InstanceRef.controladoraJuego.objetoPulsado.Nombre));
		
		//Cambiamos a false el valor de objetoActivo a false
		GameCenter.InstanceRef.controladoraJuego.objetoPulsado.ObjetoActivo = false;
		
		//Le indicamos a la caja de texto que esta en el inventario
		GameCenter.InstanceRef.controladoraGUI.Insertar_Ventana_Inferior_Texto(GameCenter.InstanceRef.controladoraJuego.Traduccion_Coger_Objeto(GameCenter.InstanceRef.controladoraJuego.objetoPulsado.DescripcionNombre), Color.yellow);
	}

	private void Inspeccionar_Objeto()
	{
		//Deshabilitamos el boton Inspeccionar para no poder pulsar mas veces sobre el mientras se esta inspeccionando
		GameCenter.InstanceRef.controladoraGUI.panelObjetos.GetComponent<PanelObjetosOpciones> ().Desactivar ("Inspeccionar");

		GameCenter.InstanceRef.controladoraGUI.Lanzar_Inspeccionar();
	}

	private void Hablar_Personaje()
	{
		//Deshabilitamos el boton Hablar para no poder pulsar mas veces sobre el mientras se esta hablando
		GameCenter.InstanceRef.controladoraGUI.panelObjetos.GetComponent<PanelObjetosOpciones> ().Desactivar ("Hablar");

		GameCenter.InstanceRef.controladoraGUI.Lanzar_Hablar ();
	}

	public void BotonPregunta()
	{
	}

	public void CajaTexto_OnChange(Vector2 nuevaPosicion)
	{
		Debug.Log (nuevaPosicion);
	}
}
