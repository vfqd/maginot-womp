using Library;
using TMPro;

namespace UI
{
    public class InfoArea : MonoSingleton<InfoArea>
    {
        public TextMeshProUGUI textArea;
        
        public void SHowText(string text)
        {
            textArea.text = text;
        }

        public void HideText()
        {
            textArea.text = "";
        }
    }
}