Joystick v1.0 - 8/20/2015

Features:
- Accessable via "GameObject/UI/Joystick" menu 
- Round, square, horizontal and vertical types
- Configurable initial handle position 
- Configurable auto return speed 
- Optional dead zone radius around initial handle position 
- Optional dead zone magnet

Handle is a joystick's handle object
Type is one of Round, Square, Horizontal and Vertical joystick types
InitialPosition is default handle position
AutoReturnSpeed is a speed, that will be used to move handle back after pointer is up
Radius is a maximum distance from joystick's center, that handle may be moved
DeadZoneMagnet makes handle go to it's initial position if pointer is within DeadZoneRadius
DeadZoneRadius is a distance from handle's initial position, within which joystick's handle coordinates are represented as zero