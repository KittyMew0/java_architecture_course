package org.example.task3;

import java.util.ArrayList;
import java.util.Collection;

public class Database {
    private static int counter;
    private Collection<Ticket> tickets = new ArrayList<>();
    private Collection<Customer> customers = new ArrayList<>();

    public Database() {
        tickets.add(new Ticket());
        tickets.add(new Ticket());
        tickets.add(new Ticket());
    }

    public Collection<Ticket> getTickets() {
        return tickets;
    }

    public Collection<Customer> getCustomers() {
        return customers;
    }

    public double getTicketPrice() {
        return 45;
    }

    public int createTicketOrderId(int clientId) {
        return ++counter;
    }
}
