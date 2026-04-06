package org.example.task3;

import java.util.ArrayList;
import java.util.Collection;

public class Customer {
    private static int counter;
    private final int id;
    private Collection<Ticket> tickets;

    {
        id = ++counter;
    }

    public int getId() {
        return id;
    }

    public void setTickets(ArrayList<Ticket> tickets) {
        this.tickets = tickets;
    }

    public Collection<Ticket> getTickets() {
        return tickets;
    }
}
