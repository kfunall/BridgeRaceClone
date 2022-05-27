using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterControl : Character
{
    [SerializeField] Transform[] firstStageBridgeSlots;
    [SerializeField] Transform[] secondStageBridgeSlots;
    [SerializeField] Stack stack;
    [SerializeField] List<Transform> targets;
    [SerializeField] Animator animator;
    [SerializeField] float radius = 2f;
    Transform targett;
    NavMeshAgent agent;
    int stage = 1;
    public int Stage { get { return stage; } private set { } }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        Initialize(agent, stack);
    }
    private void Update()
    {
        if (!GameManager.Instance.GameEnded)
        {
            if (!agent.hasPath)
                TargetSituation(false);
            if ((!HaveTarget && targets.Count > 0 && stack.CollectedBricks.Count < 4) && !stack.GoBack)
                ChooseCollactable(radius, targets, animator);
            if (!HaveTarget && stack.CollectedBricks.Count >= 4)
                ChooseBridge(stage, firstStageBridgeSlots, secondStageBridgeSlots);
            if (stack.GoBack)
                GoBack(targets);
        }
    }
    public void AgentDestination()
    {
        if (!GameManager.Instance.GameEnded)
        {
            agent.SetDestination(targets[0].position);
            agent.isStopped = false;
            TargetSituation(true);
        }
    }
    public void RemoveTarget(Transform targetToRemove) => targets.Remove(targetToRemove);
    public void AddTarget(Transform targetToAdd) => targets.Add(targetToAdd);
    public void RemoveAllTargets() => targets.Clear();
    public void ChangeStage(int number)
    {
        stage = number;
        if (stage == 3)
            GameManager.Instance.EndGame(transform.tag == "RedCharacter" ? 0 : 2);
    }
    public void End(int x)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        agent.enabled = false;
        animator.SetBool("Run", false);
        if (x == 0)
        {
            if (transform.tag == "RedCharacter")
                animator.SetBool("RedDance", true);
            else
                animator.SetBool("BlueDance", true);
        }
        transform.position = new Vector3(x, 3.5f, 53f);
        transform.eulerAngles = Vector3.up * 180f;
        stack.ClearStack();
    }
}