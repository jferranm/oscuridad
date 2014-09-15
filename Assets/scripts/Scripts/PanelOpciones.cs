using UnityEngine;
using System.Collections;
using Oscuridad.Estados;
using Oscuridad.Enumeraciones;

public class PanelOpciones : MonoBehaviour 
{
	[HideInInspector]
	public estadoUsuario estadosUsuario;

	void Awake()
	{
		DontDestroyOnLoad(this);
		this.gameObject.SetActive(false);
	}

	void OnEnable()
	{
		estadosUsuario = estadoUsuario.enJuego;
	}

	void Update()
	{
		switch (estadosUsuario) 
		{
			case estadoUsuario.enJuego:	
				UsuarioEnJuego();
				break;
				
			case estadoUsuario.enHojaDePersonaje:
				UsuarioEnHojaPersonaje();
				break;
				
			case estadoUsuario.enInventario:
				UsuarioEnInventario();
				break;
				
			case estadoUsuario.enMapa:
				UsuarioEnMapa();
				break;
				
			case estadoUsuario.enOpciones:
				UsuarioEnOpciones();
				break;
		}
	}

	void OnGUI () 
	{
	}

	private void UsuarioEnJuego()
	{

	}

	private void UsuarioEnHojaPersonaje()
	{
		//TODO: Mostrar Hoja de Personaje
	}

	private void UsuarioEnInventario()
	{
		//TODO: Mostrar Inventario
	}

	private void UsuarioEnMapa()
	{
		//TODO: Mostrar Mapa
	}

	private void UsuarioEnOpciones()
	{
		//TODO: Mostrar Menu Opciones
	}
}
