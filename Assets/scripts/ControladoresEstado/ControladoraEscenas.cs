using UnityEngine;
using System.Collections;
using Oscuridad.Interfaces;
using Oscuridad.Personajes;
using Oscuridad.Enumeraciones;
using Oscuridad.Estados;

[System.Serializable]
public class ControladoraEscenas
{
	public ControladoraEscenas()
	{
	}

	private static ControladoraEscenas instanceRef;
	
	public static ControladoraEscenas InstanceRef()
	{
		if (instanceRef == null)
		{
			instanceRef = new ControladoraEscenas();
		}
		
		return instanceRef;
	}

	public IEscenario estadoActivo;

	public void Update()
	{
		if(estadoActivo != null)
		{
			estadoActivo.EstadoUpdate();
		}
	}
	
	public void OnGUI()
	{
		if(estadoActivo != null)
			estadoActivo.Mostrar();
	}
	
	public void OnLevelWasLoaded(int level)
	{
		if (estadoActivo != null)
		{
			estadoActivo.NivelCargado();
		}
	}
	
	private void CambiarEstado(IEscenario nuevoEstado)
	{
		estadoActivo = nuevoEstado;
	}
	
	public void IrMenuPrincipal()
	{
		estadoActivo = new MenuPrincipal(this);
		GameCenter.InstanceRef.EstadoActual = EstadoJuego.MenuPrincipal;
	}
	
	public void IrEscena1()
	{
		estadoActivo = new Escena1(this);
		GameCenter.InstanceRef.EstadoActual = EstadoJuego.Escena1;
	}
	
	public void IrEscena2()
	{
		estadoActivo = new Escena2(this);
		GameCenter.InstanceRef.EstadoActual = EstadoJuego.Escena2;
	}
	
	public void IrEscena3()
	{
		estadoActivo = new Escena3(this);
		GameCenter.InstanceRef.EstadoActual = EstadoJuego.Escena3;
	}
	
	public void IrEscena10()
	{
		estadoActivo = new Escena10(this);
		GameCenter.InstanceRef.EstadoActual = EstadoJuego.Escena10;
	}
	
	public void IrEscena11()
	{
		estadoActivo = new Escena11(this);
		GameCenter.InstanceRef.EstadoActual = EstadoJuego.Escena11;
	}
	
	public void IrEscena12()
	{
		estadoActivo = new Escena12(this);
		GameCenter.InstanceRef.EstadoActual = EstadoJuego.Escena12;
	}
	
	public void IrEscena13()
	{
		estadoActivo = new Escena13(this);
		GameCenter.InstanceRef.EstadoActual = EstadoJuego.Escena13;
	}

	public void IrEscena14()
	{
		estadoActivo = new Escena14(this);
		GameCenter.InstanceRef.EstadoActual = EstadoJuego.Escena14;
	}
	
	public void IrEscena15()
	{
		estadoActivo = new Escena15(this);
		GameCenter.InstanceRef.EstadoActual = EstadoJuego.Escena15;
	}
	
	public void IrEscena16()
	{
		estadoActivo = new Escena16(this);
		GameCenter.InstanceRef.EstadoActual = EstadoJuego.Escena16;
	}

	public void IrEscena17()
	{
		estadoActivo = new Escena17(this);
		GameCenter.InstanceRef.EstadoActual = EstadoJuego.Escena17;
	}
	
	public void IrEscena18()
	{
		estadoActivo = new Escena18(this);
		GameCenter.InstanceRef.EstadoActual = EstadoJuego.Escena18;
	}
	
	public void IrEscena19()
	{
		estadoActivo = new Escena19(this);
		GameCenter.InstanceRef.EstadoActual = EstadoJuego.Escena19;
	}
	
	public void CambiarSceneSegunEnum(EstadoJuego temp)
	{
		switch (temp) 
		{
			case EstadoJuego.MenuPrincipal:
					IrMenuPrincipal ();
					break;

			case EstadoJuego.Escena1:
					IrEscena1 ();
					break;

			case EstadoJuego.Escena2:
					IrEscena2 ();
					break;
		
			case EstadoJuego.Escena3:
					IrEscena3 ();
					break;

			case EstadoJuego.Escena10:
					IrEscena10 ();
					break;
			
			case EstadoJuego.Escena11:
					IrEscena11 ();
					break;
			
			case EstadoJuego.Escena12:
					IrEscena12 ();
					break;

			case EstadoJuego.Escena13:
					IrEscena13 ();
					break;
			
			case EstadoJuego.Escena14:
					IrEscena14 ();
					break;
			
			case EstadoJuego.Escena15:
					IrEscena15 ();
					break;

			case EstadoJuego.Escena16:
					IrEscena16 ();
					break;
			
			case EstadoJuego.Escena17:
					IrEscena17 ();
					break;
			
			case EstadoJuego.Escena18:
					IrEscena18 ();
					break;

			case EstadoJuego.Escena19:
					IrEscena19 ();
					break;
		}

	}

	public EstadoJuego Devolver_Enum_Escena (string escenaSeleccionada)
	{
		EstadoJuego nuevoEstadoJuego = EstadoJuego.MenuPrincipal;

		switch(escenaSeleccionada)
		{
			case "MenuPrincipal":
				nuevoEstadoJuego = EstadoJuego.MenuPrincipal;
				break;

			case "Escena1":
				nuevoEstadoJuego = EstadoJuego.Escena1;
				break;
		
			case "Escena2":
				nuevoEstadoJuego = EstadoJuego.Escena2;
				break;

			case "Escena3":
				nuevoEstadoJuego = EstadoJuego.Escena3;
				break;

			case "Escena10":
				nuevoEstadoJuego = EstadoJuego.Escena10;
				break;

			case "Escena11":
				nuevoEstadoJuego = EstadoJuego.Escena11;
				break;

			case "Escena12":
				nuevoEstadoJuego = EstadoJuego.Escena12;
				break;

			case "Escena13":
				nuevoEstadoJuego = EstadoJuego.Escena13;
				break;

			case "Escena14":
				nuevoEstadoJuego = EstadoJuego.Escena14;
				break;

			case "Escena15":
				nuevoEstadoJuego = EstadoJuego.Escena15;
				break;

			case "Escena16":
				nuevoEstadoJuego = EstadoJuego.Escena16;
				break;

			case "Escena17":
				nuevoEstadoJuego = EstadoJuego.Escena17;
				break;
				
			case "Escena18":
				nuevoEstadoJuego = EstadoJuego.Escena18;
				break;
				
			case "Escena19":
				nuevoEstadoJuego = EstadoJuego.Escena19;
				break;
		}

		return nuevoEstadoJuego;
	}
}
