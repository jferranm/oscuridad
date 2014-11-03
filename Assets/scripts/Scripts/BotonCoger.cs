using UnityEngine;
using System.Collections;
using Oscuridad.Estados;

public class BotonCoger : MonoBehaviour 
{
	private PanelVentana panelVentanaRef;
	private PanelObjetos panelObjetosRef;

	void Start()
	{
		panelVentanaRef = GameObject.Find("PanelGuiVentana").GetComponent<PanelVentana> ();
		panelObjetosRef = GameObject.Find("PanelGuiObjetos").GetComponent<PanelObjetos> ();
	}

	void Update()
	{
		foreach (Touch touch in Input.touches)
		{
			if (this.guiTexture.HitTest(touch.position))
			{
				//Volvemos al estado del jugador EnEspera
				Coger_Objeto();
				break;
			}
		}
	}
	
	void OnMouseDown() 
	{
		if (this.guiTexture.texture == panelObjetosRef.botonCoger) 
		{
			Coger_Objeto();
		}
	}

	private void Coger_Objeto()
	{
		GameCenter.InstanceRef.controladoraJugador.objetoPulsado.SetActive (false);

		panelObjetosRef.Activar_Desactivar_Textura ("BotonCoger", panelObjetosRef.botonCogerInactivo);
		panelObjetosRef.Activar_Desactivar_Textura ("BotonHablar", panelObjetosRef.botonHablarInactivo);
		panelObjetosRef.Activar_Desactivar_Textura ("BotonInspeccionar", panelObjetosRef.botonInspeccionarInactivo);
		
		//TODO:Insertar_En_Inventario();
		
		//Le indicamos a la caja de texto que esta en el inventario
		panelVentanaRef.Insertar_Label_Tabla (false, "'" + GameCenter.InstanceRef.controladoraJugador.objetoPulsado.tag + "' ahora esta en el inventario", Color.white);
		
		//Guardamos el dato que lo hemos cogido en el controlador del objeto
		//GameCenter.InstanceRef.controladoraObjetos.Cambiar_Opcion_Objeto (GameCenter.InstanceRef.controladoraJugador.objetoPulsado.name, true, true, true, true, true);
	}
}
