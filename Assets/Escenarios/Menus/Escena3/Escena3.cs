using UnityEngine;
using UnityEngine.SceneManagement;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;
using System.IO;

namespace Oscuridad.Estados
{
	public class Escena3: IEscenario
	{
		public Escena3(ControladoraEscenas managerRef)
		{
			if(SceneManager.GetActiveScene().name != Escenas.Escena3.ToString())
			{
				SceneManager.LoadScene(Escenas.Escena3.ToString());
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
