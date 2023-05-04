using UnityEngine;
using System.Linq;
using EventManager;

namespace Assets.Scripts.Controllers.Objects.Fridge {

    public class InteractiveFridgeController : InteractiveObjectClass {

        [SerializeField] private uint maxAvailableInteractions = 10;
        private uint currentAvailableInteractions = 10;

        private void Awake() {
                _view = GetComponentInChildren<InteractiveFridgeView>();

                _view?.UpdateCurrentProductCount(currentAvailableInteractions);
                _view?.UpdateMaxProductCount(maxAvailableInteractions);
        }

        // void DayTimeChanged(DayTimeState state) {
        //     Debug.Log(state);
        // }

        private InteractiveFridgeView _view;

        public override bool Interact() {
            if (currentAvailableInteractions > 0) {
                currentAvailableInteractions--;
                _view?.UpdateCurrentProductCount(currentAvailableInteractions);
            } else {
                return false;
            }
            
            return true;
        }

        public override bool IsInteractionAvailable()
        {
            return currentAvailableInteractions > 0;
        }
    }
}