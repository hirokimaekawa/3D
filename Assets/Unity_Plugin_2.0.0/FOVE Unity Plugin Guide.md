# Fove Unity Plugin

This plugin automates setting up a rendering interface to the Fove Compositor from within Unity via one simple prefab. It also exposes eye-tracking information in a Unity-native way via a static interface.

## Getting Started
1. Install the Fove runtime and make sure it's running.
2. Import the package into Unity.
3. Drag the _Fove Rig 2_ prefab from the `FoveUnityPlugin/Prefabs/` directory into your scene.
4. Press Play.

That's all you need to do for a simple setup. Of course, this only gets you the most basic setup, and there are many customizations you may want. We think we've figured out the majority of use cases, so check the rest of this guide before deciding you need to do something too extreme.

## What You Get
The Unity package adds a folder called _FoveUnityPlugin_ to your project. Within this folder, there are several items. We will go over them here.

* *Editor*: This folder contains the prebuilt DLL housing the editor scripts that we use, and it can safely be ignored.
* *Prefabs*: This folder contains a few prefabs for you to use to get started.
  * *Fove Interface* (Legacy): This is an old interface prefab, compatible with the old (<2.0) version of the plugin so that your projects don't break. We highly recommend that you not use this version for new projects.
  * *Fove Interface 2*: This is a sample prefab and the minimum requirement for running powering your Unity VR project with FOVE. This prefab can safely be dragged into a scene and it should "just work" for you. Note that this prefab will automatically set its local coordinates from the FOVE runtime, so _you cannot control directly where this object ends up_ in your scene. For that, you need to place the _Fove Interface 2_ as a child of another object and move the parent.
  * *Fove Rig* (Legacy): This is just such a hierarchy, but it's only here to prevent breaking users' projects from past versions. Please use _Fove Rig 2_ instead.
  * *Fove Rig 2*: This is simply an empty _GameObject_ with a _Fove Interface 2_ setup as a child. __This__ is the object you should probably be dragging into your scene unless you already have a similar setup for other VR samples. If you're just getting started, definitely just plop one of these dudes in your scene and go from there. If you want to change the origin point of the VR experience, you would move the top-level container object.
  * *FoveCameraExample* (Legacy): This is a prefab used in the legacy setup. Feel free to ignore it. It's really just a regular camera prefab, anyway.
* *Resources* (Legacy): This folder contains some shaders that were/are used with the legacy setup for displaying the rendered imaged to your screen. It's perfectly safe to ignore this folder. In fact, I won't even go into the two shaders that it contains -- that's how much you shouldn't worry about this guy. Go ahead and look, but it's really boring.
* *x86_64*: This folder contains required native libraries for FOVE to run. It's a standard name to indicate that these libraries are for 64-bit x86 machines.
  * *D3DCompiler_47.dll*: This is a Microsoft DLL, and is required for the current runtime to function on Windows 7. It is not required outside of Windows 7, but feel free to distribute it anyway. It's fairly small, and that's what it's made for.
  * *FoveClient.dll*: The magic library. Well, it's the FOVE library, anyway. This is just the publicly released client library that you can get off our website. We upgrade this with every release of the Unity plugin, as there may be new features which are specific to new client versions, so it's best to leave this guy alone. If you keep up-to-date with the Unity plugin, you will also stay caught up with the FoveClient library.
* *FoveClient_CSharp.dll*: This is the official managed C# bindings DLL. It essentially builds up a couple of C# classes which call into the C API bindings we expose in *FoveClient.dll*, and presents you the data and structs as close to raw as we can while still keeping it looking and feeling like healthy C#. If you are so inclined, feel free to use this directly, but you'll probably end up rewriting a lot of rather tedious code that we already wrote for you if you use the `FoveInterface2` behaviour.
* *FoveUnityPlugin.dll*: This contains the prebuild Unity behaviours that are at the core of the plugin. It's not a folder, but Unity lets you expand it like it is, so here's a description of what you'll find inside.
  * *FoveEyeCamera* (Legacy): This behaviour is automatically added to generated camera objects when using the legacy interface. You shouldn't have been using it before and you DEFINITELY shouldn't be using it now. Please ignore. In fact, just forget this exists.
  * *FoveInterface* (Legacy): Much like the similarly named *Fove Interface* prefab, this is only here to avoid breaking people's projects. Please ignore this. We will maintain it as long as possible, but make no promises.
  * *FoveInterface2*: This is the current only class you should be working with. It is the current standard for Unity/FOVE interaction. If you drag this onto an empty `GameObject` in your scene, you'll end up with what is functionally identical to the *Fove Interface 2* prefab. (Obviously without the prefab connection.) It requires a Camera component, FYI, but Unity will automatically create one for you. You could also drag this to the default Unity camera if you're just starting a new scene. That would work too. Note that we do still recommend placing your VR camera underneath (as a child of) another empty object so that you can control where the basis for the experience is rather than (0, 0, 0).
  * *FoveInterfaceBase*: Okay, you caught us. We extracted a bunch of shared functionality from the original `FoveInterface` into a base class that's shared between `FoveInterface` and `FoveInterface2`. You can't really do anything with this, since it's an abstract class and you can't drag an abstract behaviour onto game objects. But it's how we'll probably be able to keep your legacy interfaces up to date more easily in the future, and how we avoid copy-pasting a bunch of identical code all over the place, introducing bugs and making life difficult. So now you know.

