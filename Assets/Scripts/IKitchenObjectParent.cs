using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 接口，放置厨房物品的容器，如桌面和玩家的手中
/// </summary>
public interface IKitchenObjectParent
{
    /// <summary>
    /// 放置物品的位置
    /// </summary>
    /// <returns></returns>
    public Transform GetKitchenObjectFollowTransform();
    
    /// <summary>
    /// 设置容器上的物体
    /// </summary>
    /// <param name="kitchenObject"></param>
    public void SetKitchenObject(KitchenObject kitchenObject);

    /// <summary>
    /// 获取容器上的物品
    /// </summary>
    /// <returns></returns>
    public KitchenObject GetKitchenObject();

    /// <summary>
    /// 清空容器上的物品
    /// </summary>
    public void ClearKitchenObject();

    /// <summary>
    /// 容器上是否有物品
    /// </summary>
    /// <returns></returns>
    public bool HasKitchenObject();
}
