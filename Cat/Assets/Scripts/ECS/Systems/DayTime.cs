using Client;
using Components;
using Leopotam.EcsLite;
using UnityEngine;
using EventManager;

namespace Systems
{
    sealed class DayLight : IEcsInitSystem, IEcsRunSystem
    {
        private int dayTimeEnitity;
        private float maxTime = 301f;

        private float GetCurrentDuration (DayTimeState state, Components.DayTime dayTime) {
            switch (state) {
                case DayTimeState.DAY:
                    return dayTime.dayDuration;
                case DayTimeState.NIGHT:
                    return dayTime.nightDuration;
                default:
                    return 30;
            }
        }

        public void Init(EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            dayTimeEnitity = world.NewEntity();

            var dayTimePool = world.GetPool<Components.DayTime>();

            dayTimePool.Add(dayTimeEnitity);
            
            ref var dayTimeComponent = ref dayTimePool.Get(dayTimeEnitity);

            dayTimeComponent.currentTime = 0;
            dayTimeComponent.nightDuration = 60;
            dayTimeComponent.dayDuration = 20;
        }
        public void Run(EcsSystems systems) {
            EcsWorld world = systems.GetWorld();
            var dayTimePool = world.GetPool<Components.DayTime>();

            ref Components.DayTime dayTimeComponent = ref dayTimePool.Get(dayTimeEnitity);

            float currentDuration = GetCurrentDuration(DayTimeEventManager.CurrentDayTimeState, dayTimeComponent);

            if (dayTimeComponent.currentTime + Time.deltaTime > currentDuration) {
                dayTimeComponent.currentTime = 0;
                DayTimeState currentState = 
                    DayTimeEventManager.CurrentDayTimeState == DayTimeState.DAY 
                    ? DayTimeState.NIGHT 
                    : DayTimeState.DAY;

                DayTimeEventManager.ChangeDayTime(currentState);
            }
            else {
                dayTimeComponent.currentTime += Time.deltaTime;
            }

            LightRotation(world, dayTimeComponent);

        }

        private void LightRotation(EcsWorld world, DayTime dayTime) {
            var filter = world.Filter<Components.LightRotator>().End();
            var lightRotatorPool = world.GetPool<Components.LightRotator>();
            
            foreach (var entity in filter) {
                ref Components.LightRotator lightRotator = ref lightRotatorPool.Get(entity);
                float time = dayTime.currentTime / lightRotator.timeToEnd;
                float curRotation = Mathf.Lerp(lightRotator.startAngle, lightRotator.endAngle, time);
                lightRotator.lightTransform.rotation = Quaternion.Euler(curRotation, -30, 0);
            }
        }
    }
}