---

## Upgrading from 1.3.1 (or before)
First of all, we took a lot of care to keep FoveInterface backwards compatible with previous versions. So if you are happy to keep things as-is, you should be able to just keep using your setup as before. Keep in mind, however, that new features may or may not be released for the legacy interface in the future. So if you are worried about having all the latest features, it might be worth upgrading your project.

There are a number of key major differences between version 1 and version 2. First and foremost is that `FoveInterface2` uses Unity's stereo rendering functionality to take advantage of some of the optimizations built into the engine for VR (things like only rendering shadow buffers once for the VR camera, or culling objects, etc...). What this means for you is that we no longer use prefabs for the camera, but rather the camera itself -- so any screen effects you have on the `FoveInterface2`'s game object 

Here are some simple steps to take to upgrade everything after importing the new version of the Unitypackage into your project:

0. This one won't likely affect many people, but because we created a new C# library for proper bindings, the old `Fove` namespace has moved to `Fove.Managed`, and we have adopted the same names in there that are used in the C++ SDK. Primarily this means that `Fove.FoveHeadset` becomes `Fove.Managed.FVR_Headset`; and `Fove.FoveCompositor` becomes `Fove.Managed.FVR_Compositor`. It likely that you weren't messing around with these guys, but if you were you'll have to make the changes accordingly. Most of the functionality within those classes has remained the same, though a few methods that *were* returning Unity-native objects now return FOVE-native objects, so keep an eye out.
1. Replace your FoveInterface behaviour with FoveInterface2 on any Game Objects in your scene and any prefabs. You should be able to simply drag the `FoveInterface2` behaviour out from the Assets view onto your existing _Fove Interface_ object(s), copy the shared settings by hand, and then delete the `FoveInterface` behaviour.
2. Make sure no scripts use static methods. `FoveInterface2` removes all obsolete static methods from `FoveInterface`. If you're still using some, you will need to add a reference to a `FoveInterface2` to your behaviour and then either link it in the inspector or find a `FoveInterface2` at runtime. (You are, of course, more than welcome to create your own static reference to a `FoveInterface2` behaviour instance if you know you will only have one, we just no longer enforce that patter ourselves.)
3. Change `FoveInterface` references in your scripts to `FoveInterface2`. This should be pretty easy to find, as once you delete the original FoveInterface behaviour, Unity is going to throw a fit about missing references when you try to run your project.
4. Reattach any missing hooks to `FoveInterface2`. If you've been following along with updates, you likely already had inspector-visible references to the `FoveInterface`. Now that those are `FoveInterface2`, you'll have to reattach the references.

And with that you should be good to go! If between that and reading this document you still have questions, hit us up on the forum: https://support.getfove.com/hc/en-us/community/topics

From here on, the guide assumes you are using `FoveInterface2` and not any legacy objects or classes.

---

## The `FoveInterface2` Inspector Panel
We tried to come up with helpful tooltips for each of the Inspector panel options, but they are admittedly a bit clunky, and you probably don't want multiple paragraphs of tooltip appearing for each item -- and who knows, maybe you are actually reading the docs to learn? (most people don't) -- so here's a bit more detail about each item in the Inspector for our interface component.

### World Scale
This number represents how many **units** there are in one meter in your game. By default, one "Unity unit" is one meter. That is, if you have a unity cube that's 1 unit long on each side, that cube is also 1m^3 (one cubic meter). Not every game uses this system, however. Have some examples:

* If you want each Unity unity to be 1cm, then you would set *World Scale* to 100 (meaning that there are 100cm in 1m).
* If you want each Unity unity to be 1ft, then you would set *World Scale* to 3.28084, which is the number of feet in a meter.
* If you want each Unity unit to be decameters (an uncommon but actually real measurement where each decameter is 10 meters), you would set *World Scale* to 0.1, indicating that one meter is 1/10th of a Unity unit in your world system.

