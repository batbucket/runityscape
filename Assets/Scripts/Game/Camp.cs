
using Scripts.Model.Acts;
using Scripts.Model.Pages;

public class Camp : PageGroup {

    public Camp() : base(new Page("Unknown")) {

    }

    private void SetupNameInput() {
        Page page = Get(ROOT_INDEX);
        page.Body = "Welcoem tot he game";
    }
}
