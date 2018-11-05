using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShirtTextUI : MonoBehaviour {

    [SerializeField]
    private Text shirtText;

    private void Start()
    {
        shirtText.text = "Shirts: 0";
    }
    public void UpdateText(int n)
    {
        shirtText.text = "Shirts: " + n;
    }
}
