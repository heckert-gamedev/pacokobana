using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace he
{
    public class CrateManager : MonoBehaviour
    {

        [SerializeField] List<CrateMovementHandler> crates;
        LevelTargetManager levelTargetManager;


        // Start is called before the first frame update
        void Start()
        {
            levelTargetManager = FindObjectOfType<LevelTargetManager>();
            Debug.Log($"{name} got TargetManager( '{levelTargetManager.name} ).");

            if (null == crates)
            {
                crates = new List<CrateMovementHandler>();
            }

            foreach (Transform t in transform)
            {
                if (t.gameObject.TryGetComponent<CrateMovementHandler>(out CrateMovementHandler crateChild))
                {
                    crates.Add(crateChild);
                }
            }
            OnEnable(); // Subscribe to crates' arrival events
        }


        private void OnDisable()
        {
            foreach (CrateMovementHandler c in crates)
            {
                //Debug.Log($"{name} unsubscribes to ArrivalAction of {c}");
                c.ArrivalAction -= OnCrateArrival;
            }
        }


        private void OnEnable()
        {
            foreach (CrateMovementHandler c in crates)
            {
                //Debug.Log($"{name} subscribe to ArrivalAction of {c}");
                c.ArrivalAction += OnCrateArrival;
            }
        }


        private void OnCrateArrival()
        {
            Debug.Log($"Crate arrived on target");
            levelTargetManager.UpdateTargetsLeftAndUI(levelTargetManager.TargetsLeft - 1);
        }
    }
}
