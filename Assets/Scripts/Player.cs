using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    //限制事件参数为OnSelectedCounterChangedEventArgs，其包含了当前面对的桌子对象
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        //参数为当前面对的桌子
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;

    [Header("拿着物品的位置")] [SerializeField] private Transform kitchenObjectHoldPoint; //玩家拿着物品的坐标点

    private bool _isWalking;
    private Vector3 _lastInteractDir;
    private BaseCounter _selectedCounter; //当前玩家所在(面对)的桌子
    private KitchenObject _kitchenObject; //玩家当前拿着的物品

    public bool IsWalking => _isWalking;

    //放置物体的位置
    public Transform GetKitchenObjectFollowTransform() => kitchenObjectHoldPoint;

    //设置桌子上的物体
    public void SetKitchenObject(KitchenObject kitchenObject) => _kitchenObject = kitchenObject;

    //获取桌子上的物品
    public KitchenObject GetKitchenObject() => _kitchenObject;

    //清空桌子上的物品
    public void ClearKitchenObject() => _kitchenObject = null;

    //桌子上是否有物品
    public bool HasKitchenObject() => _kitchenObject != null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void OnDisable()
    {
        gameInput.OnInteractAction -= GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
        {
            //按下互动按钮后，面前有可互动的桌子，进行互动
            _selectedCounter.Interact(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //移动控制
        HandleMovement();
        
        //互动控制
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 inputVec = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVec.x, 0, inputVec.y);
        if (moveDir != Vector3.zero)
        {
            //根据移动方向，判断当前面对的方向，不移动时，方向为最后一次移动的方向
            _lastInteractDir = moveDir;
        }

        float interactDistance = 2f;

        //使用射线检测当前面前有没有桌子
        if (Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit hit, interactDistance,
                counterLayerMask))
        {
            if (hit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != _selectedCounter)
                {
                    //更新玩家目前面对的桌子
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                //玩家面前的不是桌子
                SetSelectedCounter(null);
            }
        }
        else
        {
            //玩家面前没有桌子
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVec = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVec.x, 0, inputVec.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius, moveDir, moveDistance);

        _isWalking = moveDir != Vector3.zero;

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
    }

    private void SetSelectedCounter(BaseCounter clearCounter)
    {
        _selectedCounter = clearCounter;
        OnSelectedCounterChanged?.Invoke(this,
            new OnSelectedCounterChangedEventArgs { selectedCounter = _selectedCounter });
    }
}