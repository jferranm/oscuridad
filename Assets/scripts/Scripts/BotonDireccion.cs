using UnityEngine;
using System.Collections;
using Oscuridad.Enumeraciones;

public class BotonDireccion : MonoBehaviour 
{
	[HideInInspector]
	public bool salida = false;
	[HideInInspector]
	public EstadoJuego nuevaSalida;

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
		//GameCenter.InstanceRef.controladoraObjetos.Guardar_Estado_Objetos ();
		GameCenter.InstanceRef.controladoraGUI.DesactivarGUI ();
		GameCenter.InstanceRef.controladoraEscenas.CambiarSceneSegunEnum(nuevaSalida);
	}
}
