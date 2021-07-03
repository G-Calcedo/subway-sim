using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerBehaviour : MonoBehaviour
{
    public GameObject sweeper_1, sweeper_2;
    public GameObject currentTrash;

    private StateMachineEngine cleanerSM;
    private RandomMovement randomMovement;

    private void Start()
    {
        cleanerSM = new StateMachineEngine();
        randomMovement = GetComponent<RandomMovement>();
        currentTrash = null;

        Perception isWalking = cleanerSM.CreatePerception<ValuePerception>(() => !randomMovement.IsMoving());

        //Perception trashThrown = cleanerSM.CreatePerception<PushPerception>();
        Perception trashFound = cleanerSM.CreatePerception<ValuePerception>(() => !(currentTrash is null));
        Perception sweepingTrash = cleanerSM.CreatePerception<TimerPerception>(1);
        Perception workingShift = cleanerSM.CreatePerception<TimerPerception>(30);
        Perception startResting = cleanerSM.CreatePerception<ValuePerception>(() => !randomMovement.IsMoving());

        Tween cleanAnim = null;

        State movingCharacter = cleanerSM.CreateEntryState("Moving", () =>
        {
            cleanAnim.Kill();
            CancelInvoke();
            randomMovement.active = true;
            sweeper_2.SetActive(false);
            sweeper_1.SetActive(true);
            InvokeRepeating(nameof(ScanTrash), 0, 1);
        });

        State GoingToTrash = cleanerSM.CreateState("Going", () =>
        {
            CancelInvoke();
            randomMovement.active = false;
            randomMovement.SetDestination(currentTrash.transform.position);
            sweeper_1.transform.DOScale(200, 0.15f).OnComplete(() =>
            {
                cleanAnim = sweeper_1.transform.DOScale(250, 0.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);
            });
        });

        State SweepingTrash = cleanerSM.CreateState("Sweeping", () =>
        {
            cleanAnim.Kill();
            sweepingTrash.Reset();
            sweeper_1.SetActive(false);
            sweeper_2.SetActive(true);
            InvokeRepeating(nameof(Sweep), 0, 0.25f);
            Destroy(currentTrash);
            currentTrash = null;
        });

        State goBackHome = cleanerSM.CreateState("GoBackHome",
            () =>
            {
                cleanAnim.Kill();
                CancelInvoke();
                randomMovement.active = false;
                sweeper_1.SetActive(true);
                sweeper_2.SetActive(false);
                randomMovement.SetDestination(SubwayStation.main.NearestCleanerSpot(transform.position));
            });

        State rest = cleanerSM.CreateState("Resting",
            () =>
            {
                SubwayStation.main.cleanerCount--;
                Destroy(gameObject);
            });

        cleanerSM.CreateTransition("Move", movingCharacter, trashFound, GoingToTrash);
        cleanerSM.CreateTransition("Go", GoingToTrash, isWalking, SweepingTrash);
        cleanerSM.CreateTransition("Clean", SweepingTrash, sweepingTrash, movingCharacter);
        cleanerSM.CreateTransition("EndWorkingShift", movingCharacter, workingShift, goBackHome);
        cleanerSM.CreateTransition("Rest", goBackHome, startResting, rest);
    }

    private void Update()
    {
        cleanerSM.Update();
    }

    private void Sweep()
    {
        sweeper_2.transform.localScale = new Vector3(
            sweeper_2.transform.localScale.x * -1,
            sweeper_2.transform.localScale.y,
            sweeper_2.transform.localScale.z);
    }

    private void ScanTrash()
    {
        Trash nearestTrash = null;
        float distance = Mathf.Infinity;

        foreach (Collider c in Physics.OverlapSphere(transform.position, 20))
        {
            if (c.CompareTag("Trash"))
            {
                Trash trash = c.GetComponent<Trash>();
                float currentDistance = Vector3.Distance(transform.position, trash.transform.position);

                if (!trash.found && currentDistance < distance)
                {
                    nearestTrash = trash;
                    distance = currentDistance;
                }
            }
        }

        if (nearestTrash is null) return;

        nearestTrash.found = true;
        currentTrash = nearestTrash.gameObject;
    }
}
