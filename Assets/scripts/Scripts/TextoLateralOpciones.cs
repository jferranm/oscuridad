using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextoLateralOpciones : MonoBehaviour 
{
	public Text textoCuerpo;
	public RectTransform rectCajaTexto;
	public RectTransform scrollRectTransform;
	public float y;
	public float velocidadDeslizamiento;

	private bool deslizar = false;

	void Start()
	{
		if (GameCenter.InstanceRef != null) 
		{
			GameCenter.InstanceRef.controladoraGUI.textoLateral = textoCuerpo;
			GameCenter.InstanceRef.controladoraGUI.textoLateralOpciones = gameObject.GetComponent<TextoLateralOpciones>();
		}

		y = rectCajaTexto.localPosition.y;
	}


	void OnEnable()
	{
		if (GameCenter.InstanceRef != null) 
		{
			if(GameCenter.InstanceRef.controladoraJuego.objetoPulsado != null)
				textoCuerpo.text = GameCenter.InstanceRef.controladoraJuego.objetoPulsado.MostrarDescripcionBasica();
			else
				textoCuerpo.text = "";
		}
	}

	void OnDisable()
	{
		scrollRectTransform.GetComponent<ScrollRect>().vertical = true;
		Posicion_Inicial_Caja ();
	}

	void Update()
	{
		if (deslizar) 
		{
			if(rectCajaTexto.localPosition.y > rectCajaTexto.rect.height)
				deslizar = false;
			else
				rectCajaTexto.localPosition = new Vector2 (0, rectCajaTexto.localPosition.y + (velocidadDeslizamiento + Time.deltaTime));
		} 
		else 
		{
			if (textoCuerpo.preferredHeight > scrollRectTransform.rect.height) 
			{
				scrollRectTransform.GetComponent<ScrollRect> ().vertical = true;

				if (rectCajaTexto.localPosition.y < y) 
					Posicion_Inicial_Caja ();

				if (rectCajaTexto.localPosition.y > rectCajaTexto.rect.height) 
					rectCajaTexto.localPosition = new Vector2 (0, rectCajaTexto.rect.height);
			} 
			else 
				scrollRectTransform.GetComponent<ScrollRect> ().vertical = false;
		}
	}

	public void Posicion_Inicial_Caja()
	{
		rectCajaTexto.localPosition = new Vector2 (0, y);
	}

	public void Deslizar_Texto()
	{
		deslizar = true;
	}
}