using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GatheringManager : MonoBehaviour
{
    public int None = 0;
    public int Wood = 0;
    public int Food = 0;
    public int Stone = 0;
    public int Gold = 0;

    public TMP_Text woodText;
    public TMP_Text foodText;
    public TMP_Text stoneText;
    public TMP_Text goldText;
    private void Awake()
    {

    }
    void Start()
    {
        Wood = 300;
        Food = 500;
        Stone = 100;
        Gold = 0;
        
        woodText.text = Wood.ToString();
        foodText.text = Food.ToString();
        stoneText.text = Stone.ToString();
        goldText.text = Gold.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        woodText.text = Wood.ToString();
        foodText.text = Food.ToString();
        stoneText.text = Stone.ToString();
        goldText.text = Gold.ToString();
    }

    public bool HasEnoughResources(int woodCost, int foodCost, int stoneCost, int goldCost)
    {
        return Wood >= woodCost && Food >= foodCost && Stone >= stoneCost && Gold >= goldCost;
    }

    public void SpendResources(int woodCost, int foodCost, int stoneCost, int goldCost)
    {
        Wood -= woodCost;
        Food -= foodCost;
        Stone -= stoneCost;
        Gold -= goldCost;
    }
}
