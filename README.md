Deformable Body Physics for Unity 3D
by Conor Liddy

Contents:
1. Make3DArrayMass.cs 
2. VoxelCollider.cs  
3. SpringPlaneDemo.cs
4. SpringSystem.cs

Usage:
For use in Unity you can import the package which contains three (3) test scenes
You can also download the files indevidually

Mass Spring System / Deformable Body Physics System (WIP)

1. Apply the "SpringSystem.cs" script to the object within Unity that you wish to be "Deformable".
2. Ensure the object also has the following components:
	  - Mesh Filter
	  - Mesh Collider
	  - Rigidbody (If Physics are wanted)
3. If a sine function is what is wanted the select the "Sine Wave" boolean in the Unity Inspector window. You can then adjust the wave "Speed", "Height", and the "Plane Resolution" (this functions as the plane density)
      (Note: Using a Sine function diasables the collision detection functions)
4. If you want to make the object "Deformable" then select the boolean to do so.
5. Select if you want the deformation to be "Physics Based" or a set amount each time. 
6. Non-physics based deformation can be adjusted by the "Deformation Amount" float.
7. The strength of physics based deformations can be adjusted by the "Physics Multiplier" Float
      (Note: The strength of physics based deformation can also be affected by the mass of the colliding object)

3D Array Mass System (WIP)

1. Apply the "Make3DArrayMass.cs" script to the object you wish to have "voxelized".
2. Set the desired dimentions for the 3D Array Mass in the Unity inspector window.
3. Set the booleans for the object (E.g. Has Physics, Crumble)
4. Hit "Play" in the Unity editor to run the code. 



