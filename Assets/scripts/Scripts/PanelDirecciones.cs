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
		Input.touches.Initialize();

		//Poner Texturas
		Inicializar_Texturas();

		//Obtenemos la activacion de las nuevas direcciones
		Reorganizar_Direcciones ();
	}

	private void Reorganizar_Direcciones()
	{
		//Accedemos al xml para saber que direcciones activar
		ControlXMLGlobal nuevoControlGlobal = new ControlXMLGlobal ();
		Hashtable listaSalidas = new Hashtable (nuevoControlGlobal.Devolver_Salidas(Application.loadedLevelName));

		//Recorremos la lista k nos devuelve y activamos direccciones
		foreach (Salidas nuevaSalida in listaSalidas.Values) 
		{
			switch(nuevaSalida.orientacionSalida)
			{
			case "izquierda":
				CambiarTextura("Izq", GameCenter.InstanceRef.controladoraGUI.texturaFlechasIzqOK, GameCenter.InstanceRef.controladoraEscenas.Devolver_Enum_Escena(nuevaSalida.escenaSalida));
				break;
			case "derecha":	
				CambiarTextura("Der", GameCenter.InstanceRef.controladoraGUI.texturaFlechasDerOK, GameCenter.InstanceRef.controladoraEscenas.Devolver_Enum_Escena(nuevaSalida.escenaSalida));
				break;
			case "arriba":
				CambiarTextura("Arr", GameCenter.InstanceRef.controladoraGUI.texturaFlechasArrOK, GameCenter.InstanceRef.controladoraEscenas.Devolver_Enum_Escena(nuevaSalida.escenaSalida));
				break;
			case "abajo":	
				CambiarTextura("Abj", GameCenter.InstanceRef.controladoraGUI.texturaFlechasAbjOK, GameCenter.InstanceRef.controladoraEscenas.Devolver_Enum_Escena(nuevaSalida.escenaSalida));
				break;
			}
		}
	}

	private void CambiarTextura(string direccion, Texture2D textura, EstadoJuego nuevoEstado)
	{
		foreach (Transform objetoHijo in this.transform) 
		{
			if(objetoHijo.name.Contains (direccion))
			{
				objetoHijo.guiTexture.texture = textura;
				objetoHijo.GetComponent<BotonDireccion>().salida = true;
				objetoHijo.GetComponent<BotonDireccion>().nuevaSalida = nuevoEstado;
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
