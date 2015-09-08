using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	public void Generar_Preguntas(PreguntaUsuarioBase[] preguntas)
	{
		GameCenter.InstanceRef.controladoraGUI.textoInferiorOpciones.gameObject.SetActive (false);
		GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.gameObject.SetActive (true);

		itemCount = preguntas.Length;
		RectTransform rowRectTransform = itemPrefab.GetComponent<RectTransform>();
		cuerpo = Buscar_Contenedor ("PanelPreguntas");
		foreach (Transform hijo in cuerpo.transform) 
		{
			GameObject.Destroy(hijo.gameObject);
		}
		GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.rectPanelPreguntas.offsetMax = new Vector2(0,0);
		RectTransform containerRectTransform = cuerpo.GetComponent<RectTransform> ();
		containerRectTransform.offsetMax = new Vector2 (0, 0);
		containerRectTransform.offsetMin = new Vector2 (0, 0);
		
		float width = containerRectTransform.rect.width;
		float height = rowRectTransform.rect.height;

		float posicionUltimoElemento = 0;
		
		int j = 0;
		for (int i = 0; i < itemCount; i++)
		{
			if (i % columnCount == 0)
				j++;

			GameObject newItem = Instantiate(itemPrefab) as GameObject;
			newItem.name = preguntas[i].IdPreguntaUsuario.ToString();
			Text contenedorTexto = newItem.GetComponent<Button>().GetComponentInChildren<Text>();

			contenedorTexto.text = preguntas[i].PreguntaEjecutada ? "<color=white>- " + preguntas[i].TextoPregunta + "</color>" : "<color=yellow>- " + preguntas[i].TextoPregunta + "</color>";
			//contenedorTexto.text = "- " + preguntas[i].TextoPregunta;
			//contenedorTexto.color = preguntas[i].PreguntaEjecutada ? new Color(186,179,18) : new Color(244,234,4);
			newItem.transform.SetParent(cuerpo.transform);
			
			RectTransform rectTransform = newItem.GetComponent<RectTransform>();

			float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);
 			if(GameCenter.InstanceRef.controladoraJuego.Devolver_Tamanyo_Cadena(contenedorTexto.text, 16) > containerRectTransform.rect.width)
				j++;
			float y = containerRectTransform.rect.height / 2 - height * j;
			rectTransform.offsetMin = new Vector2(x, y);

			x = rectTransform.offsetMin.x + width;
			if(GameCenter.InstanceRef.controladoraJuego.Devolver_Tamanyo_Cadena(contenedorTexto.text, 16) > containerRectTransform.rect.width)
				y = rectTransform.offsetMin.y + (height * 2) ;
			else
				y = rectTransform.offsetMin.y + height;
			rectTransform.offsetMax = new Vector2(x, y);
		
			posicionUltimoElemento = rectTransform.offsetMin.y + rectTransform.rect.size.y;
		}

		GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.rectPanelPreguntas.offsetMin = new Vector2(GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.rectPanelPreguntas.offsetMin.x, posicionUltimoElemento);
		GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.y = GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.rectPanelPreguntas.localPosition.y;

		foreach (Button boton in cuerpo.GetComponentsInChildren<Button>()) 
		{
			boton.transform.localPosition = new Vector2(cuerpo.transform.localPosition.x, boton.transform.localPosition.y + (GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.y * -1));
		}
	}

	private GameObject Buscar_Contenedor(string nombreContenedor)
	{
		return gameObject.GetComponentsInChildren<Transform> ().ToList().Find (x => x.gameObject.name.Equals(nombreContenedor)).gameObject;
	}
}

