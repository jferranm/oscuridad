using UnityEngine;
using UnityEngine.SceneManagement;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;
using System.IO;

namespace Oscuridad.Estados
{
	public class Escena1: IEscenario
	{
		public Escena1(ControladoraEscenas managerRef)
		{
			if(SceneManager.GetActiveScene().name != Escenas.Escena1.ToString())
			{
				SceneManager.LoadScene(Escenas.Escena1.ToString());
			}
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