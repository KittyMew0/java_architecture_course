package org.example.notes.core.application.interfaces;

import org.example.notes.core.domain.Note;

public interface NoteEditor extends Editor<Note, Integer>{
    void printAll();
}
