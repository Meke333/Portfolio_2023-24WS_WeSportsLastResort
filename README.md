We Sports: Last Resort
-by Mike Li, Alessia Brinkmann, Sven Bauer, Annabelle Räth
-----------------------------
#IMPORTANT: 
This is only a showcase of my contributions (mainly code) and thereforethis repository has only a fraction of the full project. 
The full project is in a private repository, whose access can and are only given by me personally. This is done for 
avoiding legal issues due to having licensed Unity Asset Store packages open on Github.


#Basic Information:
3rd semester game project for winter semester 2023-2024 in "Animation & Game" study course from the Darmstadt University of Applied Sciences 
"Hochschule Darmstadt" (h_da).
This GitLab project is now uploaded later onto my personal GitHub repository to display it as part of a portfolio.


#Summary:
A "Wii Sports Resort" parody game, using a "Wii Remote" with "Motion Plus Extension" and a "Wii Balance Board" on a Windows PC 
to fight off a zombie apocalypse, the player started, in a sunny and invigorating resort with nothing better than a swingable tennis racket. 
The Player must workout by taking steps on a Wii Balance Board and swinging the Wii Motion Plus controller to reach the goal in order 
to cure the zombies and save this resort. This Project uses the Unity Engine with Assets and software to make the Wii Hardware useable.


#How to view:
To view the project use the Unity Editor (Version: 2022.3.11f1).
There are prototype videos in 3), a walkthrough video in 2) and exhibition video showing the technology at work in 4).


#How to play:
Unfortunately the game is not really playable due to the fact that not only does it use Wii hardware, which is a pain to connect to
a Windows PC (especially since Windows 11), but it also requires the old non Wii U Wii Motes (TR-... serialnumber). But if anyone wants
to try to make it work: There is a step by step guide in the root folder of the repository and a link in 6).
  

#My Contribution:
Roles in the project:
- Game Programmer
- Game Designer
- Version Control Manager

My main work was being the Game Programmer, Game Designer and the Version Control Manager. But also small contributions 
in Design (Game Design: Game Loop, Mechanics, Zombie Variables; UX Layouting; Fade In/Out Animations (Game Over Screen, Tutorial UI))
and Technical Art (Player & Zombie Animators) Department. 

Contibution Showreel Video in 5).


#Skills learned:
- Wii Motion Controls & Balance Board Controls (with help with an Unity Asset, called "WiiBuddy" 
			& Dolphin Emulator's technology to connect Wii devices) with Wii Motion Plus & IR technology
- But also learned that using outdated and non PC-Controllers is frustrating and also not recommended!!!
- Programming Design Patterns, especially: Observer Pattern, Finite State Machine, Object-Pooling
- Unity's Cinemachine Camera
- Menu Work, UX Layouting (Main Menu, Game Over Screen, Win Screen, Buttons)
- "Multi-Scene" work for avoiding version control conflicts and also smoother transitions and adaptability of Additve Scene Loading
- More Version Control work


#Challenges:
More details to challenges are listed in the "IndividualProjectDocumentation.pdf"


#Relevant Assets:
We Sports Last Resort\Assets\Scripts\...
- ScriptableObjects\LoadSceneSetups.cs
- EnemyScripts\Zombies\Scripts\ZombieScript.cs
- EnemyScripts\Zombies\ZombieStateMachine.cs
- EnemyScripts\Zombies\ZombieStateVariableContainer.cs
- EnemyScripts\EnemyManagerScript.cs
- Effects\ParticleEffectManager.cs
- FigureCharacterController.cs
- PlayerParticleSystem.cs


#Links:
1) Trailer: 			https://youtu.be/QO1kZH53R94
2) Walkthrough Video:	https://youtu.be/IWFPtJjkcsE
3) Prototypes: 		https://youtu.be/FBrEXAnx15s
4) Exhibition Footage: 	https://youtu.be/Ljs8Tyc8Qiw
5) Contribution Showreel: 	https://youtu.be/0pAezGX8ueo
6) Game Build: 		https://drive.google.com/drive/folders/1_NFIdMTSlrTAEhdQtWLmLX5okjvEX1K4?usp=drive_link


#Software used:
- Unity Engine (2022.3.11f1)
- Blender
- Autodesk Maya
- GitLab
- Fork


#Credits:
- "C# / Unity Wii Remote API" from Flafla2: 	https://github.com/Flafla2/Unity-Wiimote
- "Multi Scene Setup" from svermeulen: 		https://gist.github.com/svermeulen/8927b29b2bfab4e84c950b6788b0c677
- "WiiBuddy" from BitLegit in Unity Asset Store: 	https://assetstore.unity.com/packages/tools/input-management/wiibuddy-4929

"Wii Sports Resort" and all the Hardware for the Nintendo Wii used in this project belong to Nintendo.
