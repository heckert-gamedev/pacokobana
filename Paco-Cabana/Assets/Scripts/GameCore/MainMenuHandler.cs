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
        [SerializeField] string[] blurbLoopTexts;


        int blurbIndex = 0;

        WaitForSeconds waitForBlurb = new WaitForSeconds(1f);   // We cannot allow this *not* to be initialized, ever!

        string cachedInitialBlurb;

        private Coroutine blurbLoopRoutine;


        private GameCoreSceneManager gameCoreSceneManager;


        private void Awake()
        {
            cachedInitialBlurb = blurbText.text;
            blurbLoopTexts[0] = blurbText.text;
            waitForBlurb = new WaitForSeconds(blurbDelay);
        }


        void Start()
        {
            gameCoreSceneManager = FindObjectOfType<GameCoreSceneManager>();
            blurbLoopRoutine = StartCoroutine(nameof(BlurbLoop));
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

        IEnumerable BlurbLoop()
        {
            while (true) // **Attention** Make sure to yield inside!
            {
                blurbIndex += 1 % blurbLoopTexts.Length;
                blurbText.text = blurbLoopTexts[blurbIndex];
                yield return waitForBlurb;
            }

            //yield return null;
        }

    }

}
