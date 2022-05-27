using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class Character : MonoBehaviour
{
    Transform target;
    NavMeshAgent character;
    bool haveTarget = false;
    Transform closestTarget;
    Stack stackComponent;

    public bool HaveTarget { get { return haveTarget; } private set { } }

    public void Initialize(NavMeshAgent charactherNavMesh, Stack characterStack)
    {
        character = charactherNavMesh;
        stackComponent = characterStack;
    }
    public void TargetSituation(bool targetSituation)
    {
        haveTarget = targetSituation;
    }
    public void GoBack(List<Transform> characterTargets)
    {
        target = characterTargets[0];
        character.SetDestination(target.position);
        haveTarget = true;
        stackComponent.Back();
    }
    public void ChooseBridge(int stage, Transform[] firstBSlots, Transform[] secondBSlots)
    {
        switch (stage)
        {
            case 1:
                int firstStageRandomIndex = Random.Range(0, firstBSlots.Length);
                int last = firstBSlots[firstStageRandomIndex].childCount - 1;
                target = firstBSlots[firstStageRandomIndex].GetChild(last);
                break;
            case 2:
                int secondStageRandomIndex = Random.Range(0, secondBSlots.Length);
                int lastSlot = secondBSlots[secondStageRandomIndex].childCount - 1;
                target = secondBSlots[secondStageRandomIndex].GetChild(lastSlot);
                break;
        }
        character.SetDestination(target.position);
        haveTarget = true;
    }
    public void ChooseCollactable(float rad, List<Transform> characterTargets, Animator anim)
    {
        Transform closest = GetClosestTarget(characterTargets);
        if (Vector3.Distance(transform.position, closest.position) < rad)
            target = closest;
        else
        {
            int random = Random.Range(0, characterTargets.Count);
            target = characterTargets[random];
        }
        character.SetDestination(target.position);
        if (!anim.GetBool("Run"))
            anim.SetBool("Run", true);
        haveTarget = true;
    }
    public void StopAgent()
    {
        character.isStopped = true;
    }
    private Transform GetClosestTarget(List<Transform> characterTargets)
    {
        float closestDistance = Mathf.Infinity;
        foreach (Transform closestOne in characterTargets)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, closestOne.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestTarget = closestOne;
            }
        }
        return closestTarget;
    }
}