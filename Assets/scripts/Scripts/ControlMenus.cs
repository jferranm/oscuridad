using UnityEngine;
using System.Collections;
using Oscuridad.Clases;

public class ControlMenus : MonoBehaviour 
{
	void OnEnable()
	{
		ControlXMLGlobal nuevoControl = new ControlXMLGlobal ();
		if (nuevoControl.Checkear_Estado ()) 
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
