using Unity.Entities;

namespace DefenseGame
{
    public struct GameOverTag : IComponentData
    {
        public float experienceGained;
        public int levelReached;
    }
}
