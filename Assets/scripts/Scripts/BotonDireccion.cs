using UnityEngine;
using System;
using System.Collections;
using Oscuridad.Enumeraciones;

public class BotonDireccion : MonoBehaviour 
{
	[HideInInspector]
	public bool salida = false;
	[HideInInspector]
	public Escenas nuevaSalida;

	void Update()
	{
		foreach (Touch touch in Input.touches)
		{
			if (this.guiTexture.HitTest(touch.position))
			{
				if(salida)
				{
					//Input.touches.Initialize();
					CambiarEscena();
				}
				break;
			}
		}
	}
	
	void OnMouseDown() 
	{
		if(salida)
		{
			CambiarEscena();
		}
	}

	private void CambiarEscena()
	{
		GameCenter.InstanceRef.controladoraGUI.DesactivarGUI ();
		GameCenter.InstanceRef.controladoraJuego.Guardar_Escena ((Escenas)Enum.Parse (typeof(Escenas), Application.loadedLevelName));
		GameCenter.InstanceRef.controladoraEscenas.CambiarSceneSegunEnum(nuevaSalida);
	}
}
