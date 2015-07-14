using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Oscuridad.Clases;

public class ListaPreguntas: MonoBehaviour
{
	public GameObject itemPrefab;

	private int itemCount;
	private int columnCount = 1;
	private GameObject cuerpo;

	void Start()
	{
		if (GameCenter.InstanceRef != null) 
		{
			GameCenter.InstanceRef.controladoraGUI.listaPreguntas = this.gameObject.GetComponent<ListaPreguntas> ();
		}
	}

	public void Generar_Preguntas(PreguntaBase[] preguntas)
	{
		GameCenter.InstanceRef.controladoraGUI.textoInferiorOpciones.gameObject.SetActive (false);
		GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.gameObject.SetActive (true);

		itemCount = preguntas.Length;
		RectTransform rowRectTransform = itemPrefab.GetComponent<RectTransform>();
		cuerpo = Buscar_Contenedor ("PanelPreguntas");
		RectTransform containerRectTransform = cuerpo.GetComponent<RectTransform> ();
		
		float width = containerRectTransform.rect.width;
		float height = rowRectTransform.rect.height;

		float sizeY = 0;
		
		int j = 0;
		for (int i = 0; i < itemCount; i++)
		{
			if (i % columnCount == 0)
				j++;

			GameObject newItem = Instantiate(itemPrefab) as GameObject;
			newItem.name = preguntas[i].IdRespuesta.ToString();
			Text contendorTexto = newItem.GetComponent<Button>().GetComponentInChildren<Text>();

			contendorTexto.text = preguntas[i].TextoPregunta;
			newItem.transform.SetParent(cuerpo.transform);
			
			RectTransform rectTransform = newItem.GetComponent<RectTransform>();

			float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);
 			if(GameCenter.InstanceRef.controladoraJuego.Devolver_Tamanyo_Cadena(contendorTexto.text, 16) > containerRectTransform.rect.width)
				j++;
			float y = containerRectTransform.rect.height / 2 - height * j;
			rectTransform.offsetMin = new Vector2(x, y);

			x = rectTransform.offsetMin.x + width;
			if(GameCenter.InstanceRef.controladoraJuego.Devolver_Tamanyo_Cadena(contendorTexto.text, 16) > containerRectTransform.rect.width)
				y = rectTransform.offsetMin.y + (height * 2) ;
			else
				y = rectTransform.offsetMin.y + height;
			rectTransform.offsetMax = new Vector2(x, y);

			sizeY += y;
		}

		GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.rectPanelPreguntas.offsetMax = new Vector2(GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.rectPanelPreguntas.offsetMax.x, (sizeY * -1));
		GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.rectPanelPreguntas.localPosition = new Vector2 (GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.rectPanelPreguntas.localPosition.x, (sizeY/2));
		GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.y = GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.rectPanelPreguntas.localPosition.y;

		foreach (Button boton in cuerpo.GetComponentsInChildren<Button>()) 
		{
			boton.transform.localPosition = new Vector2(cuerpo.transform.localPosition.x, boton.transform.localPosition.y + (GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.y * -1));
		}
	}

	private GameObject Buscar_Contenedor(string nombreContenedor)
	{
		foreach (Transform objetoHijo in gameObject.transform) 
		{
			if(objetoHijo.gameObject.name.Contains(nombreContenedor))
				return objetoHijo.gameObject;
		}

		return null;
	}
}

