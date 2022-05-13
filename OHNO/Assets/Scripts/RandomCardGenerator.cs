using UnityEngine;


public static class RandomCardGenerator
{   
    // Generates a random card
    
    //4 colors
    private static string[] colors = {"r", "b", "g", "y"};
    
    public static string getRandomCard(float pWild = 0.2f)
    {
        // generate a random number b/w 0 and 1 and if it is < pwild, then the card is wild
        string color = Random.Range(0.0f, 1.0f) < pWild ? "w" : colors[Random.Range(0, 4)];
        int min =  0;
        int max = color == "w" ? 2 : 13; //13 colored cards, 2 wild cards
        return color + Random.Range(min, max);
    }
}



