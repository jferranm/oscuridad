using UnityEngine;
using System.Collections;
using System.IO;
using Oscuridad.Interfaces;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;
using System.Collections.Generic;

namespace Oscuridad.Estados
{
	public class Escena11: IEscenario
	{
		public Escena11(ControladoraEscenas managerRef)
		{
			if(Application.loadedLevelName != Escenas.Escena11.ToString())
			{
				Application.LoadLevel(Escenas.Escena11.ToString());
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
			//Cargamos Datos Serializados
			GameCenter.InstanceRef.controladoraJuego.escenaActual = GameCenter.InstanceRef.controladoraJuego.Cargar_Escena(Escenas.Escena11);

			//Inicializamos la escena
			GameCenter.InstanceRef.controladoraGUI.Activar_Fade ();
			GameCenter.InstanceRef.Inicializar_Escenario ();
		}
	}
}
