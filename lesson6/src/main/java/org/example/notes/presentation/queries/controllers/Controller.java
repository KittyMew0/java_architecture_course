package org.example.notes.presentation.queries.controllers;

import org.example.notes.presentation.queries.views.Presenter;

public abstract class Controller {
    public <T extends Presenter> void view(T presenter) {

    }
}
