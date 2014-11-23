using UnityEngine;
using System.Collections;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;
using Oscuridad.Estados;

public class PanelDirecciones : MonoBehaviour 
{
	private bool opcionArriba;
	private string escenaArriba;
	private bool opcionAbajo;
	private string escenaAbajo;
	private bool opcionIzquierda;
	private string escenaIzquierda;
	private bool opcionDerecha;
	private string escenaDerecha;

	private estadoDirecciones estadosDirecciones;

	void Awake()
	{
		DontDestroyOnLoad(this);
		this.gameObject.SetActive(false);
	}

	void OnEnable()
	{
		//Input.touches.Initialize();

		//Poner Texturas
		Inicializar_Texturas();

		//Obtenemos la activacion de las nuevas direcciones
		Reorganizar_Direcciones ();
	}

	private void Reorganizar_Direcciones()
	{
		if (GameCenter.InstanceRef.controladoraJuego.escenaActual.EscenaNorte != Escenas.ninguna) 
		{
			CambiarTextura("Arr", GameCenter.InstanceRef.controladoraGUI.texturaFlechasArrOK, GameCenter.InstanceRef.controladoraJuego.escenaActual.EscenaNorte);
		}

		if (GameCenter.InstanceRef.controladoraJuego.escenaActual.EscenaSur != Escenas.ninguna) 
		{
			CambiarTextura("Abj", GameCenter.InstanceRef.controladoraGUI.texturaFlechasAbjOK, GameCenter.InstanceRef.controladoraJuego.escenaActual.EscenaSur);
		}

		if (GameCenter.InstanceRef.controladoraJuego.escenaActual.EscenaEste != Escenas.ninguna) 
		{
			CambiarTextura("Izq", GameCenter.InstanceRef.controladoraGUI.texturaFlechasIzqOK, GameCenter.InstanceRef.controladoraJuego.escenaActual.EscenaEste);
		}

		if (GameCenter.InstanceRef.controladoraJuego.escenaActual.EscenaOeste != Escenas.ninguna) 
		{
			CambiarTextura("Der", GameCenter.InstanceRef.controladoraGUI.texturaFlechasDerOK, GameCenter.InstanceRef.controladoraJuego.escenaActual.EscenaOeste);
		}
	}

	private void CambiarTextura(string direccion, Texture2D textura, Escenas nuevaEscena)
	{
		foreach (Transform objetoHijo in this.transform) 
		{
			if(objetoHijo.name.Contains (direccion))
			{
				objetoHijo.guiTexture.texture = textura;
				objetoHijo.GetComponent<BotonDireccion>().salida = true;
				objetoHijo.GetComponent<BotonDireccion>().nuevaSalida = nuevaEscena;
			}
		}
	}

	private void Inicializar_Texturas()
	{
		foreach (Transform objetoHijo in this.transform) 
		{
			if(objetoHijo.name.Contains ("Izq"))
			{
				objetoHijo.guiTexture.texture = GameCenter.InstanceRef.controladoraGUI.texturaFlechasIzqNo;
				objetoHijo.GetComponent<BotonDireccion>().salida = false;
			}

			if(objetoHijo.name.Contains ("Der"))
			{
				objetoHijo.guiTexture.texture = GameCenter.InstanceRef.controladoraGUI.texturaFlechasDerNo;
				objetoHijo.GetComponent<BotonDireccion>().salida = false;
			}

			if(objetoHijo.name.Contains ("Arr"))
			{
				objetoHijo.guiTexture.texture = GameCenter.InstanceRef.controladoraGUI.texturaFlechasArrNo;
				objetoHijo.GetComponent<BotonDireccion>().salida = false;
			}

			if(objetoHijo.name.Contains ("Abj"))
			{
				objetoHijo.guiTexture.texture = GameCenter.InstanceRef.controladoraGUI.texturaFlechasAbjNo;
				objetoHijo.GetComponent<BotonDireccion>().salida = false;
			}
		}
	}
}
