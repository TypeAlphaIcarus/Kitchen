using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ClearCounter : BaseCounter, IInteractable, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    //当前桌子摆放的物体
    private KitchenObject _kitchenObject;

    //放置物体的位置
    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;

    //设置桌子上的物体
    public void SetKitchenObject(KitchenObject kitchenObject) => _kitchenObject = kitchenObject;

    //获取桌子上的物品
    public KitchenObject GetKitchenObject() => _kitchenObject;

    //清空桌子上的物品
    public void ClearKitchenObject() => _kitchenObject = null;

    //桌子上是否有物品
    public bool HasKitchenObject() => _kitchenObject != null;

    /// <summary>
    /// 进行互动
    /// </summary>
    /// <param name="player"></param>
    public override void Interact(Player player)
    {
        Debug.Log($"和 {name} 互动");
        if (_kitchenObject == null)
        {
            //桌子上没有东西，新生成一个
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);

            //设置新物品所在桌子
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        }
        else
        {
            //桌子上有东西，拿走，将物品给玩家
            _kitchenObject.SetKitchenObjectParent(player);
        }
    }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }
}