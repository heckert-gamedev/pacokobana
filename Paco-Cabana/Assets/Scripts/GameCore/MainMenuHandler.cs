using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace he
{
    public class MainMenuHandler : MonoBehaviour
    {

        [SerializeField] Button startButton;
        [SerializeField] Button quitButton;


        [Tooltip("TMPro object to play the blurb texts in")]
        [SerializeField] TMP_Text blurbText;

        [SerializeField] float blurbDelay = 5f;


        [Tooltip("Add Blurb Texts to this list")]
        [TextArea(5, 5)]
        [SerializeField] string[] blurbLoopTexts;


        char[] blurbOverwriteString;
        char[] blurbReplacementString;

        int blurbIndex = 0;

        WaitForSeconds waitForBlurb = new WaitForSeconds(1f);   // We cannot allow this *not* to be initialized, ever!
        WaitForEndOfFrame waitForFrame = new WaitForEndOfFrame();   // We cannot allow this *not* to be initialized, ever!

        string cachedInitialBlurb;

        private Coroutine blurbLoopRoutine;


        private GameCoreSceneManager gameCoreSceneManager;


        private void Awake()
        {
            cachedInitialBlurb = blurbText.text;
            //blurbLoopTexts[0] = blurbText.text;   // all blurb text from the prefab!
            waitForBlurb = new WaitForSeconds(blurbDelay);
        }


        void Start()
        {
            gameCoreSceneManager = FindObjectOfType<GameCoreSceneManager>();
            blurbLoopRoutine = StartCoroutine(BlurbLoop());
            Debug.Log($"{name} started Coroutine {blurbLoopRoutine}");
        }


        private void OnEnable()
        {
            //uiView.SetActive(true);
            startButton.onClick.AddListener(() => gameCoreSceneManager.LoadMainScene());
            quitButton.onClick.AddListener(() => gameCoreSceneManager.QuitGame());
        }


        private void OnDisable()
        {
            //uiView.SetActive(false);
            startButton.onClick.RemoveListener(() => gameCoreSceneManager.LoadMainScene());
            quitButton.onClick.RemoveListener(() => gameCoreSceneManager.QuitGame());
        }


        IEnumerator BlurbLoop()
        {
            yield return waitForFrame;
            while (true) // **Attention** Make sure to yield inside!
            {
                blurbIndex++;
                if (blurbIndex >= blurbLoopTexts.Length)
                {
                    blurbIndex = 0;
                }

                // Immediate replacement
                //blurbText.text = blurbLoopTexts[blurbIndex];

                // Fancy effect replacement

                blurbOverwriteString = blurbText.text.ToCharArray();
                blurbReplacementString = blurbLoopTexts[blurbIndex].ToCharArray();

                for (int charIndex = 0; charIndex < blurbOverwriteString.Length; charIndex++)
                {
                    blurbOverwriteString[charIndex] = blurbReplacementString[charIndex];

                    blurbText.text = blurbOverwriteString.ArrayToString();

                    // in case the new one is shorter, break off
                    if (charIndex == blurbReplacementString.Length - 1)
                    {
                        // actually not needed if we overwrite the field at the end anyway...
                        //blurbText.text.Remove(blurbReplacementString.Length);
                        break;
                    }

                    yield return waitForFrame;
                }

                // in case the new one is longer, and to truncate if shorter, just fill it up at the end...
                blurbText.text = blurbLoopTexts[blurbIndex];
                yield return waitForBlurb;
            }

        }

    }

}
