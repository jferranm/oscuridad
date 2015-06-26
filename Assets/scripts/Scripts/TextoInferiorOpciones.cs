using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Oscuridad.Enumeraciones;

public class TextoInferiorOpciones : MonoBehaviour 
{
	public Text textoVentana;
	public RectTransform rectCajaTexto;
	public RectTransform scrollRectTransform;
	public float y;

	void Start()
	{
		if (GameCenter.InstanceRef != null) 
			GameCenter.InstanceRef.controladoraGUI.textoInferior = textoVentana;

		y = rectCajaTexto.localPosition.y;
	}

	void OnEnable()
	{
		if (GameCenter.InstanceRef != null) 
			Reiniciar_Texto();
	}

	void OnDisable()
	{
		scrollRectTransform.GetComponent<ScrollRect>().vertical = true;
		Posicion_Inicial_Caja ();
	}
	
	private void JugadorEnEspera()
	{
		textoVentana.text = GameCenter.InstanceRef.controladoraJuego.camaraActiva.Descripcion;
	}
	
	private void JugadorEnZoomEspera()
	{
		if (GameCenter.InstanceRef.controladoraJuego.objetoPulsado == null) 
			textoVentana.text = "";
		else 
			textoVentana.text = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.Inspeccionando + " \"" + GameCenter.InstanceRef.controladoraJuego.objetoPulsado.DescripcionNombre + "\"";
	}

	void Update()
	{
		if (textoVentana.preferredHeight > scrollRectTransform.rect.height) 
		{
			scrollRectTransform.GetComponent<ScrollRect>().vertical = true;
			
			if (rectCajaTexto.localPosition.y < y) 
				Posicion_Inicial_Caja();
			
			if (rectCajaTexto.localPosition.y > rectCajaTexto.rect.height) 
				rectCajaTexto.localPosition = new Vector2 (0, rectCajaTexto.rect.height);
		} 
		else 
		{
			scrollRectTransform.GetComponent<ScrollRect>().vertical = false;
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
		rectCajaTexto.localPosition = new Vector2 (0, y);
	}
}
