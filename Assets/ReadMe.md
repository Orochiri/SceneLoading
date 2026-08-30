# Introduction
This project presents a structure to additively load scenes in Unity2D. Three scripts manage the whole architecture: A SceneGroup script that uses a open source project (https://github.com/starikcetin/Eflatun.SceneReference) to address scenes and collect them in a group, a SceneGroupManager that handles all of the actual Scene-loading and unloading and lastly a SceneLoader script that takes commands to Load new SceneGroups. The core of it is the Bootstrapper scene. It will be running the whole time and manage the scene loading. Therefor it comes with a Scene Loader object that has one component, the Scene Loader Script, and two children, a Canvas and a Camera in case you want to show a loading screen with a loading bar. Lastly the Scene has a Event System, so your other script wont need one. 

### Quick Warning
Using this scene loading structure is fairly easy. The project comes with an example on how to implement it. The only 5 things you need to be careful with changing are the Bootstrapper scene and script, the SceneGroup, SceneGroupManager and the SceneLoading script. The rest is only here for a demonstration and can be changed easily. 
# How To Use

1. In the Bootstrapper scene create your desired SceneGroups (in this example they're named rooms). The Scene Type can be referenced in scripts but is great for general organization. Remember that the first group will be the first to load in (defined in the Start() function of the SceneLoader script).

2. Create a way to trigger the LoadSceneGroup(int index, bool loadingScreen) function. The index is defined by the list you created in the first step and if loadingScreen is true, you will get a loadingScreen with a loading bar.
	- The given example creates a Instance of the SceneLoader class and calls it OnTriggerEnter2D() in a ScemeTrigger script. However you could combine this with my other project about the event structure and create an event for triggering the scenes for example. If you find another way, remember to remove the lines for creating an instance of SceneLoader (line 21 and 25-28)

**congrats, you completed all the basic steps!!!**
the following points are optional :)

3. Change the canvas in the Bootstrapper scene, you can add a custom image and/or change the loading bar to your liking. 
4. In the SceneGroupManager script line 43 can be activated to lengthen the loading time in case it's too fast and you want to see the loading bar actually filling up.
5. Now it's up to you to change the rest to your preferences, be creative, have fun <3

# Acknowledges 
I made this project with a youtube tutorial from git-amend (https://www.youtube.com/watch?v=JFP-cCFID7o). It expands the functionality a bit but heavily relies on the structure given in this tutorial. Check it out if you want to learn more about the structure and leave a like of course xDD. 
Also referenced in that tutorial is the already mentioned open source project of [starikcetin](https://github.com/starikcetin) on github (https://github.com/starikcetin/Eflatun.SceneReference.git). It enables us to more effectively reference Scenes in unity. 



