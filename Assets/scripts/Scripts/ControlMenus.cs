using UnityEngine;
using System.Collections;
using System.IO;
using Oscuridad.Clases;

public class ControlMenus : MonoBehaviour 
{
	void OnEnable()
	{
		string archivoJugador = Path.Combine (GameCenter.InstanceRef.USERPATH, "Jugador.xml");
		if (File.Exists (archivoJugador))
			Desactiva("Comenzar");
		else 
			Desactiva("Continuar");
	}

	public void Desactiva(string botonDesactivar)
	{
		foreach (Transform objetoHijo in this.transform) 
		{
			if(objetoHijo.name == botonDesactivar)
			{
				objetoHijo.gameObject.SetActive(false);
			}
		}
	}
}
