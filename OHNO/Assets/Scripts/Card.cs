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
    public HandOfCards Hand = null;
    [SerializeField] private float floatingDistanceOnHover = 0.3f;
    private bool isDisplaced = false;
    public bool movingUsingCode = true;
    public bool freeFall = false;

    
    private int counter = 0;
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
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    private void Update()
    {
        
        if (!movingUsingCode && Vector3.Distance(transform.position,position) < 0.1f)
        {
            freeFall = true;
            GetComponent<Rigidbody>().isKinematic = false;
        }
        
        if (!freeFall && (transform.position != position || transform.rotation != rotation))
        {
            UpdateCardPositionAndRotation();
        }
        
        
   
    }

    void playCard()
    {
        if (GameManager.MyTurn && GameManager.IsValidMove(label, Hand.tableTop.topCard, Hand.tableTop.color))
        {
           Hand.ThrowCard(gameObject);
           EventHandler.TriggerPlayedCard(label);
        }
    }
    
    
    void OnMouseEnter()
    {
        if (Hand != null && GameManager.MyTurn)
        {
            position += new Vector3(0, floatingDistanceOnHover, 0);
            isDisplaced = true;
        }
    }
    
    void OnMouseExit()
    {
        if (Hand != null && GameManager.MyTurn || isDisplaced)

        {
            isDisplaced = false;
            position-= new Vector3(0,floatingDistanceOnHover,0);
        }
    }
    
    void OnMouseOver()
    {
        
        if (Hand != null && GameManager.MyTurn && Input.GetMouseButtonDown(0))
        {
            playCard();
        }
    }

    
}

