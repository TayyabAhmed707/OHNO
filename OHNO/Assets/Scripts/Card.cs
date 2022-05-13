using UnityEngine;

public class Card : MonoBehaviour
{
    private string label;
    private Texture2D texture;
    public Vector3 position = new Vector3(0f, 0f, 0f);

    public void setLabel(string label)  // also updates the texture
    {
        this.label = label;
        updateTexture();
    }

    public void setPosition(Vector3 pos)
    {
        this.position = pos;
    }
    private void updateTexture()
    {
        texture = Resources.Load<Texture2D>(this.label);
        GetComponent<MeshRenderer>().material.SetTexture("Texture2D_cdfa9ed97e8f4e6bafb1a1406ee86887", texture);
    }
    
}

