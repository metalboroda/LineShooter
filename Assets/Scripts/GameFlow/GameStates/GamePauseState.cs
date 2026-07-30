using UnityEngine;

namespace Assets.Scripts.GameFlow.GameStates
{
    public class GamePauseState : GameBaseState
    {
        public override void Enter()
        {
            Time.timeScale = 0f;
        }

        public override void Exit()
        {
            Time.timeScale = 1f;
        }
    }
}