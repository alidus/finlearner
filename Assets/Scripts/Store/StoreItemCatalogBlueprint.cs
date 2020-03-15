using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "StoreItemCatalogBlueprint", menuName = "ScriptableObjects/Store/Catalog")]
public class StoreItemCatalogBlueprint : ScriptableObject
{
    public List<StoreItemBlueprint> storeItemBlueprints = new List<StoreItemBlueprint>();
    
    [System.Serializable]
    public class StoreItemBlueprint
    {
        [SerializeField]
        private string name;
        public string Title
        {
            get { return name; }
            set { name = value; }
        }
        [SerializeField]
        private string desc;
        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }

        [SerializeField]
        private ItemCategory category = ItemCategory.Misc;
        public ItemCategory Category
        {
            get { return category; }
            set { category = value; }
        }
        [SerializeField]
        private ItemType type = ItemType.Other;
        public ItemType Type
        {
            get { return type; }
            set { type = value; }
        }
        [SerializeField]
        private List<StatusEffect> statusEffects;

        [SerializeField]
        private Sprite sprite;
        public UnityEngine.Sprite Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }
        [SerializeField]
        private ItemPurchaseBehaviour purchaseBehaviour;
    }
}
