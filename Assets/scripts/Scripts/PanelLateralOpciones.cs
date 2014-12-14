using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelLateralOpciones: MonoBehaviour 
{
	public Text textoCabecera;
	public Text textoCuerpo;

	void OnEnable()
	{
		if (GameCenter.InstanceRef != null) 
		{
			if(GameCenter.InstanceRef.controladoraJuego.personajePulsado != null)
				textoCabecera.text = GameCenter.InstanceRef.controladoraJuego.personajePulsado.DescripcionNombre.ToUpper();
			else
				textoCabecera.text = GameCenter.InstanceRef.controladoraJuego.objetoPulsado.DescripcionNombre.ToUpper();
		}
	}
}
