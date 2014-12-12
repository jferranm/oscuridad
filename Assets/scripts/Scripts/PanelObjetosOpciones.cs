using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Oscuridad.Enumeraciones;

public class PanelObjetosOpciones : MonoBehaviour 
{
	public GameObject botonVolver;
	public GameObject botonCoger;
	public GameObject botonHablar;
	public GameObject botonInspeccionar;
	
	void OnEnable()
	{
		if (GameCenter.InstanceRef != null) 
		{
			Normalizar_Botones();
			Reorganizar_Objetos ();
		}
	}
	
	private void Reorganizar_Objetos()
	{
		if (GameCenter.InstanceRef.controladoraJuego.objetoPulsado == null) 
		{
			//Es un Personaje y Desactivamos las opciones de coger e inspeccionar
			Desactivar(botonCoger);
			Desactivar(botonInspeccionar);
		}
		else
		{
			//Es un Objeto
			foreach (opcionObjeto opcion in GameCenter.InstanceRef.controladoraJuego.objetoPulsado.ObjetoOpciones) 
			{
				// Si no tiene ninguna opcion de objeto lo desconectamos todo
				if (opcion.Equals(opcionObjeto.Ninguna))
				{
					//Si no tiene ninguna opcion ese objeto desactivamos todas las opciones
					Desactivar(botonCoger);
					Desactivar(botonHablar);
					Desactivar(botonInspeccionar);
					break;
				}
				
				//Ponemos la textura al boton coger, activo o inactivo
				if (!opcion.Equals(opcionObjeto.Coger))
					Desactivar(botonCoger);
				
				//Ponemos la textura al boton hablar, activo o inactivo
				if (!opcion.Equals(opcionObjeto.Hablar))
					Desactivar(botonHablar);
				
				//Ponemos la textura al boton inspeccionar, activo o inactivo
				if (opcion.Equals(opcionObjeto.Observar))
				{
					if(GameCenter.InstanceRef.controladoraJuego.objetoPulsado.ObjetoInspeccionado)
						Desactivar(botonInspeccionar);
				}
				else
					Desactivar(botonInspeccionar);
			}
		}
	}
	
	private void Normalizar_Botones()
	{
		botonCoger.GetComponent<Image> ().color = new Color (255, 255, 255);
		botonHablar.GetComponent<Image> ().color = new Color (255, 255, 255);
		botonInspeccionar.GetComponent<Image> ().color = new Color (255, 255, 255);
	}
	
	private void Desactivar (GameObject objeto)
	{
		Image imagenBoton = objeto.GetComponent<Image> ();
		
		imagenBoton.color = new Color (255,0,0);
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
}
