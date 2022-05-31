
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameManager: MonoBehaviour
{   
    
    private Dictionary<int, string> playerNames = new Dictionary<int, string>();
    private Dictionary<int, EnemyHand> enemyHands = new Dictionary<int,  EnemyHand>();
    private int numberOfPlayers = 0;
    private string topCard;
    private char color;
    private NetworkGamePlayer gamePlayer;
    int myId;
    
    public static bool MyTurn = false;
    [SerializeField] private Material cardFront;
    [SerializeField] private TableTop tableTop;
    [SerializeField] private Deck deck;
    [SerializeField] private HandOfCards ownHand;
    [SerializeField] private EnemyHand enemyHandLeft;
    [SerializeField] private EnemyHand enemyHandFront;
    [SerializeField] private EnemyHand enemyHandRight;
    [SerializeField] private TMP_Text wintext;
    [SerializeField] private GameObject colorSelect;
    
    private string cardBuffer;
    public void SetupPlayerNames(string[] pNames, int myId, int numPlayers,NetworkGamePlayer gamePlayer)
    {

        this.gamePlayer = gamePlayer;
        
        
        
        for (int i = 0; i < numPlayers; i++)
        {
            playerNames[i] = pNames[i];
        }

        numberOfPlayers = numPlayers;
        this.myId = myId;

        Debug.Log("MyID = " + myId);
        
        switch (numberOfPlayers)
        {
            case 2:
                enemyHands[(myId + 1) % 2] = enemyHandFront;
                break;
            case 3:
                enemyHands[(myId + 1) % 2] = enemyHandLeft;
                enemyHands[(myId + 2) % 2] = enemyHandFront;
                break;
            case 4:
                enemyHands[(myId + 1) % 2] = enemyHandLeft;
                enemyHands[(myId + 2) % 2] = enemyHandFront;
                enemyHands[(myId + 3) % 2] = enemyHandRight;
                break;
        }

        foreach (var key in enemyHands.Keys)
        {
            enemyHands[key].SetName(playerNames[key]);
        }
        foreach (var value in enemyHands.Values)
        {
            value.DrawCards(7);
        }
        
        ownHand.DrawCards(7);
    }

    public void InitialSetup(string card)
    {
        EventHandler.ClickedOnDeck += iDrewCard;
        EventHandler.PlayedCard += iPlayedCard;
        tableTop.drawFirstCard(card);
    }

    public void TakeTurn(string topcard,char color = '.')
    {
        deck.isClickable = true;
        MyTurn = true;
        Debug.Log("IT is my turn");
        topCard = topcard;
        color = topcard[0];
        ownHand.TakeTurn(topcard,color);
    }

    public static bool IsValidMove(string card,string topCard, char color)
    {
        return card[0] == 'w' || card[0] == color || card.Substring(1) == topCard.Substring(1);
    }
    
    
    private void OnDisable()
    {
        EventHandler.ClickedOnDeck -= iDrewCard;
        EventHandler.PlayedCard -= iPlayedCard;
    }
    
    
    private void iDrewCard()
    {
        
        gamePlayer.iDrewCard(myId);
    }
    
    private void iPlayedCard(string card)
    {
        deck.isClickable = false;
        MyTurn = false;
        if (card[0] == 'w')
        {
            colorSelect.SetActive(true);
            cardBuffer = card;
            
            return;
        }

        if (ownHand.numberOfCards == 0)
        {
            gamePlayer.iWon(playerNames[myId]);
        }

        int turn = 1;
        if (card.Substring(1) == "10")
        {
            turn = -1;
        }else if (card.Substring(1) == "12")
        {
            turn = 2;
        }
        gamePlayer.iPlayedCard(myId,card,card[0],turn);
    }

    
    public void PickedColor(int colorCode)
    {
        char color = '.';
        switch (colorCode)
        {
            case 0:
                color = 'r';
                break;
            
            case 1:
                color = 'g';
                break;
            
            case 2:
                color = 'b';
                break;
            
            case 3:
                color = 'y';
                break;
        }

        GameObject wildCard = tableTop.getTopCardObject();
        Texture2D texture = Resources.Load<Texture2D>(cardBuffer + color);
        wildCard.GetComponent<MeshRenderer>().material.SetTexture("Texture2D_cdfa9ed97e8f4e6bafb1a1406ee86887", texture);
        
        colorSelect.SetActive(false);
        gamePlayer.iPlayedCard(myId,cardBuffer,color,1);
        cardBuffer = "";
    }

    public void playerDrewCards(int id, int howMany)
    {
        if (id != myId)
        {
            enemyHands[id].DrawCards(howMany);
        }
    }
    
    
    public void playerDrewCard(int id)
    {
        if (id != myId)
        {
            enemyHands[id].DrawCard();
        }
    }
    
    public void playerPlayedCard(int id,string card, char color)
    {
        if (card[0] == 'w')
        {
            GameObject wildCard = tableTop.getTopCardObject();
            Texture2D texture = Resources.Load<Texture2D>(card + color);
            wildCard.GetComponent<MeshRenderer>().material.SetTexture("Texture2D_cdfa9ed97e8f4e6bafb1a1406ee86887", texture);
        }
        
        if (id != myId)
        {
            enemyHands[id].ThrowCard(card);
        }
    }

    public void DrawNCards(int howMany)
    {
        ownHand.DrawCards(howMany);
    }


    public void playerWon(string Name)
    {
        wintext.text = Name + " Won!!!!";
        
    }
}   
