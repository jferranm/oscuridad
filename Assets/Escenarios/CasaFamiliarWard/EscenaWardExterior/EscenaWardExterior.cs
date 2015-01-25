using UnityEngine;
using System.Collections;
using System.IO;
using Oscuridad.Interfaces;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;
using System.Collections.Generic;

namespace Oscuridad.Estados
{
	public class EscenaWardExterior: IEscenario
	{
		public EscenaWardExterior(ControladoraEscenas managerRef)
		{
			if(Application.loadedLevelName != Escenas.EscenaWardExterior.ToString())
			{
				Application.LoadLevel(Escenas.EscenaWardExterior.ToString());
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
			GameCenter.InstanceRef.controladoraJuego.escenaActual = GameCenter.InstanceRef.controladoraJuego.Cargar_Escena(Escenas.EscenaWardExterior);
			
			//Inicializamos la escena
			GameCenter.InstanceRef.controladoraGUI.Activar_Fade();
			GameCenter.InstanceRef.Inicializar_Escenario ();
		}
	}
}
