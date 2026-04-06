package org.example.database;

import java.util.Date;

public class NotesRecord {
    private static int counter;

    {
        id = ++counter;
    }

    private int id;
    private int userId;
    private String title;
    private String details;
    private Date creationDate;
    private Date editDate;

    public int getId() {
        return id;
    }

    public int getUserId() {
        return userId;
    }

    public String getTitle() {
        return title;
    }

    public String getDetails() {
        return details;
    }

    public Date getCreationDate() {
        return creationDate;
    }

    public Date getEditDate() {
        return editDate;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public void setCreationDate(Date creationDate) {
        this.creationDate = creationDate;
    }

    public void setEditDate(Date editDate) {
        this.editDate = editDate;
    }

    public NotesRecord(String title, String details) {
        this.title = title;
        this.details = details;
        this.creationDate = new Date();
    }
}
