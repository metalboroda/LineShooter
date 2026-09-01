using Assets.Scripts.Input;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Player
{
	public class PlayerMovement : MonoBehaviour
	{
		[Inject] private IMovementInput _input;

		[SerializeField] private float speed = 5f;

		public bool HasMovementInput => !Mathf.Approximately(_input.Horizontal, 0f);

		public void Move()
		{
			Vector3 position = transform.position;
			float delta = _input.Horizontal * speed * Time.deltaTime;
    
			Debug.Log($"Horizontal={_input.Horizontal}, speed={speed}, deltaTime={Time.deltaTime}, delta={delta}");
    
			position.x += delta;
			transform.position = position;
		}

		public void Stop()
		{
		}
	}
}