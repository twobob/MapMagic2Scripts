# **MapMagic2Scripts**
Useful (mainly generator) scripts for MapMagic version > 2.1.8, 
>   Requires the paid-for app 'MapMagic2' ; Also the 'Spline', and 'Object', addons where noted. 


**STATE OF DEVELOPMENT: Utterly alpha. These are barely tested, never mind battle-hardened. Pulls welcomed.**


## **_Output_**

### Hole
Provides a way to directly pass a regular map output to be parsed like :  **HOLE** _1_ (White) > (BLACK) _0_ **TERRAIN** 

SO THAT MEANS: any value > `float.Epsilon` results in a hole.  Multiple instances of the output are now supported.


_The Holes output eliding huge sections of terrain_
![image](https://user-images.githubusercontent.com/915232/134504021-4a905a33-db3c-458e-b0da-ee493cf748f2.png)


##  **_Controls_**  **MAP**

AceDisplay - simple - tiny - fast display that also clamp01's

Invert - invert the data so it is effectively outlined (see picture)

MaskToZero - Just like the regular mask except it provides an internal null input that reduces the complexity of operation

##  **_Controls_**  **SPLINE** (Needs Den.Tools.Spline addon)

Subdivide - Utility control to increase segment count in input splines by factor to output

WigglerMini - adds a set of random offsets into an input set of splines 

Bendy - (created To follow Wiggler) - or other inputted splines. Accentuates Tangent. Increases segment count by factor.

Clamp - Utility to ensure splines are indeed clamped to the Active or Full Area - or not at all.

_A few simple straight lines being turned into complex system with these controls_
![image](https://user-images.githubusercontent.com/915232/134523551-e3e6cd56-2761-4860-9f72-9a6cc123b665.png)


##  **_Controls_**  **OBJECT**

NodeToObject - Converts every node on a spline to an object. No, it does not respect the orientation of the original spline.

##  **_Helpful Other Monobehaviours and classes_**

**RandomGen - A required helper script for the controls** that creates a thread safe random using cryptography  

LerpToGround - helper to jump objects to ground

LerpToGroundAfterDelay - same as above but with delay

---

CAN I INCLUDE THIS AS A PACKAGE?  No, despite it being ready to be delivered as one, Unity has not yet implemented the ability to reference an Asset Store package via UPM packages.
When that changes? We are ready.

---

## SUPPORT THIS DEVELOPER: https://paypal.me/Geekwife     
###  A kind soul has agreed to collect any money from other nice people that want to support more scripts like these and time spent making these solid / better
