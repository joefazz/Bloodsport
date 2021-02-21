using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Camera camera;

        [SerializeField] private CharacterController characterController;

        [SerializeField] private float health;
        
        [SerializeField] private Vector2 sensitivity;

        [SerializeField] private float playerSpeed = 1;

        private Vector2 moveDirection;
        private float lookX;
        private float lookY;

        private float minY = -60f;
        private float maxY = 60f;
        private int layerMask;
        
        private UIEventDispatcher uiEventDispatcher;

        public float Health => health;


        public void InitDependencies(UIEventDispatcher uiEventDispatcher)
        {
            this.uiEventDispatcher = uiEventDispatcher;
        }
        
        private void Start()
        {
            this.layerMask = 1 << 8;
        }

        private void Update()
        {
            if (health > 0)
            {
                Move();
                Rotate();
            }
            else
            {
                uiEventDispatcher.PlayerKilled();
            }
        }

        private void Rotate()
        {
            lookY = Mathf.Clamp(lookY, minY, maxY);
            transform.localEulerAngles = new Vector3(lookY, lookX, 0);
        }

        private void Move()
        {
            Vector3 movement =
                (transform.forward * this.moveDirection.y + transform.right * this.moveDirection.x)
                * playerSpeed;

            characterController.Move(movement);
        }

        public void OnFire(InputAction.CallbackContext ctx)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
            {
                Debug.Log("Hit thing");
                hit.collider.gameObject.GetComponent<EnemyAI>().Damage();
            }
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            this.moveDirection = ctx.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext ctx)
        {
            this.lookX += ctx.ReadValue<Vector2>().x * sensitivity.x;
            this.lookY -= ctx.ReadValue<Vector2>().y * sensitivity.y;
        }

        private void OnCollisionEnter(Collision other)
        {
            Damage();
        }

        public void Damage()
        {
            health -= 5;
        }
    }
}