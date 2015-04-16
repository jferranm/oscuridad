using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using Oscuridad.Clases;

public class OpcionesCanvasMenuPrincipal : MonoBehaviour 
{
	public Text botonComenzar;
	public Text botonOpciones;
	public Text botonSalir;
	public Text botonPartidaNueva;
	public Text botonMusica;
	public Text botonSonido;
	public Text botonIdioma;

	public GameObject escena0;
	public GameObject escena1;
	public GameObject escena2;

	public Toggle toggleMusica;
	public Slider sliderMusica;
	public Toggle toggleSonido;
	public Slider sliderSonido;

	private string archivoJugador;
	
	void Update()
	{
		if (this.gameObject.activeSelf) 
		{
			if (Input.GetKeyDown (KeyCode.Escape)) 
			{ 
				if (escena0.activeSelf) 
				{
					GameCenter.InstanceRef.Salir();
					return;
				}

				if (escena1.activeSelf) 
				{
					escena1.SetActive(false);
					escena0.SetActive(true);
					escena2.SetActive(false);

					return;
				}

				if (escena2.activeSelf) 
				{
					escena2.SetActive(false);
					escena0.SetActive(true);
					escena1.SetActive(false);

					return;
				}
			}
		}
	}

	public void TraduccionMenu()
	{
		archivoJugador = Path.Combine (Application.persistentDataPath, "Jugador.xml");

		botonComenzar.text = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.DevolverTexto("btComenzar");
		if (File.Exists (archivoJugador)) 
			botonComenzar.text = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.DevolverTexto("btContinuar");
		botonOpciones.text = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.DevolverTexto("btOpciones");
		botonSalir.text = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.DevolverTexto("btSalir");
		botonPartidaNueva.text = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.DevolverTexto("btPartidaNueva");
		botonMusica.text = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.DevolverTexto("toggleMusica");
		botonSonido.text = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.DevolverTexto("toggleSonido");
		botonIdioma.text = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.DevolverTexto("btIdioma");
	}
}
