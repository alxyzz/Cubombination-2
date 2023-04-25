using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeModule 
{
    int TIME_LEFT; //time left before this segment explodes.
    int TIME_GAINED_ON_WIN; //if you win, you gain extra time.
    //if you lose, you lose the time you invested. seems fair.
    private string blurb = "Undefined module description."; //appears at the bottom of the screen to tell the player what to do
    string Description { get { return blurb; } }
    

    /// <summary>
    /// runs every beat in GameManager for each facet. ticks down the timer for each and checks if it has exploded.
    /// </summary>
    /// <returns></returns>
    public bool DoTick()
    {



        TIME_LEFT--;
        if (TIME_LEFT <= 0)
        {
            return true;
        }
        return false;
    }

    //replenishes the timer.
    void WonSegment()
    {
        TIME_LEFT += TIME_GAINED_ON_WIN;
    }


    

    /// <summary>
    /// method that will be overwritten for each module, drop it in and define what the input from the arduino controller will do. Tilt in this case refers to when the Tilt controls are switched to the game mode instead of the cube navigation mode
    /// </summary>
     void OnTiltLeft()
    {//these have to be detected by checking the movement axis of the rotation control and firing off either this one or the other one

    }

    /// <summary>
    /// method that will be overwritten for each module, drop it in and define what the input from the arduino controller will do. Tilt in this case refers to when the Tilt controls are switched to the game mode instead of the cube navigation mode
    /// </summary>
     void OnTiltRight()
    {

    }

     void OnPressButton()
    {

    }

}
