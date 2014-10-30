using UnityEngine;
using System.Collections;
using Oscuridad.Clases;

public class ControlMenus : MonoBehaviour 
{
	void OnEnable()
	{
		if(GameCenter.InstanceRef.controladoraJuego.jugadorActual.TipoPersonaje == null)
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
