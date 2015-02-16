using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class OpcionesCanvasMenuPrincipal : MonoBehaviour 
{
	public GameObject escena0;
	public GameObject escena1;
	public GameObject escena2;

	void OnEnable()
	{
		if (escena0.activeSelf) 
		{
			string archivoJugador = Path.Combine (Application.persistentDataPath, "Jugador.xml");

			foreach (Transform objetoHijo in escena0.transform) 
			{
				if (objetoHijo.name.Equals ("botonComenzar")) 
				{
					if (File.Exists (archivoJugador)) 
						objetoHijo.GetChild (0).GetComponent<Text> ().text = "CONTINUAR";

					return;
				}
			}

			return;
		}

		if (escena1.activeSelf) 
		{
		}

		if (escena2.activeSelf) 
		{
		}
	}

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
}
