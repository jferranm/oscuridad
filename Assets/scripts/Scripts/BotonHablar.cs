using UnityEngine;
using System.Collections;
using Oscuridad.Estados;

public class BotonHablar : MonoBehaviour 
{
	//private PanelObjetos panelObjetosRef;

	void Start()
	{
		//panelObjetosRef = GameObject.Find("PanelGuiObjetos").GetComponent<PanelObjetos> ();
	}

	void Update()
	{
		/*foreach (Touch touch in Input.touches)
		{
			if (this.guiTexture.HitTest(touch.position))
			{
				//Volvemos al estado del jugador EnEspera
				Hablar_Personaje();
				break;
			}
		}*/
	}

	void OnMouseDown() 
	{
		//if (this.guiTexture.texture == panelObjetosRef.botonHablar) 
		//{
		//	Hablar_Personaje ();
		//}
	}

	private void Hablar_Personaje()
	{
		//Desactivamos el icono de Hablar
		//panelObjetosRef.Desactivar_Opcion("BotonHablar");
		
		//Mostramos el Inicio de la pregunta
		GameCenter.InstanceRef.controladoraGUI.Lanzar_Hablar();
	}
}