We do this because eye separation is a large part of how we sense scale, and if you use units other than meters, we have to know what that scaling factor is so we can scale eye separation and head movement to still feel properly like you are human-size in the game world according to your intended scale.

### Client Uses...
The next three checkboxes are essentially hint flags which indicate what pieces you intend to use. Typically you would use all three (gaze, orientation, and position), and so the default is for all three to be selected. Technically this has the potential to not wind up certain systems if you aren't using them, reducing CPU load, but there is no guarantee and you probably should actually just forget I said that.

As an example for a case where you may not want to have all the attributes checked, if you have a constant in-place HUD overlay that you don't want to move as the player moves their head around, you may want to un-check all three. Or if you want to render a sky box, you wouldn't want to have position for sure, and you likely wouldn't want gaze on that interface, but I could see some fun effects having a skybox that responds to gaze...

### Skip Auto Calibration Check
This toggle tells the `FoveInterface2` component that you intend to handle checking for calibration yourself. See below for more details, but essentially you should want this to be checked in a finished product. However for some research projects or trade show demonstration versions you may prefer to check for calibration on ever instance of the program, and `FoveInterface2` will do so for you if you uncheck this toggle.

### Oversampling Ratio
Oversampling ratio determines the texel-to-pixel ratio when rendering to FOVE. This is important because the various shaders involved in transfering the images to the headset and making it look correct have a tendency to expand pixels near the center, make a 1-to-1 ratio looking kind of blocky and pixellated. We've picked a reasonable tradeoff between getting good performance and looking nice. But if your game requires more performance you can potentially decrease the oversampling ratio and get back a bit of perf at the cost of visual quality. We currently cap this between 0.01 (which looks just so awful) and 2.5 (which is probably overkill). If you need something else, let us know -- I'm very curious to hear your use case!

### Headset Overrides
This section is closed off by default because you typically wouldn't mess around with it. We use it promarily for prototyping and debugging. But since it's there, we decided to make it usable. So here goes.

#### Use Custom Position Scaling
This toggle indicates that you want to use the specified values below instead of the defaults.

#### Position Scales
Position scaling refers to the X/Y/Z values reported by the SDK, and how those map to the Unity engine. This is a direct multiplicative relationship between the values received by the SDK and the values set on the `FoveInterface2` game object's transform. The default is a direct pass through, but if (for whatever reason -- likely a desire to make your users sick) you want to change these values, you can adjust each axis individually to make, e.g., side-to-side movement exaggerated while front-to-back is scaled down or something.

#### Use Custom Eye Placement
Toggle this if you want the FOVE eyes to be somewhere other than where we think they should be, according to the three values below. If you don't check this box, none of the values below will be used.

#### Inter Ocular Distance
This is the distance between your eyes. Many people call this "interpupilary distance", which is a misnomer as the distance between your pupils actually changes quite frequently as you look at objects closer and farther away -- and most other headsets can't even SEE your eyes, so there's no way they could even know your IPD. And anyway, IPD doesn't need to be used to get pretty good results compared to IOD.

So if you want to feel like your eyes are unusually close together or far apart (or if they are in real life) you can change this value. Otherwise it will pull the value specified in the SDK.

#### Eye Height
The position of the eyes isn't necessarily the same as the position origin of the HMD. This value is how high the eyes are from the HMD's position origin. If you wanted to see how dizzy/sick you would feel if your eyes were suddenly above your head or below, you could change this value. It's trippy and uncomfortable and not recommended for the faint of heart.

#### Eye Forward
This value represents the forward-facing offset from the HMD origin to where your eyes sit. If you want to feel what it's like having your eyes either in front of or behind your head (and I can't recommend it) you could hard-code this value yourself. But you probably shouldn't inflict that on others.

### Compositor Options
There are a few possible options which may be useful to you once you start getting more involved in developing your game with regard to how each camera communicates with the compositor.

#### Layer Type
The FOVE compositor supplies you with one of three layer types you can render to: "Base", "Overlay", and "Diagnostic":

* Base: This layer is considered fully opaque, and should represent the core of your game experience. There is only allowed to be a single base layer, and if you request more than one, you'll get an error when running. But you should probably *have* one for most games.
* Overlay: You can have multiple of these (keep in mind that each layer incurs a performance toll on the overall system, so we highly recommend keeping the number of layers low). They are considered to have an alpha channel and will show the base layer through any transparent areas. These are good for HUDs and special scene effects, for example.
* Diagnostic: These layers are treated the same as Overlay layers in terms of translucency, but they are always drawn over/in front of the other layers. They can be useful if you want to display some debug information that will never be blocked by the gameplay.

