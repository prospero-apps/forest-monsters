using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Story : MonoBehaviour
{   
    private StringBuilder sb = new StringBuilder();
    private string story;
      
    // UI elements
    [SerializeField] private TMP_Text storyText;
    [SerializeField] private Button mainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        CreateStoryText();
        StartCoroutine(PrintText());
    }    

    IEnumerator PrintText()
    {
        // Wait for two seconds.
        yield return new WaitForSeconds(2);

        // Start printing the story text letter by letter.
        foreach (char letter in story)
        {
            yield return new WaitForSeconds(0.05f);
            storyText.text += letter;
        }
    }

    void CreateStoryText()
    {
        sb.Append("Many a year ago, a sorcerer cast a spell on a cheerful forest, ");
        sb.Append("turning it into a most abominable place.");
        sb.Append("\n\nHordes of forest monsters were created by him to protect him and ");
        sb.Append("prevent anyone from entering the forest. For years the bravest of ");
        sb.Append("the brave have tried to free the forest. All in vain. Nobody has ");
        sb.Append("managed to get through all the dangers of the horrifying place and ");
        sb.Append("kill the sorcerer, who, after a couple years of relative peace and ");
        sb.Append("quiet is now beginning to take interest in the neighboring villages ");
        sb.Append("and towns, trying to cast his spell farther and farther and thus ");
        sb.Append("enlarge his territory. If nothing is ventured in order to prevent ");
        sb.Append("this from happening, the sorcerer will grow in power.");
        sb.Append("\n\nAnd here you come. Go, kill the monsters in the forest and find the ");
        sb.Append("sorcerer. Hard as it will turn out to be, don’t hesitate to kill him, ");
        sb.Append("he’s not a human being.");

        story = sb.ToString();
    }
}
