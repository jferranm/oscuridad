using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Oscuridad.Enumeraciones;

public class OpcionesEscena1 : MonoBehaviour 
{
	private Vector3 posicionMarla;
	private Vector3 posicionWarren;
	private Vector3 posicionRobert;

	public RectTransform btMarla;
	public RectTransform btWarren;
	public RectTransform btRobert;
	public GameObject panelTexto;

	void Start()
	{
		posicionMarla = btMarla.localPosition;
		posicionWarren = btWarren.localPosition;
		posicionRobert = btRobert.localPosition;
	}

	public void SeleccionarPersonaje(string nombre)
	{
		switch (nombre) 
		{
			case "btMarlaWibbs":
			{
				if(btMarla.localPosition.Equals(posicionMarla))
				{
					SubirPersonaje(btMarla);
					BajarPersonaje(btWarren, posicionWarren);
					BajarPersonaje(btRobert, posicionRobert);
				}
				else
				{
					GameCenter.InstanceRef.controladoraJuego.Inicializar_Partida(Personaje.MarlaGibbs);
					GameCenter.InstanceRef.controladoraEscenas.IrEscenaWardExterior ();
				}

				break;
			}

			case "btRobertDuncan":
			{
				if(btRobert.localPosition.Equals(posicionRobert))
				{
					SubirPersonaje(btRobert);
					BajarPersonaje(btWarren, posicionWarren);
					BajarPersonaje(btMarla, posicionMarla);
				}
				else
				{
					GameCenter.InstanceRef.controladoraJuego.Inicializar_Partida(Personaje.RobertDuncan);
					GameCenter.InstanceRef.controladoraEscenas.IrEscenaWardExterior ();
				}

				break;
			}

			case "btWarrenBedford":
			{
				if(btWarren.localPosition.Equals(posicionWarren))
				{
					SubirPersonaje(btWarren);
					BajarPersonaje(btRobert, posicionRobert);
					BajarPersonaje(btMarla, posicionMarla);
				}
				else
				{
					GameCenter.InstanceRef.controladoraJuego.Inicializar_Partida(Personaje.WarrenBedford);
					GameCenter.InstanceRef.controladoraEscenas.IrEscenaWardExterior ();
				}

				break;
			}
		}
	}

	private void SubirPersonaje(RectTransform seleccionRect)
	{
		seleccionRect.localPosition = new Vector3(seleccionRect.localPosition.x , seleccionRect.localPosition.y + 100, seleccionRect.localPosition.z);
	}

	private void BajarPersonaje(RectTransform seleccionRect, Vector3 seleccionVector)
	{
		seleccionRect.localPosition = new Vector3(seleccionVector.x , seleccionVector.y, seleccionVector.z);
	}
}
