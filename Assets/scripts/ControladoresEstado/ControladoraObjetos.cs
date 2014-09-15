using UnityEngine;
using System.Collections;
using Oscuridad.Estados;
using Oscuridad.Clases;
using System.Collections.Generic;

[System.Serializable]
public class ControladoraObjetos
{
	private List<Objetos> objetosEnPantalla;

	public ControladoraObjetos()
	{
	}
	
	private static ControladoraObjetos instanceRef;
	
	public static ControladoraObjetos InstanceRef()
	{
		if (instanceRef == null)
		{
			instanceRef = new ControladoraObjetos();
		}
		
		return instanceRef;
	}

	public void Cargar_Estado_Objetos()
	{
		ControlXMLGlobal nuevoXMLGlobal = new ControlXMLGlobal ();
		objetosEnPantalla = new List<Objetos> ();
		
		objetosEnPantalla = nuevoXMLGlobal.Lista_Objetos (Application.loadedLevelName);
		
		foreach (Objetos nuevoObjeto in objetosEnPantalla) 
		{
			//Si lo hemos cogido anteriormente o esta en el inventario, lo deshabilitamos
			if (nuevoObjeto.cogido)
			{
				if(nuevoObjeto.enInventario)
					GameObject.Find(nuevoObjeto.nombreObjeto).SetActive(false);
			}
		}
	}
	
	public void Guardar_Estado_Objetos()
	{
		ControlXMLGlobal nuevoXMLGlobal = new ControlXMLGlobal ();
		
		foreach(Objetos nuevoObjeto in objetosEnPantalla)
		{
			nuevoXMLGlobal.Cambiar_Estado_Objeto(nuevoObjeto, Application.loadedLevelName);
		}
	}
	
	public Objetos Buscar_Objeto(string nombreObjeto)
	{
		Objetos objetoAux = new Objetos ();
		
		foreach (Objetos nuevoObjeto in objetosEnPantalla) 
		{
			if(nuevoObjeto.nombreObjeto.Equals(nombreObjeto))
			{
				objetoAux = nuevoObjeto;
				break;
			}
		}
		
		return objetoAux;
	}
	
	public void Cambiar_Opcion_Objeto(string nombreObjeto, bool cogido, bool inventario, bool conversacion, bool inspeccionar, bool animacion)
	{
		//Campturamos el script del objeto
		Objetos nuevoObjetoInteractuable = Buscar_Objeto(nombreObjeto);
		
		if (inventario)
			nuevoObjetoInteractuable.enInventario = true;
		
		if (conversacion)
			nuevoObjetoInteractuable.hablado = true;
		
		if (inspeccionar)
			nuevoObjetoInteractuable.inspeccionado = true;
		
		if (animacion)
			nuevoObjetoInteractuable.animacion = true;
		
		if (cogido)
			nuevoObjetoInteractuable.cogido = true;

		Guardar_Estado_Objetos();
	}
}
