using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerBehaviour : MonoBehaviour
{
    public GameObject sweeper_1, sweeper_2;

    private StateMachineEngine cleanerSM;
    private RandomMovement randomMovement;

    private void Start()
    {
        cleanerSM = new StateMachineEngine();
        randomMovement = GetComponent<RandomMovement>();

        Perception isWalking = cleanerSM.CreatePerception<ValuePerception>(() => !randomMovement.IsMoving());

        Perception trashThrown = cleanerSM.CreatePerception<PushPerception>();
        Perception sweepingTrash = cleanerSM.CreatePerception<TimerPerception>(0.5f);

        Tween cleanAnim = null;



        State movingCharacter = cleanerSM.CreateEntryState("Moving", () =>
        {
            sweeper_2.SetActive(false);
            sweeper_1.SetActive(true);
            Debug.Log("Walking");

        });



        State GoingToTrash = cleanerSM.CreateEntryState("Going", () =>
        {
            Debug.Log("Going to trash");
            sweeper_1.transform.DOScale(200, 0.15f).OnComplete(() =>
            {
                cleanAnim = sweeper_1.transform.DOScale(250, 0.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);
            });
        });

        State SweepingTrash = cleanerSM.CreateState("Sweeping", () =>
        {
            sweepingTrash.Reset();
            sweeper_1.SetActive(false);
            sweeper_2.SetActive(true);
            Debug.Log("Sweeping");
        });


        cleanerSM.CreateTransition("Move", movingCharacter, trashThrown, GoingToTrash);
        cleanerSM.CreateTransition("Go", GoingToTrash, isWalking, SweepingTrash);
        cleanerSM.CreateTransition("Clean", SweepingTrash, sweepingTrash, movingCharacter);
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.G))
        {
            cleanerSM.Fire("Thank");
        }

        cleanerSM.Update();*/
    }
}
