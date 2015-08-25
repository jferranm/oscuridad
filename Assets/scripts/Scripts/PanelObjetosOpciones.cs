using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Oscuridad.Enumeraciones;

public class PanelObjetosOpciones : MonoBehaviour 
{
	public GameObject botonVolver;
	public GameObject botonCoger;
	public GameObject botonHablar;
	public GameObject botonInspeccionar;

	void Start()
	{
		if (GameCenter.InstanceRef != null) 
			GameCenter.InstanceRef.controladoraGUI.panelObjetosOpciones = gameObject.GetComponent<PanelObjetosOpciones>();
	}

	void OnEnable()
	{
		if (GameCenter.InstanceRef != null) 
		{
			Normalizar_Botones();
			Desactivar(botonCoger);
			Desactivar(botonHablar);
			Desactivar(botonInspeccionar);
			Reorganizar_Objetos ();
		}
	}

	public void Normalizar_Botones()
	{
		botonCoger.GetComponent<Image> ().color = new Color (255, 255, 255);
		botonHablar.GetComponent<Image> ().color = new Color (255, 255, 255);
		botonInspeccionar.GetComponent<Image> ().color = new Color (255, 255, 255);
	}

	private void Reorganizar_Objetos()
	{
		foreach (OpcionInteractuable opcion in GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.MostrarOpciones()) 
		{
			if(opcion.Equals(OpcionInteractuable.Hablar))
			   Activar(botonHablar);

			if(opcion.Equals(OpcionInteractuable.Observar))
				if(!GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.InteractuableInspeccionado)
					Activar(botonInspeccionar);

			if(opcion.Equals(OpcionInteractuable.Coger))
				Activar(botonCoger);
		}
	}
	
	private void Desactivar (GameObject objeto)
	{
		Image imagenBoton = objeto.GetComponent<Image> ();
		
		imagenBoton.color = new Color (255,0,0);
	}

	private void Activar (GameObject objeto)
	{
		Image imagenBoton = objeto.GetComponent<Image> ();
		
		imagenBoton.color = new Color (255,255,255);
	}

	public void Desactivar (string nombreBoton)
	{
		if (botonCoger.name.Contains (nombreBoton))
			Desactivar (botonCoger);

		if (botonHablar.name.Contains (nombreBoton))
			Desactivar (botonHablar);

		if (botonInspeccionar.name.Contains (nombreBoton))
			Desactivar (botonInspeccionar);
	}

	public void Activar (string nombreBoton)
	{
		if (botonCoger.name.Contains (nombreBoton))
			Activar (botonCoger);
		
		if (botonHablar.name.Contains (nombreBoton))
			Activar (botonHablar);
		
		if (botonInspeccionar.name.Contains (nombreBoton))
			Activar (botonInspeccionar);
	}

}
