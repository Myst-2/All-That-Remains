// DeathMenu.cs
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    // Player chooses to try again
    public void OnResist()
    {
        BattleData.Clear();                      
        SceneFader.LoadScene("Game");            
    }

    // Player gives up and returns to main menu
    public void OnGiveIn()
    {
        BattleData.Clear();                      
        SceneFader.LoadScene("StartMenu");      
    }
}
