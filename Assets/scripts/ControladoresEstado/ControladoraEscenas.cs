using UnityEngine;
using System.Collections;
using Oscuridad.Interfaces;
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
	}
	
	public void IrEscena1()
	{
		estadoActivo = new Escena1(this);
	}
	
	public void IrEscena2()
	{
		estadoActivo = new Escena2(this);
	}
	
	public void IrEscena3()
	{
		estadoActivo = new Escena3(this);
	}
	
	public void IrEscena10()
	{
		estadoActivo = new Escena10(this);
	}
	
	public void IrEscena11()
	{
		estadoActivo = new Escena11(this);
	}
	
	public void IrEscena12()
	{
		estadoActivo = new Escena12(this);
	}
	
	public void IrEscena13()
	{
		estadoActivo = new Escena13(this);
	}

	public void IrEscena14()
	{
		estadoActivo = new Escena14(this);
	}
	
	public void IrEscena15()
	{
		estadoActivo = new Escena15(this);
	}
	
	public void IrEscena16()
	{
		estadoActivo = new Escena16(this);
	}

	public void IrEscena17()
	{
		estadoActivo = new Escena17(this);
	}
	
	public void IrEscena18()
	{
		estadoActivo = new Escena18(this);
	}
	
	public void IrEscena19()
	{
		estadoActivo = new Escena19(this);
	}
	
	public void CambiarSceneSegunEnum(Escenas temp)
	{
		switch (temp) 
		{
			case Escenas.MenuPrincipal:
					IrMenuPrincipal ();
					break;

			case Escenas.Escena1:
					IrEscena1 ();
					break;

			case Escenas.Escena2:
					IrEscena2 ();
					break;
		
			case Escenas.Escena3:
					IrEscena3 ();
					break;

			case Escenas.Escena10:
					IrEscena10 ();
					break;
			
			case Escenas.Escena11:
					IrEscena11 ();
					break;
			
			case Escenas.Escena12:
					IrEscena12 ();
					break;

			case Escenas.Escena13:
					IrEscena13 ();
					break;
			
			case Escenas.Escena14:
					IrEscena14 ();
					break;
			
			case Escenas.Escena15:
					IrEscena15 ();
					break;

			case Escenas.Escena16:
					IrEscena16 ();
					break;
			
			case Escenas.Escena17:
					IrEscena17 ();
					break;
			
			case Escenas.Escena18:
					IrEscena18 ();
					break;

			case Escenas.Escena19:
					IrEscena19 ();
					break;
		}

	}
}
