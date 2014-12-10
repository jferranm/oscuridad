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
}
