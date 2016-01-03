using UnityEngine;
using System.Collections;

public static class TextBoxFactory {
    public static readonly Color DEFAULT_COLOR = Color.white;
    public const float DEFAULT_WRITE_SPEED = 0.0f;

    public static TextBoxManager createTextBox(TextBoxHolderManager textBoxHolderManager, string fullText, float lettersPerSecond, Color color) {
        TextBoxManager textBoxManager = textBoxHolderManager.addTextBox();
        textBoxManager.post(fullText, lettersPerSecond, color);
        return textBoxManager;
    }

    public static TextBoxManager createTextBox(TextBoxHolderManager textBoxHolderManager, string fullText, float lettersPerSecond) {
        TextBoxManager textBoxManager = textBoxHolderManager.addTextBox();
        textBoxManager.post(fullText, lettersPerSecond, DEFAULT_COLOR);
        return textBoxManager;
    }

    public static TextBoxManager createTextBox(TextBoxHolderManager textBoxHolderManager, string fullText, Color color) {
        TextBoxManager textBoxManager = textBoxHolderManager.addTextBox();
        textBoxManager.post(fullText, DEFAULT_WRITE_SPEED, color);
        return textBoxManager;
    }

    public static TextBoxManager createTextBox(TextBoxHolderManager textBoxHolderManager, string fullText) {
        TextBoxManager textBoxManager = textBoxHolderManager.addTextBox();
        textBoxManager.post(fullText, DEFAULT_WRITE_SPEED, DEFAULT_COLOR);
        return textBoxManager;
    }
}
