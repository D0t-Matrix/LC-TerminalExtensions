## MatrixTermExtensions 1.1.2
Changes
	* Fixed issue where the logic to reset the cooldown for the Inverse Teleporter was nonfunctional.
	* Changed Lights commands to be back to a toggle state
		* Left alternate shorthand commands to force lights on or off.
	* Changed the expanded command for activating the Inverse Teleporter, as it was clashing with the shorthand to buy said Inverse Teleporter.
	* Added `enum LightState` to the Lights Command structure to clean up checking for if the lights are on or off.
		* Included adding logic to explicitly toggle state.
	* Removed `[CommandInfo()]` Attributes form alias commands (and cheat commands) to hide them from the Terminal help screen.

## MatrixTermExtensions 1.1.1
Changes
	* Updated to v`1.0.1` of LethalAPI's TerminalAPI
	* Added logic to prevent logic of certain commands when in space
		* all door commands
		* using the Inverse Teleporter

## MatrixTermExtensions 1.1.0
Changes
	* Cleaned up SDK setup of solution
	* Added Config file to turn the cheat commands off
		* Includes a sync to match host.

## MatrixTermExtensions 1.0.0
Initial Upload