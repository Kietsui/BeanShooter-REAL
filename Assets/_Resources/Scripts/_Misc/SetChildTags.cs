using UnityEngine;

public class SetChildTags : MonoBehaviour
{
    [SerializeField] private string tagToAssign; // The tag you want to assign to the child objects

    // Call this function to assign tags
    public void AssignTagsToChildren()
    {
        // Get all child objects recursively
        Transform[] childTransforms = GetComponentsInChildren<Transform>();

        // Loop through each child object and set its tag
        foreach (Transform child in childTransforms)
        {
            child.gameObject.tag = tagToAssign;
        }

        Debug.Log("Tags assigned to all child objects.");
    }
}
