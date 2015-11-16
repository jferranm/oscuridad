using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Oscuridad.Enumeraciones;

public class OpcionesEscena1 : MonoBehaviour 
{
	public GameObject panelTexto;
	public Text textoPanel;
	private OpcionesTextoPersonaje opcionesTexto;

	void Start()
	{
		opcionesTexto = textoPanel.GetComponent<OpcionesTextoPersonaje> ();
	}

	public void SeleccionarPersonaje(Button boton)
	{
		if (boton.tag.Equals ("Deseleccionado")) 
		{
			if(GameObject.FindGameObjectsWithTag("Seleccionado").Length > 0)
				BajarPersonaje (GameObject.FindGameObjectWithTag ("Seleccionado").GetComponent<RectTransform> ());

			SubirPersonaje (boton.GetComponent<RectTransform> ());
			ActivarTexto(boton);
		}
		else 
		{
			GameCenter.InstanceRef.controladoraJuego.Inicializar_Partida((Personaje)Enum.Parse(typeof(Personaje), boton.name));
			GameCenter.InstanceRef.controladoraEscenas.IrEscenaWardExterior ();
		}
	}

	private void SubirPersonaje(RectTransform seleccionRect)
	{
		StartCoroutine(GameCenter.InstanceRef.controladoraJuego.Mover2D(seleccionRect, new Vector2(seleccionRect.localPosition.x , seleccionRect.localPosition.y + 100), 0f));
		seleccionRect.gameObject.tag = "Seleccionado";
	}

	private void BajarPersonaje(RectTransform seleccionRect)
	{
		StartCoroutine(GameCenter.InstanceRef.controladoraJuego.Mover2D(seleccionRect, new Vector2(seleccionRect.localPosition.x , seleccionRect.localPosition.y - 100), 0f));
		seleccionRect.gameObject.tag = "Deseleccionado";
	}

	private void ActivarTexto(Button boton)
	{
		panelTexto.SetActive (true);
		opcionesTexto.Posicion_Inicial_Caja ();
		textoPanel.text = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.DevolverTexto("bt" + boton.name);
	}
}
