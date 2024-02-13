using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;
    // Start is called before the first frame update

    [SerializeField]
    private Sprite[] cardSprites;

    private List<int> cardIdList = new List<int>();

    private List<Card> cardList = new List<Card>();
    void Start()
    {
        GenerateCardID();
        ShuffleCardID();
        InitBoard();    
    }

    void GenerateCardID()
    {
        for(int i = 0; i < cardSprites.Length; i++)
        {
            cardIdList.Add(i);
            cardIdList.Add(i);
        }

    }
    void ShuffleCardID()
    {
        int cardCount = cardIdList.Count;
        for(int i = 0; i < cardCount; i++)
        {
            int randomIndex = Random.Range(i, cardCount);
            int temp = cardIdList[randomIndex];
            cardIdList[randomIndex] = cardIdList[i];
            cardIdList[i] = temp;
        }


    }
    // Update is called once per frame
    void InitBoard()
    {
        float spaceY = 1.8f;
        float spaceX = 1.3f;
        int rowCount = 5;
        int colCount = 4;
        int cardIndex = 0;
        for(int row = 0; row< rowCount; row++)
        {
            for(int col = 0; col<colCount; col++)
            {
                float posX = (col - (colCount /2)) * spaceX + (spaceX /2);
                float posY = (row - (int)(rowCount) / 2) * spaceY;
                Vector3 pos = new Vector3(posX, posY, 0f);
                GameObject cardObject = Instantiate(cardPrefab,pos ,Quaternion.identity);
                Card card = cardObject.GetComponent<Card>();
                int cardID = cardIdList[cardIndex++];
                card.SetCardID(cardID);
                card.SetAnimalSprite(cardSprites[cardID]);
                cardList.Add(card);
                //if(cardIndex >= cardSprites.Length)
                
            }
        }

    }

    public List<Card> GetCards()
    {
        return cardList;
    }
}
