using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Xml;

/// <summary>
/// An event from within the conversation.
/// </summary>
[System.Serializable]
public class ConversationEvent : UnityEvent<string> { };

/// <summary>
/// A conversation loads a series of speech items and events from an XML file,
/// playing them through the linked up entities.
/// </summary>
public class Conversation : MonoBehaviour
{
    /// <summary>
    /// The conversation XML file.
    /// </summary>
    public TextAsset conversationFile;
    /// <summary>
    /// The chatBox is the main area where chat text comes up.
    /// It is technically optional-- you could have a whole
    /// conversation of just events, I guess.
    /// </summary>
    public Text chatBox;
    /// <summary>
    /// The nameTag is where the name of the speaker comes up.
    /// It is optional.
    /// </summary>
    public Text nameTag;

    /// <summary>
    /// The loaded XML document.
    /// </summary>
    private XmlDocument doc;
    /// <summary>
    /// The current node in the conversation.
    /// </summary>
    private XmlNode current;

    [SerializeField]
    public ConversationEvent eventListeners;

    void OnValidate()
    {
        if (conversationFile == null) Debug.LogError("Missing conversation file");
    }

    void Start()
    {
        doc = new XmlDocument();
        doc.LoadXml(conversationFile.text);
        current = doc["convo"].FirstChild;
        UpdateSelf();
    }

    void UpdateUI()
    {
        if (current == null) return;
        if (current.Name != "speech") return;

        if (chatBox != null) chatBox.text = current.InnerText;
        if (nameTag != null)
        {
            var speaker = (XmlAttribute)current.Attributes["speaker"];

            nameTag.text = (speaker != null) ? speaker.InnerText : "";
        }
    }

    public void ConvoOver()
    {
        // do we want to close the chat boxes, always?
        // or just have this always be implemented by our own events?
        Debug.Log("TODO: ConvoOver");
    }

    /// <summary>
    /// Update our current state based on the current node type.
    /// Different from Update()-- we call this as needed, not once
    /// per frame.
    /// </summary>
    void UpdateSelf()
    {
        if (current == null)
        {
            ConvoOver();
            return;
        }

        switch (current.Name)
        {
            case "speech":
                UpdateUI();
                break;
            case "event":
                eventListeners.Invoke(current.InnerText);
                break;
            default:
                Debug.Log("Unknown node type: " + current.Name);
                break;
        }
    }

    /// <summary>
    /// Go to the next node in the conversation.
    /// This should probably be linked up to a button and/or key.
    /// </summary>
    public void Next()
    {
        if (current != null)
        {
            current = current.NextSibling;
            UpdateSelf();
        }
    }

    void Update() { }
}
