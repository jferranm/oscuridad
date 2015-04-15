using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using Oscuridad.Clases;

public class OpcionesCanvasMenuPrincipal : MonoBehaviour 
{
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
					Application.Quit();
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

		foreach (Transform objetoHijo in escena0.transform) 
		{
			objetoHijo.GetChild (0).GetComponent<Text> ().text = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.DevolverTexto(objetoHijo.name);
			if (objetoHijo.name.Equals ("btComenzar")) 
			{
				if (File.Exists (archivoJugador)) 
					objetoHijo.GetChild (0).GetComponent<Text> ().text = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.DevolverTexto("btContinuar");
			}
		}

		foreach (Transform objetoHijo in escena2.transform) 
		{
			objetoHijo.GetComponent<Text> ().text = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.DevolverTexto(objetoHijo.name);
		}
	}
}
