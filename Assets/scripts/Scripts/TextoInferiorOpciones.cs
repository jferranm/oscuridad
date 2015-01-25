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
	public float yTotal;
	
	private bool final;

	void Awake()
	{
		y = rectCajaTexto.localPosition.y;
		yTotal = 1;
	}

	void OnEnable()
	{
		if (GameCenter.InstanceRef != null) 
		{
			GameCenter.InstanceRef.controladoraGUI.textoInferior = textoVentana;
			
			Reiniciar_Texto();
		}
	}

	void OnDisable()
	{
		rectCajaTexto.localPosition = new Vector2 (0, y);
		final = false;
		scrollRectTransform.GetComponent<ScrollRect>().vertical = true;
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
			textoVentana.text = "Inspeccionando \"" + GameCenter.InstanceRef.controladoraJuego.objetoPulsado.DescripcionNombre + "\"";
	}

	void Update()
	{
		if (textoVentana.preferredHeight > scrollRectTransform.rect.height) 
		{
			if (!final) 
			{
				if (rectCajaTexto.offsetMax.y > rectCajaTexto.sizeDelta.magnitude) 
				{
					yTotal = rectCajaTexto.localPosition.y;
					final = true;
				}
			}

			if (rectCajaTexto.localPosition.y < y) 
				rectCajaTexto.localPosition = new Vector2 (0, y);

			if (final) 
			{
				if (rectCajaTexto.localPosition.y > yTotal) 
					rectCajaTexto.localPosition = new Vector2 (0, yTotal);
			}
		} 
		else 
		{
			scrollRectTransform.GetComponent<ScrollRect>().vertical = false;
		}
	}

	public void Reiniciar_Texto()
	{
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
}
