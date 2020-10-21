using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InterviewTest.Scripts;
using TMPro;

namespace InterviewTest.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public enum Toggles
        {
            FavoriteShow,
            RandomFact,
            FovoriteColour,
            MiniGame
        }

        [SerializeField]
        private Toggle[] _toggles;

        [SerializeField]
        private RawImage _favoriteShow, _favoriteColour;

        [SerializeField]
        private Button _revealColourButton;

        [SerializeField]
        private TextMeshProUGUI _catFactText;

        [SerializeField]
        private Image _catImage;

        [SerializeField]
        private Sprite[] _catImageSelection;

        public void Toggled(int toggleID)
        {
            //check to see if toggle is active
            if (_toggles[toggleID].isOn == true)
            {
                //check for which toggle selected
                Toggles toggleType = _toggles[toggleID].GetComponent<ToggleType>().GetToggleType();

                ResetSlides();

                switch (toggleType)
                {
                    case Toggles.FavoriteShow:
                        //Display favorite CN show
                        _favoriteShow.gameObject.SetActive(true);
                        break;
                    case Toggles.RandomFact:
                        //Display random cat fact from catfact.ninja
                        StartCoroutine(RandomCatFact.Instance.GetCatFact((catFact) =>
                        {
                            if (catFact != null)
                            {
                                CatFact(catFact);
                            }
                        }));
                        break;
                    case Toggles.FovoriteColour:
                        //Display favorite colour
                        _revealColourButton.gameObject.SetActive(true); //TODO: change to a shader effect for blue
                        break;
                    case Toggles.MiniGame:
                        //Run minigame
                        LaunchMiniGame();
                        break;
                    default:
                        Debug.LogError("Invalid toggle selected");
                        break;
                }
            }
        }

        private void ResetSlides()
        {
            _favoriteShow.gameObject.SetActive(false);
            _favoriteColour.gameObject.SetActive(false);
            _revealColourButton.gameObject.SetActive(false);
            _catFactText.gameObject.SetActive(false);
        }

        private void CatFact (string newCatFact)
        {
            //Display cat fact
            _catFactText.text = newCatFact;
            _catFactText.gameObject.SetActive(true);
            //generate random cat image and display
            int randomNum = Random.Range(0, _catImageSelection.Length);
            _catImage.sprite = _catImageSelection[randomNum];
        }

        public void RevealColour ()
        {
            _revealColourButton.gameObject.SetActive(false);
            _favoriteColour.gameObject.SetActive(true);
        }

        private void LaunchMiniGame ()
        {
            Debug.Log("launching mini-game");
        }
    }
}