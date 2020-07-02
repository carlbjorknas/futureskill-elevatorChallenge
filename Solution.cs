using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge
{
    public class Solution : ISolution
    {
        ElevatorBase _elevator;

        public Solution()
        {
            _elevator = new FullUpAndDownAndOpensOnlyWhenSomeoneWantsOffOrOnAndThereIsCapacityAvailableElevator();
        }

        public void Update()
        {
            _elevator.Update();
        }
    }

    // occupants entering the elevator all have a weight between 60-100 kilograms.
    // Note that floor 0 is the bottom floor.

    public abstract class ElevatorBase
    {
        protected const int _weightOfLightestRider = 60;
        protected const int _weightOfHeaviestRider = 100; 
        protected int _currentFloor = 0;
        protected int _numberFloors;
        protected bool _hasOpenedDoorOnFloor = false;

        public ElevatorBase()
        {
            _numberFloors = API.GetFloorCount();
            Log($"Floor count: {_numberFloors}");
        }

        protected void MoveUp()
        {
            Log("--> Moving up.");
            API.MoveUp();
            _currentFloor++;
            _hasOpenedDoorOnFloor = false;
        }

        protected void MoveDown()
        {
            Log("--> Moving down.");
            API.MoveDown();
            _currentFloor--;
            _hasOpenedDoorOnFloor = false;
        }

        protected void OpenDoor()
        {
            Log("--> Opening door.");
            API.OpenDoor();
            _hasOpenedDoorOnFloor = true;
        }

        public void Update()
        {
            Log($"Buttons pressed: {ButtonsPressed}");
            Log($"Weight: {API.GetCurrWeight()} / {API.GetElevatorCapacity()}");
            Log($"Level: {_currentFloor}");            
            InternalUpdate();
        }

        protected abstract void InternalUpdate();

        string ButtonsPressed { 
            get {
                var pressedButtons = Enumerable.Range(0, _numberFloors)
                    .Select(floor => API.GetBtnPressedStatus(floor) ? floor : -1)
                    .Where(floor => floor >= 0)
                    .Select(floor => floor.ToString())
                    .ToArray();
                return string.Join(", ", pressedButtons);
            }
        }

        protected bool AtTopFloor => _currentFloor == _numberFloors - 1;
        protected bool AtBottomFloor => _currentFloor == 0;
        protected bool RiderWantsOffAtCurrentFloor => API.GetBtnPressedStatus(_currentFloor);
        protected bool HasCapacityForLightRider => API.GetElevatorCapacity() - API.GetCurrWeight() >= _weightOfLightestRider;
        protected bool RiderWaitingOnCurrentFloor => API.GetDownBtnStatus(_currentFloor) || API.GetUpBtnStatus(_currentFloor);

        protected void Log(string message)
            => Console.Out.WriteLine(message);
    }

    public class FullUpAndDownAndOpensOnlyWhenSomeoneWantsOffOrOnAndThereIsCapacityAvailableElevator : ElevatorBase
    {
        bool _goingUp = true;

        protected override void InternalUpdate()
        {
            if (!_hasOpenedDoorOnFloor && (RiderWantsOffAtCurrentFloor || (HasCapacityForLightRider && RiderWaitingOnCurrentFloor)))
            {
                OpenDoor();
                return;
            }

            if (_goingUp && AtTopFloor)
                _goingUp = false;

            if (!_goingUp && AtBottomFloor)
                _goingUp = true;

            if (_goingUp)
            {
                MoveUp();
            }
            else
            {
                MoveDown();
            }
        }
    }

    public class FullUpAndDownOpenAlwaysElevator : ElevatorBase
    {
        bool _goingUp = true;

        protected override void InternalUpdate()
        {
            if (!_hasOpenedDoorOnFloor)
            {
                OpenDoor();
                return;
            }
            
            if (_goingUp && AtTopFloor)
                _goingUp = false;

            if (!_goingUp && AtBottomFloor)
                _goingUp = true;

            if (_goingUp)
            {
                MoveUp();
            }
            else
            {
                MoveDown();
            }
        }
    }
}
