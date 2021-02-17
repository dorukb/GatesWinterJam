using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoomManager : MonoBehaviour
{
    [SerializeField]
    private GameObject charScreen;
    [SerializeField]
    private GameObject enemyScreen1;
    [SerializeField]
    private GameObject enemyScreen2;
    [SerializeField]
    private GameObject enemyScreen3;
    [SerializeField]
    private GameObject enemyScreen4;
    [SerializeField]
    private GameObject enemyScreen5;
    public void ToEnemyScreen1(string ScreenToChange)
    {
        charScreen.SetActive(false);
        enemyScreen1.SetActive(true);
    }
    public void ToEnemyScreen2(string ScreenToChange)
    {
        charScreen.SetActive(false);
        enemyScreen2.SetActive(true);
    }
    public void ToEnemyScreen3(string ScreenToChange)
    {
        charScreen.SetActive(false);
        enemyScreen3.SetActive(true);
    }
    public void ToEnemyScreen4(string ScreenToChange)
    {
        charScreen.SetActive(false);
        enemyScreen4.SetActive(true);
    }
    public void ToEnemyScreen5(string ScreenToChange)
    {
        charScreen.SetActive(false);
        enemyScreen5.SetActive(true);
    }
    public void TurnBackCharScreen(string ScreenToChange)
    {
        charScreen.SetActive(true);
        enemyScreen1.SetActive(false);
        enemyScreen2.SetActive(false);
        enemyScreen3.SetActive(false);
        enemyScreen4.SetActive(false);
        enemyScreen5.SetActive(false);
    }
}
