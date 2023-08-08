using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Game { 
public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lobbyCodeText;
    [SerializeField] private Button buttonNextCharacter;
    [SerializeField] private Button buttonPrevCharacter;
    public Image CharacterUI;
    public Sprite[] characters;
    private int selectedCharacter = 0;

    void Start()
    {
        _lobbyCodeText.text = $"Codigo: {GameLobbyManager.Instance.GetLobbyCode()}";
        if (CharacterUI != null && characters.Length > 0)
            CharacterUI.sprite = characters[selectedCharacter];
        buttonNextCharacter.onClick.AddListener(() => nextCharacter());
        buttonPrevCharacter.onClick.AddListener(() => previousCharacter());
    }

    public void nextCharacter()
    {
        int characterArrayLength = characters.Length;
        selectedCharacter = (selectedCharacter + 1) % characterArrayLength;
        CharacterUI.sprite = characters[selectedCharacter];
    }

    public void previousCharacter()
    {
        int characterArrayLength = characters.Length;
        selectedCharacter = (selectedCharacter - 1 + characterArrayLength) % characterArrayLength;
        CharacterUI.sprite = characters[selectedCharacter];
    }

    public void startGame()
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
    }
    }
}