
Xbox 360 Controller Plugin for Unity
By Dan Buckstein
(c) 2013


I developed this plugin for use with the Unity 3D engine.

The DLL in this package should be placed in your project's plugins folder. The C# scripts can be placed anywhere in your project's assets and resource directories.


How it Works: 

Xbox360ControllerPlugin.cs: This is the class that interfaces with the plugin. It is a completely static class, so it can be referenced from pretty much anywhere. However, there is another script which implements a tool to make things a bit easier to read...

Xbox360Controller.cs: This class can be applied to objects to make a controller behaviour. The controller object has variables and facade functions for everything in the plugin interface. The only difference now is that calling these functions will take care of managing the correct controller. 

When this script is applied to an object, the only thing to set is the player index: 0 is for player 1, 1 for player 2, etc. (yes it's kinda silly... but I'm a programmer and it could be worse). The default value is 0. 

Any class that references a controller behaviour can check button states: Button Pressed/Released (returns true on the frame that the button is pressed/released), Is Button Down/Up (returns true if this is the state of the button), with a button variable (also defined in the class) as an argument to each of these functions.

The joystick and trigger values are returned as normalized floats in the range -1 to 1 on each axis. By default, this does not account for the dead-zones associated with each axis, so the user can tell the plugin to "Enable LeftJoystick/RightJoystick/Trigger Clamp", which will then force the joysticks and trigger to return zero while in their resting states.

There is also rumble!  Just call SetRumble to activate the left and right motors. Note that the left motor is for slower rumbles and the right motor is faster.

Other than that...

****NOTE: If you want to be certain about the player order when dealing with multiple controllers, unplug all controllers; the order in which you plug them in again will determine the player order!

****ALSO NOTE: If you are interested in using the Xbox 360 Racing Wheel controller, the plugin will still work. However, this controller does not have a dead-zone, so the user should not enable the joystick deadzones!


Thanks for being interested in this plugin! I hope you enjoy !  :)
Happy development!
- DB
