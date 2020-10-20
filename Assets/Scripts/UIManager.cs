using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InterviewTest.UI
{
    public class UIManager : MonoBehaviour
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
        // Start is called before the first frame update

        public void Toggled(int toggleID)
        {
            //check to see if toggle is active
            if (_toggles[toggleID].isOn == true)
            {
                //check for which toggle selected
                Toggles toggleType = _toggles[toggleID].GetComponent<ToggleType>().GetToggleType();

                _favoriteShow.gameObject.SetActive(false);
                _favoriteColour.gameObject.SetActive(false);

                switch (toggleType)
                {
                    case Toggles.FavoriteShow:
                        //Display favorite CN show
                        _favoriteShow.gameObject.SetActive(true);
                        break;
                    case Toggles.RandomFact:
                        //Display random cat fact from catfact.ninja
                        StartCoroutine(CatFact());
                        break;
                    case Toggles.FovoriteColour:
                        //Display favorite colour
                        _favoriteColour.gameObject.SetActive(true); //TODO: change to a shader effect for blue
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

        IEnumerator CatFact ()
        {
            Debug.Log("Here is a cat fact");
            yield return null;
        }

        private void LaunchMiniGame ()
        {
            Debug.Log("launching mini-game");
        }
    }
}