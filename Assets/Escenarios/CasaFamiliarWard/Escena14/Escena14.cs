using UnityEngine;
using System.Collections;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;
using System.Collections.Generic;

namespace Oscuridad.Estados
{
	public class Escena14: IEscenario
	{
		private ControladoraEscenas manager;

		public Escena14(ControladoraEscenas managerRef)
		{
			manager = managerRef;

			if(Application.loadedLevelName != EstadoJuego.Escena14.ToString())
			{
				Application.LoadLevel(EstadoJuego.Escena14.ToString());
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
