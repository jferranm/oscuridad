using UnityEngine;
using System.Collections;
using Oscuridad.Estados;
using Oscuridad.Enumeraciones;

public class BotonVolver : MonoBehaviour 
{
	void Update()
	{
		/*foreach (Touch touch in Input.touches)
		{
			if (this.guiTexture.HitTest(touch.position))
			{
				//Volvemos al estado del jugador EnEspera
				GameCenter.InstanceRef.controladoraJugador.Cambiar_Estado(EstadosJugador.enZoomOut);
				break;
			}
		}*/
	}

	void OnMouseDown() 
	{
		GameCenter.InstanceRef.controladoraJugador.EstadoJugador = EstadosJugador.enZoomOut;
	}
}
