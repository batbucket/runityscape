using UnityEngine;
using System.Collections;

public static class TextBoxFactory {
    public static TextBoxManager createTextBox(TextBoxHolderManager textBoxHolderManager, string fullText, float lettersPerSecond, Color color) {
        TextBoxManager textBoxManager = textBoxHolderManager.addTextBox();
        textBoxManager.post(fullText, lettersPerSecond, color);
        return textBoxManager;
    }
}
