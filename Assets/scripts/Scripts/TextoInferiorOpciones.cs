using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Oscuridad.Enumeraciones;

public class TextoInferiorOpciones : MonoBehaviour 
{
	public Text textoVentana;
	public RectTransform rectCajaTexto;
	public RectTransform scrollRectTransform;
	public RectTransform rectPanelPreguntas;
	public RectTransform rectContenedor;
	public float y;

	void Start()
	{
		if (GameCenter.InstanceRef != null) 
		{
			GameCenter.InstanceRef.controladoraGUI.textoInferior = textoVentana;
			GameCenter.InstanceRef.controladoraGUI.textoInferiorOpciones = gameObject.GetComponent<TextoInferiorOpciones>();
		}
		rectContenedor = rectCajaTexto;
		y = rectContenedor.localPosition.y;
	}

	void OnEnable()
	{
		if (GameCenter.InstanceRef != null) 
			Reiniciar_Texto ();
	}

	void OnDisable()
	{
		scrollRectTransform.GetComponent<ScrollRect>().vertical = true;
		Posicion_Inicial_Caja ();
	}
	
	private void JugadorEnEspera()
	{
		textoVentana.text = GameCenter.InstanceRef.controladoraJuego.camaraActiva.Descripcion;
		rectCajaTexto.gameObject.SetActive (true);
		rectPanelPreguntas.gameObject.SetActive (false);
		rectContenedor = rectCajaTexto;
	}
	
	private void JugadorEnZoomEspera()
	{
		if (GameCenter.InstanceRef.controladoraJuego.objetoPulsado == null) 
		{
			textoVentana.text = "";
			rectCajaTexto.gameObject.SetActive(false);
			rectPanelPreguntas.gameObject.SetActive(true);
			rectContenedor = rectPanelPreguntas;
		} 
		else
		{
			textoVentana.text = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.Inspeccionando + " \"" + GameCenter.InstanceRef.controladoraJuego.objetoPulsado.DescripcionNombre + "\"";
			rectCajaTexto.gameObject.SetActive(true);
			rectPanelPreguntas.gameObject.SetActive(false);
			rectContenedor = rectCajaTexto;
		}
	}

	void Update()
	{
		Debug.Log ("y: " + y);
		if (GameCenter.InstanceRef.controladoraJuego.objetoPulsado == null) 
		{
			Debug.Log ("rectCajaTexto.rect.height: " + rectCajaTexto.rect.height.ToString() + " - scrollRectTransform.rect.height: " + scrollRectTransform.rect.height.ToString ());    
			if (rectContenedor.rect.height  > scrollRectTransform.rect.height) 
			{
				scrollRectTransform.GetComponent<ScrollRect> ().vertical = true;
				
				if (rectContenedor.localPosition.y < y) 
					Posicion_Inicial_Caja ();
				
				if (rectContenedor.localPosition.y > rectContenedor.rect.height) 
					rectContenedor.localPosition = new Vector2 (0, rectContenedor.rect.height);
			} 
			else 
				scrollRectTransform.GetComponent<ScrollRect> ().vertical = false;
		} 
		else 
		{
			if (textoVentana.preferredHeight > scrollRectTransform.rect.height) 
			{
				scrollRectTransform.GetComponent<ScrollRect> ().vertical = true;

				if (rectContenedor.localPosition.y < y) 
					Posicion_Inicial_Caja ();

				if (rectContenedor.localPosition.y > rectContenedor.rect.height) 
					rectContenedor.localPosition = new Vector2 (0, rectContenedor.rect.height);
			} 
			else 
				scrollRectTransform.GetComponent<ScrollRect> ().vertical = false;
		}
	}

	public void Reiniciar_Texto()
	{
		Posicion_Inicial_Caja ();
		switch (GameCenter.InstanceRef.controladoraJugador.EstadoJugador) 
		{
			case EstadosJugador.enEspera:
				JugadorEnEspera ();
				break;
			
			case EstadosJugador.enZoomEspera:
				JugadorEnZoomEspera ();
				break;
		}
	}

	public void Posicion_Inicial_Caja()
	{
		rectContenedor.localPosition = new Vector2 (0, y);
	}
}
