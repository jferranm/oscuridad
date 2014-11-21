using UnityEngine;
using System.Collections;
using System;
using Oscuridad.Estados;
using Oscuridad.Enumeraciones;

public class BotonCoger : MonoBehaviour 
{
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
		//Desactivamos el objeto
		GameObject.FindGameObjectWithTag(GameCenter.InstanceRef.controladoraJuego.objetoPulsado.Nombre).SetActive(false);

		//Deshabilitamos los botones
		panelObjetosRef.Desactivar_Opcion("BotonCoger");
		panelObjetosRef.Desactivar_Opcion("BotonHablar");
		panelObjetosRef.Desactivar_Opcion("BotonInspeccionar");
		
		//Insertar objeto en el inventario del jugador
		GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddInventario ((Objetos)Enum.Parse(typeof(Objetos), GameCenter.InstanceRef.controladoraJuego.objetoPulsado.Nombre));

		//Cambiamos a false el valor de objetoActivo a false
		GameCenter.InstanceRef.controladoraJuego.objetoPulsado.ObjetoActivo = false;
		
		//Le indicamos a la caja de texto que esta en el inventario
		GameCenter.InstanceRef.controladoraGUI.Insertar_Ventana_Inferior_Texto(GameCenter.InstanceRef.controladoraJuego.objetoPulsado.Objeto, Color.yellow);
	}
}
