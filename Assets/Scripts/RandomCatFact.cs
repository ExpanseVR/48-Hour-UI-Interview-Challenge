using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class CatFact
{
    public string fact;
}

namespace InterviewTest.Scripts
{
    public class RandomCatFact : MonoSingleton<RandomCatFact>
    {
        public static event Action<string> OnCatFact;

        [SerializeField]
        private string _catFactURL;

        public IEnumerator GetCatFact (Action<string> catfact)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(_catFactURL))
            {
                //get cat fact
                yield return webRequest.SendWebRequest();
                //check for errors
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.LogError(webRequest.error);
                }
                else
                {
                    //convert json to string and return
                    var json = webRequest.downloadHandler.text;
                    var newCatFact = JsonUtility.FromJson<CatFact>(json);
                    catfact(newCatFact.fact);
                    //OnCatFact(newCatFact.fact); //JON <--- which option is better or is either OK?
                }
            }
        }
    }
}