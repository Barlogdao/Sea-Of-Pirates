using Project.Configs.GameResources;
using Project.Interfaces.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TestQuestandUpgrade : MonoBehaviour
{
    [SerializeField] private GameResource _gameResource;
    private IPlayerStorage _playerStorage;

    [Inject]
    public void Construct(IPlayerStorage playerStorage)
    {
        _playerStorage = playerStorage;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            _playerStorage.AddResource(_gameResource, 100);
        }
    }
}
