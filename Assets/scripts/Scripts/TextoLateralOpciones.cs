using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;

public class TextoLateralOpciones : MonoBehaviour 
{
	public Text textoCuerpo;
	public RectTransform rectCajaTexto;
	public RectTransform scrollRectTransform;
	public float y;
	public float velocidadDeslizamiento;

	public float valorFreno = 0;

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
			{
				textoCuerpo.text = GameCenter.InstanceRef.controladoraJuego.objetoPulsado.MostrarDescripcionBasica();
				ObjetoTiradaBase tiradaBasica = GameCenter.InstanceRef.controladoraJuego.objetoPulsado.BuscarTirada(Habilidades.Ninguna);
				if(tiradaBasica.Accion)
				{
					foreach(Localizaciones localizacion in tiradaBasica.LocalizacionAccion)
					{
						if(!GameCenter.InstanceRef.controladoraJuego.jugadorActual.LocalizacionesDescubiertas.Contains(localizacion))
						{
							GameCenter.InstanceRef.controladoraGUI.Insertar_Ventana_Inferior_Texto(localizacion, Color.yellow);
							GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddLocalizacionDescubierta(localizacion);
						}
					}
					
					foreach(Acciones accion in tiradaBasica.AccionesAccion)
					{
						if(!GameCenter.InstanceRef.controladoraJuego.jugadorActual.AccionRealizada(accion))
							GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddAccionRealizada(accion);
					}
				}

			}
			else
				textoCuerpo.text = "";

			deslizar = false;
		}
	}

	void OnDisable()
	{
		scrollRectTransform.GetComponent<ScrollRect>().vertical = true;
		Posicion_Inicial_Caja ();
		valorFreno = 0;
	}

	void Update()
	{
		if (deslizar) 
		{
			if(rectCajaTexto.localPosition.y > valorFreno)
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

	public void Deslizar_Texto(float anteriorSizeCajaTexto)
	{
		deslizar = true;
		valorFreno = y + anteriorSizeCajaTexto;
	}
}