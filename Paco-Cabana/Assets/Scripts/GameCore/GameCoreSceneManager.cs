using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace he
{
    public class GameCoreSceneManager : MonoBehaviour
    {
        [SerializeField] private float timeToWaitForGameStart = 1f;

        private MainMenuHandler mainMenuHandler;


        private void Awake()
        {
            HookupPanel();
        }


        IEnumerator Start()
        {
            // When testing the main menu might not be the first scene to run and in this case the
            // Core prefab (and thus the MySceneManager) gets spawned and we have no mainMenuUI nor
            // any intro sequence...
            if (null != mainMenuHandler)
            {
                //fader.FadeOutImmediate();
                yield return new WaitForSeconds(timeToWaitForGameStart);
                //audioManager.PlayMenuMusic();
                //mainMenuUI.IntroSequence?.gameObject.SetActive(enabled);
                //mainMenuHandler.IntroSequence?.Begin(fader);
            }
            yield return null;
        }


        public void LoadMainScene()
        {
            SceneManager.LoadScene(1);
        }


        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }


        private void HookupPanel()
        {
            mainMenuHandler = FindObjectOfType<MainMenuHandler>();
        }

    }
}
