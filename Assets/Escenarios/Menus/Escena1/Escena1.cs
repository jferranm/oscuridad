using UnityEngine;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;
using System.IO;

namespace Oscuridad.Estados
{
	public class Escena1: IEscenario
	{
		public Escena1(ControladoraEscenas managerRef)
		{
			if(Application.loadedLevelName != Escenas.Escena1.ToString())
			{
				Application.LoadLevel(Escenas.Escena1.ToString());
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