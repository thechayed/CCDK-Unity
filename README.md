# Chayed Creates Development Kit-Unity

  ![enter image description
       here](https://media0.giphy.com/media/N3DdJvRCQxkEpZpO9V/giphy.gif)

The CCDK-Unity is a Toolkit that simplifies development of any kind of game by abstracting many elements into just a few Bolt nodes and Scriptable Objects, while still allowing for more complex extension through C#. 

While I will be adding a lot to this toolkit for my own use with my game Byte Sized Heroes, this may not be the best backend for your game's development. I will be created some Documentation on using it after I feel like it's at a solid enough point for total game creation.

Most of it's design is inspired by the Unreal Development Kit.

# Current Features
- Runtime State Machine easily separates global data and code
- Simple Level managment allows switching between scenes with ease, by using only a scene's name
- Easy Playable Pawn Construction
- Control Pawns through Controllers
- Objects and their components have unique States, seperating functionality into cleaner and more efficient code.
- Designed to be used with free Unity tools, no purchases necessary 

## Pawns
Pawns are basically Characters the can be possessed by a Controller. The Pawn decides what to do based on what was communicated to it from its Controller. A pawn could be a player character, a Vehicle, or anything you can think of that takes input from a Player or AI to interact with the world. There are 5 pawn classes that are generated for a Pawn:
- Movement: This controls the generation and control of necessary components for Movement.
- Inventory: This contains information about what items the Pawn is holding, and can hold, as well as how it happens.
- Audio: This controls how sound is emitted from the Pawn
- Costume: This controls the Mesh, Sprite, or any other visual data of the pawn
- Life: This controls statistics related to health, dying status and other things such as magic use or stamina

## Controllers
There are two types of Controllers, Player Controllers and AI Controllers. The only difference between the two is fairly obvious. One is the Players existence in the game world, and defines the ways a player can interact with it. The other does the same for AI.
