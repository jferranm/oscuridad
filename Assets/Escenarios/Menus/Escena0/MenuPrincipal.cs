using UnityEngine;
using Oscuridad.Interfaces;
using Oscuridad.Personajes;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;
using System.IO;

namespace Oscuridad.Estados
{
	public class MenuPrincipal: IEscenario
	{
		private ControladoraEscenas manager;
		private Texture2D texturaAuxiliar;
		private Texture2D botonOpciones;
		private Texture2D botonSalir;
		private Texture2D botonContinuarComenzar;
		private bool continuarPartida;

		public MenuPrincipal(ControladoraEscenas managerRef)
		{
			manager = managerRef;

			if(!Application.loadedLevelName.Contains(EstadoJuego.MenuPrincipal.ToString()))
			{
				Application.LoadLevel(EstadoJuego.MenuPrincipal.ToString());
			}

			InicializarDatos ();
		}

		public void InicializarDatos()
		{
			GameCenter.InstanceRef.controladoraJugador.Cambiar_Estado (estadoJugador.enMenus);
			GameCenter.InstanceRef.controladoraJugador.estadoCambiado = true;
		}
		
		public void EstadoUpdate()
		{

		}
		
		public void Mostrar()
		{
			//Cambiar cursor
			//Cursor.SetCursor(ControladorEstadoGUI.instanceRef.texturaCursorNormal, Vector2.zero, CursorMode.Auto);
		}

		public void NivelCargado()
		{
		}
	}
}