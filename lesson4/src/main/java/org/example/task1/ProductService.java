package org.example.task1;

import java.io.File;
import java.util.ArrayList;
import java.util.Collection;

public class ProductService implements Readable {

    private ArrayList<String> res;

    public void process1(){

    }

    public void process2(){
        res = null;
    }

    public Collection<String> readTextFile(File file) throws RuntimeException {

        if (file.exists()) {
            if (file.length() > 5000) {
                throw new RuntimeException("file > 5 mb, error");
            }
        }
        else {
            throw new RuntimeException("not found");
        }

        //region work with data

        res = new ArrayList<>();
        res.add("A");
        res.add("B");
        res.add("C");

        process1();
        validateResult(res);
        process2();
        validateResult(res);

        //endregion

        if (res == null) {
            throw new RuntimeException("error data something");
        }
        return res;
    }

    private void validateResult(Collection<String> res) {
        if (res == null || res.size() == 0) {
            throw new RuntimeException("something's wrong");
        }
    }
}
