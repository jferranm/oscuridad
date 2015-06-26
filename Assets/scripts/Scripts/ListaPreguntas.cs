﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Oscuridad.Clases;

public class ListaPreguntas: MonoBehaviour
{
	public GameObject itemPrefab;

	private int itemCount;
	private int columnCount = 1;

	void Start()
	{
		if(GameCenter.InstanceRef != null)
			GameCenter.InstanceRef.controladoraGUI.listaPreguntas = this.gameObject.GetComponent<ListaPreguntas>();
	}

	public void Generar_Preguntas(PreguntaBase[] preguntas)
	{
		Vaciar_Contenedor ();

		itemCount = preguntas.Length;

		RectTransform rowRectTransform = itemPrefab.GetComponent<RectTransform>();
		RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();
		
		float width = containerRectTransform.rect.width;
		float height = rowRectTransform.rect.height;
		
		int j = 0;
		for (int i = 0; i < itemCount; i++)
		{
			if (i % columnCount == 0)
				j++;

			GameObject newItem = Instantiate(itemPrefab) as GameObject;
			newItem.name = preguntas[i].IdRespuesta.ToString();
			Text contendorTexto = newItem.GetComponent<Button>().GetComponentInChildren<Text>();

			contendorTexto.text = preguntas[i].TextoPregunta;
			newItem.transform.SetParent(gameObject.transform);
			
			RectTransform rectTransform = newItem.GetComponent<RectTransform>();

			float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);
			float y = containerRectTransform.rect.height / 2 - height * j;
			rectTransform.offsetMin = new Vector2(x, y);

			x = rectTransform.offsetMin.x + width;
			if(GameCenter.InstanceRef.controladoraJuego.Devolver_Tamanyo_Cadena(contendorTexto.text) > x)
				y = rectTransform.offsetMin.y + (height * 2) ;
			else
				y = rectTransform.offsetMin.y + height;
			rectTransform.offsetMax = new Vector2(x, y);
		}
	}

	private void Vaciar_Contenedor()
	{
		foreach (Transform objetoHijo in gameObject.transform) 
		{
			if(!objetoHijo.gameObject.name.Contains("Cuerpo"))
				GameObject.Destroy(objetoHijo.gameObject);
		}
	}
}

