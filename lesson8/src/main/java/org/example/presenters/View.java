package org.example.presenters;

import org.example.models.Table;

import java.util.Collection;
import java.util.Date;

public interface View {

    void showTables(Collection<Table> tables);

    void showReservationTableResult(int reservationNo);

    void setObserver(ViewObserver observer);

    void reservationTable(Date orderDate, int tableNo, String name);
}
