using UnityEngine;
using System.Collections;

public class ScrollingUVs_Layers : MonoBehaviour 
{
	//public int materialIndex = 0;
	public float speedX;
	public float speedY;
	private Vector2 uvAnimationRate;
	public string textureName = "_MainTex";
	
	Vector2 uvOffset = Vector2.zero;

    private void Start()
    {
        uvAnimationRate = new Vector2(speedX, speedY) / 100;
    }

    void LateUpdate() 
	{
		uvOffset += ( uvAnimationRate * Time.deltaTime );
		if( GetComponent<Renderer>().enabled )
		{
			GetComponent<Renderer>().sharedMaterial.SetTextureOffset( textureName, uvOffset );
		}
	}
}