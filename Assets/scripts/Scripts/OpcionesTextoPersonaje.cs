using UnityEngine;
using System.Collections;

public class OpcionesTextoPersonaje : MonoBehaviour 
{
	public RectTransform scrollRectTransform;
	public RectTransform rectPanel;
	public float y;
	
	void OnEnable()
	{
		Posicion_Inicial_Caja ();
	}
	
	void OnDisable()
	{
		Posicion_Inicial_Caja ();
	}
	
	void Update()
	{
		//Debug.Log ("OffsetMin: " + rectPanel.offsetMin);
		//Debug.Log ("OffsetMax: " + rectPanel.offsetMax);
		if (rectPanel.offsetMax.y < 0)
			Posicion_Inicial_Caja ();

		if (rectPanel.offsetMin.y > 0)
			rectPanel.offsetMin = new Vector2 (0, 0);
	}
	
	public void Posicion_Inicial_Caja()
	{
		rectPanel.offsetMax = new Vector2 (0, 0);
	}
}
