using UnityEngine;

namespace Interface
{
    public interface IHurtbox
    {
        void GetHit(Vector3 hitDirection);

        BlockReaction IsGettingBlocked(Vector3 hitDirection);
    }
}

public enum BlockReaction
{
    Hit,
    Blocked,
    Ignored
}
