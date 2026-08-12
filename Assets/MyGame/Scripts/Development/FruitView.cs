using Core.Fruits;
using UnityEngine;

namespace Development
{
    public class FruitView : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private CircleCollider2D hitCollider;
        public FruitTier Tier { get; private set; }
        public bool IsMerging { get; private set; }

        public virtual void ApplyMetadata(FruitMetadata fruitMetadata)
        {
            if (fruitMetadata == null)
            {
                Debug.LogError("FruitMetadata is null.", this);
                return;
            }

            Tier = fruitMetadata.tier;

            if (body != null)
            {
                body.mass = fruitMetadata.mass;
            }

            if (hitCollider != null)
            {
                hitCollider.radius = fruitMetadata.radius;
            }
        }

        public void SetTier(FruitTier tier)
        {
            Tier = tier;
        }

        public void Merging() => IsMerging = true;
    }
}
