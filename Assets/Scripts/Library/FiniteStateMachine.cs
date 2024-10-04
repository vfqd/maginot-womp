using System;
using Library.Sprites;
using UnityEngine;

namespace Library
{
    public class FiniteStateMachine
    {
        private IState _currentState;
        public IState CurrentState => _currentState;

        public void ChangeState(IState newState)
        {
            // Debug.Log($"Changing from {_currentState} to {newState}");
            if (_currentState != null)
                _currentState.Exit();
 
            _currentState = newState;
            _currentState.Enter();
        }
 
        public void Update()
        {
            if (_currentState != null) _currentState.Update();
        }

        public FiniteStateMachine(IState defaultState = null)
        {
            if (defaultState != null)
            {
                ChangeState(defaultState);
            }
        }
    }

    public interface IState
    {
        public void Enter();
        public void Exit();
        public void Update();
    }
}