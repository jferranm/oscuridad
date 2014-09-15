using UnityEngine;

public class ParametrosGUIText : MonoBehaviour 
{
	public float posicion_x;
	public float posicion_y;
	public float size_fuente;

	void Awake()
	{
		SituarGUIText();
		this.GetComponent<PosicionadorGUIText>().enabled = false;
	}

	public void SituarGUIText()
	{
		this.guiText.fontSize = (int)(size_fuente * Screen.width);  
		this.transform.position = new Vector3(0, 0, 1);
		this.transform.localScale = Vector3.zero; 
		this.guiText.pixelOffset = new Vector2(posicion_x * Screen.width, posicion_y * Screen.height);
	}

	public void SetPosicion_X (float p)
	{
		posicion_x = p;
	}

	public void SetPosicion_Y (float p)
	{
		posicion_y = p;
	}

	public void SetSizeFuente(float s)
	{
		size_fuente = s;
	}
}