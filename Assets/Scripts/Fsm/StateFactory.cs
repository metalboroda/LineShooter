using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.FSM
{
	public class StateFactory<TContext> where TContext : class
	{
		private readonly Dictionary<Type, IState<TContext>> _states = new Dictionary<Type, IState<TContext>>();
		private readonly TContext _context;
		private readonly DiContainer _container;

		public StateFactory(TContext context, DiContainer container)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_container = container ?? throw new ArgumentNullException(nameof(container));
		}

		public TState GetState<TState>() where TState : class, IState<TContext>
		{
			Type type = typeof(TState);

			if (!_states.TryGetValue(type, out IState<TContext> state))
			{
				try
				{
					state = _container.Instantiate<TState>();
					state.Setup(_context);

					_states[type] = state;
				}
				catch (Exception ex)
				{
					Debug.LogError(
						$"[StateFactory] Failed to create state of type {type.Name}. Error: {ex}");

					throw new InvalidOperationException($"Failed to create state of type {type.Name}.", ex);
				}
			}

			return (TState)state;
		}
	}
}