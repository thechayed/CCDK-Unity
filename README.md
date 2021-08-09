# Chayed Creates Development Kit-Unity

  ![enter image description
       here](https://media.giphy.com/media/pXJvRVdjkpkws6tZGr/giphy.gif)

- To use the Toolkit, download the latest release and extract the .zip within your project folder.

The CCDK-Unity is a Toolkit that simplifies development of any kind of game by abstracting many elements into just a few Bolt nodes and Scriptable Objects, while still allowing for more complex extension through C#. 

While I will be adding a lot to this toolkit for my own use with my game Byte Sized Heroes, this may not be the best backend for your game's development. I will be created some Documentation on using it after I feel like it's at a solid enough point for total game creation.

It is intended to be a free alternative to paid tools like Game Creator.

# Current Features
- State Machine is simple and clean, allowing for an easier user experience
- Simple Level managment allows switching between scenes with ease, by using only a scene's name

- Pawns: Objects that interact with the Game World directly. They are meant to be controlled, either by a Player Controller or AI Controller.
- Controllers: Objects that represent AI/Player presence within the game. They communicate with the world, but do not interact with it directly. 

- AI: Comes with built in forms of AI, as well as AI built off of UnityNEAT. AI exist as Controllers, and can communicate with all faccets of the game. 
- Component Creater: Creates and Manages children Components within a Parent Component.
- Much more.

## Pawns
Pawns are basically Characters the can be possessed by a Controller. The Pawn decides what to do based on what was communicated to it from its Controller. A pawn could be a player character, a Vehicle, or anything you can think of that takes input from a Player or AI to interact with the world. There are 5 pawn classes that are generated for a Pawn:
- Input Handler: This is the class that recieves Input information from the Controller and communicates it to it's children.
- Movement: This controls the generation and control of necessary components for Movement.
- Inventory: This contains information about what items the Pawn is holding, and can hold, as well as how it happens.
- Audio: This controls how sound is emitted from the Pawn
- Costume: This controls the Mesh, Sprite, or any other visual data of the pawn
- Life: This controls statistics related to health, dying status and other things such as magic use or stamina

## Controllers
There are two types of Controllers, Player Controllers and AI Controllers. Both can possess Pawns and communicate with the world. The only difference between the two is fairly obvious. 
- Players: Gives the Player control to communicate with the Game World. 
- AI: Gives the same power to AI. This implementation allows AI, also, to generate game content on during Runtime. Data backends will be coming soon to allow the generation of Multiplayer content in real-time as well.