Having layers can be pretty powerful, but also can cause problems, so please be responsible with how you use them.

#### Disable Timewarp
A little background:In an ideal, real-time, deterministic operating system, one could theoretically guarantee that all frames reached the HMD SDK before the HMD screen needed to refresh. These systems are very uncommon (and not generally considered useful for broad user consumption these days), and so even if your game runs quickly there's a good chance you'll end up with frame skipping every now and then. Time warp exists to catch those little skips and prevent them from becoming apparent to your users.

But if you are rendering an overlay that's attached to the camera, time warp during those little skips can actually make your supposedly-steady HUD appear to jump around rather a lot! For these instances, we have the *Disable Timewarp* toggle. If you're making a layer that's intended to stay in one place relative to the user's vision, definitely turn this on.

---

## FOVE Interface Customization
Out of the box, we think that the `FoveInterface2` class works pretty well. (Fun story, the *Fove Interface 2* prefab is literally just an empty object with a `FoveInterface2` and a Camera behaviour attached. If you want to be super low-level, you could add that yourself to any object you like.)

But here are some notes about what's going on and how you may want to customize the prefab for your own needs:

### A `FoveInterface2` instance is where it's at.
Without a `FoveInterface2` component, you can't take use any of our stuff, so it's kind of important.

You MUST instantiate `FoveInterface` by attaching it to a Unity GameObject in your scene. We have a bunch of options (and we'd like to think sensible defaults), but most/all of them are only modifiable via the inspector. So even if you wanted to change the default options in code, you couldn't (and shouldn't -- that's why the inspector exists, and you should use it).

Your means to interact with eye tracking is through the methods attached to `FoveInterface2`. So if you have an object which responds to eye gaze or blinking or whatever, you need to have a reference to a `FoveInterface2` to get that. We highly recommend using Unity's functionality of exposing fields via the Inspector and requiring a `FoveInterface2` that way -- but for runtime spawning of prefabs, it's perfectly acceptable to grab a reference as indicated below. (Note, the example below uses a static reference to the `FoveInterface2` because `FindObjectOfType` is pretty expensive. You should determine if a static singleton pattern works best for your situation. Please program responsibly.)
```
using UnityEngine;

public RespondToEyeTracking : MonoBehaviour {
  private static FoveInterface2 _fove;
  
  void Awake() {
    if (_fove == null) {
      _fove = FindObjectOfType(typeof(FoveInterface2)) as FoveInterface2;
      if (_fove == null) {
        Debug.LogError("No FoveInterface2 object found in the scene! :,(");
        return;
      }
    }
    
    //... do other setup stuff here
  }
}
```

### Calibration Checks
We assume that you want to make sure that the user has a valid calibration when using your program, so we have provided a function to make sure that they have a valid calibration. The function you're looking for is `EnsureEyeTrackingCalibration`. You should take the player somewhere nice that they don't need to be interacting with or seeing anything specific (we recommend a separate scene specifically for this purpose), call `EnsureEyeTrackingCalibration` on a `FoveInterface2` instance, and then keep checking `IsEyeTrackingCalibrating()`. When it returns false, you should be safe to resume the rest of your game.

If you do *not* want to deal with calling this yourself, you can use the *Skip Auto Calibration Check* toggle in the Inspector and make sure it's unchecked, which will force a calibration check to begin as soon as the first `FoveInterface2` object wakes up in your game. It works, but it's a bit clunky. (The automatic check is disabled by default in our prefabs, as it gets a bit tedious going through the check all the time while developing a game.) Also, sometimes you have publisher/tech videos and copyright notices that you want your users to see and which shouldn't be skippable (for licensing reasons); and having calibration running atop all that kind of defeats the purpose.

Note that calibration checks will probably take over the HMD screen, so you can't see anything _other_ than the calibration check while it's going on; and eye gaze is undefined in this state. Hence why you should take your user somewhere safe where there won't be accidentally interacting with things during calibration check..

### You move in local coordinates
The FOVE tracks your position and orientation and feeds that back into Unity. Your `FoveInterface2` behaviour will automatically position itself in accordance with what the FOVE runtime reports for your headset. What this means is for you, dear dev, is that your `GameObject` is going to be spending a lot of time near (but generally not quite at) its origin.

If you don't want your view to always snap back to nearly-world-origin when you hit "Play", then you have to put it underneath (as a child of) another object. (I like to call this something like "FOVE Body" or "FOVE Root" in my projects.) You can now place and rotate (and even scale, if that's your "thing") this root object and the FOVE interface's GameObject will always move relative to its parent object.

