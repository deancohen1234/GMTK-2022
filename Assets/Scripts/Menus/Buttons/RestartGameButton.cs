using UnityEngine.SceneManagement;

public class RestartGameButton : BattleButton
{
    public override void OnClick()
    {
        SceneManager.LoadScene(0);
    }
}
