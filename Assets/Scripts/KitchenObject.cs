using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    //物体当前所处的容器
    private IKitchenObjectParent _kitchenObjectParent;

    //厨房物品的ScriptableObject
    public KitchenObjectSO GetKitchenObjectSO() => kitchenObjectSO;

    //获取当前所处的容器
    public IKitchenObjectParent GetKitchenObjectParent() => _kitchenObjectParent;

    /// <summary>
    /// 设置物体当前所处的容器
    /// </summary>
    /// <param name="kitchenObjectParent">新容器</param>
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (_kitchenObjectParent != null)
        {
            //旧桌子清空物品
            _kitchenObjectParent.ClearKitchenObject();
        }

        //设置新桌子
        _kitchenObjectParent = kitchenObjectParent;
        if (_kitchenObjectParent.HasKitchenObject())
        {
            //新桌子已经有物品
        }

        kitchenObjectParent.SetKitchenObject(this);

        //获取新桌子的放置点
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();

        //更新模型位置
        transform.localPosition = Vector3.zero;
    }
}