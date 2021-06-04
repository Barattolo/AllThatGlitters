using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyRobot : Robot
{
    private bool isActivated = false;
    [SerializeField] private float distanceThreshold = 1f;
    AudioManager audioM;

    private void Start()
    {
        base.Start();
        audioM = FindObjectOfType<AudioManager>();
    }
    protected override void Update()
    {
        if (player != null && isActivated && !startedFollowing)
        {
            GameManager.singleton.CurrentRescuedRobots++;
            startedFollowing = true;
            StartCoroutine(FollowPlayer(player, distanceThreshold));
        }
    }

    public override void Interact()
    {
        base.Interact();
        Debug.Log("va");
        audioM.PlaySound("FollowFriend");
        isActivated = true;
        GameManager.singleton.Player.GetComponent<Interact>().Remove(gameObject.GetComponent<Interactable>());

    }

    protected override void OnPlayerNearby(GameObject _player)
    {
        base.OnPlayerNearby(_player);
    }
}
