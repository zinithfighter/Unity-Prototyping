using UnityEngine;
using System.Collections;

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

    public enum Direction
    {
        forward,
        back,
    }

    

    public class CombatUnit : MonoBehaviour, IUnit
    {

        public Camera defaultCam;
        public Camera battleCam;
        Animator anim;
        void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            if (_active)
            {
                Combat.CombatSystem.OnAbilitySelected += Attack;
            }

            source = transform.position;
            
        }

        public Vector3 source;
        
        void Attack()
        {
            print("move " + name);
            defaultCam.enabled = false;
            battleCam.enabled = true;
            StartCoroutine(Move(target.position));
        }
        
        public Transform target;
        public float offset = .02f;
        IEnumerator Move(Vector3 destination)
        {
            
            Vector3 facing =  Vector3.Normalize(destination - transform.position);
            transform.rotation = Quaternion.LookRotation(facing, Vector3.up);
            anim.SetTrigger("run");
            while (transform.position != destination)
            {
                Debug.DrawLine(destination, transform.position);
                float distance = Vector3.Magnitude(transform.position - destination);
                float speed = Time.deltaTime /distance; 
               
                if (distance < offset)
                    break;
                label.text = speed.ToString();
                
                transform.position = Vector3.MoveTowards(transform.position, destination, step);
                yield return null;

            }
            anim.SetTrigger("uppercut");
            anim.SetTrigger("idle");
            yield return new WaitForSeconds(2.0f);
            yield return StartCoroutine("RunBack");
        }

        public float step = .06f;
        public UnityEngine.UI.Text label;
        IEnumerator RunBack()
        {
            
            Vector3 facing = Vector3.Normalize(source - transform.position);
            transform.rotation = Quaternion.LookRotation(facing, Vector3.up); 
            anim.SetTrigger("run");
            
            while (transform.position != source)
            {
                Debug.DrawLine(source, transform.position);
                float distance = Vector3.Magnitude(transform.position - source);
                if (distance < offset)
                    break;
                label.text = distance.ToString();
                transform.position = Vector3.MoveTowards(transform.position, source, step);
                yield return null;

            }
            
            transform.rotation = Quaternion.LookRotation(facing * -1, Vector3.up);
            anim.SetTrigger("idle");
            defaultCam.enabled = true;
            battleCam.enabled = false;
            StopAllCoroutines();
            
                
        }
        void OnActive()
        {

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
