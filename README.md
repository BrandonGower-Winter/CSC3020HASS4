# CSC3020HASS4 by Brandon Gower-Winter
Assignment 4 for CSC3020H

Controls:

WASD for movement controls.
Mouse for camera rotation
Left click to shoot.
c - change camera
g & h are used to ajust height in the 3rd person and orbit cameras;

Run in unity or the executable included in the submission.

Player Model Setup and Movement:
	Character holds a gun
	Can move with WASD
	Animations are included

Cameras:
	1st 3rd and orbit cameras.
	Can adjust height and distance
	Cameras can be interchanged with 'c'

Environment Setup and Physics:
	3 different types of 'ai' models are available. They are assigned random textures and death sounds and are randomly spawned across the level. They will wonder around aimlessly
	Shoot with left mouse button and kill them.
	Static models are also present to decorate the level.
	Ai and static objects are all collidable with the player.
	
Raycasting:
	A raycast is used to determine if a player has hit an 'enemy' ai.
	A hit will destroy the creature.

Visual Effects:
	The level contains various lights (Point, spot and a directional light) The point and spot lights have random colors (Except the police light)
	The directional light gives the atmosphere.
	All objects are textures. (The ai have a base texture and random texture strung onto them giving each 'enemy' some uniqueness).
	Particle effects are used by the gun to represent a muzzle flash.

Sounds:
	Background music and sounds effects are included.
	
All assets were taken from the unity asset store and are not my own, however all code present is my own.
