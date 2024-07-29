# RSLib (Unity)
A collection of scripts and shaders written or found, and used over the years for Unity projects.
All of the library content is made to usable in any Unity project, although the Miscellaneous folder contains stuff that could be used but is either WIP or very specific.

Please keep in mind that some of this stuff has been made a while ago, or initially for specific needs, or even for pure personal training purpose: some better working codes, even Unity packages, may already exist and do a better job at what some of my tools tend to do.


## Scripts folders content
- **A\*:** generic A* pathfinding implementation, with some demo scripts and scene.
- **Audio:** audio management system, working with ScriptableObjects.
- **Collections:** FixedSizedConcurrentQueue, Heap, Loop and WeightedList (for probability stuff).
- **ColorPicker:** UI color wheel and script to click on a color to select it.
- **Data:** ScriptableObjects containing data, such as integers, floats, strings, layer masks, etc.
- **Disabled:** custom script to reproduce Odin Inspector's ReadOnly attribute.
- **Encryption:** Rjindeal encryption implementation.
- **Localization:** custom localization implementation working with .csv files.
- **Maths:** lots of math functions (geometry, algebra, BowyerWatson triangulation, easing curves, etc.).
- **Noise:** scripts related to noise functions or textures generation.
- **Optionals:** bound between a bool and another value, to make some fields optional to the users.
- **UI:** user interface related scripts.
- **AnimationCurves:** static properties for predefined AnimationCurves.
- **DontDestroyOnLoad:** makes the gameObject this script is attached to not destroyed on scene load.
- **HealthSystem:** generic class for a unit health system (with min/max values, events on values changed, etc.).
- **Helpers:** class containing methods that can be used in many contexts (random boolean, CopyToClipboard function, etc.).
- **RandomNumberGenerator:** deterministic random number generator, that state can be saved/loaded.

_And some more minor standalone scripts..._


## Framework
- **Events:** ScriptableObjects based event system.
- **FSM:** generic finite state machine implementation.
- **GameSettings:** game settings, accessible to players, generic implementation.
- **InputSystem:** input manager with a remapping and save/load system.
- **Pooling:** custom pooling system, with an IPoolItem interface for any pooled objects.
- **Yield:** custom wait instructions for IEnumerators.
- **CSVReader:** csv file parser.
- **SceneField:** draws a scene field in any custom editor.
- **Singleton:** to make any object use the singleton pattern by deriving from this class.
- **TopologicalSorter:** sorts items based on dependencies between them, handling cyclic dependencies.


## Extensions
List of all the types having their set of extension methods:
- AnimationCurve
- Animator
- Array
- BoxCollider2D
- CircleCollider2D
- Color
- Dictionary
- Float
- GameObject
- IList
- Int
- KeyCode
- LayerMask
- MonoBehaviour
- Quaternion
- Queue
- RectTransform
- Renderer
- Rigidbody
- Selectable
- SpriteRenderer
- Stack
- String
- Texture2D
- Vector2/Vector3
- XDocument


## Debug
- **DebugConsole:** in game accessible console, to call methods from the application.
- **GizmosUtilities** helper used when working with OnDrawGizmos/OnDrawGizmosSelected methods.
- **ValuesDebugger** displays any values on screen, using OnGUI method (no canvas required).


## Editor
- **AssetDatabaseUtilities:** utility methods when working with assets and project folders.
- **ButtonProviderEditor:** generic custom editor class, with a method allowing to draw buttons.
- **FilterStaticObjects:** select all objects from the scene based on their static flag.
- **FindMissingScripts:** helper used to locate missing scripts from assets.
- **GameObjectsGrouper:** select a group of gameObjects and group them into a new empty gameObject.
- **GameObjectsRenamer:** select a group of gameObjects and renamed them using a given name pattern.
- **GameObjectsSorter:** select a group of gameObjects and sort them by name.
- **GradientTextureCreator:** window used to define a color gradient, and extract it as a texture.
- **LayerMaskFieldEditor:** draws a layer mask field in a custom inspector.
- **LayerRecursiveSetter:** sets the layer mask of a gameObject and all of its children.
- **MeshesStaticSetter:** sets the static flag of all meshes found in the children of a gameObject.
- **OpenPersistentDataPathMenu:** static method opening persistent data path in explorer.
- **PrefabEditorUtilities:** utility methods when working with prefabs.
- **PresetImportPerFolder:** Unity tool used to apply import preset to subfolders.
- **SceneManagerUtilities:** utility methods when working with scenes.
- **Screenshot:** used to take screenshot of the game window, excluding UI.
- **SolutionSynchronizer:** fixes an issue when sometimes the project and the VS solution are not synchronized correctly.
- **SpriteOrderingDataSetterEditor:** sets the ordering values of the sprites in a gameObject and all of its children.
- **TexturesScanner:** checks the textures in a given folder to locate possible issues.
- **TilemapEditorTools:** some utilities regarding Tilemaps.

## Shaders folders content
- **Debug shaders:** visualizers for UV, normal, vertex color, etc.
- **cginc files:** color blending, color correction, easing functions, stochastic sampling, maths utils.
