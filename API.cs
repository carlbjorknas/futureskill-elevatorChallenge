using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge
{
    public static class API
    {
        public static void OpenDoor() { }
        public static int GetFloorCount() => 0;
        public static bool GetUpButtonStatus(int floor) => true;
        public static bool GetDownBtnStatus(int floor) => true;
        public static bool GetBtnPressedStatus(int floor) => true;
        public static int GetCurrWeight() => 0;
        public static int GetElevatorCapacity() => 0;
        public static void MoveUp() { }
        public static void MoveDown() { }

    }
}
