using System;
using System.SceneManagement;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private int _sceneGroupIndex;

    private async void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            await SceneLoader.Instance.LoadSceneGroup(_sceneGroupIndex, false);
        }
    }
}
