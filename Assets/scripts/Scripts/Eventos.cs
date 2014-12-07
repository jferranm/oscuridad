using UnityEngine;
using System.Collections;
using System.IO;
using Oscuridad.Clases;
using Oscuridad.Estados;

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
}
