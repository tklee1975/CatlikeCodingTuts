# 

This code sample is from  CatlikeCoding Tutorial 
https://catlikecoding.com/unity/tutorials/basics/mathematical-surfaces/

# Learning Notes:

- Using Delegate: 
- Simple Math Function 
- 2D Math Function
- From 2D to 3D

# Using Delegate
- @see GraphFunction.cs
- Binding Function and Enum 
  1. Define the delegate
  2. Define Array of delegates (matching the enum index)
    static GraphFunction[] functions = {
		PlaneY, SineFunction, ....
	};
  When use GraphFunction f = functions[(int)function];	



# Simple Math Function
- SineFunction:
    y = Sin(PI * (x + t))
- MultiSin:
    y = Sin(PI * (x + t)) + Sin(2*PI * (x + t)) / 2

#  2D Math Function 
- Sine 2D:  y = ( Sin(PI * (z + t)) + Sin(PI * (x + t)) ) * 0.5;
- Multi Sine 2D:  

# From 2D to 3D
- origin X / Z = > u, v
- turn u, v => x, y, z
- Circle: x=Sin(pi * u ), y = 0  z = Sin(pi *v)
- Circle: x=Sin(pi * u ), y = v z = Sin(pi *u)
