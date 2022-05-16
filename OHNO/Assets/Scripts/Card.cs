using System;
using UnityEngine;

public class Card : MonoBehaviour
{
    private string label;
    private Texture2D texture;
    public Vector3 position = new Vector3(0f, 0f, 0f);
    public Quaternion rotation = Quaternion.identity;
    public float movementSpeed = 1f;
    public float rotationSpeed = 1f;
    public void setLabel(string label)  // also updates the texture
    {
        this.label = label;
        updateTexture();
    }

    public string getLabel()
    {
        return label;
    }

    public void setPosition(Vector3 pos)
    {
        position = pos;
    }
    private void updateTexture()
    {
        texture = Resources.Load<Texture2D>(this.label);
        GetComponent<MeshRenderer>().material.SetTexture("Texture2D_cdfa9ed97e8f4e6bafb1a1406ee86887", texture);
    }
    
    private void UpdateCardPositionAndRotation()
    {
        transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * movementSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * rotationSpeed);
    }

    private void Update()
    {
        if (transform.position != position || transform.rotation != rotation)
        {
            UpdateCardPositionAndRotation();
        }
    }
}

