using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace he
{
    public class LevelTargetManager : MonoBehaviour
    {

        //[SerializeField] TargetField[] levelTargets;
        [SerializeField] List<TargetField> levelTargets;


        [SerializeField]    // for debugging
        int targetsLeft = 0;

        [SerializeField] TMP_Text targetsCounterTMP;
        [SerializeField] string targetsCounterBaseString = "Done: ";

        public int TargetsLeft { get => targetsLeft; }

        GameCoreManager gameCoreManager;


        void Awake()
        {
            if (null == levelTargets)
            {
                levelTargets = new List<TargetField>();
                Debug.Log($"{name} - new list for targets");
            }

            if (0 == levelTargets.Count)
            {
                Debug.Log($"{name} iterates children for target fields");
                foreach (Transform t in transform)
                {
                    if (t.gameObject.TryGetComponent<TargetField>(out TargetField tf))
                    {
                        levelTargets.Add(tf);
                        targetsLeft++;
                        Debug.Log($"{name} found field '{t.name}', counting: {targetsLeft}");
                    }
                    else
                    {
                        Debug.Log($"{name} child {t.name} is not a TargetField");
                    }
                }
            }
            else
            {
                targetsLeft = levelTargets.Count;
                Debug.Log($"{name} awakes with {targetsLeft} targets already assigned");
            }
        }

        private void Start()
        {
            gameCoreManager = FindObjectOfType<GameCoreManager>();
            Debug.Log($"{name} got core '{gameCoreManager}'");
            UpdateTargetsLeftAndUI(targetsLeft);
        }

        public void UpdateTargetsLeftAndUI(int value)
        {
            Debug.Log($"LTM updating ui; targetsLeft={targetsLeft}; incoming value: {value}");
            targetsLeft = value;
            //targetsLeft --;
            //targetsCounterTMP.text = string.Format($"{targetsCounterBaseString}{0}", levelTargets.Count - value);
            targetsCounterTMP.text = $"{targetsCounterBaseString}{levelTargets.Count - value}/{levelTargets.Count}";
        }

    }
}
