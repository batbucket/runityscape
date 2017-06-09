using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.TextBoxes;
using Scripts.View.ActionGrid;
using Scripts.Presenter;
using System;

namespace Scripts.Model.World.Serialization {

    /// <summary>
    /// Page that allows users to save and overwrite data.
    /// </summary>
    public class SavePage : Page {

        public SavePage() : base("Save") {
            Body = "Make a new save, or overwrite an existing one.";
        }
    }
}