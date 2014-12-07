using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class OpcionesCanvasMenuPrincipal : MonoBehaviour 
{
	void OnEnable()
	{
		string archivoJugador = Path.Combine (Application.persistentDataPath, "Jugador.xml");

		foreach (Transform objetoHijo in this.transform) 
		{
			if(objetoHijo.name == "BotonComenzar")
			{
				if(File.Exists(archivoJugador))
				{
					objetoHijo.GetChild(0).GetComponent<Text>().text = "CONTINUAR";
				}
			}
		}
	}
}