Just keep in mind that, if you're going to make an FPS and move the root around, stairs are considered bad in VR.

### Is a user looking at this collider?
There are a number of convenience methods built out for you which determine if the user is looking at a given collider, or a collection of colliders. These methods are designed to be similar to many of the `Raycast` methods provided by Unity, just based around the user's gaze. As such, they're called `Gazecast`. Check them out. They're pretty efficient and easy to use.

If you simply must do things by hand, you can also grab the gaze rays as UnityEngine `Ray` objects from the method `GetGazeRays()` on your `FoveInterface2` instance.

### Using camera effects
We all want to have just the right amount of Bloom. It makes sense. Or tone mapping, or toon shading... Everyone wants something different and `FoveInterface2` makes that something pretty easy to put together. `FoveInterface2` uses the camera it's attached to for both eyes by manipulating some of Unity's VR settings. This means that you get all the effects attached to the *Fove Interface 2* object.

It' probably important to remind you that some full-screen effects are rather intensive and they will be handled twice for each VR camera (essentially each `FoveInterface2` instance) in the scene, so try adding screen effects one at a time as needed later and make sure they don't kill your performance.

### VR Overlay Via Multiple Interfaces
You can now have multiple `FoveInterface2` instances, which allows for a variety of effects -- most notably, you can have HMD-locked interface overlays. There should be an example of this in the Unity sample project; however the general idea is that you have two `FoveInterface2`s, one for the main scene rendering and the other for only the overlay.

On the overlay interface, you would set the camera it uses (ideally a prefab) to only render your overlay layer (you may or may not want to prevent the main camera from rendering the overlay layer depending on your goals). It's likely that you may also want to prevent the overlay's orientation and position from changing as the user moves their headset, which can be done by unchecking the "Position" and "Orientation" checkboxes in the `FoveInterface2` inspector panel. Of course, if you're going to have an overlay, you should open the Compositor options" foldout on the inspector and change "Layer Type" to "Overlay", so the compositor knows to preserve the transparency.

Lastly, if you disable orientation and position on the FoveInterface, you should definitely check "Disable Timewarp" in the Compositor options foldout, otherwise you will almost certainly get uncomfortable judder as your stationary layer gets timewarped unnecessarily.

### "Asynchronous" data access
The plugin automatically handles acquiring pose and gaze data for you in sync with the rendering pipeline, however Unity's FixedUpdate function can be called more frequently than and semi-out-of-sync with the standard Update function. (Default looks like 50 times per second for physics -- you can chage this from `Edit > Project Settings > Time`.) For more information on execution of Unity events, see this document: https://docs.unity3d.com/Manual/ExecutionOrder.html (`FixedUpdate` is not ACTUALLY out of sync, so if that's what you need, you should probably come up with something else.)

If you want to get the immediate, most up-to-date, current information from the SDK, there are methods available to do so:

* `GetHMDRotation_Immediate`
* `GetHMDPosition_Immediate`
* `GetLeftEyeVector_Immediate`
* `GetRightEyeVector_Immediate`

---

## Known Limitations
There are some limitations with the current system which we may be fixing in the near future. (If you have any requests, please contact us!)

* Calling `EnsureEyeTrackingCalibration` will force the calibration to reset, even if you have an active calibrated profile. This will be fixed in a future update to the SDK and runtime.
* SteamVR maintains its own HMD offset values which get applied to the camera in your scene. These are almost certainly different from the position as reported by FOVE. Because those offsets are handled by SteamVR, there will be a notable offset between your scenes when running in SteamVR mode versus FOVE native mode. It may be possible for you to detect which mode and adjust the VR camera's parent object's placement, but you would have to make that decision for yourself. In future versions of the FOVE runtime, we will add a similar offset/calibration feature which should help ensure that both systems are configured similarly. For now, they are likely pretty far off.
* In addition, because SteamVR/OpenVR handle the position of the interface object on their own, the world scale values won't work when running in SteamVR mode. We recommend keeping your world scale at 1 for highest compatibility with other VR systems.
* With the Legacy interface, you cannot use the game object itself as an eye camera prototype. In fact, you cannot use any  Game Object which has a FoveInterface component as the prototype, as this would cause a horrible cascade of instantiating more and more objects until you run out of memory and crash. If you try to use a FoveInterface as the prototype, it will emit a few errors on launch and disable the base FoveInterface. (The rest of the Game Object's components should still function as normal.)

