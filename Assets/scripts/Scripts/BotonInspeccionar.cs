using UnityEngine;
using System.Collections;
using Oscuridad.Estados;

public class BotonInspeccionar : MonoBehaviour 
{
	private PanelVentana panelVentanaRef;
	private PanelObjetos panelObjetosRef;
	
	void Start()
	{
		panelObjetosRef = GameObject.Find("PanelGuiObjetos").GetComponent<PanelObjetos> ();
	}

	void Update()
	{
		foreach (Touch touch in Input.touches)
		{
			if (this.guiTexture.HitTest(touch.position))
			{
				//Volvemos al estado del jugador EnEspera
				Inspeccionar_Objeto();
				break;
			}
		}
	}
	
	void OnMouseDown() 
	{
		Inspeccionar_Objeto ();
	}

	private void Inspeccionar_Objeto()
	{
		if (this.guiTexture.texture == panelObjetosRef.botonInspeccionar) 
		{
			//Desactivamos el icono de Inspeccionar
			panelObjetosRef.Activar_Desactivar_Textura ("BotonInspeccionar", panelObjetosRef.botonInspeccionarInactivo);
			
			//Mostramos el Inicio de la pregunta
			GameCenter.InstanceRef.controladoraTextos.Lanzar_Inspeccionar();

			//Marcamos el objeto como que ya esta inspeccionado
			GameCenter.InstanceRef.controladoraObjetos.Cambiar_Opcion_Objeto (GameCenter.InstanceRef.controladoraJugador.objetoPulsado.tag.ToString(), false, false, false, true, false);
		}
	}
}
