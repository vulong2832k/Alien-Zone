using System.Collections.Generic;
using UnityEngine;

public class ScreenEffectManager : MonoBehaviour
{
    public static ScreenEffectManager Instance { get; private set; }

    [System.Serializable]
    public class EffectEntry
    {
        public ScreenEffectType type;
        public ScreenEffectBase effect;
    }

    [SerializeField] private List<EffectEntry> _effects;

    private Dictionary<ScreenEffectType, ScreenEffectBase> _effectMap;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _effectMap = new Dictionary<ScreenEffectType, ScreenEffectBase>();
        foreach (var entry in _effects)
        {
            if (entry.effect != null)
                _effectMap[entry.type] = entry.effect;
        }
    }

    public void Play(ScreenEffectType type, float duration = 0.2f)
    {
        if (_effectMap.TryGetValue(type, out var effect))
        {
            effect.Play(duration);
        }
    }
}
