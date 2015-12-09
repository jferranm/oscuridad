using UnityEngine;
using UnityEngine.SceneManagement;
using Oscuridad.Interfaces;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;
using System.IO;

namespace Oscuridad.Estados
{
	public class MenuPrincipal: IEscenario
	{
		private Texture2D texturaAuxiliar;
		private Texture2D botonOpciones;
		private Texture2D botonSalir;
		private Texture2D botonContinuarComenzar;
		private bool continuarPartida;

		public MenuPrincipal(ControladoraEscenas managerRef)
		{
			if(!SceneManager.GetActiveScene().name.Contains(Escenas.MenuPrincipal.ToString()))
			{
				SceneManager.LoadScene(Escenas.MenuPrincipal.ToString());
			}

			InicializarDatos ();
		}

		public void InicializarDatos()
		{
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