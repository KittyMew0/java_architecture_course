package org.example;

import org.example.models.TableModel;
import org.example.presenters.BookingPresenter;
import org.example.presenters.Model;
import org.example.presenters.View;
import org.example.views.BookingView;

import java.util.Date;

// дз - добавить какие то методы - change reservation whatever

public class Main {
    public static void main(String[] args) {
        View view = new BookingView();
        Model model = new TableModel();
        BookingPresenter presenter = new BookingPresenter(model, view);

        presenter.updateUIShowTables();

        view.reservationTable(new Date(), 2, "name");


        // this should work
        view.changeReservationTable(1001, new Date(), 3, "name 2");
    }
}