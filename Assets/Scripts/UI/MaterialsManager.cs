using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaterialsManager : MonoBehaviour
{
    public TMP_Text woodText;
    public float wood;
    public TMP_Text foodText;
    public float food;
    public TMP_Text stoneText;
    public float stone;
    public TMP_Text goldText;
    public float gold;
    // Start is called before the first frame update
    void Start()
    {
        wood = 300f;
        food = 500f;
        stone = 100f;
        gold = 0f;
        
        woodText.text = "Wood: " + wood.ToString();
        foodText.text = "Food: " + food.ToString();
        stoneText.text = "Stone: " + stone.ToString();
        goldText.text = "Gold: " + gold.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
