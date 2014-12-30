using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextoLateralOpciones : MonoBehaviour 
{
	public Text textoCuerpo;
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
			GameCenter.InstanceRef.controladoraGUI.textoLateral = textoCuerpo;

			if(GameCenter.InstanceRef.controladoraJuego.objetoPulsado != null)
				textoCuerpo.text = GameCenter.InstanceRef.controladoraJuego.objetoPulsado.MostrarDescripcionBasica();
			else
				textoCuerpo.text = "";
		}
	}

	void OnDisable()
	{
		rectCajaTexto.localPosition = new Vector2 (0, y);
		final = false;
		scrollRectTransform.GetComponent<ScrollRect>().vertical = true;
	}

	void Update()
	{
		if (textoCuerpo.preferredHeight > scrollRectTransform.rect.height) 
		{
			scrollRectTransform.GetComponent<ScrollRect>().vertical = true;

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
}