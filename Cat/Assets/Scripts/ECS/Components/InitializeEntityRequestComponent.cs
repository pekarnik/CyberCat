using System;

namespace Assets.Scripts.ECS.Components
{
    [Serializable]
    public struct InitializeEntityRequestComponent
    {
        public EntityReference EntityReference;
    }
}