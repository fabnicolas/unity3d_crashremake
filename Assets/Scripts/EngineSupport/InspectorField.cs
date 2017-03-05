/*
    Unity3D helper class for creating lists accessible directly from the Unity inspector.
    You should use this system as less as possible, because it will slow down the building of a Dictionary, even if it's a must in some cases.

    Consider this use case.

    USE CASE:You need to insert inside a ThreadsafeDictionary<string,Texture> a list of textures associated with keys of string type.
    
    PROBLEM: In order to reference Textures efficiently, you must use Unity3D inspector. The Unity3D inspector doesn't provide directly
    injection ability inside ThreadsafeDictionary (or any Dictionary), not even table-like arrays.

    SOLUTION: Make a class that represents a field containing key and value; make an array of that class.
    
    - PROBLEM 2: You can't use directly generic types inside Unity3D inspector.

    - SOLUTION 2: Make, whenever you need, a class that extends this class, like in the code below.

    CODE:
    [System.Serializable]
	public class TextureField : InspectorField<string,Texture>{}

    [SerializeField]
    public TextureField[] texture_list;     // This will appear inside the inspector!
*/

public class InspectorField<K, V>{
    public K key;
    public V value;
}

