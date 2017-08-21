# MonsterScape #
* An in-development text-based role-playing game project for browser play
* Written in the Unity engine using C#

# TODOs after Downloading #
* Setup the fields needed for the GameJolt API to work. From the editor, Edit → Project Settings → Game Jolt API

# Other Plugins #
* NUnit for unit testing
* [Unity API](http://gamejolt.com/games/unity-api/15887) for GameJolt, modified slightly

# Demo #
* [Here](https://gamejolt.com/games/monsterscape/270136) on GameJolt

# Old Version (deprecated) #
* [Here](https://drive.google.com/open?id=0B5E_IBqde8fLdGpmdUYyYmNzNHc) as a download
* The downloadable version only works on devices with a screen ratio of 16:9. Other dimensions may cause cutoffs

# Game Features #
* Turn-based combat
* NPC AI
* Spell system with buff support
* Item system
* Inventory system
* Equipment system
* Character statistics

# Technical Features #
* [JSON serialization](https://docs.unity3d.com/Manual/JSONSerialization.html) for saving and loading data
    * Object-oriented serialization system that allows for extensions
        * Each object that needs to be serialized implements ISaveable`<T>` where T is an equivalent class that holds serializable data
            * Examples: Character : ISaveable`<CharacterSave>`, Buff : ISaveable`<BuffSave>`
    * This is achieved through an ID Table, which associates each unique type with a unique string
        * The table automatically checks that each type and string is unique
        * The table has an associated unit test segment that ensures that all serializable content is included in itself
        * This table also allows class names to be changed without impacting any saves
* Dynamic [object pooling](https://unity3d.com/learn/tutorials/topics/scripting/object-pooling) system for performance
    * Requires inheritance from the abstract PooledBehaviour, which handles the resetting of fields
    * Object pooler will call Instantiate on GameObjects if it runs out of pooled objects of a specific type, dynamically increasing the size of the object pool as neccessary
    * Object pooler will reject already-returned objects, performing a contains check on a HashSet using the object's InstanceID as its hash
* Uses GameJolt's API for scoreboards and achievements
* Saves can be exported and imported using into GameJolt's data storage by associating it with a unique key
    * This reduces the ~1770 character save string to a more manageable 1-8 character string
    * This is needed because WebGL prevents the use of the clipboard
* Programmed a solution for maintaining references when serializing
    * For instance, if A casts a buff on B that redirects damage taken by B to A, the game will maintain that reference between loads
* Code structure based on the Model-View-Presenter pattern
* Heavy use of object-oriented programming and lambda expressions
* Regex-based string parser for writing text one letter at a time, while maintaining word wrap for the full string, and allowing rich text tags
* Regex-based string parser for easily creating dialogue for characters by detecting specific tags in a string
* Wrote unit tests to test the serialization process
* Flyweight pattern used to store constant string, integer, and color values