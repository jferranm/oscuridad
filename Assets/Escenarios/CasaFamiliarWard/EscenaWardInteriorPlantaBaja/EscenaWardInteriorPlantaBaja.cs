using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using Oscuridad.Interfaces;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;
using System.Collections.Generic;

namespace Oscuridad.Estados
{
	public class EscenaWardInteriorPlantaBaja: IEscenario
	{
		public EscenaWardInteriorPlantaBaja(ControladoraEscenas managerRef)
		{
			if(SceneManager.GetActiveScene().name != Escenas.EscenaWardInteriorPlantaBaja.ToString())
			{
				SceneManager.LoadScene(Escenas.EscenaWardInteriorPlantaBaja.ToString());
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
			GameCenter.InstanceRef.controladoraJuego.escenaActual = GameCenter.InstanceRef.controladoraJuego.Cargar_Escena(Escenas.EscenaWardInteriorPlantaBaja);
			
			//Inicializamos la escena
			GameCenter.InstanceRef.controladoraGUI.Activar_Fade();
			GameCenter.InstanceRef.Inicializar_Escenario ();
		}
	}
}

