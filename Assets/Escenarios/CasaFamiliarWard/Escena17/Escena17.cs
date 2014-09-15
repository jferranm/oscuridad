using UnityEngine;
using System.Collections;
using Oscuridad.Interfaces;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;
using System.Collections.Generic;

namespace Oscuridad.Estados
{
	public class Escena17: IEscenario
	{
		private ControladoraEscenas manager;

		public Escena17(ControladoraEscenas managerRef)
		{
			manager = managerRef;

			if(Application.loadedLevelName != EstadoJuego.Escena17.ToString())
			{
				Application.LoadLevel(EstadoJuego.Escena17.ToString());
			}

			InicializarDatos ();
		}

		public void InicializarDatos()
		{
			GameCenter.InstanceRef.controladoraGUI.Activar_Cargando();
		}
		
		public void EstadoUpdate()
		{
		}
		
		public void Mostrar()
		{
		}

		public void NivelCargado()
		{
			//Inicializamos la escena
			GameCenter.InstanceRef.controladoraGUI.Devolver_Pantalla_Carga().comenzarFade = true;
			GameCenter.InstanceRef.Inicializar_Escenario ();
		}
	}
}
