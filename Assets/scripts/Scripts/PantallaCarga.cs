using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Oscuridad.Enumeraciones;

public class PantallaCarga : MonoBehaviour 
{
	private float fadeSpeed = 5f;
	private Image imagenTextura;
	private Text textoCargando;
	private Color colorInicial = new Color();
	private Color colorInicialTexto = new Color();

	public bool comenzarFade = false;

	void Awake()
	{
		imagenTextura = this.gameObject.GetComponentInChildren<Image> ();
		textoCargando = this.gameObject.GetComponentInChildren<Text> ();
		colorInicial = imagenTextura.color;
		colorInicialTexto = textoCargando.color;
	}
	
	void OnEnable()
	{
		imagenTextura.color = colorInicial;
		textoCargando.color = colorInicialTexto;
		if(GameCenter.InstanceRef != null)
			textoCargando.text = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.DevolverTexto ("Cargando");
		comenzarFade = false;
	}

	void Update()
	{
		if(comenzarFade)
			Hacer_Fade();
	}

	private void Hacer_Fade()
	{
		//Hacer Fade del la imagen a imagen vacia
		FadeToClear();
		
		//Si la imagen esta casi clara
		if(imagenTextura.color.a <= 0.05f)
		{
			//hacemos que el color sea claro y desactivamos el objeto
			imagenTextura.color = Color.clear;
			comenzarFade = false;

			this.gameObject.SetActive(false);
			GameCenter.InstanceRef.controladoraJugador.EstadoJugador = EstadosJugador.enEspera;
		}
	}

	void FadeToClear ()
	{
		imagenTextura.color = Color.Lerp(imagenTextura.color, Color.clear, fadeSpeed * Time.deltaTime);
		textoCargando.color = Color.Lerp (textoCargando.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
}
