﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureAnswersDisplayer : MonoBehaviour {

    public GameObject gamePanel;
    private Sprite correctSprite;

    public Text questionText;
    private Image pickedOption;
    public Button[] optionButtons;
    public Image[] optionImages;
    private Button pickedButton;

    public GameController gameController;

    void OnEnable()
    {
        gamePanel.SetActive(true);
    }

    void OnDisable()
    {
        gamePanel.SetActive(false);
    }
    public List<Sprite> Shuffle(List<Sprite> listas)
    {
        for (int i = 0; i < listas.Count; i++)
        {
            Sprite temp = listas[i];
            int randomIndex = Random.Range(i, listas.Count);
            listas[i] = listas[randomIndex];
            listas[randomIndex] = temp;
        }
        return listas;
    }

    public void DisplayQuestion(string question, List<Sprite> listas, Sprite correctSprite, GameController game)
    {
        this.correctSprite = correctSprite;
        gameController = game;
        this.questionText.text = question;

        var list = Shuffle(listas);

        for(int i = 0; i < list.Count; i++)
        {
            optionImages[i].sprite = list[i];
        }
        TurnOnButtonInteract();
    }

    public void TurnOffButtonInteract()
    {
        foreach (Button bt in optionButtons)
        {
            if (bt != pickedButton)
            {
                bt.interactable = false;
                var buttonColor = bt.colors;
                buttonColor.disabledColor = new Color32(200, 200, 200, 200);
                bt.colors = buttonColor;
                bt.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(200, 200, 200, 200);
            }
        }
    }

    public void TurnOnButtonInteract()
    {
        foreach (Button bt in optionButtons)
        { 
            bt.interactable = true;
        }
    }

    public void Win()
    {
        pickedButton.gameObject.GetComponent<Image>().color = new Color32(55, 167, 69, 255);
        Invoke("NextRound", 1.5f); 
    }

    public void Lose()
    {
        pickedButton.gameObject.GetComponent<Image>().color = new Color32(150, 40, 40, 255);
        for(int i = 0; i < optionButtons.Length; i++)
        {
            if(optionImages[i].sprite == correctSprite)
            {
                optionButtons[i].gameObject.GetComponent<Image>().color = new Color32(55, 167, 69, 255);
            }
        }
        Invoke("NextRound", 1.5f);
    }

    public void ButtonClick(Image img)
    {
        pickedOption = img;
        pickedButton = img.transform.parent.gameObject.GetComponent<Button>();
        TurnOffButtonInteract();

        gameController.CheckAnswer(pickedOption.sprite);
    }

    private void NextRound()
    {
        pickedOption = null;
        foreach(var btn in optionButtons)
        {
            btn.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        foreach(var img in optionImages)
        {
            img.color = new Color32(255, 255, 255, 255);
        }
        gameController.RoundFinish();
    }

}
