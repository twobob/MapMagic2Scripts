**MapMagic2Scripts**
Useful (mainly generator) scripts for MapMagic version > 2.1.8, Requires the paid for app Map Magic; Also the Spline, and Object, addons where noted. 

**Holes _Output_**

Provides a way to directly pass a regular map output to be parsed like :  **HOLE** _0_ (BLACK) < _0.5f_ > (WHITE) _1_ **TERRAIN** 

_The Holes output eliding huge sections of terrain_
![image](https://user-images.githubusercontent.com/915232/134504021-4a905a33-db3c-458e-b0da-ee493cf748f2.png)


**_Control_**
**MAP**

AceDisplay - simple - tiny - fast display that also clamp01's

Invert - invert the data so it is effectively outlined (see picture)

**SPLINE** (Needs Den.Tools.Spline addon)

Subdivide - Utility control to increase segment count in input splines by multiplier to output

WigglerMini - adds a set of random offsets into an input set of splines 

Bendy - (created To follow Wiggler) - or other inputted splines. Accentuates EXISTING bezier, 
however if there is no Tangent data to accentuate it won't add Tangent itself. in addition to this tangent pass "Bendy" also adds "Wiggly" straight details pass.

Clamp - Utility to ensure splines are indeed clamped to the Active or Full Area - or not at all.

_A few simple straight lines being turned into complex system with these controls_
![image](https://user-images.githubusercontent.com/915232/134523551-e3e6cd56-2761-4860-9f72-9a6cc123b665.png)



**_Helpful Other Monobehaviours and classes_**

**RandomGen - A required helper script for the controls** that creates a thread safe random using cryptography  

LerpToGround - helper to jump objects to ground

LerpToGroundAfterDelay - same as above but with delay

