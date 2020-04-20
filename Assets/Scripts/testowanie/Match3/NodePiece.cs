using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int value;
    public Point index;

    [HideInInspector]
    public Vector2 pos;
    [HideInInspector]
    public RectTransform rect;

    bool updating;
    Image img;

    public void Initialize(int v, Point p, Sprite piece)
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        value = v;
        SetIndex(p);
        img.sprite = piece;
    }

    public void SetIndex(Point p)
    {
        index = p;
        ResetPosition();
        UpdateName();
    }
    
    public void ResetPosition()
    {
        pos = new Vector2(Match3.tileSize/2 + (Match3.tileSize * index.x), -(Match3.tileSize/2) - (Match3.tileSize * index.y));
    }

    public void MovePosition(Vector2 move) // bierze kierunek i przemieszcza w tym kierunku
    {
        rect.anchoredPosition += move * Time.deltaTime * 16f;
    }
    public void MovePositionTo(Vector2 move)    // bierze pozycje i przemieszcza do tej pozycji
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, move, Time.deltaTime * 16f);
    }

    void UpdateName()
    {
        transform.name = "Node [" + index.x + ", " + index.y + "]";
    }

    public bool UpdatePiece()
    {
        if(Vector3.Distance(rect.anchoredPosition, pos) > 1)
        {
            MovePositionTo(pos);
            updating = true;
            return true;
        }
        else
        {
            rect.anchoredPosition = pos;
            updating = false;
            return false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (updating) return;
        MovePieces.instance.MovePiece(this);
    }

    
    public void OnPointerUp(PointerEventData eventData)
    {
        MovePieces.instance.DropPiece();
    }
}
