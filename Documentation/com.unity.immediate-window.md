# Immediate Window

The immediate window is used to instantly evaluate C# code inside the Unity Editor.

## Installation

It is currently only available as a *preview package*.

Add ```"registry": "https://staging-packages.unity.com",``` to
your project manifest and install from the *Package Manager Window*.

Alternatively, simply ```git clone git@gitlab.internal.unity3d.com:mathieur/com.unity.immediate-window.git```
in your project's *Packages* folder and you're good to go!

The window is accessible through the ```Window/Analysis/Immediate Window``` editor menu.

## Overview

The immediate window lets you run code and inspect returned object and their properties. It's not a tool aim at developement but rather
debug scenarios or editor API discovery.

## Usage

### Creating your own type view

Simply create a class that implements `ITypeView` and you're good to go! Inspection on your type will automatically be using your new view.

You also inherits form `ATypeView` which already provides a default implementation of most detail methods. You really only need to implement `GetView` in this case.

# FAQ

###### Q: This is an overcomplicated design and too hard to use
*A: [Pull Requests](https://gitlab.internal.unity3d.com/mathieur/com.unity.immediate-window) are welcomed, whinning is not!*

###### Q: You rock!
*A: Don't we all?*
