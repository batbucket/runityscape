using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.View.ActionGrid;
using Scripts.Presenter;
using System;
using Scripts.Model.TextBoxes;

namespace Scripts.Model.World.Serialization {

    /// <summary>
    /// Page that allows users to load their saved games.
    /// </summary>
    public class LoadPage : Page {

        public LoadPage() : base("Load") {
            Body = "Select a file to load.";
        }
    }
}