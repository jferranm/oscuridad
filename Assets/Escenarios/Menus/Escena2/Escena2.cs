using UnityEngine;
using UnityEngine.SceneManagement;
using Oscuridad.Interfaces;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;
using System.IO;

namespace Oscuridad.Estados
{
	public class Escena2: IEscenario
	{
		public Escena2(ControladoraEscenas managerRef)
		{
			if(SceneManager.GetActiveScene().name != Escenas.Escena2.ToString())
			{
				SceneManager.LoadScene(Escenas.Escena2.ToString());
			}

			InicializarDatos ();
		}
				
		public void InicializarDatos()
		{
			GameCenter.InstanceRef.controladoraJugador.EstadoJugador = EstadosJugador.enMenus;
		}
		
		public void EstadoUpdate()
		{
		}
		
		public void Mostrar()
		{
		}

		public void NivelCargado()
		{
		}
	}
}
