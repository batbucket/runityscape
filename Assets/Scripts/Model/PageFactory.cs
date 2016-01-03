using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PageFactory {
    public static Page createPage(string text = "", PageType pageType = PageType.NORMAL, List<Character> left = null, List<Character> right = null,
        Process onFirstEnter = null, Process onEnter = null, Process onFirstExit = null, Process onExit = null,
        List<Process> actions = null) {
        Page page = new Page();
        page.setText(text);
        page.setPageType(pageType);
        page.setLeft(left);
        page.setRight(right);
        page.setOnFirstEnter(onFirstEnter);
        page.setOnEnter(onEnter);
        page.setOnFirstExit(onFirstExit);
        page.setOnExit(onExit);
        page.setActions(actions);
        return page;
    }
}
