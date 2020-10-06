using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOverlayImage : MonoBehaviour
{
    private SpriteRenderer spriteBase;

    private int baseInitialSortingOrder;

    public void SetSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        spriteBase = spriteRenderer;
        baseInitialSortingOrder = spriteBase.sortingOrder;
    }

    public void SetSortingOrder(int sortingLayer, int sortingOrderOffset)
    {
        spriteBase.sortingLayerID = sortingLayer;
        spriteBase.sortingOrder = baseInitialSortingOrder + sortingOrderOffset;
    }
}
