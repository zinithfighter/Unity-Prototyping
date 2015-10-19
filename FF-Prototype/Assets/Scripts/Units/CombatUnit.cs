using UnityEngine;

namespace Unit
{
    public enum State
    {
        idle,
        ready,
        attack,
        defend,
        dead,
    }

    public class CombatUnit : MonoBehaviour, IUnit
    {
        void OnActive()
        {
            Animator anim = GetComponentInChildren<Animator>();
            anim.SetTrigger("idle");
        }

        void OnIdle()
        {
            Animator anim = GetComponentInChildren<Animator>();
            anim.SetTrigger("idle");
        }

        void OnAbilitySelected()
        {

        }
        public void SetState(State state)
        {
            Animator anim = GetComponentInChildren<Animator>();

            switch (state)
            {
                case State.idle:
                    anim.SetTrigger("idle");
                    break;
                case State.ready:
                    anim.SetTrigger("ready");
                    break;
                case State.attack:
                    anim.SetTrigger("attack");
                    break;
                case State.defend:
                    anim.SetTrigger("defend");
                    break;
                case State.dead:
                    anim.SetTrigger("dead");
                    break;
            }
        }



        #region Variables
        [SerializeField]
        int _health;
        [SerializeField]
        float _attack;
        [SerializeField]
        float _defense;
        [SerializeField]
        bool _active;


        public float attack { get { return _attack; } set { _attack = value; } }

        public float defense { get { return _defense; } set { _defense = value; } }

        public int health { get { return _health; } set { _health = value; } }
        #endregion Variables
    }
}
