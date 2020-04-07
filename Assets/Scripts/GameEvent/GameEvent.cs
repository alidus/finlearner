using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(menuName = "SO/GameEvents/GameEvent", fileName = "GameEvent")]
public class GameEvent : ScriptableObject
{
    [SerializeField]
    private string title;
    [SerializeField]
    private string description;
    [SerializeField]
    private List<StatusEffect> statusEffects = new List<StatusEffect>();

    public string Title { get => title; set => title = value; }
    public string Description { get => description; set => description = value; }
    public List<StatusEffect> StatusEffects { get => statusEffects; set => statusEffects = value; }

    public void Execute(MonoBehaviour gameEventsContainer)
    {
        HintsManager.instance?.ShowHint(title, description);
        foreach (StatusEffect statusEffect in StatusEffects)
        {
            StatusEffectsManager.instance?.ApplyStatusEffects(statusEffect);
        }
    }

    public class GameEventBuilder
    {
        GameEvent gameEvent;
        public GameEventBuilder()
        {
            New();
        }

        public GameEventBuilder New()
        {
            gameEvent = new GameEvent();
            return this;
        }

        public GameEventBuilder SetTitle(string title)
        {
            gameEvent.Title = title;
            return this;
        }

        public GameEventBuilder SetDescription(string description)
        {
            gameEvent.Description = description;
            return this;
        }

        public GameEventBuilder AddStatusEffect(StatusEffect statusEffect)
        {
            gameEvent.StatusEffects.Add(statusEffect);
            return this;
        }

        public GameEvent Build()
        {
            return gameEvent;
        }
    }
}