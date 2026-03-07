using System;
using System.Collections.Generic;
using KooliProjekt.WinFormsApp.Models;

namespace KooliProjekt.WinFormsApp.Views
{
    /// <summary>
    /// ITeamView - View interface for MVP pattern
    /// Form must implement this interface to work with Presenter
    /// </summary>
    public interface ITeamView
    {
        // Properties - data binding between View and Presenter
        List<Team> Teams { get; set; }
        Team? SelectedTeam { get; set; }
        int TeamId { get; set; }
        string TeamName { get; set; }
        string StatusMessage { get; set; }

        // Events - triggered by user actions in View
        event EventHandler LoadRequested;
        event EventHandler NewRequested;
        event EventHandler SaveRequested;
        event EventHandler DeleteRequested;

        // Methods - actions Presenter can call on View
        void ShowError(string message);
        void ShowSuccess(string message);
        void ClearForm();
    }
}
