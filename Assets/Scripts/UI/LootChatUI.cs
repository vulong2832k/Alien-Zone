using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootChatUI : MonoBehaviour
{
    public static LootChatUI Instance;

    [SerializeField] private Transform _content;
    [SerializeField] private LootChatItemUI _itemPrefab;

    [SerializeField] private int _maxLine = 4;
    [SerializeField] private float _lifeTime = 15f;

    private Queue<LootChatItemUI> _items = new Queue<LootChatItemUI>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddMessage(string msg)
    {
        if (this._items.Count >= this._maxLine)
        {
            var oldItem = this._items.Dequeue();
            Destroy(oldItem.gameObject);
        }

        var item = Instantiate(_itemPrefab, _content);
        item.SetText(msg);

        this._items.Enqueue(item);

        StartCoroutine(RemoveAfterTime(item));
    }

    private IEnumerator RemoveAfterTime(LootChatItemUI item)
    {
        yield return new WaitForSeconds(_lifeTime);

        if (this._items.Contains(item))
        {
            var newQueue = new Queue<LootChatItemUI>();

            foreach (var i in _items)
            {
                if (i != item)
                    newQueue.Enqueue(i);
            }

            this._items = newQueue;
            Destroy(item.gameObject);
        }
    }

}
