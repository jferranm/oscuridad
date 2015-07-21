using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Oscuridad.Enumeraciones;

public class PanelPreguntasOpciones : MonoBehaviour 
{
	public RectTransform scrollRectTransform;
	public RectTransform rectPanelPreguntas;
	public float y;
	
	void Start()
	{
		if (GameCenter.InstanceRef != null) 
			GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones = gameObject.GetComponent<PanelPreguntasOpciones>();

		y = rectPanelPreguntas.localPosition.y;
		GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.gameObject.SetActive (false);
	}
	
	void OnEnable()
	{
		scrollRectTransform.GetComponent<ScrollRect> ().content = rectPanelPreguntas;
	}
	
	void OnDisable()
	{
		scrollRectTransform.GetComponent<ScrollRect>().vertical = true;
		Posicion_Inicial_Caja ();
	}
	
	void Update()
	{
		if (GameCenter.InstanceRef.controladoraJugador.EstadoJugador.Equals (EstadosJugador.enEspera) || GameCenter.InstanceRef.controladoraJugador.EstadoJugador.Equals (EstadosJugador.enZoomEspera)) 
		{
			if (GameCenter.InstanceRef.controladoraJuego.objetoPulsado == null) 
			{
				if (rectPanelPreguntas.rect.height > scrollRectTransform.rect.height) 
				{
					scrollRectTransform.GetComponent<ScrollRect> ().vertical = true;
					
					if (rectPanelPreguntas.localPosition.y < y) 
						Posicion_Inicial_Caja ();

					if (rectPanelPreguntas.localPosition.y > (y * -1)) 
						rectPanelPreguntas.localPosition = new Vector2 (0, (y * -1));
				} 
				else 
					scrollRectTransform.GetComponent<ScrollRect> ().vertical = false;
			} 
		}
	}
	
	public void Posicion_Inicial_Caja()
	{
		rectPanelPreguntas.localPosition = new Vector2 (0, y);
	}
}
